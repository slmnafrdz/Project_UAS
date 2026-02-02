
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class FrmLaporanDataDosenPengampuMatkul

    Private isloading As Boolean = True


    Sub TampilkanFilterNamaProdi()
        Call KoneksiDB()
        Kode_Jurusan = ""

        CMD = New MySqlCommand("SELECT tbl_prodi.Nm_Prodi FROM tbl_prodi ORDER BY tbl_prodi.Kd_Prodi", DbKoneksi)
        DR = CMD.ExecuteReader

        CbNamaJurusan.Items.Clear()
        Do While DR.Read
            CbNamaJurusan.Items.Add(DR.Item("Nm_Prodi"))
        Loop
        DR.Close()
    End Sub

    Sub IsiSemester()
        CbSemester.Items.Clear()
        CbSemester.Items.Add("GANJIL")
        CbSemester.Items.Add("GENAP")
        CbSemester.SelectedIndex = -1
    End Sub

    Sub IsiTahunAkademik()
        CbTahunAkademik.Items.Clear()
        CbTahunAkademik.SelectedIndex = -1

        If CbNamaJurusan.SelectedIndex < 0 Then Exit Sub
        If CbSemester.SelectedIndex < 0 Then Exit Sub

        Try
            Call KoneksiDB()

            Dim sql As String =
                "SELECT DISTINCT tbl_pengampu_matakuliah.Tahun_Akademik " &
                "FROM tbl_pengampu_matakuliah " &
                "INNER JOIN tbl_dosen ON tbl_pengampu_matakuliah.Kd_Dosen = tbl_dosen.Kd_Dosen " &
                "INNER JOIN tbl_prodi ON tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi " &
                "INNER JOIN tbl_matakuliah ON tbl_pengampu_matakuliah.Kd_Matakuliah = tbl_matakuliah.Kd_Matakuliah " &
                "WHERE tbl_prodi.Nm_Prodi = @Nm_Prodi " &
                "AND (CASE WHEN MOD(tbl_matakuliah.Semester_Matakuliah, 2) = 0 THEN 'GENAP' ELSE 'GANJIL' END) = @Nama_Semester " &
                "ORDER BY tbl_pengampu_matakuliah.Tahun_Akademik DESC"

            CMD = New MySqlCommand(sql, DbKoneksi)
            CMD.Parameters.Clear()
            CMD.Parameters.AddWithValue("@Nm_Prodi", CbNamaJurusan.Text)
            CMD.Parameters.AddWithValue("@Nama_Semester", CbSemester.Text)

            DR = CMD.ExecuteReader
            Do While DR.Read
                CbTahunAkademik.Items.Add(DR.Item("Tahun_Akademik").ToString())
            Loop
            DR.Close()

            'JANGAN auto pilih tahun
            CbTahunAkademik.SelectedIndex = -1
            CbTahunAkademik.Text = ""

            'If CbTahunAkademik.Items.Count > 0 Then
            '    CbTahunAkademik.SelectedIndex = 0
            'End If

        Catch
            Try
                If DR IsNot Nothing AndAlso Not DR.IsClosed Then DR.Close()
            Catch
            End Try
        End Try
    End Sub

    Private Sub FrmLapDosenPengampu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isloading = True

        Call TampilkanFilterNamaProdi()
        Call IsiSemester()

        CbSemester.Enabled = True
        CbTahunAkademik.Enabled = True
        TxtKdDosen.Enabled = True

        BtnCetakPengampu.Enabled = True
        BtnCetakTahunAkademik.Enabled = True

        TxtKdDosen.Text = ""
        TxtNIDN.Text = ""
        TxtNamaDosen.Text = ""
        TxtTahunAkademik.Text = ""

        CrystalReportViewer1.ReportSource = Nothing

        isloading = False
    End Sub

    Private Sub CbNamaJurusan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaJurusan.SelectedIndexChanged
        If isloading Then Exit Sub
        If CbNamaJurusan.SelectedIndex < 0 Then Exit Sub

        'reset detail
        TxtKdDosen.Text = ""
        TxtNIDN.Text = ""
        TxtNamaDosen.Text = ""
        TxtTahunAkademik.Text = ""
        CrystalReportViewer1.ReportSource = Nothing

        'reset filter lanjutan
        CbSemester.SelectedIndex = -1
        CbTahunAkademik.Items.Clear()
        CbTahunAkademik.SelectedIndex = -1


        Call KoneksiDB()
        CMD = New MySqlCommand("SELECT tbl_prodi.Kd_Prodi FROM tbl_prodi WHERE tbl_prodi.Nm_Prodi = @Nm", DbKoneksi)
        CMD.Parameters.Clear()
        CMD.Parameters.AddWithValue("@Nm", CbNamaJurusan.Text)

        DR = CMD.ExecuteReader
        If DR.Read() Then
            Kode_Jurusan = DR("Kd_Prodi").ToString()
        End If
        DR.Close()

        BtnCetakPengampu.Enabled = True
        BtnCetakTahunAkademik.Enabled = True
    End Sub

    Private Sub CbSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbSemester.SelectedIndexChanged
        If isloading Then Exit Sub
        If CbSemester.SelectedIndex < 0 Then Exit Sub

        TxtKdDosen.Text = ""
        TxtNIDN.Text = ""
        TxtNamaDosen.Text = ""
        TxtTahunAkademik.Text = ""
        CrystalReportViewer1.ReportSource = Nothing

        CbTahunAkademik.Enabled = True
        Call IsiTahunAkademik()

        BtnCetakPengampu.Enabled = True
        BtnCetakTahunAkademik.Enabled = True
    End Sub

    Private Sub CbTahunAkademik_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbTahunAkademik.SelectedIndexChanged
        If isloading Then Exit Sub
        If CbTahunAkademik.SelectedIndex < 0 Then Exit Sub

        TxtTahunAkademik.Text = CbTahunAkademik.Text

        TxtKdDosen.Enabled = True
        BtnCetakTahunAkademik.Enabled = True

        'tombol cetak pengampu (per dosen) baru aktif kalau kode dosen sudah diisi & valid
        'BtnCetakPengampu.Enabled = (TxtKdDosen.Text.Trim() <> "")

        BtnCetakPengampu.Enabled = True
        BtnCetakTahunAkademik.Enabled = True
    End Sub

    Private Sub TxtKdDosen_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtKdDosen.KeyDown
        If e.KeyCode <> Keys.Enter Then Exit Sub
        e.SuppressKeyPress = True

        TxtNIDN.Text = ""
        TxtNamaDosen.Text = ""

        If TxtKdDosen.Text.Trim() = "" Then
            BtnCetakPengampu.Enabled = True
            BtnCetakTahunAkademik.Enabled = True
            Exit Sub
        End If


        If CbNamaJurusan.SelectedIndex < 0 OrElse CbSemester.SelectedIndex < 0 OrElse CbTahunAkademik.SelectedIndex < 0 Then
            MsgBox("Pilih Prodi, Semester, dan Tahun Akademik terlebih dahulu!", vbExclamation, "Info")
            BtnCetakPengampu.Enabled = False
            Exit Sub
        End If

        Try
            Call KoneksiDB()

            CMD = New MySqlCommand(
                "SELECT tbl_dosen.Kd_Dosen, tbl_dosen.Nidn_Dosen, tbl_dosen.Nm_Dosen " &
                "FROM tbl_dosen " &
                "INNER JOIN tbl_prodi ON tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi " &
                "WHERE tbl_dosen.Kd_Dosen = @Kd_Dosen AND tbl_prodi.Nm_Prodi = @Nm_Prodi " &
                "LIMIT 1", DbKoneksi)

            CMD.Parameters.Clear()
            CMD.Parameters.AddWithValue("@Kd_Dosen", TxtKdDosen.Text.Trim())
            CMD.Parameters.AddWithValue("@Nm_Prodi", CbNamaJurusan.Text)

            DR = CMD.ExecuteReader
            If DR.Read() Then
                TxtNIDN.Text = DR("Nidn_Dosen").ToString()
                TxtNamaDosen.Text = DR("Nm_Dosen").ToString()
                BtnCetakPengampu.Enabled = True
            Else
                MsgBox("Kode Dosen tidak ditemukan untuk Prodi ini!", vbExclamation, "Info")
                BtnCetakPengampu.Enabled = False
            End If
            DR.Close()

        Catch ex As Exception
            Try
                If DR IsNot Nothing AndAlso Not DR.IsClosed Then DR.Close()
            Catch
            End Try
            MsgBox("Gagal ambil data dosen: " & ex.Message, vbCritical)
            BtnCetakPengampu.Enabled = False
        End Try
    End Sub

    Function ValidasiDataLaporan() As Boolean
        Try
            Call KoneksiDB()

            QUERY =
                "SELECT COUNT(*) " &
                "FROM tbl_pengampu_matakuliah " &
                "INNER JOIN tbl_dosen ON tbl_pengampu_matakuliah.Kd_Dosen = tbl_dosen.Kd_Dosen " &
                "INNER JOIN tbl_prodi ON tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi " &
                "INNER JOIN tbl_matakuliah ON tbl_pengampu_matakuliah.Kd_Matakuliah = tbl_matakuliah.Kd_Matakuliah " &
                "WHERE tbl_prodi.Nm_Prodi = @Nm_Prodi " &
                "AND (CASE WHEN MOD(tbl_matakuliah.Semester_Matakuliah, 2) = 0 THEN 'GENAP' ELSE 'GANJIL' END) = @Nama_Semester " &
                "AND tbl_pengampu_matakuliah.Tahun_Akademik = @Tahun_Akademik"

            If TxtKdDosen.Text.Trim() <> "" Then
                QUERY &= " AND tbl_dosen.Kd_Dosen = @Kd_Dosen"
            End If

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            CMD.Parameters.Clear()
            CMD.Parameters.AddWithValue("@Nm_Prodi", CbNamaJurusan.Text)
            CMD.Parameters.AddWithValue("@Nama_Semester", CbSemester.Text)
            CMD.Parameters.AddWithValue("@Tahun_Akademik", CbTahunAkademik.Text)

            If TxtKdDosen.Text.Trim() <> "" Then
                CMD.Parameters.AddWithValue("@Kd_Dosen", TxtKdDosen.Text.Trim())
            End If

            Dim jumlah As Integer = Convert.ToInt32(CMD.ExecuteScalar())
            Return (jumlah > 0)

        Catch ex As Exception
            MsgBox("Kesalahan Validasi Laporan: " & ex.Message, vbCritical)
            Return False
        End Try
    End Function

    Private Sub BtnCetakPengampu_Click(sender As Object, e As EventArgs) Handles BtnCetakPengampu.Click
        Call KoneksiDB()

        Try
            If CbNamaJurusan.SelectedIndex < 0 Then
                MsgBox("Pilih Prodi Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            If CbSemester.SelectedIndex < 0 Then
                MsgBox("Pilih Semester (GANJIL/GENAP) Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            If CbTahunAkademik.SelectedIndex < 0 OrElse CbTahunAkademik.Text.Trim() = "" Then
                MsgBox("Pilih Tahun Akademik Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            'sesuai gambar: cetak pengampu per dosen -> wajib kode dosen
            If TxtKdDosen.Text.Trim() = "" Then
                MsgBox("Isi Kode Dosen terlebih dahulu!", vbExclamation)
                TxtKdDosen.Focus()
                Exit Sub
            End If

            If ValidasiDataLaporan() = False Then
                MsgBox("Data Dosen Pengampu tidak ditemukan untuk filter yang dipilih!", vbCritical + vbOKOnly, "Peringatan")
                Exit Sub
            End If

            Dim CryPath As String = Application.StartupPath & "\CrystLaporan\CrystLaporanPengampuMatkul.rpt"
            If Not IO.File.Exists(CryPath) Then
                MsgBox("File Laporan Tidak Ditemukan", vbCritical)
                Exit Sub
            End If

            Dim Lap As New ReportDocument
            Lap.Load(CryPath)

            Dim formula As String = "({vlapdosenpengampu1.Nm_Prodi}) = '" & CbNamaJurusan.Text & "' " &
                                "AND ({vlapdosenpengampu1.Nama_Semester}) = '" & CbSemester.Text & "' " &
                                "AND ({vlapdosenpengampu1.Tahun_Akademik}) = '" & CbTahunAkademik.Text & "' "

            If TxtKdDosen.Text.Trim() <> "" Then
                formula &= "AND ({vlapdosenpengampu1.Kd_Dosen}) = " & Val(TxtKdDosen.Text.Trim())
            End If


            CrystalReportViewer1.SelectionFormula = formula
            CrystalReportViewer1.ReportSource = Lap
            CrystalReportViewer1.RefreshReport()

            'SETELAH CETAK PENGAMPU -> tombol tahun akademik nonaktif
            BtnCetakTahunAkademik.Enabled = False
            BtnCetakPengampu.Enabled = True


        Catch ex As Exception
            MsgBox("Gagal mencetak laporan dosen pengampu: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub BtnCetakTahunAkademik_Click(sender As Object, e As EventArgs) Handles BtnCetakTahunAkademik.Click
        Call KoneksiDB()

        Try
            If CbNamaJurusan.SelectedIndex < 0 Then
                MsgBox("Pilih Prodi Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            If CbSemester.SelectedIndex < 0 Then
                MsgBox("Pilih Semester (GANJIL/GENAP) Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            If CbTahunAkademik.SelectedIndex < 0 OrElse CbTahunAkademik.Text.Trim() = "" Then
                MsgBox("Pilih Tahun Akademik Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            'validasi tanpa kode dosen (cetak semua dosen pada TA tsb)
            Dim sqlValidasi As String =
                "SELECT COUNT(*) " &
                "FROM tbl_pengampu_matakuliah " &
                "INNER JOIN tbl_dosen ON tbl_pengampu_matakuliah.Kd_Dosen = tbl_dosen.Kd_Dosen " &
                "INNER JOIN tbl_prodi ON tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi " &
                "INNER JOIN tbl_matakuliah ON tbl_pengampu_matakuliah.Kd_Matakuliah = tbl_matakuliah.Kd_Matakuliah " &
                "WHERE tbl_prodi.Nm_Prodi = @Nm_Prodi " &
                "AND (CASE WHEN MOD(tbl_matakuliah.Semester_Matakuliah, 2) = 0 THEN 'GENAP' ELSE 'GANJIL' END) = @Nama_Semester " &
                "AND tbl_pengampu_matakuliah.Tahun_Akademik = @Tahun_Akademik"

            CMD = New MySqlCommand(sqlValidasi, DbKoneksi)
            CMD.Parameters.Clear()
            CMD.Parameters.AddWithValue("@Nm_Prodi", CbNamaJurusan.Text)
            CMD.Parameters.AddWithValue("@Nama_Semester", CbSemester.Text)
            CMD.Parameters.AddWithValue("@Tahun_Akademik", CbTahunAkademik.Text)

            Dim jumlah As Integer = Convert.ToInt32(CMD.ExecuteScalar())
            If jumlah <= 0 Then
                MsgBox("Data Dosen Pengampu Tahun Akademik tersebut belum ada!", vbCritical + vbOKOnly, "Peringatan")
                Exit Sub
            End If

            Dim CryPath As String = Application.StartupPath & "\CrystLaporan\CrystLaporanPengampuMatkul.rpt"
            If Not IO.File.Exists(CryPath) Then
                MsgBox("File Laporan Tidak Ditemukan", vbCritical)
                Exit Sub
            End If

            Dim Lap As New ReportDocument
            Lap.Load(CryPath)

            Dim formula As String =
                "({vlapdosenpengampu1.Nm_Prodi}) = '" & CbNamaJurusan.Text & "' " &
                "AND ({vlapdosenpengampu1.Nama_Semester}) = '" & CbSemester.Text & "' " &
                "AND ({vlapdosenpengampu1.Tahun_Akademik}) = '" & CbTahunAkademik.Text & "' "

            CrystalReportViewer1.SelectionFormula = formula
            CrystalReportViewer1.ReportSource = Lap
            CrystalReportViewer1.RefreshReport()

            'SETELAH CETAK TAHUN -> tombol pengampu nonaktif
            BtnCetakPengampu.Enabled = False
            BtnCetakTahunAkademik.Enabled = True


        Catch ex As Exception
            MsgBox("Gagal mencetak laporan tahun akademik: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        If BtnKeluar.Text = "Keluar" Then
            If MsgBox("Anda yakin ingin keluar?!", vbQuestion + vbYesNo, "Informasi") = vbYes Then
                Me.Close()
            End If
        End If
    End Sub

End Class
