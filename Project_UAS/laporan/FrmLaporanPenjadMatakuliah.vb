Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class FrmLaporanPenjadMatakuliah

    Private isLoading As Boolean = True
    Private Sub FrmLaporanPenjadMatakuliah_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' ================= LOAD FORM =================
        Try
            Isloading = True
            Call KoneksiDB()

            TampilkanProdi()
            TampilkanTahunAkademik()
            IsiSemester()
            IsiJenisKelas()

            CrystalReportViewer1.ReportSource = Nothing
            CrystalReportViewer1.Refresh()

        Catch ex As Exception
            MsgBox("Gagal load form : " & ex.Message, vbCritical)
        Finally
            Isloading = False
        End Try
    End Sub

    ' ================= COMBO PRODI =================
    Private Sub TampilkanProdi()
        Dim dt As New DataTable
        Dim sql As String = "SELECT Kd_Prodi, Nm_Prodi FROM tbl_prodi ORDER BY Kd_Prodi"

        Using da As New MySqlDataAdapter(sql, DbKoneksi)
            da.Fill(dt)
        End Using

        CbNamaProdi.DataSource = dt
        CbNamaProdi.DisplayMember = "Nm_Prodi"
        CbNamaProdi.ValueMember = "Kd_Prodi"
        CbNamaProdi.SelectedIndex = -1
    End Sub

    ' ================= COMBO TAHUN AKADEMIK =================
    Private Sub TampilkanTahunAkademik()
        Dim dt As New DataTable
        Dim sql As String = "SELECT DISTINCT Tahun_Akademik FROM tbl_pengampu_matakuliah ORDER BY Tahun_Akademik DESC"

        Using da As New MySqlDataAdapter(sql, DbKoneksi)
            da.Fill(dt)
        End Using

        CbTahunAkademik.DataSource = dt
        CbTahunAkademik.DisplayMember = "Tahun_Akademik"
        CbTahunAkademik.ValueMember = "Tahun_Akademik"
        CbTahunAkademik.SelectedIndex = -1
    End Sub

    ' ================= COMBO SEMESTER =================
    Private Sub IsiSemester()
        CbNamaSemester.Items.Clear()
        CbNamaSemester.Items.Add("GANJIL")
        CbNamaSemester.Items.Add("GENAP")
        CbNamaSemester.SelectedIndex = -1
    End Sub

    ' ================= COMBO JENIS KELAS =================
    Private Sub IsiJenisKelas()
        CbJenisKelas.Items.Clear()
        CbJenisKelas.Items.Add("REGULER")
        CbJenisKelas.Items.Add("KARYAWAN")
        CbJenisKelas.Items.Add("MALAM")
        CbJenisKelas.SelectedIndex = -1
    End Sub

    ' ================= CETAK =================
    Private Sub BtnCetak_Click(sender As Object, e As EventArgs) Handles BtnCetak.Click
        Try
            If CbNamaProdi.SelectedIndex < 0 OrElse
               CbTahunAkademik.SelectedIndex < 0 OrElse
               CbNamaSemester.SelectedIndex < 0 Then
                MsgBox("Lengkapi semua filter terlebih dahulu!", vbExclamation)
                Exit Sub
            End If

            ' Pakai report asli (datasource vlappenjadwalan1), jangan SetDataSource(dt)
            ' Tambahkan nama folder sebelum nama laporan
            Dim rpt As New CrystLaporanPenjadMatkul()
            CrystalReportViewer1.ReportSource = rpt

            Dim prodiNama As String = EscapeCrystalString(CbNamaProdi.Text.Trim())
            Dim thn As String = EscapeCrystalString(CbTahunAkademik.Text.Trim())
            Dim smt As String = CbNamaSemester.Text.Trim().ToUpper()

            ' Semester ganjil/genap dari field Semester (angka)
            Dim formulaSemester As String
            If smt = "GANJIL" Then
                formulaSemester = "({vlappenjadwalanmatkul1.Semester} MOD 2) = 1"
            Else
                formulaSemester = "({vlappenjadwalanmatkul1.Semester} MOD 2) = 0"
            End If

            ' Jika kamu mau filter jenis kelas juga (opsional)
            Dim formulaKelas As String = ""
            If CbJenisKelas.SelectedIndex >= 0 AndAlso CbJenisKelas.Text.Trim() <> "" Then
                Dim kelas As String = EscapeCrystalString(CbJenisKelas.Text.Trim())
                ' Field di report ada: Nama_Kelas
                formulaKelas = " AND {vlappenjadwalanmatkul1.Nama_Kelas} = '" & kelas & "'"
            End If

            ' Selection Formula utama
            Dim formula As String =
                "{vlappenjadwalanmatkul1.Nm_Prodi} = '" & prodiNama & "' " &
                "AND {vlappenjadwalanmatkul1.Tahun_Akademik} = '" & thn & "' " &
                "AND " & formulaSemester &
                formulaKelas

            CrystalReportViewer1.SelectionFormula = formula
            CrystalReportViewer1.RefreshReport()

        Catch ex As Exception
            MsgBox("Gagal cetak : " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Function EscapeCrystalString(s As String) As String
        Return s.Replace("'", "''")
    End Function

    ' ================= KELUAR =================
    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        Me.Close()
    End Sub

End Class
