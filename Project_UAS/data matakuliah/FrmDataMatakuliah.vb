Imports MySql.Data.MySqlClient

Public Class FrmDataMatakuliah


    ''' <summary> 
    ''' Mmbuat variabel untuk pagging (TAMBAHAN DARI DOSEN)
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

    Private isloading As Boolean = True

    ' --- SUB PAGING (CACHE KEY DITAMBAH STATUS SEMESTER) ---
    Function GEtCacheKey(Page As Integer) As String
        Dim Prodi As String = If(CbNamaJurusan.SelectedValue Is Nothing, "", CbNamaJurusan.SelectedValue.ToString())
        Dim NamaMatkul As String = TxtCari.Text.Trim()
        Dim StatusSem As String = CbNamaSemester.Text ' Biar cache gak ketuker ganjil/genap
        Dim Limit As String = ComboBoxEntries.Text
        Return $"{Prodi}|{NamaMatkul}|{StatusSem}|{Limit}|PAGE:{Page}"
    End Function

    Sub ClearPagingCache()
        PageCache.Clear()
        LastCacheKey = ""
    End Sub

    Sub NumberEntriesHalaman()
        ComboBoxEntries.Items.Clear()
        ComboBoxEntries.Items.AddRange({"10", "15", "20", "25", "50", "100"})
        ComboBoxEntries.SelectedIndex = 0
    End Sub

    Sub RefreshPaging()
        CurrentPage = 1
        PageCache.Clear()
        HitungTotalData()
        TampilkanDataGridMatakuliah()
    End Sub

    Private Sub FormatRowHeader(grid As DataGridView, startIndex As Integer, PageSize As Integer)
        grid.SuspendLayout()
        grid.AllowUserToAddRows = False
        grid.RowHeadersVisible = True
        grid.RowHeadersWidth = 58
        grid.RowHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold)

        For i As Integer = 0 To grid.Rows.Count - 1
            grid.Rows(i).HeaderCell.Value = (startIndex + i + 1).ToString()
        Next
        grid.ResumeLayout()
    End Sub

    Sub UpdateNavigatorState()
        BindingNavigatorMoveFirstItem.Enabled = (CurrentPage > 1)
        BindingNavigatorMovePreviousItem.Enabled = (CurrentPage > 1)
        BindingNavigatorMoveNextItem.Enabled = (CurrentPage < TotalPage)
        BindingNavigatorMoveLastItem.Enabled = (CurrentPage < TotalPage)

        BindingNavigatorPositionItem.Text = CurrentPage.ToString()
        BindingNavigatorCountItem.Text = $"of {TotalPage}"
    End Sub

    Sub HitungTotalData()
        Try
            If CbNamaJurusan.SelectedIndex = -1 Then
                TotalData = 0
                Exit Sub
            End If

            Call KoneksiDB()
            ' Kita gunakan filter LIKE pada Kd_Matakuliah sebagai pengganti Kd_Prodi
            QUERY = "SELECT COUNT(*) FROM tbl_matakuliah WHERE 1=1 "

            ' --- LOGIKA FILTER BERDASARKAN TEKS JURUSAN ---
            ' Kita ambil inisial dari nama jurusan untuk dicocokkan ke Kode Matkul
            Dim inisial As String = ""
            If CbNamaJurusan.Text.Contains("TEKNOLOGI REKAYASA PERANGKAT LUNAK") Then
                inisial = "TRPL%"
            ElseIf CbNamaJurusan.Text.Contains("MANUFACTUR") Then
                inisial = "MKK%"
            ElseIf CbNamaJurusan.Text.Contains("LISTRIK") Then
                inisial = "TL%"
            ElseIf CbNamaJurusan.Text.Contains("PARMASI") Then
                inisial = "PM%"
            ElseIf CbNamaJurusan.Text.Contains("MEKATRONIKA") Then
                inisial = "RM%"
            End If

            If inisial <> "" Then
                QUERY &= " AND Kd_Matakuliah LIKE @inisial "
            End If

            ' Filter Ganjil/Genap
            If CbNamaSemester.Text = "GANJIL" Then
                QUERY &= " AND (Semester_Matakuliah % 2 <> 0) "
            ElseIf CbNamaSemester.Text = "GENAP" Then
                QUERY &= " AND (Semester_Matakuliah % 2 = 0) "
            End If

            ' Filter Cari Nama
            If TxtCari.Text <> "" Then QUERY &= " AND Nm_Matakuliah LIKE @Nama "
            ' Filter Angka Semester
            If CmbTahun.Text <> "" Then QUERY &= " AND Semester_Matakuliah = @Semester "

            Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                If inisial <> "" Then CMD.Parameters.AddWithValue("@inisial", inisial)
                If TxtCari.Text <> "" Then CMD.Parameters.AddWithValue("@Nama", "%" & TxtCari.Text & "%")
                If CmbTahun.Text <> "" Then CMD.Parameters.AddWithValue("@Semester", CmbTahun.Text)

                TotalData = CInt(CMD.ExecuteScalar())
            End Using

            ' Logika Paging
            If TotalData > 0 Then
                TotalPage = Math.Ceiling(TotalData / PageSize)
            Else
                TotalPage = 0
            End If

            lbTotalBaris.Text = "Total Seluruh Data: " & TotalData
            LbHasilBagiHalaman.Text = "Jumlah Halaman: " & TotalPage

        Catch ex As Exception
            MsgBox("Gagal hitung data: " & ex.Message)
        End Try
    End Sub

    ' --- SUB FORM CONTROL ---
    Sub FormNonAktif()
        ComboBoxEntries.Enabled = False
        CbNamaJurusan.Enabled = False
        CbNamaSemester.Enabled = False
        CmbTahun.Enabled = False
        TxtCari.Enabled = False
        BtnCari.Enabled = False
        DataGridMatakuliah.Enabled = False
        BtnTambah.Text = "&AKTIFKAN FORM"
    End Sub

    Sub FormAktif()
        ComboBoxEntries.Enabled = True
        CbNamaJurusan.Enabled = True
        CbNamaSemester.Enabled = True
        CmbTahun.Enabled = True
        TxtCari.Enabled = True
        BtnCari.Enabled = True
        BtnTambah.Text = "&TAMBAH DATA"
        BtnTambah.BackColor = Color.White
        DataGridMatakuliah.Enabled = True
        DataGridMatakuliah.ReadOnly = True
    End Sub

    Sub KosongkanData()
        CbNamaJurusan.SelectedIndex = -1
        CbNamaSemester.SelectedIndex = -1
        CmbTahun.Items.Clear()
        TxtCari.Text = ""
    End Sub

    Sub AktifDataGridMatakuliah()
        With DataGridMatakuliah
            .EnableHeadersVisualStyles = False

            .Font = New Font(DataGridMatakuliah.Font, FontStyle.Bold)
            DataGridMatakuliah.DefaultCellStyle.Font = New Font("Arial", 9)
            DataGridMatakuliah.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            DataGridMatakuliah.ColumnHeadersHeight = 35

            .Columns(0).Width = 120
            .Columns(0).HeaderText = "KODE"
            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(1).Width = 340
            .Columns(1).HeaderText = "NAMA MATAKULIAH"
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(2).Width = 60
            .Columns(2).HeaderText = "SKS"
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(3).Width = 60
            .Columns(3).HeaderText = "SKS TEORI"
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(4).Width = 70
            .Columns(4).HeaderText = "SKS PRAKTEK"
            .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(5).Width = 80
            .Columns(5).HeaderText = "SEMESTER"
            .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            .Columns(6).Width = 120
            .Columns(6).HeaderText = "NAMA SEMESTER"
            .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

        End With
    End Sub

    ' --- TAMPIL DATA ---

    ' --- TAMPIL DATA (FIX JUMLAH BARIS ENTRI) ---
    Sub TampilkanDataGridMatakuliah()
        Try
            ' 1. Logika Inisial
            Dim inisial As String = ""
            If CbNamaJurusan.Text.Contains("REKAYASA PERANGKAT LUNAK") Then
                inisial = "TRPL%"
            ElseIf CbNamaJurusan.Text.Contains("MANUFACTUR") Then
                inisial = "MKK%"
            ElseIf CbNamaJurusan.Text.Contains("LISTRIK") Then
                inisial = "TL%"
            ElseIf CbNamaJurusan.Text.Contains("PARMASI") Then
                inisial = "PM%"
            ElseIf CbNamaJurusan.Text.Contains("MEKATRONIKA") Then
                inisial = "RM%"
            End If

            Call KoneksiDB()
            ' Perhitungan Offset untuk Paging
            Offset = (CurrentPage - 1) * PageSize

            Dim sql As String = "SELECT Kd_Matakuliah, Nm_Matakuliah, Sks_Matakuliah, " &
                             "Teori_Matakuliah, Praktek_Matakuliah, Semester_Matakuliah, " &
                             "IF(Semester_Matakuliah % 2 = 0, 'GENAP', 'GANJIL') as Nama_Semester " &
                             "FROM tbl_matakuliah WHERE 1=1 "

            If inisial <> "" Then sql &= " AND Kd_Matakuliah LIKE @inisial "

            ' Filter Ganjil/Genap
            If CbNamaSemester.Text = "GANJIL" Then
                sql &= " AND (Semester_Matakuliah % 2 <> 0) "
            ElseIf CbNamaSemester.Text = "GENAP" Then
                sql &= " AND (Semester_Matakuliah % 2 = 0) "
            End If

            ' Filter Nama & Semester Angka
            If TxtCari.Text <> "" Then sql &= " AND Nm_Matakuliah LIKE @Nama "
            If CmbTahun.Text <> "" Then sql &= " AND Semester_Matakuliah = @Semester "

            sql &= " ORDER BY Kd_Matakuliah ASC LIMIT @Offset, @Limit"

            Using CMD As New MySqlCommand(sql, DbKoneksi)
                ' --- BAGIAN KRUSIAL: DAFTAR PARAMETER ---
                If inisial <> "" Then CMD.Parameters.AddWithValue("@inisial", inisial)
                If TxtCari.Text <> "" Then CMD.Parameters.AddWithValue("@Nama", "%" & TxtCari.Text & "%")
                If CmbTahun.Text <> "" Then CMD.Parameters.AddWithValue("@Semester", CmbTahun.Text)

                CMD.Parameters.AddWithValue("@Offset", Offset)
                CMD.Parameters.AddWithValue("@Limit", PageSize)

                Dim DT As New DataTable
                Using DA As New MySqlDataAdapter(CMD)
                    DA.Fill(DT)
                End Using

                DataGridMatakuliah.DataSource = DT

                If DT.Rows.Count > 0 Then
                    Call AktifDataGridMatakuliah()
                    Call FormatRowHeader(DataGridMatakuliah, Offset, PageSize)
                End If

                LbJumlahBaris.Text = "Jumlah Baris Entri: " & DT.Rows.Count
                Call UpdateNavigatorState()
            End Using

        Catch ex As Exception
            MsgBox("Gagal tampil: " & ex.Message)
        End Try
    End Sub

    ' --- COMBOBOX DATA ---
    Sub TampilkanFilterNamaProdi()
        Try
            Call KoneksiDB()
            QUERY = "SELECT Kd_Prodi, Nm_Prodi FROM tbl_prodi ORDER BY Kd_Prodi"
            DA = New MySqlDataAdapter(QUERY, DbKoneksi)
            Dim DT As New DataTable
            DA.Fill(DT)

            ' Lepas Handler dulu biar gak error pas loading data ke combo
            RemoveHandler CbNamaJurusan.SelectedIndexChanged, AddressOf CbNamaJurusan_SelectedIndexChanged

            CbNamaJurusan.DataSource = DT
            CbNamaJurusan.DisplayMember = "Nm_Prodi"
            CbNamaJurusan.ValueMember = "Kd_Prodi"
            CbNamaJurusan.SelectedIndex = -1

            ' Pasang lagi Handlernya biar pas dipilih user baru jalan validasinya
            AddHandler CbNamaJurusan.SelectedIndexChanged, AddressOf CbNamaJurusan_SelectedIndexChanged
        Catch ex As Exception
            ' MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CbNamaJurusan_SelectedIndexChanged(sender As Object, e As EventArgs)
        If isloading Or CbNamaJurusan.SelectedIndex = -1 Then Exit Sub

        TxtCari.Clear()
        Call HitungTotalData()

        ' VALIDASI SESUAI GAMBAR 13
        If TotalData = 0 Then
            Dim Pesan As String = "Data Matakuliah pada Jurusan : " & CbNamaJurusan.Text.ToUpper() & " Belum Ada!!" & vbCrLf &
                                 "Untuk Menambahkan Status Semester (" & CbNamaSemester.Text & "), Silahkan Klik Tambah Data!!"

            MsgBox(Pesan, vbCritical, "PERINGATAN")

            ' Bersihin Grid & Label
            DataGridMatakuliah.DataSource = Nothing
            LbJumlahBaris.Text = "Jumlah Baris Entri: 0"
            UpdateNavigatorState()
        Else
            RefreshPaging()
        End If
    End Sub

    Sub IsiStatusSemester()
        CbNamaSemester.Items.Clear()
        CbNamaSemester.Items.AddRange({"GANJIL", "GENAP"})
        CbNamaSemester.SelectedIndex = -1
    End Sub

    Sub IsiAngkaSemesterByStatus()
        CmbTahun.Items.Clear()
        If CbNamaSemester.Text = "GANJIL" Then
            CmbTahun.Items.AddRange({"1", "3", "5", "7"})
        ElseIf CbNamaSemester.Text = "GENAP" Then
            CmbTahun.Items.AddRange({"2", "4", "6", "8"})
        End If
        CmbTahun.SelectedIndex = -1
    End Sub

    ' --- EVENT HANDLERS ---
    Private Sub FrmDataMatakuliah_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isloading = True
        Call KoneksiDB()
        Call FormNonAktif()
        Call NumberEntriesHalaman()
        Call TampilkanFilterNamaProdi()
        Call IsiStatusSemester()
        RefreshPaging()
        Me.AcceptButton = BtnCari
        BtnKeluar.BackColor = Color.CornflowerBlue
        isloading = False
    End Sub

    Private Sub CbNamaSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaSemester.SelectedIndexChanged
        If isloading Then Exit Sub
        Call IsiAngkaSemesterByStatus()
        RefreshPaging()
    End Sub

    Private Sub CmbTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbTahun.SelectedIndexChanged
        If isloading Then Exit Sub
        RefreshPaging()
    End Sub

    ' Navigator
    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click
        If CurrentPage < TotalPage Then CurrentPage += 1 : TampilkanDataGridMatakuliah()
    End Sub
    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click
        If CurrentPage > 1 Then CurrentPage -= 1 : TampilkanDataGridMatakuliah()
    End Sub
    Private Sub BindingNavigatorMoveFirstItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click
        CurrentPage = 1 : TampilkanDataGridMatakuliah()
    End Sub
    Private Sub BindingNavigatorMoveLastItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveLastItem.Click
        CurrentPage = TotalPage : TampilkanDataGridMatakuliah()
    End Sub

    Private Sub ComboBoxEntries_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEntries.SelectedIndexChanged
        If isloading Then Exit Sub
        RefreshPaging()
    End Sub

    ' CODE INI DI FRMDATAMATKUL.VB
    ' CODE INI DI FRMDATAMATKUL.VB
    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        ' 1. CEK DULU APAKAH TOMBOL MASIH BERSTATUS AKTIFKAN
        If BtnTambah.Text = "&AKTIFKAN FORM" Then
            Call FormAktif() ' Jalankan fungsi aktivasi form lu
            Exit Sub ' Berhenti di sini, jangan lanjut ke validasi jurusan dulu
        End If

        ' 2. JIKA SUDAH AKTIF (Teks tombol biasanya berubah jadi &TAMBAH DATA atau sejenisnya)
        ' BARU KITA CEK VALIDASI JURUSANNYA
        If CbNamaJurusan.Text = "" Or CbNamaJurusan.SelectedIndex = -1 Then
            MsgBox("Isi Terlebih Dahulu Jurusannya ", vbCritical + vbOKOnly, "Validasi Input")
            CbNamaJurusan.Focus()
            Exit Sub
        End If

        ' 3. JIKA VALIDASI JURUSAN LOLOS, LANJUT BUKA FORM INPUT
        FrmInputDataMatakuliah.Show()
        Me.Enabled = False

        With FrmInputDataMatakuliah
            .FormNormal()

            ' Otomatis isi Nama Jurusan ke form input
            .CbJurusan.Items.Clear()
            .CbJurusan.Items.Add(Me.CbNamaJurusan.Text)
            .CbJurusan.SelectedIndex = 0

            ' Otomatis isi Nama Semester (Ganjil/Genap)
            If Me.CbNamaSemester.Text <> "" Then
                .CbNamaSemester.Text = Me.CbNamaSemester.Text
                ' Pastikan sub ini sudah PUBLIC di FrmMatakuliah
                .IsiAngkaSemesterByStatus()
            End If

            ' Otomatis isi Angka Semester (1, 2, dst)
            If Me.CmbTahun.Text <> "" Then
                .CmbSemester.Text = Me.CmbTahun.Text
            End If

            .TxtKodeMatkul.Focus()
        End With
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        If MsgBox("Yakin keluar bray?", vbQuestion + vbYesNo) = vbYes Then Me.Close()
    End Sub

    ' --- EVENT CARI DATA ---
    Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles BtnCari.Click
        ' 1. Cek dulu, input cari kosong kagak?
        If TxtCari.Text.Trim() = "" Then
            MsgBox("Isi dulu nama matakuliah yang mau dicari,", vbExclamation, "Warning")
            TxtCari.Focus()
            Exit Sub
        End If

        ' 2. Hitung ulang total data berdasarkan keyword pencarian
        Call HitungTotalData()

        ' 3. VALIDASI: Kalau datanya kagak ada di DB
        If TotalData = 0 Then
            MsgBox("DATA '" & TxtCari.Text & "' TIDAK ADA DISINI .", vbInformation, "IMFOMASI")
            TxtCari.Clear()
            TxtCari.Focus()

            ' Refresh balik biar grid nampilin semua data lagi
            RefreshPaging()
            Exit Sub
        End If

        ' 4. Kalau ada, tampilkan!
        CurrentPage = 1
        Call TampilkanDataGridMatakuliah()
    End Sub

    Private Sub TxtCari_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtCari.KeyDown
        If e.KeyCode = Keys.Enter Then
            BtnCari_Click(Nothing, Nothing)
            e.SuppressKeyPress = True
        End If
    End Sub

    ' CARI BAGIAN INI DI FrmDataMatakuliah
    Private Sub DataGridMatakuliah_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridMatakuliah.CellMouseDoubleClick
        Try
            If e.RowIndex < 0 Then Exit Sub
            Dim KdMatkul As String = DataGridMatakuliah.Rows(e.RowIndex).Cells(0).Value.ToString()
            Dim StatusSmt As String = DataGridMatakuliah.Rows(e.RowIndex).Cells(6).Value.ToString()

            ' JANGAN panggil .Show() atau .ShowDialog() di sini dulu!

            With FrmInputDataMatakuliah
                .BtnSimpan.Text = "&UBAH"
                .BtnSimpan.BackColor = Color.CornflowerBlue
                .BtnSimpan.ForeColor = Color.White

                ' BARIS KUNCI: Sinkronisasi Jurusan
                .CbJurusan.Items.Clear()
                .CbJurusan.Items.Add(Me.CbNamaJurusan.Text)
                .CbJurusan.SelectedIndex = 0

                ' Ambil detail dari DB
                Call KoneksiDB()
                Using CMD_DETAIL As New MySqlCommand("SELECT * FROM tbl_matakuliah WHERE Kd_Matakuliah = @kd", DbKoneksi)
                    CMD_DETAIL.Parameters.AddWithValue("@kd", KdMatkul)
                    Using DR_DETAIL = CMD_DETAIL.ExecuteReader
                        If DR_DETAIL.Read() Then
                            .TxtKodeMatkul.Text = DR_DETAIL("Kd_Matakuliah").ToString()
                            .TxtNamaMatkul.Text = DR_DETAIL("Nm_Matakuliah").ToString()
                            .CmbSks.Text = DR_DETAIL("Sks_Matakuliah").ToString()
                            .TxtTeoriMatkul.Text = DR_DETAIL("Teori_Matakuliah").ToString()
                            .TxtPraktekMatkul.Text = DR_DETAIL("Praktek_Matakuliah").ToString()

                            ' Kunci kode biar gak bisa diubah
                            .TxtKodeMatkul.Enabled = False

                            .CbNamaSemester.Text = StatusSmt
                            .IsiAngkaSemesterByStatus()
                            .CmbSemester.Text = DR_DETAIL("Semester_Matakuliah").ToString()
                        End If
                    End Using
                End Using

                ' Pengaturan tombol lainnya
                .BtnHapus.Enabled = True
                .BtnHapus.BackColor = Color.CornflowerBlue
                .BtnKeluar.Text = "&BATAL"

                ' --- PINDAHKAN KE SINI ---
                Me.Enabled = False
                .ShowDialog()
                ' -------------------------
            End With

        Catch ex As Exception
            MsgBox("Error Double Click: " & ex.Message)
            Me.Enabled = True
        End Try
    End Sub
End Class
