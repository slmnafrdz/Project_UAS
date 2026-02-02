Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class FrmLaporanDataDosen

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
    End Sub

    Private Sub FrmLapDataDosen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isloading = True

        Call TampilkanFilterNamaProdi()
    End Sub

    Private Sub BtnCetak_Click(sender As Object, e As EventArgs) Handles BtnCetak.Click
        Call KoneksiDB()

        Try
            If CbNamaJurusan.SelectedIndex < 0 Then
                MsgBox("Pilih Prodi Terlebih Dahulu", vbExclamation)
                Exit Sub
            End If

            If ValidasiDataLaporan() = False Then
                MsgBox("Data Dosen Jurusan " & CbNamaJurusan.Text & " Belum ada", vbCritical + vbOKOnly, "Peringatan")
                Exit Sub
            End If

            'Dim CryLapDosenPath As String = Application.StartupPath & "\" + "Laporan" + "CryDataDosen.rpt"
            Dim CryLapDosenPath As String = Application.StartupPath & "\CrystLaporan\CrystLaporanDataDosen.rpt"


            If Not IO.File.Exists(CryLapDosenPath) Then
                MsgBox("File Laporan Tidak Ditemukan", vbCritical)
                Exit Sub
            End If

            Dim LapDataMhs As New ReportDocument
            LapDataMhs.Load(CryLapDosenPath)

            'set parameter crystal report
            CrystalReportViewer1.SelectionFormula = "({vlapdatadosen1.Nm_Prodi}) = '" & CbNamaJurusan.Text & "' "

            CrystalReportViewer1.RefreshReport()

            CrystalReportViewer1.ReportSource = LapDataMhs
            CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            MsgBox("Gagal mencetak laporan data dosen" & ex.Message, vbCritical)
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
        Call KoneksiDB()

        CMD = New MySqlCommand("SELECT Kd_Prodi FROM tbl_prodi WHERE Nm_Prodi = @Nm", DbKoneksi)
        CMD.Parameters.AddWithValue("@Nm", CbNamaJurusan.Text)

        DR = CMD.ExecuteReader
        If DR.Read() Then
            Kode_Jurusan = DR("Kd_Prodi").ToString()
            'LbKdProdi.Text = Microsoft.VisualBasic.Right(Kode_Jurusan, 2)
        End If
        DR.Close()

    End Sub

    Function ValidasiDataLaporan() As Boolean
        Try
            Call KoneksiDB()
            QUERY = "SELECT COUNT(*) FROM tbl_dosen INNER JOIN tbl_prodi on tbl_dosen.Kd_Prodi = tbl_prodi.Kd_Prodi
                    WHERE tbl_prodi.Kd_Prodi = @Kd_Prodi"

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            CMD.Parameters.AddWithValue("@Kd_Prodi", Kode_Jurusan)

            Dim jumlah As Integer = Convert.ToInt32(CMD.ExecuteScalar())
            If jumlah > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MsgBox("Kesalahan Validasi Laporan" & ex.Message, vbCritical)
            Return False
        End Try
    End Function
End Class


