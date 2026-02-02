Imports MySql.Data.MySqlClient

Public Class FrmDataPenjadMatkul

    ' --- VARIABEL STATE ---
    Dim CurrentPage As Integer = 1
        Dim PageSize As Integer = 10
        Dim TotalData As Integer = 0
        Dim TotalPage As Integer = 0
        Private isloading As Boolean = True

        Sub RefreshData()
            HitungTotalData()
            CurrentPage = 1
            TampilkanDataGridJadwal()
        End Sub

        Private Sub ComboBoxEntries_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEntries.SelectedIndexChanged
            If Not isloading Then
                PageSize = Val(ComboBoxEntries.Text)
                RefreshData() ' Reset ke hal 1 tiap kali ganti jumlah entri
            End If
        End Sub

        ' Event buat gambar nomor urut otomatis bre
        Private Sub DataGridPenjadwalanMatkul_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridPenjadwalanMatkul.RowPostPaint
            Dim rowNumber As Integer = ((CurrentPage - 1) * PageSize) + (e.RowIndex + 1)
            Dim rowIdx As String = rowNumber.ToString()

            Dim centerFormat As New StringFormat() With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center
        }

            ' Ukuran kotak untuk nomor di header kiri
            Dim headerBounds As New Rectangle(e.RowBounds.Left, e.RowBounds.Top, DataGridpenjadwalanMatkul.RowHeadersWidth, e.RowBounds.Height)

            ' Gambar nomornya
            e.Graphics.DrawString(rowIdx, Me.Font, SystemBrushes.ControlText, headerBounds, centerFormat)
        End Sub

        ' --- ISI DATA COMBO ---
        Sub TampilkanJurusan()
            Call KoneksiDB()
            Dim dt As New DataTable
            QUERY = "SELECT Kd_Prodi, Nm_Prodi FROM tbl_prodi "
            Dim da As New MySqlDataAdapter(QUERY, DbKoneksi)
            da.Fill(dt)

            CbNamaJurusan.DataSource = dt
            CbNamaJurusan.DisplayMember = "Nm_Prodi"
            CbNamaJurusan.ValueMember = "Kd_Prodi"

            CbNamaJurusan.SelectedIndex = -1
            Lbkdprodi.Text = ""
        End Sub

        Sub IsiComboSemesterPilihan()
        CbNamaSemester.Items.Clear()
        CbNamaSemester.Items.AddRange({"ALL", "GANJIL", "GENAP"})
        CbNamaSemester.SelectedIndex = 0
    End Sub

    Sub AktifDataGridJadwal()
        With DataGridpenjadwalanMatkul
            .AllowUserToAddRows = False
            .EnableHeadersVisualStyles = False

            .Font = New Font(DataGridpenjadwalanMatkul.Font, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Arial", 9)
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            .ColumnHeadersHeight = 35

            ' 0. KODE PENGAMPU
            .Columns(0).Width = 110
            .Columns(0).HeaderText = "KODE PENGAMPU"
            .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 1. TAHUN AKADEMIK
            .Columns(1).Width = 100
            .Columns(1).HeaderText = "TAHUN AKADEMIK"
            .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 2. HARI
            .Columns(2).Width = 80
            .Columns(2).HeaderText = "HARI"
            .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 3. NAMA SEMESTER (GANJIL/GENAP)
            .Columns(3).Width = 100
            .Columns(3).HeaderText = "NAMA SEMESTER"
            .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 4. JAM AWAL
            .Columns(4).Width = 70
            .Columns(4).HeaderText = "JAM AWAL"
            .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 5. JAM AKHIR
            .Columns(5).Width = 70
            .Columns(5).HeaderText = "JAM AKHIR"
            .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 6. NAMA DOSEN
            .Columns(6).Width = 180
            .Columns(6).HeaderText = "NAMA DOSEN"
            .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 7. SMT (ANGKA)
            .Columns(7).Width = 50
            .Columns(7).HeaderText = "SMT"
            .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 8. KODE MATAKULIAH
            .Columns(8).Width = 90
            .Columns(8).HeaderText = "KODE"
            .Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 9. NAMA MATAKULIAH
            .Columns(9).Width = 220
            .Columns(9).HeaderText = "NAMA MATAKULIAH"
            .Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 10. SKS
            .Columns(10).Width = 45
            .Columns(10).HeaderText = "SKS"
            .Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 11. TEORI
            .Columns(11).Width = 50
            .Columns(11).HeaderText = "TEORI"
            .Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 12. PRAKTIK
            .Columns(12).Width = 60
            .Columns(12).HeaderText = "PRAKTIK"
            .Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' 13. NAMA RUANG
            .Columns(13).Width = 120
            .Columns(13).HeaderText = "NAMA RUANG"
            .Columns(13).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter



        End With
    End Sub

    Sub TampilkanTahunAkademik()
        Call KoneksiDB()
        Dim dt As New DataTable
        QUERY = "SELECT DISTINCT Tahun_Akademik FROM tbl_pengampu_matakuliah ORDER BY Tahun_Akademik DESC"
        Dim da As New MySqlDataAdapter(QUERY, DbKoneksi)
        da.Fill(dt)

        ' Tambah opsi ALL manual
        Dim row As DataRow = dt.NewRow()
        row("Tahun_Akademik") = "ALL"
        dt.Rows.InsertAt(row, 0)

        CbTahunAkademik.DataSource = dt
        CbTahunAkademik.DisplayMember = "Tahun_Akademik"
        CbTahunAkademik.ValueMember = "Tahun_Akademik"
        CbTahunAkademik.SelectedIndex = 0
    End Sub

    ' --- LOGIKA UTAMA: PAS JURUSAN DIPILIH ---
    Private Sub CbNamaJurusan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaJurusan.SelectedIndexChanged
        If isloading Or CbNamaJurusan.SelectedIndex = -1 Then Exit Sub

        'Mengambil Kode Prodi 2 Huruf Dari Kanan
        If CbNamaJurusan.SelectedValue IsNot Nothing Then
            Dim kodeFull As String = CbNamaJurusan.SelectedValue.ToString()
            Lbkdprodi.Text = Microsoft.VisualBasic.Right(kodeFull, 2)
        End If
        ' ----------------------------------------

        HitungTotalData()
        If TotalData = 0 Then
            DataGridpenjadwalanMatkul.DataSource = Nothing
            MsgBox("Data Jadwal untuk Jurusan '" & CbNamaJurusan.Text & "' Belum Ada!", vbInformation, "Informasi")
        Else
            CurrentPage = 1
            TampilkanDataGridJadwal()
        End If
    End Sub

    'SUB HITUNG TOTAL DATA
    Sub HitungTotalData()
        Try
            Call KoneksiDB()
            ' Kita pake LIKE biar fleksibel, tapi tetep menyaring baris
            Dim cari As String = "%" & TxtCariNama.Text & "%"

            QUERY = "SELECT COUNT(*) FROM tbl_pengampu_matakuliah dp " &
        "JOIN tbl_dosen d ON dp.Kd_Dosen = d.Kd_Dosen " &
        "INNER JOIN tbl_jadwal_matkul j ON dp.KdPengampu = j.KdPengampu " &
        "LEFT JOIN tbl_hari h ON j.Id_Hari = h.Id_Hari " & ' <-- Tambahin ini
        "WHERE d.Kd_Prodi = @prodi AND d.Nm_Dosen LIKE @cari "

            ' Tambahin filter semester/tahun juga di sini biar sinkron bre
            If CbTahunAkademik.Text <> "ALL" Then QUERY &= " AND dp.Tahun_Akademik = @thn "

            Using cmd = New MySqlCommand(QUERY, DbKoneksi)
                cmd.Parameters.AddWithValue("@prodi", CbNamaJurusan.SelectedValue)
                cmd.Parameters.AddWithValue("@cari", cari)
                If CbTahunAkademik.Text <> "ALL" Then cmd.Parameters.AddWithValue("@thn", CbTahunAkademik.Text)

                TotalData = CInt(cmd.ExecuteScalar())
            End Using

            TotalPage = Math.Ceiling(TotalData / PageSize)
            If TotalPage = 0 Then TotalPage = 1
        Catch ex As Exception
            MsgBox("Error Hitung: " & ex.Message)
        End Try
    End Sub

    Sub TampilkanDataGridJadwal()
        Try
            Call KoneksiDB()
            Dim offset As Integer = (CurrentPage - 1) * PageSize
            Dim cari As String = "%" & TxtCariNama.Text & "%"

            QUERY = "SELECT dp.KdPengampu, dp.Tahun_Akademik, " &
                    "COALESCE(h.Nm_Hari, j.Id_Hari) as Nm_Hari, " & ' <-- Biar SENIN manual tetep muncul
                    "IF(m.Semester_Matakuliah % 2 = 0, 'GENAP', 'GANJIL') as NAMA_SEMESTER, " &
                    "j.JamAwal, j.JamAkhir, d.Nm_Dosen, m.Semester_Matakuliah, m.Kd_Matakuliah, m.Nm_Matakuliah, " &
                    "m.Sks_Matakuliah, m.Teori_Matakuliah, m.Praktek_Matakuliah, " &
                    "r.Nm_Ruangan, 'SUDAH TERJADWAL' as STATUS " &
                    "FROM tbl_pengampu_matakuliah dp " &
                    "JOIN tbl_dosen d ON dp.Kd_Dosen = d.Kd_Dosen " &
                    "JOIN tbl_matakuliah m ON dp.Kd_Matakuliah = m.Kd_Matakuliah " &
                    "INNER JOIN tbl_jadwal_matkul j ON dp.KdPengampu = j.KdPengampu " &
                    "LEFT JOIN tbl_hari h ON j.Id_Hari = h.Id_Hari " & ' <-- Pake LEFT JOIN
                    "INNER JOIN tbl_ruangkelas r ON j.Kd_Ruangan = r.Kd_Ruangan " &
                    "WHERE d.Kd_Prodi = @prodi AND d.Nm_Dosen LIKE @cari "

            ' Filter tambahan (pake spasi di awal kata AND)
            If CbTahunAkademik.Text <> "ALL" Then
                QUERY &= " AND dp.Tahun_Akademik = @thn "
            End If

            If CbNamaSemester.Text = "GANJIL" Then
                QUERY &= " AND m.Semester_Matakuliah IN (1,3,5,7) "
            ElseIf CbNamaSemester.Text = "GENAP" Then
                QUERY &= " AND m.Semester_Matakuliah IN (2,4,6,8) "
            End If

            QUERY &= " LIMIT @off, @lim"

            Dim dt As New DataTable
            Using da = New MySqlDataAdapter(QUERY, DbKoneksi)
                da.SelectCommand.Parameters.AddWithValue("@prodi", CbNamaJurusan.SelectedValue)
                da.SelectCommand.Parameters.AddWithValue("@cari", cari)
                da.SelectCommand.Parameters.AddWithValue("@thn", CbTahunAkademik.Text)
                da.SelectCommand.Parameters.AddWithValue("@off", offset)
                da.SelectCommand.Parameters.AddWithValue("@lim", PageSize)
                da.Fill(dt)
            End Using

            DataGridpenjadwalanMatkul.DataSource = dt
            AktifDataGridJadwal()
            Call UpdateNavigatorState()

            DataGridpenjadwalanMatkul.Refresh()
            LbTotalBaris.Text = "Jumlah Baris Halaman: " & TotalData.ToString()
            ' Format: 1 Total Hal: 4
            LbHasilBagiHalaman.Text = " Total Hal: " & TotalPage.ToString()
            LbJumlahBaris.Text = "Jumlah Baris Entries: " & DataGridpenjadwalanMatkul.Rows.Count.ToString()
        Catch ex As Exception
            MsgBox("Gagal Tampilkan Data: " & ex.Message)
        End Try
    End Sub

    ' --- LOAD FORM ---
    Private Sub FrmDataJadwalMatkul_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isloading = True

        If Not SystemInformation.TerminalServerSession Then
            Dim dgvType As Type = DataGridpenjadwalanMatkul.GetType
            Dim pi As Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            pi.SetValue(DataGridpenjadwalanMatkul, True, Nothing)
        End If

        BtnTambah.BackColor = Color.CornflowerBlue
        btnKeluar.BackColor = Color.CornflowerBlue
        Btncari.BackColor = Color.CornflowerBlue

        Try
            ' Isi semua ComboBox
            TampilkanJurusan()
            IsiComboSemesterPilihan()
            TampilkanTahunAkademik()

            ' Set default entries paging
            ComboBoxEntries.Items.Clear()
            ComboBoxEntries.Items.AddRange({"10", "15", "25", "50", "100"})
            ComboBoxEntries.SelectedIndex = 0
            DataGridpenjadwalanMatkul.DataSource = Nothing ' Pastiin Grid kosong bre
            isloading = False
            ' Jangan panggil TampilkanData dulu, biar user pilih Jurusan
        Catch ex As Exception
            MsgBox("Gagal Load Form: " & ex.Message)
        End Try
    End Sub

    ' --- EVENT LAINNYA ---
    Private Sub CbNamaSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaSemester.SelectedIndexChanged
        If Not isloading Then RefreshData()
    End Sub

    Private Sub CbTahunAkademik_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbTahunAkademik.SelectedIndexChanged
        If Not isloading Then RefreshData()
    End Sub

    Private Sub TxtCariNama_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtCariNama.KeyDown
        ' Cek kalau yang ditekan tombol Enter
        If e.KeyCode = Keys.Enter Then
            ' Biar gak bunyi 'ding' pas tekan enter
            e.SuppressKeyPress = True

            ' Langsung eksekusi pencarian
            Btncari.PerformClick()
        End If
    End Sub

    Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles Btncari.Click
        If CbNamaJurusan.SelectedIndex < 0 Then
            MsgBox("Pilih Jurusan dulu bre biar carinya enak!", vbExclamation)
            Exit Sub
        End If

        Call RefreshData()
    End Sub

    Sub UpdateNavigatorState()
        ' Tombol First & Previous aktif kalau kita bukan di halaman 1
        BindingNavigatorMoveFirstItem.Enabled = (CurrentPage > 1)
        BindingNavigatorMovePreviousItem.Enabled = (CurrentPage > 1)

        ' Tombol Next & Last aktif kalau CurrentPage masih kurang dari TotalPage
        BindingNavigatorMoveNextItem.Enabled = (CurrentPage < TotalPage)
        BindingNavigatorMoveLastItem.Enabled = (CurrentPage < TotalPage)

        ' Update label halaman di Navigator
        BindingNavigatorPositionItem.Text = CurrentPage.ToString()
        BindingNavigatorCountItem.Text = "of " & TotalPage.ToString()
    End Sub

    ' --- EVENT HANDLERS NAVIGATOR ---

    ' Tombol Next
    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click
        If CurrentPage < TotalPage Then
            CurrentPage += 1
            TampilkanDataGridJadwal()
        End If
    End Sub

    ' Tombol Previous
    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click
        If CurrentPage > 1 Then
            CurrentPage -= 1
            TampilkanDataGridJadwal()
        End If
    End Sub

    ' Tombol First (Halaman 1)
    Private Sub BindingNavigatorMoveFirstItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click
        CurrentPage = 1
        TampilkanDataGridJadwal()
    End Sub

    ' Tombol Last (Halaman Terakhir)
    Private Sub BindingNavigatorMoveLastItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveLastItem.Click
        CurrentPage = TotalPage
        TampilkanDataGridJadwal()
    End Sub

    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        ' Cek apakah jurusan sudah dipilih
        If CbNamaJurusan.SelectedIndex = -1 Then
            MsgBox("Pilih Jurusan dulu bre!", vbExclamation)
            Exit Sub
        End If

        ' Kirim data ke Form Input (FrmPenjaMatkul)
        With FrmInputDataPenjadMatkul
            ' Set nilai awal berdasarkan pilihan di menu utama

            .btnKeluar.BackColor = Color.CornflowerBlue
            .btnSimpan.BackColor = Color.CornflowerBlue
            .btnHapus.BackColor = Color.Red
            .btnHapus.Enabled = False

            .CbNamaJurusan.Text = Me.CbNamaJurusan.Text
            .CmbSemester.Text = If(Me.CbNamaSemester.Text = "ALL", "", Me.CbNamaSemester.Text)
            .CmbTahunAkademik.Text = If(Me.CbTahunAkademik.Text = "ALL", "", Me.CbTahunAkademik.Text)

            ' Kunci filter agar sinkron dengan menu utama
            .CbNamaJurusan.Enabled = False

            ' Reset field inputan
            .TxtKodePengampu.Clear()
            .BersihkanKotakBesar() ' Pastikan sub ini ada di FrmPenjaMatkul

            ' Buka Form Input
            .ShowDialog()
        End With

        ' Setelah input selesai, refresh grid utama
        RefreshData()
        End Sub

        Private Sub DataGridPenjadwalanMatkul_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridPenjadwalanMatkul.CellMouseDoubleClick
            Try
                ' 1. Validasi baris: Jangan sampai klik Header
                If e.RowIndex < 0 Then Exit Sub
                Dim row As DataGridViewRow = DataGridpenjadwalanMatkul.Rows(e.RowIndex)

                ' Ambil Kode Pengampu buat kunci pencarian
                Dim kdKey As String = row.Cells(0).Value.ToString()

            With FrmInputDataPenjadMatkul
                ' 2. Pindahin data dasar dari Grid ke Form Input
                .TxtKodePengampu.Text = kdKey
                .CmbTahunAkademik.Text = row.Cells(1).Value.ToString()
                .CbNamaHari.Text = row.Cells(2).Value.ToString()
                .CmbSemester.Text = row.Cells(3).Value.ToString()
                .TxtJamAwal.Text = row.Cells(4).Value.ToString()
                .TxtJamAkhir.Text = row.Cells(5).Value.ToString()
                .CbNamaJurusan.Text = Me.CbNamaJurusan.Text

                ' --- MISS FIX: Pastikan Koneksi Terbuka & Bersih ---
                Call KoneksiDB()

                ' 3. Isi List Ruangan (Combobox dipopulasi ulang biar up-to-date)
                Dim dtRuang As New DataTable
                Using cmdR As New MySqlCommand("SELECT Nm_Ruangan FROM tbl_ruangkelas", DbKoneksi)
                    dtRuang.Load(cmdR.ExecuteReader)
                    .CbRuangKelas.DataSource = dtRuang
                    .CbRuangKelas.DisplayMember = "Nm_Ruangan"
                End Using

                ' 4. Isi List Hari jika kosong
                If .CbNamaHari.Items.Count = 0 Then
                    .CbNamaHari.Items.AddRange(New Object() {"SENIN", "SELASA", "RABU", "KAMIS", "JUMAT", "SABTU"})
                End If

                ' 5. TARIK DATA DETAIL DOSEN & MATKUL
                ' MISS FIX: Gunakan JOIN yang tepat agar data Lb (Label) terisi meski ada data null di jadwal
                QUERY = "SELECT d.Kd_Dosen, d.Nidn_Dosen, d.Nm_Dosen, " &
                    "m.Kd_Matakuliah, m.Nm_Matakuliah, m.Sks_Matakuliah, " &
                    "m.Teori_Matakuliah, m.Praktek_Matakuliah, m.Semester_Matakuliah, " &
                    "r.Nm_Ruangan " &
                    "FROM tbl_pengampu_matakuliah pm " &
                    "JOIN tbl_dosen d ON pm.Kd_Dosen = d.Kd_Dosen " &
                    "JOIN tbl_matakuliah m ON pm.Kd_Matakuliah = m.Kd_Matakuliah " &
                    "LEFT JOIN tbl_jadwal_matkul j ON pm.KdPengampu = j.KdPengampu " &
                    "LEFT JOIN tbl_ruangkelas r ON j.Kd_Ruangan = r.Kd_Ruangan " &
                    "WHERE pm.KdPengampu = @kd"

                ' MISS FIX: Re-open koneksi jika tertutup oleh dtRuang.Load tadi
                If DbKoneksi.State = ConnectionState.Closed Then DbKoneksi.Open()

                Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                    CMD.Parameters.AddWithValue("@kd", kdKey)
                    Using DR = CMD.ExecuteReader
                        If DR.Read() Then
                            .LbKdDosen.Text = DR("Kd_Dosen").ToString()
                            .LbNidnDosen.Text = DR("Nidn_Dosen").ToString()
                            .LbNamaDosen.Text = DR("Nm_Dosen").ToString()
                            .LbKodeMatkul.Text = DR("Kd_Matakuliah").ToString()
                            .LbNamaMatkul.Text = DR("Nm_Matakuliah").ToString()
                            .LbSKS.Text = DR("Sks_Matakuliah").ToString()
                            .LbSKSTeori.Text = DR("Teori_Matakuliah").ToString()
                            .LbSKSPraktek.Text = DR("Praktek_Matakuliah").ToString()
                            .LbSemester.Text = DR("Semester_Matakuliah").ToString()

                            ' Set Ruangan dari database detail
                            .CbRuangKelas.Text = DR("Nm_Ruangan").ToString()
                        End If
                    End Using ' DR otomatis tertutup di sini
                End Using


                .btnSimpan.Text = "UBAH"
                .btnSimpan.Enabled = True
                .btnSimpan.BackColor = Color.CornflowerBlue
                .btnHapus.Enabled = True
                .btnHapus.BackColor = Color.CornflowerBlue ' Tetep biru sesuai permintaan lo
                .btnKeluar.Text = "BATAL"
                .btnKeluar.BackColor = Color.CornflowerBlue

                ' 7. Eksekusi Form
                .ShowDialog()

                ' Refresh grid utama setelah form input ditutup
                RefreshData()
            End With
        Catch ex As Exception
                MsgBox("Error bre: " & ex.Message, vbCritical)
            Finally
                ' Pastikan koneksi ditutup biar nggak memory leak
                If DbKoneksi.State = ConnectionState.Open Then DbKoneksi.Close()
            End Try
        End Sub

        Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
            If MsgBox("Yakin Keluar Form Ini?", vbQuestion + vbYesNo) = vbYes Then Me.Close()
        End Sub

End Class
