Imports MySql.Data.MySqlClient

Public Class FrmInputDataDosen

    Sub FormAktif()
        TxtNidn.Enabled = False
        TxtNidn.Enabled = True
        CbJenisKelamin.Enabled = True
        TxtNoHp.Enabled = True
        TxtEmail.Enabled = True
        CbJurusan.Enabled = True
    End Sub

    Sub FormNonAktif()
        TxtNidn.Enabled = False
        TxtNidn.Enabled = False
        CbJenisKelamin.Enabled = False
        TxtNoHp.Enabled = False
        TxtEmail.Enabled = False
        CbJurusan.Enabled = False
    End Sub

    Sub KosongkanData()
        LbKdDosen.Text = ""
        TxtNidn.Text = ""
        TxtNidn.Text = ""
        CbJenisKelamin.SelectedIndex = -1
        TxtNoHp.Text = ""
        TxtEmail.Text = ""
        CbJurusan.SelectedIndex = -1
        CbJurusan.Text = ""
        Lbkdprodi.Text = ""
    End Sub

    Sub FormNormal()
        Call FormAktif()
        BtnHapus.Enabled = False
        BtnSimpan.Text = "Simpan"

        BtnSimpan.Enabled = False
        BtnKeluar.Text = "Keluar"
    End Sub

    Sub KosongkanCombobox()
        CbJenisKelamin.Items.Clear()
        CbJurusan.Items.Clear()
    End Sub

    Sub JenisKelamin()
        CbJenisKelamin.Items.Add("Laki-Laki")
        CbJenisKelamin.Items.Add("Perempuan")
    End Sub

    Sub TampilkanFilterNamaProdi()
        Call KoneksiDB()
        Kode_Jurusan = ""
        CMD = New MySqlCommand("SELECT tbl_prodi.Nm_Prodi 
                                FROM tbl_prodi 
                                ORDER BY tbl_prodi.Kd_Prodi", DbKoneksi)
        DR = CMD.ExecuteReader
        CbJurusan.Items.Clear()
        Do While DR.Read
            CbJurusan.Items.Add(DR.Item("Nm_Prodi"))
        Loop
        DR.Close()
    End Sub

    Private Sub CbJurusan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbJurusan.SelectedIndexChanged
        Call KoneksiDB()
        CMD = New MySqlCommand("SELECT * FROM tbl_prodi WHERE Nm_Prodi = '" & CbJurusan.Text & "'", DbKoneksi)
        DR = CMD.ExecuteReader
        DR.Read()
        If DR.HasRows Then
            Kode_Jurusan = DR.Item("Kd_Prodi")
            Lbkdprodi.Text = Kode_Jurusan
        End If
        DR.Close()

        'DR.Close()

        'If BtnSimpan.Text = "Simpan" Then
        '    BuatKodeDosenOtomatis()
        '    BuatNidnOtomatis()
        'End If
    End Sub

    Private Sub CbJurusan_DropDown(sender As Object, e As EventArgs) Handles CbJurusan.DropDown
        'CbJurusan.SelectedIndex = -1
    End Sub

    Sub BuatKodeDosenOtomatis()
        Try
            Call KoneksiDB()
            QUERY = "SELECT IFNULL(MAX(Kd_Dosen), 130000) + 1 AS NextKd FROM tbl_dosen"
            CMD = New MySqlCommand(QUERY, DbKoneksi)
            Dim nextKd As Object = CMD.ExecuteScalar()
            LbKdDosen.Text = nextKd.ToString()

        Catch ex As Exception
            MessageBox.Show("Error BuatKodeDosenOtomatis: " & ex.Message)
        End Try
    End Sub


    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        ' Jika tombol bertuliskan "Keluar"
        If BtnKeluar.Text = "Keluar" Then
            Pesan = MsgBox("Apakah Anda Yakin Ingin Keluar Dari Data Dosen?!", vbQuestion + vbYesNo, "Informasi")
            If Pesan = vbYes Then
                Me.Close()
                FrmDataDosen.Show()
                FrmDataDosen.Enabled = True
                FrmDataDosen.TampilkanDataGridDosen()

                FrmDataDosen.Show()
                FrmDataDosen.Enabled = True
                FrmDataDosen.Activate()
                FrmDataDosen.TampilkanDataGridDosen()
            End If
            Exit Sub
        End If


        ' Jika tombol bertuliskan "Batal"
        If BtnKeluar.Text = "Batal" Then
            Pesan = MsgBox("Batalkan perubahan data ini?", vbQuestion + vbYesNo, "Konfirmasi Batal")

            If Pesan = vbYes Then
                Call FormNormal()
                Call KosongkanData()

                BtnSimpan.Enabled = False
                BtnSimpan.BackColor = Color.Red

                BtnHapus.Enabled = False
                BtnHapus.BackColor = Color.Red

                ' Batal berubah jadi Keluar
                BtnKeluar.Text = "Keluar"
            End If

            Exit Sub
        End If
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
        Call KoneksiDB()
        If TxtNidn.Text = "" Then
            MsgBox("Silahkan Isi Semua Datanya Terlebih Dahulu!", vbInformation)
            Exit Sub
        End If

        Try
            ' GANTI BLOK SIMPAN DENGAN KODE INI
            If BtnSimpan.Text = "Simpan" Then
                ' Gunakan query yang bersih dengan parameter @
                SQLinsert = "INSERT INTO tbl_dosen (Kd_Dosen, Kd_Prodi, Nidn_Dosen, Nm_Dosen, Jk_Dosen, NoHp_Dosen, Email_Dosen) " &
                "VALUES (@kd, @prodi, @nidn, @nama, @jk, @hp, @email)"

                CMD = New MySqlCommand(SQLinsert, DbKoneksi)

                ' Mengisi parameter agar tidak ada masalah tanda petik (')
                CMD.Parameters.AddWithValue("@kd", LbKdDosen.Text)
                CMD.Parameters.AddWithValue("@prodi", Lbkdprodi.Text)
                CMD.Parameters.AddWithValue("@nidn", TxtNidn.Text)
                CMD.Parameters.AddWithValue("@nama", TxtNama.Text)
                CMD.Parameters.AddWithValue("@jk", CbJenisKelamin.Text)
                CMD.Parameters.AddWithValue("@hp", TxtNoHp.Text)
                CMD.Parameters.AddWithValue("@email", TxtEmail.Text)

                CMD.ExecuteNonQuery()

                Call FormNormal()
                MsgBox("Data Dosen Berhasil Ditambahkan!", vbInformation)

                Me.Close()
                FrmDataDosen.Show()
                FrmDataDosen.TampilkanDataGridDosen()

            ElseIf BtnSimpan.Text = "Ubah" Then
                SQLUpdate = "UPDATE tbl_dosen SET " _
                    & "Nidn_Dosen='" & TxtNidn.Text & "'," _
                    & "Nm_Dosen='" & TxtNama.Text & "'," _
                    & "Jk_Dosen='" & CbJenisKelamin.Text & "'," _
                    & "NoHp_Dosen='" & TxtNoHp.Text & "'," _
                    & "Email_Dosen='" & TxtEmail.Text & "'," _
                    & "Kd_Prodi='" & Lbkdprodi.Text & "' " _
                    & "WHERE Kd_Dosen='" & LbKdDosen.Text & "'"

                CMD = New MySqlCommand(SQLUpdate, DbKoneksi)
                CMD.ExecuteNonQuery()
                Call FormNormal()
                MsgBox("Data Dosen Berhasil Diubah!", vbInformation)

                Me.Close()
                FrmDataDosen.Enabled = True
                FrmDataDosen.Show()
                FrmDataDosen.Activate()
                FrmDataDosen.TampilkanDataGridDosen()
            End If

        Catch ex As Exception
            MsgBox("Terjadi Kesalahan Saat Menyimpan Data: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub BtnHapus_Click(sender As Object, e As EventArgs) Handles BtnHapus.Click
        'Dim Konfirmasi As String
        'Konfirmasi = MsgBox("Anda yakin akan menghapus data ini?", vbYesNo + vbQuestion)
        'If Konfirmasi = vbYes Then
        '    SQLHapus = "DELETE FROM tbl_dosen WHERE Kd_Dosen ='" & LbKdDosen.Text & "'"
        '    CMD = New MySqlCommand(SQLHapus, DbKoneksi)
        '    CMD.ExecuteNonQuery()

        '    MsgBox("Data dosen berhasil dihapus!", vbInformation)
        '    Me.Close()
        '    FrmDataDosen.Show()
        '    FrmDataDosen.TampilkanDataGridDosen()
        'End If

        Dim Konfirmasi As Integer
        Konfirmasi = MsgBox("Anda yakin akan menghapus data ini?", vbYesNo + vbQuestion, "Informasi")

        If Konfirmasi = vbYes Then
            Try
                Call KoneksiDB()

                SQLHapus = "DELETE FROM tbl_dosen WHERE Kd_Dosen = @KdDosen"
                CMD = New MySqlCommand(SQLHapus, DbKoneksi)
                CMD.Parameters.AddWithValue("@KdDosen", LbKdDosen.Text)

                CMD.ExecuteNonQuery()

                ' Kosongkan form
                Call KosongkanData()

                ' Notifikasi berhasil
                MsgBox("Data berhasil dihapus!", vbInformation, "Sukses")

                'Aktifkan & Refresh() form grid
                FrmDataDosen.Enabled = True
                FrmDataDosen.Show()
                FrmDataDosen.Activate()
                FrmDataDosen.TampilkanDataGridDosen()

                'atur tombol (kalau form masih kebuka)
                BtnSimpan.Enabled = False
                BtnSimpan.BackColor = Color.Red

                BtnHapus.Text = "Keluar"
                BtnHapus.Enabled = False
                BtnHapus.BackColor = Color.Red

                Me.Close()

            Catch ex As Exception
                MsgBox("Gagal menghapus data: " & ex.Message, vbCritical, "Error")
            End Try

        Else
            BtnSimpan.Enabled = False
            BtnSimpan.BackColor = Color.Red

            BtnHapus.Enabled = False
            BtnHapus.BackColor = Color.Red

            'FrmDataDosen.Enabled = True
            Me.Close()
        End If

    End Sub

    Private Sub FrmDosen_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call KoneksiDB()
        Call JenisKelamin()
        Call TampilkanFilterNamaProdi()

        BtnSimpan.Enabled = True
        BtnSimpan.BackColor = Color.CornflowerBlue

        BtnHapus.Enabled = False
        BtnHapus.BackColor = Color.Red
    End Sub

End Class
