Imports MySql.Data.MySqlClient
Public Class FrmDataDosen

    ''' <summary> 
    ''' Mmbuat variabel untuk pagging 
    ''' </summary> 
    Dim CurrentPage As Integer = 1
    Dim PageSize As Integer = 10
    Dim TotalData As Integer = 0
    Dim TotalPage As Integer = 0
    Dim Offset As Integer = 0
    Dim Batas As Integer
    'Cache Paging 
    Dim PageCache As New Dictionary(Of String, DataTable)
    Dim LastCacheKey As String = ""

    Function GEtCacheKey(Page As Integer) As String
        Dim Prodi As String = If(CbNamaJurusan.SelectedValue Is Nothing, "", CbNamaJurusan.SelectedValue.ToString())
        Dim NamaDosen As String = TxtCariNama.Text.Trim()
        Dim Limit As String = ComboBoxEntries.Text
        Return $"{Prodi}|{NamaDosen}|{Limit}|PAGE:{Page}"
    End Function

    Sub ClearPagingCache()
        PageCache.Clear()
        LastCacheKey = ""
    End Sub

    Sub NumberEntriesHalaman()
        ComboBoxEntries.Items.Add("10")
        ComboBoxEntries.Items.Add("15")
        ComboBoxEntries.Items.Add("20")
        ComboBoxEntries.Items.Add("25")
        ComboBoxEntries.Items.Add("50")
        ComboBoxEntries.Items.Add("100")

        'menampilkan data index ke 0 
        ComboBoxEntries.SelectedIndex = 0
    End Sub

    Private isloading As Boolean = True

    'menonaktifkan object
    Sub FormNonAktif()
        ComboBoxEntries.Enabled = False
        CbNamaJurusan.Enabled = False
        TxtCariNama.Enabled = False
        BtnCari.Enabled = False

        BtnTambah.Enabled = True
        BtnTambah.Text = "Aktifkan From"

        BtnKeluar.Enabled = True
        BtnKeluar.Text = "Batal"

    End Sub

    'mengaktifkan object
    Sub FormAktif()
        ComboBoxEntries.Enabled = True
        CbNamaJurusan.Enabled = True
        TxtCariNama.Enabled = True
        BtnCari.Enabled = True

        BtnTambah.Enabled = True
        BtnTambah.Text = "Tambah Data"
        BtnTambah.BackColor = Color.White

        BtnKeluar.Enabled = True
        BtnKeluar.Text = "Keluar"
    End Sub

    'mengosongkan object
    Sub KosongkanData()
        CbNamaJurusan.SelectedIndex = -1
        TxtCariNama.Text = ""
    End Sub

    Private Function ProdiSudahDipilih() As Boolean
        Return Not (CbNamaJurusan.SelectedValue Is Nothing OrElse CbNamaJurusan.SelectedIndex < 0)
    End Function

    'mengembalikan form kepada keadaan normal
    Sub FormNormal()
        Call KosongkanData()
        Call FormAktif()
        BtnKeluar.Text = "Keluar"
    End Sub

    'Membuat Nomor Urut pada datagrid 
    Private Sub FormatRowHeader(grid As DataGridView, startIndex As Integer, PageSize As Integer)
        grid.SuspendLayout()
        'atur tampilan grid 
        grid.AllowUserToAddRows = False
        grid.RowHeadersVisible = True
        grid.RowHeadersWidth = 58
        grid.RowHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold)

        For i As Integer = 0 To grid.Rows.Count - 1
            grid.Rows(i).HeaderCell.Value = (startIndex + i + 1).ToString()
        Next
        grid.ResumeLayout()
    End Sub

    Private Sub DataGridDosen_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridDosen.DataBindingComplete
        For i As Integer = 0 To DataGridDosen.Rows.Count - 1
            DataGridDosen.Rows(i).HeaderCell.Value = (Offset + i + 1).ToString()
        Next
    End Sub

    Sub RefreshPaging()
        CurrentPage = 1
        ClearPagingCache()
        HitungTotalData()
        TampilkanDataGridDosen()
    End Sub

    'membuat tampilan datagridview'
    Sub AktifDataGridDosen()
        With DataGridDosen
            .EnableHeadersVisualStyles = False
            .Font = New Font(DataGridDosen.Font, FontStyle.Bold)
            DataGridDosen.DefaultCellStyle.Font = New Font("Arial", 9)
            DataGridDosen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            DataGridDosen.ColumnHeadersHeight = 35

            DataGridDosen.Columns(0).Width = 60
            DataGridDosen.Columns(0).HeaderText = "KODE DOSEN"
            DataGridDosen.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridDosen.Columns(1).Width = 50
            DataGridDosen.Columns(1).HeaderText = "NIDN"
            DataGridDosen.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridDosen.Columns(2).Width = 340
            DataGridDosen.Columns(2).HeaderText = "NAMA DOSEN"
            DataGridDosen.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridDosen.Columns(3).Width = 60
            DataGridDosen.Columns(3).HeaderText = "JENIS KELAMIN"
            DataGridDosen.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridDosen.Columns(4).Width = 70
            DataGridDosen.Columns(4).HeaderText = "NOMOR HP"
            DataGridDosen.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridDosen.Columns(5).Width = 100
            DataGridDosen.Columns(5).HeaderText = "EMAIL"
            DataGridDosen.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Sub

    Sub HitungTotalData()
        Try
            Call KoneksiDB()

            QUERY = "SELECT COUNT(*)
                    FROM tbl_dosen
                    WHERE tbl_dosen.Kd_Prodi = @Kd_Prodi"

            If TxtCariNama.Text <> "" Then
                QUERY &= " AND Nm_Dosen LIKE @NamaDosen"
            End If

            Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                CMD.Parameters.AddWithValue("@Kd_Prodi", CbNamaJurusan.SelectedValue)

                If TxtCariNama.Text <> "" Then
                    CMD.Parameters.AddWithValue("@NamaDosen", "%" & TxtCariNama.Text & "%")
                End If

                TotalData = CInt(CMD.ExecuteScalar())
            End Using

            TotalPage = Math.Ceiling(TotalData / PageSize)
            If TotalPage < 1 Then TotalPage = 0

            'UPDATE LABEL 
            LbTotalBaris.Text = "Total Baris: " & TotalData.ToString()
            LbJumlahBaris.Text = "Jumlah Baris Entri: " & DataGridDosen.Rows.Count.ToString()
            LbHasilBagiHalaman.Text = "Jumlah Halaman: " & TotalPage


        Catch ex As Exception
            MessageBox.Show("Error HitungTotalData: " & ex.Message)
        End Try
    End Sub

    Sub LoadPage()
        Offset = (CurrentPage - 1) * PageSize
        Dim cacheKey As String = $"{CurrentPage}-{PageSize}-{CbNamaJurusan.Text}-{TxtCariNama.Text}"

        'CACHE 
        If PageCache.ContainsKey(cacheKey) Then
            DataGridDosen.DataSource = PageCache(cacheKey)
            Exit Sub
        End If

        Call KoneksiDB()

        QUERY = "SELECT
                  Kd_Dosen,
                  Nidn_Dosen,
                  Nm_Dosen,
                  Jk_Dosen,
                  NoHp_Dosen,
                  Email_Dosen 
                FROM
                  tbl_dosen 
                WHERE
                  Kd_Prodi = @Kd_Prodi"

        If TxtCariNama.Text <> "" Then
            QUERY &= " AND Nm_Dosen LIKE @NamaDosen"
        End If

        QUERY &= " ORDER BY NIDN_Dosen ASC LIMIT @Offset, @Limit"

        Using DA As New MySqlDataAdapter(QUERY, DbKoneksi)
            DA.SelectCommand.Parameters.AddWithValue("@Kd_Prodi", CbNamaJurusan.SelectedValue)
            DA.SelectCommand.Parameters.AddWithValue("@Offset", Offset)
            DA.SelectCommand.Parameters.AddWithValue("@Limit", PageSize)

            If TxtCariNama.Text <> "" Then
                DA.SelectCommand.Parameters.AddWithValue("@NamaDosen", "%" & TxtCariNama.Text & "%")
            End If

            Dim dt As New DataTable
            DA.Fill(dt)

            PageCache(cacheKey) = dt
            DataGridDosen.DataSource = dt
        End Using

        UpdateNavigator()
    End Sub

    Sub TampilkanNamaProdi()
        Try
            Call KoneksiDB()
            QUERY = "SELECT tbl_prodi.Kd_Prodi, tbl_prodi.Nm_Prodi 
                    FROM tbl_prodi 
                    ORDER BY tbl_prodi.Kd_Prodi"

            DA = New MySqlDataAdapter(QUERY, DbKoneksi)
            Dim DT As New DataTable
            DA.Fill(DT)

            ''opsi all
            'Dim row As DataRow = DT.NewRow()
            'row("Kd_Prodi") = "ALL"
            'row("Nm_Prodi") = "ALL"
            'DT.Rows.InsertAt(row, 0)

            CbNamaJurusan.DataSource = DT
            CbNamaJurusan.DisplayMember = "Nm_Prodi"
            CbNamaJurusan.ValueMember = "Kd_Prodi"
            CbNamaJurusan.SelectedIndex = -1
            Lbkdprodi.Text = ""

        Catch ex As Exception
            MsgBox("Gagal memuat data Prodi: " & ex.Message, vbCritical)
        End Try
    End Sub

    Sub TampilkanDataGridDosen()
        Try
            'hitung nilai paging 
            PageSize = Val(ComboBoxEntries.Text)
            Offset = (CurrentPage - 1) * PageSize

            Dim CacheKey As String = GEtCacheKey(CurrentPage)

            'Ambil dari cache 
            If PageCache.ContainsKey(CacheKey) Then
                Dim dtCache As DataTable = PageCache(CacheKey)

                DataGridDosen.DataSource = dtCache
                FormatRowHeader(DataGridDosen, Offset, PageSize)
                LbJumlahBaris.Text = "Jumlah Baris Entri: " & dtCache.Rows.Count
                UpdateNavigatorState()
            End If

            Call KoneksiDB()

            QUERY = "SELECT
                      tbl_dosen.Kd_Dosen,
                      tbl_dosen.Nidn_Dosen,
                      tbl_dosen.Nm_Dosen,
                      tbl_dosen.Jk_Dosen,
                      tbl_dosen.NoHp_Dosen,
                      tbl_dosen.Email_Dosen 
                    FROM
                      tbl_dosen
                      INNER JOIN tbl_prodi ON tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi 
                    WHERE
                      tbl_prodi.Kd_Prodi = @Kd_Prodi"

            If TxtCariNama.Text <> "" Then
                QUERY &= " AND tbl_dosen.Nm_Dosen LIKE @NamaDosen"
            End If

            QUERY &= " ORDER BY tbl_dosen.Kd_Dosen ASC LIMIT @Offset, @Limit"

            Using DA As New MySqlDataAdapter(QUERY, DbKoneksi)
                ' Tambahkan tanda @
                DA.SelectCommand.Parameters.AddWithValue("@Kd_Prodi", CbNamaJurusan.SelectedValue)
                DA.SelectCommand.Parameters.AddWithValue("@Offset", Offset)
                DA.SelectCommand.Parameters.AddWithValue("@Limit", PageSize)

                If TxtCariNama.Text <> "" Then
                    DA.SelectCommand.Parameters.AddWithValue("@NamaDosen", "%" & TxtCariNama.Text & "%")
                End If

                Dim DT As New DataTable
                DA.Fill(DT)
                'isi cache 
                PageCache(CacheKey) = DT

                DataGridDosen.DataSource = DT
                'Nomor Urut di sisi kiri 
                FormatRowHeader(DataGridDosen, Offset, PageSize)

                LbJumlahBaris.Text = "Jumlah Baris Entri: " & DT.Rows.Count.ToString()
                UpdateNavigatorState()
                DataGridDosen.ReadOnly = True
            End Using

        Catch ex As Exception
            MessageBox.Show("Error TampilkanDataGridDosen: " & ex.Message)
        End Try

        AktifDataGridDosen()
    End Sub

    Sub UpdateNavigator()
        BindingNavigatorPositionItem.Text = CurrentPage.ToString()
        BindingNavigatorCountItem.Text = $"of {TotalPage}"

        BindingNavigatorMoveFirstItem.Enabled = CurrentPage > 1
        BindingNavigatorMovePreviousItem.Enabled = CurrentPage > 1
        BindingNavigatorMoveNextItem.Enabled = CurrentPage < TotalPage
        BindingNavigatorMoveLastItem.Enabled = CurrentPage < TotalPage
    End Sub

    Sub UpdateNavigatorState()
        BindingNavigatorMoveFirstItem.Enabled = (CurrentPage > 1)
        BindingNavigatorMovePreviousItem.Enabled = (CurrentPage > 1)

        BindingNavigatorMoveNextItem.Enabled = (CurrentPage < TotalPage)
        BindingNavigatorMoveLastItem.Enabled = (CurrentPage < TotalPage)

        BindingNavigatorPositionItem.Text = CurrentPage.ToString()
        BindingNavigatorCountItem.Text = $"of {TotalPage}"
    End Sub

    Private Sub FrmDataDosen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' --- JURUS ANTI-LAG TEMEN LU ---
        If Not SystemInformation.TerminalServerSession Then
            Dim dgvType As Type = DataGridDosen.GetType
            Dim pi As Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            pi.SetValue(DataGridDosen, True, Nothing)
        End If
        ' -------------------------------

        isloading = True
        Call KoneksiDB()
        Call FormNonAktif()

        ' ... sisa kodingan load Lu yang lain ...
        Call NumberEntriesHalaman()
        Call TampilkanNamaProdi()

        CurrentPage = 1
        ClearPagingCache()
        RefreshPaging()
        Call TampilkanDataGridDosen()

        Me.AcceptButton = BtnCari
        DataGridDosen.Enabled = False
        isloading = False
    End Sub

    Sub HitungRowDataGrid()
        Try
            Call KoneksiDB()
            'menghitung baris halaman
            BindingNavigatorPositionItem.Text = 1
            Nomor = (Val(BindingNavigatorPositionItem.Text) - 1) * Val(ComboBoxEntries.Text)
            Batas = ComboBoxEntries.Text

            'menampilkan jumlah seluruh data record saat di tampilkan ke data grid 
            Call KoneksiDB()
            CMD = New MySqlCommand(" SELECT
                                      CEILING( COUNT( * ) ) AS Hasil 
                                    FROM
                                      tbl_dosen
                                      INNER JOIN tbl_prodi ON tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi", DbKoneksi)

            DR = CMD.ExecuteReader
            DR.Read()
            TotalData = DR!Hasil
            DR.Close()

            LbTotalBaris.Text = "Total Baris All: " & TotalData & ""

        Catch ex As Exception
            MessageBox.Show("Error HitungRowDataGrid: " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBoxEntries_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBoxEntries.KeyPress
        e.Handled = True
    End Sub

    Private Sub ComboBoxEntries_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEntries.SelectedIndexChanged
        'menghitung jumlah baris data yang akan di tampilkan
        If ComboBoxEntries.SelectedIndex = -1 Then Exit Sub

        PageSize = CInt(ComboBoxEntries.Text)
        RefreshPaging()
    End Sub

    Private Sub BindingNavigatorPositionItem_Leave(sender As Object, e As EventArgs) Handles BindingNavigatorPositionItem.Leave
        If Val(BindingNavigatorPositionItem.Text) < 1 Then BindingNavigatorPositionItem.Text = "1"

        If Val(BindingNavigatorPositionItem.Text) > Val(LbHasilBagiHalaman.Text) Then
            BindingNavigatorPositionItem.Text = LbHasilBagiHalaman.Text
        End If
    End Sub

    Private Sub CbNamaJurusan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaJurusan.SelectedIndexChanged
        If isloading OrElse CbNamaJurusan.SelectedValue Is Nothing Then Exit Sub

        Try
            Dim Kode_Jurusan As String = CbNamaJurusan.SelectedValue.ToString()
            Lbkdprodi.Text = Microsoft.VisualBasic.Right(Kode_Jurusan, 2)

            ' 1. Validasi dulu
            If ProdiBelumAdaDosen(Kode_Jurusan) Then
                ' Bersihkan Grid jika data kosong
                DataGridDosen.DataSource = Nothing
                ClearPagingCache()
                LbTotalBaris.Text = "Total Baris Halaman: 0"
                LbHasilBagiHalaman.Text = "Jumlah Halaman: 0"
                LbJumlahBaris.Text = "Jumlah Baris Entri: 0"

                MessageBox.Show("Data dosen jurusan " & CbNamaJurusan.Text & " belum ada.", "Data Kosong",
                           MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' 2. Jika ada data, baru jalankan proses loading
            CurrentPage = 1
            ClearPagingCache()
            RefreshPaging()
            TampilkanDataGridDosen()

        Catch ex As Exception
            MsgBox("Error Memilih Prodi: " & ex.Message, vbCritical)
        End Try
    End Sub

    Function ProdiBelumAdaDosen(kdProdi As String) As Boolean
        Try
            Call KoneksiDB()
            ' Gunakan @Kd_Prodi agar merujuk ke parameter, bukan nama kolom
            QUERY = "SELECT COUNT(*) FROM tbl_dosen WHERE Kd_Prodi = @Kd_Prodi"

            Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                ' Pastikan nama parameter diawali dengan @
                CMD.Parameters.AddWithValue("@Kd_Prodi", kdProdi)
                Dim jumlah As Integer = CInt(CMD.ExecuteScalar())
                Return jumlah = 0
            End Using
        Catch ex As Exception
            MessageBox.Show("Error validasi prodi: " & ex.Message)
            Return False
        End Try
    End Function

    Function ProdiAda(KdProdi As String) As Boolean
        Call KoneksiDB()

        QUERY = "SELECT COUNT( * ) 
                    FROM tbl_dosen 
                    WHERE tbl_dosen.Kd_Prodi = Kd_Prodi"

        CMD = New MySqlCommand(QUERY, DbKoneksi)
        CMD.Parameters.AddWithValue("Kd_Prodi", KdProdi)
        Return CInt(CMD.ExecuteScalar()) > 0
    End Function

    Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles BtnCari.Click
        Try
            Call KoneksiDB()

            If Not ProdiSudahDipilih() Then
                MsgBox("Pilih Prodi terlebih dahulu!", vbExclamation)
                CbNamaJurusan.Focus()
                Exit Sub
            End If

            If TxtCariNama.Text = "" Then
                MsgBox("Silahkan Inputkan nama yang akan dicari dahulu!", vbExclamation)
                TxtCariNama.Focus()
                Exit Sub
            End If

            'reset paging cache supaya hasil cari fresh
            CurrentPage = 1
            ClearPagingCache()

            QUERY = "SELECT DISTINCT 
                      tbl_dosen.Kd_Dosen,
                      tbl_dosen.Nidn_Dosen,
                      tbl_dosen.Nm_Dosen,
                      tbl_dosen.Jk_Dosen,
                      tbl_dosen.NoHp_Dosen,
                      tbl_dosen.Email_Dosen,
                    FROM tbl_dosen
                    INNER JOIN tbl_prodi ON tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi
                    WHERE tbl_prodi.Nm_Prodi = @NmProdi
                      AND tbl_dosen.Nm_Dosen LIKE @NamaDosen"

            DA = New MySqlDataAdapter(QUERY, DbKoneksi)
            DA.SelectCommand.Parameters.AddWithValue("@NmProdi", CbNamaJurusan.Text)
            DA.SelectCommand.Parameters.AddWithValue("@NamaDosen", "%" & TxtCariNama.Text & "%")

            DS = New DataSet
            DA.Fill(DS)

            DataGridDosen.DataSource = DS.Tables(0)
            DataGridDosen.Enabled = True

            'Validasi jika hasil cari kosong -> balik ke data normal
            If DS.Tables(0).Rows.Count = 0 Then
                MsgBox("Data dosen tidak ditemukan!", vbInformation)

                TxtCariNama.Text = ""
                CurrentPage = 1
                ClearPagingCache()
                RefreshPaging()
                TampilkanDataGridDosen()
                DataGridDosen.Enabled = True
                Exit Sub
            End If

            'rapihin nomor urut + navigator
            PageSize = Val(ComboBoxEntries.Text)
            Offset = (CurrentPage - 1) * PageSize
            FormatRowHeader(DataGridDosen, Offset, PageSize)
            AktifDataGridDosen()

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat pencarian data: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        Call KoneksiDB()

        If BtnTambah.Text = "Aktifkan From" Then
            Call FormAktif()
            CurrentPage = 1
            ClearPagingCache()
            RefreshPaging()
            TampilkanDataGridDosen()
            DataGridDosen.Enabled = True
            Exit Sub
        End If

        If CbNamaJurusan.SelectedIndex = -1 OrElse CbNamaJurusan.SelectedValue Is Nothing Then
            MsgBox("Silahkan Pilih Nama Prodi Terlebih Dahulu", vbCritical, "Peringatan")
            CbNamaJurusan.Focus()
            Exit Sub
        End If

        Me.Enabled = False

        With FrmInputDataDosen
            'Mengisi Data Yang Di Input
            .Lbkdprodi.Text = CbNamaJurusan.SelectedValue.ToString()
            .CbJurusan.Text = CbNamaJurusan.Text
            .BuatKodeDosenOtomatis()

            'Setting Tombol 
            .BtnSimpan.Text = "Simpan"
            .BtnSimpan.Enabled = True
            .BtnSimpan.BackColor = Color.CornflowerBlue

            .BtnHapus.Enabled = False
            .BtnHapus.BackColor = Color.Red

            .BtnKeluar.Text = "Batal"
            .BtnKeluar.BackColor = Color.CornflowerBlue

            ' 3. Buka Form
            Me.Enabled = False
            .Show()
        End With
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        If BtnKeluar.Text = "Batal" Then
            Me.Close()
            Exit Sub
        End If

        If BtnKeluar.Text = "Keluar" Then
            Pesan = MsgBox("Anda yakin mau exit dari Form Data Dosen?", vbQuestion + vbYesNo, "Informasi")
            If Pesan = vbYes Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub TxtCariNama_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtCariNama.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True 'untuk menghilangkan bunyi beep 
            BtnCari.PerformClick()
        End If
    End Sub

    Private Sub BindingNavigatorMoveFirstItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click
        CurrentPage = 1
        BindingNavigatorPositionItem.Text = CurrentPage
        UpdateNavigatorState()
        TampilkanDataGridDosen()
    End Sub

    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click
        If CurrentPage > 1 Then
            CurrentPage -= 1
            BindingNavigatorPositionItem.Text = CurrentPage
            UpdateNavigatorState()
            TampilkanDataGridDosen()
        End If
    End Sub

    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click
        If CurrentPage < TotalPage Then
            CurrentPage += 1
            BindingNavigatorPositionItem.Text = CurrentPage
            UpdateNavigatorState()
            TampilkanDataGridDosen()
            UpdateNavigatorState()
        End If
    End Sub

    Private Sub BindingNavigatorMoveLastItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveLastItem.Click
        CurrentPage = TotalPage
        BindingNavigatorPositionItem.Text = CurrentPage
        UpdateNavigatorState()
        TampilkanDataGridDosen()
    End Sub

    Private Sub DataGridDosen_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridDosen.CellMouseDoubleClick
        Try
            If e.RowIndex < 0 Or DataGridDosen.Rows.Count = 0 Then Exit Sub

            Me.Enabled = False

            ' Ambil Kode Dosen dari Grid
            Dim KdDosen As String = DataGridDosen.Rows(e.RowIndex).Cells("Kd_Dosen").Value.ToString()

            Call KoneksiDB()
            ' Query kita tambahin d.Kd_Prodi biar bisa ditampilin di form biodata
            QUERY = "SELECT d.*, p.Nm_Prodi FROM tbl_dosen d INNER JOIN tbl_prodi p ON d.Kd_Prodi = p.Kd_Prodi " &
                "WHERE d.Kd_Dosen = @KdDosen"

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            CMD.Parameters.AddWithValue("@KdDosen", KdDosen)
            DR = CMD.ExecuteReader

            If DR.Read() Then
                With FrmInputDataDosen
                    ' --- PENGATURAN TOMBOL PAS MEREKA AKTIF (MODE UBAH) ---

                    ' 1. Tombol UBAH (Warna Biru)
                    .BtnSimpan.Text = "Ubah"
                    .BtnSimpan.Enabled = True
                    .BtnSimpan.BackColor = Color.CornflowerBlue

                    .BtnHapus.Text = "Hapus"
                    .BtnHapus.Enabled = True
                    .BtnHapus.BackColor = Color.CornflowerBlue


                    .BtnKeluar.Text = "Batal"
                    .BtnKeluar.Enabled = True
                    .BtnKeluar.BackColor = Color.CornflowerBlue

                    ' --- PENGISIAN DATA KE FORM BIODATA ---
                    ' Pastiin nama Label buat Kode Prodi sudah bener (misal LbProdi)
                    .LbKdDosen.Text = DR("Kd_Prodi").ToString()
                    .CbJurusan.Text = DR("Nm_Prodi").ToString()

                    ' Sisanya sama kayak kodingan Lu sebelumnya
                    .LbKdDosen.Text = DR("Kd_Dosen").ToString()
                    .TxtNidn.Text = DR("Nidn_Dosen").ToString()
                    .TxtNama.Text = DR("Nm_Dosen").ToString()
                    .CbJenisKelamin.Text = DR("Jk_Dosen").ToString().Trim().ToUpper()
                    .TxtNoHp.Text = If(IsDBNull(DR("NoHp_Dosen")), "", DR("NoHp_Dosen").ToString())
                    .TxtEmail.Text = If(IsDBNull(DR("Email_Dosen")), "", DR("Email_Dosen").ToString())

                    .Show()
                End With
            Else
                MsgBox("Data Dosen tidak ditemukan!", vbExclamation)
                Me.Enabled = True
            End If
            DR.Close()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
            Me.Enabled = True
        End Try
    End Sub

    ' Tambahkan ini di FrmDataDosen agar bisa dipanggil dari FrmBiodataDosen
    Public Sub SinkronkanProdiDanRefresh(ByVal namaProdiBaru As String)
        ' 1. Ubah text ComboBox ke Prodi yang baru
        ' Ini akan memicu event SelectedIndexChanged secara otomatis
        CbNamaJurusan.Text = namaProdiBaru

        CurrentPage = 1
        ClearPagingCache()
        RefreshPaging()
        TampilkanDataGridDosen()

        ' 3. Aktifkan kembali form utama
        Me.Enabled = True
    End Sub

End Class
