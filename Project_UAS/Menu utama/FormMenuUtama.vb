Public Class FrmMenuUtama

    ' --- FUNGSI TAMBAHAN UNTUK MEMBERSIHKAN FORM ---
    ' Fungsi ini akan menutup semua form anak yang sedang terbuka 
    ' agar tidak terjadi penumpukan seperti di gambar.
    Private Sub TutupFormAnak()
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub FrmMenuUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DATAMASTERToolStripMenuItem.Enabled = False
        DataTransaksiToolStripMenuItem.Enabled = False
        DataLaporanToolStripMenuItem.Enabled = False
        HelpToolStripMenuItem.Enabled = False
    End Sub

    Private Sub LoginSistemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginSistemToolStripMenuItem.Click
        TutupFormAnak() ' Bersihkan form lain sebelum login
        FrmLogin.MdiParent = Me
        FrmLogin.Show()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Me.Close()
    End Sub


    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim Pesan As Integer
        Pesan = MsgBox("Apakah Anda yakin ingin keluar dari program?", vbQuestion + vbYesNo, "Konfirmasi Keluar")

        If Pesan = vbYes Then
            Me.Close()
        End If
    End Sub

    ' --- BAGIAN MENU DATA MASTER ---

    Private Sub DataJurusanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataJurusanToolStripMenuItem.Click
        TutupFormAnak()
        FrmDataJurusan.MdiParent = Me
        FrmDataJurusan.WindowState = FormWindowState.Normal
        FrmDataJurusan.Show()
    End Sub

    Private Sub DataHariToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataHariToolStripMenuItem.Click
        TutupFormAnak()
        FrmDataHari.MdiParent = Me
        FrmDataHari.WindowState = FormWindowState.Normal
        FrmDataHari.Show()
    End Sub

    Private Sub DataRuangKelasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataRuangKelasToolStripMenuItem.Click
        TutupFormAnak()
        FrmDataRuangan.MdiParent = Me
        FrmDataRuangan.WindowState = FormWindowState.Normal
        FrmDataRuangan.Show()
    End Sub

    Private Sub DataDosenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataDosenPengampuToolStripMenuItem.Click
        TutupFormAnak()
        FrmDataDosen.MdiParent = Me
        FrmDataDosen.WindowState = FormWindowState.Normal
        FrmDataDosen.Show()
    End Sub

    Private Sub DataMataKuliahToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataMatakuliahToolStripMenuItem.Click
        TutupFormAnak()
        FrmDataMatakuliah.MdiParent = Me
        FrmDataMatakuliah.WindowState = FormWindowState.Normal
        FrmDataMatakuliah.Show()
    End Sub

    Private Sub DataPenjadwalanMatakuliahDosenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataPenjadwalanMatakuliahDosenToolStripMenuItem.Click
        ' Tambahkan kode Show Form Penjadwalan di sini jika sudah ada form-nya
        TutupFormAnak()
        FrmDataPenjadMatkul.MdiParent = Me
        FrmDataPenjadMatkul.WindowState = FormWindowState.Normal
        FrmDataPenjadMatkul.Show()
    End Sub

    Private Sub DosenPengampuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DosenPengampuToolStripMenuItem.Click
        TutupFormAnak()
        FrmDataDosenPengampuMatakul.MdiParent = Me
        FrmDataDosenPengampuMatakul.WindowState = FormWindowState.Normal
        FrmDataDosenPengampuMatakul.Show()
    End Sub

    Private Sub LaporanDataDosenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanDataDosenToolStripMenuItem.Click
        TutupFormAnak()
        FrmLaporanDataDosen.MdiParent = Me
        FrmLaporanDataDosen.WindowState = FormWindowState.Normal
        FrmLaporanDataDosen.Show()
    End Sub

    Private Sub LaporanDataMatakuliahGanjilGenapToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanDataMatakuliahGanjilGenapToolStripMenuItem.Click
        TutupFormAnak()
        FrmLaporanDataMatakuliah.MdiParent = Me
        FrmLaporanDataMatakuliah.WindowState = FormWindowState.Normal
        FrmLaporanDataMatakuliah.Show()
    End Sub

    Private Sub LaporanDataDosenPengampuMatakulianTahunAkademixToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanDataDosenPengampuMatakulianTahunAkademixToolStripMenuItem.Click
        TutupFormAnak()
        FrmLaporanDataDosenPengampuMatkul.MdiParent = Me
        FrmLaporanDataDosenPengampuMatkul.WindowState = FormWindowState.Normal
        FrmLaporanDataDosenPengampuMatkul.Show()
    End Sub

    Private Sub LaporanPenjadwalanMatakuliahBerdasarkanTahunAkademixToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanPenjadwalanMatakuliahBerdasarkanTahunAkademixToolStripMenuItem.Click
        TutupFormAnak()
        FrmLaporanPenjadMatakuliah.MdiParent = Me
        FrmLaporanPenjadMatakuliah.WindowState = FormWindowState.Normal
        FrmLaporanPenjadMatakuliah.Show()
    End Sub

    Private Sub TentangAplikasiToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles TentangAplikasiToolStripMenuItem1.Click
        TutupFormAnak()
        FrmAboutBox.MdiParent = Me
        FrmAboutBox.WindowState = FormWindowState.Normal
        FrmAboutBox.Show()
    End Sub

    Private Sub DataTransaksiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataTransaksiToolStripMenuItem.Click

    End Sub
End Class