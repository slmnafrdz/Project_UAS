Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class FrmLaporanDataMatakuliah

    Private isloading As Boolean = True

    Sub TampilkanFilterNamaProdi()
        Call KoneksiDB()
        Kode_Jurusan = ""

        CMD = New MySqlCommand("SELECT tbl_prodi.Nm_Prodi 
                                FROM tbl_prodi 
                                ORDER BY tbl_prodi.Kd_Prodi", DbKoneksi)

        DR = CMD.ExecuteReader
        CbNamaJurusan.Items.Clear()
        Do While DR.Read
            CbNamaJurusan.Items.Add(DR.Item("Nm_Prodi"))
        Loop
        DR.Close()
    End Sub

    Private Sub FrmLapDataMatkul_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isloading = True

        Call TampilkanFilterNamaProdi()

        'isi semester (GANJIL/GENAP)
        CbSemester.Items.Clear()
        CbSemester.Items.Add("GANJIL")
        CbSemester.Items.Add("GENAP")
        CbSemester.SelectedIndex = -1

        isloading = False
    End Sub

    Private Sub BtnCetakLaporan_Click(sender As Object, e As EventArgs) Handles BtnCetakLaporan.Click
        Call KoneksiDB()

        Try
            If CbNamaJurusan.SelectedIndex < 0 OrElse CbNamaJurusan.Text.Trim() = "" Then
                MsgBox("Pilih Prodi Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            If CbSemester.SelectedIndex < 0 OrElse CbSemester.Text.Trim() = "" Then
                MsgBox("Pilih Semester (GANJIL / GENAP) Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            If ValidasiDataLaporan() = False Then
                MsgBox("Data Matakuliah Prodi " & CbNamaJurusan.Text & " Semester " & CbSemester.Text & " Belum ada",
                           vbCritical + vbOKOnly, "Peringatan")
                Exit Sub
            End If

            Dim CryLapMatkulPath As String = Application.StartupPath & "\CrystLaporan\CrystLaporanDataMatakuliah.rpt"

            If Not IO.File.Exists(CryLapMatkulPath) Then
                MsgBox("File Laporan Tidak Ditemukan", vbCritical)
                Exit Sub
            End If

            Dim LapDataMatkul As New ReportDocument
            LapDataMatkul.Load(CryLapMatkulPath)
            LapDataMatkul.Refresh()

            'Bersihin dulu biar gak nempel formula lama
            CrystalReportViewer1.SelectionFormula = ""
            CrystalReportViewer1.ReportSource = LapDataMatkul
            CrystalReportViewer1.RefreshReport()

            'PASTIIN nama table sesuai Field Explorer: vlapdatamatkul
            CrystalReportViewer1.SelectionFormula =
                    "({vlapdatamatkul1.Nm_Prodi}) = '" & CbNamaJurusan.Text & "' AND " &
                    "({vlapdatamatkul1.Nama_Semester}) = '" & CbSemester.Text & "'"

            CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            MsgBox("Gagal mencetak laporan data matakuliah: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        If BtnKeluar.Text = "Keluar" Then
            If MsgBox("Anda yakin ingin keluar?!", vbQuestion + vbYesNo, "Informasi") = vbYes Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub CbNamaJurusan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaJurusan.SelectedIndexChanged
        If isloading Then Exit Sub
        If CbNamaJurusan.SelectedIndex < 0 Then Exit Sub

        Call KoneksiDB()

        CMD = New MySqlCommand("SELECT Kd_Prodi FROM tbl_prodi WHERE Nm_Prodi = @Nm", DbKoneksi)
        CMD.Parameters.Clear()
        CMD.Parameters.AddWithValue("@Nm", CbNamaJurusan.Text)

        DR = CMD.ExecuteReader
        If DR.Read() Then
            Kode_Jurusan = DR("Kd_Prodi").ToString()
        End If
        DR.Close()
    End Sub

    Function ValidasiDataLaporan() As Boolean
        Try
            Call KoneksiDB()

            QUERY = "SELECT COUNT(*) FROM vlapdatamatkul
                     WHERE Nm_Prodi = @Nm_Prodi AND Nama_Semester = @Sem"

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            CMD.Parameters.Clear()
            CMD.Parameters.AddWithValue("@Nm_Prodi", CbNamaJurusan.Text)
            CMD.Parameters.AddWithValue("@Sem", CbSemester.Text)

            Dim jumlah As Integer = Convert.ToInt32(CMD.ExecuteScalar())
            If jumlah > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MsgBox("Kesalahan Validasi Laporan: " & ex.Message, vbCritical)
            Return False
        End Try
    End Function

End Class
