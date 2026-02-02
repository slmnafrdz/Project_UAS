Imports MySql.Data.MySqlClient
Public Class FrmLogin
    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        Call KoneksiDB()
        If TxtUser.Text = "" Then
            MsgBox("Nama User Tidak Boleh Kosong", MsgBoxStyle.Exclamation, "Kosong!")
            TxtUser.Focus()
        ElseIf TxtPassword.Text = "" Then
            MsgBox("Nama Password tidak boleh kosong", MsgBoxStyle.Exclamation, "Kosong!")
            TxtPassword.Focus()
        Else
            Call KoneksiDB()
            CMD = New MySqlCommand("SELECT * FROM tbl_user WHERE Nm_User='" & TxtUser.Text & "'", DbKoneksi)
            DR = CMD.ExecuteReader
            DR.Read()

            If Not DR.HasRows = 0 Then
                Call KoneksiDB()
                CMD = New MySqlCommand("SELECT * FROM tbl_user WHERE Nm_User='" & TxtUser.Text & "' AND Pass_User='" & TxtPassword.Text & "'", DbKoneksi)

                DR = CMD.ExecuteReader
                DR.Read()

                If Not DR.HasRows = 0 Then
                    If DR.Item("Level_User") = "Administrator" Or DR.Item("Level_User") = "Dosen" And DR.Item("Nm_User") = TxtUser.Text Then

                        With FrmMenuUtama
                            Me.Close()
                            .Show()
                            .DATAMASTERToolStripMenuItem.Enabled = True
                            .DataTransaksiToolStripMenuItem.Enabled = True
                            .DataLaporanToolStripMenuItem.Enabled = True
                            '.LaporanDataDosenPengampuMatakuliahBerdasarkanTahunAkademikToolStripMenuItem.Enabled = True
                            '.LaporanPenjadwalanMatakuliahBerdasarkanTahunAkademikToolStripMenuItem.Enabled = True
                            '.LaporanDataDosenToolStripMenuItem.Enabled = True
                            '.LaporanDataMatakuliahPerSemesterToolStripMenuItem.Enabled = True
                            .HelpToolStripMenuItem.Enabled = True
                            .LoginSistemToolStripMenuItem.Enabled = False
                            .LogoutToolStripMenuItem.Enabled = True
                        End With

                    ElseIf DR.Item("Level_User") = "Mahasiswa" And DR.Item("Nm_User") = TxtUser.Text Then
                        'menampilkan menu level user

                        With FrmMenuUtama
                            Me.Close()
                            .Show()
                            .DATAMASTERToolStripMenuItem.Enabled = False
                            .DataTransaksiToolStripMenuItem.Enabled = False
                            .DataLaporanToolStripMenuItem.Enabled = True
                            '.LaporanDataDosenPengampuMatakuliahBerdasarkanTahunAkademikToolStripMenuItem.Enabled = True
                            '.LaporanPenjadwalanMatakuliahBerdasarkanTahunAkademikToolStripMenuItem.Enabled = True
                            '.LaporanDataDosenToolStripMenuItem.Enabled = False
                            '.LaporanDataMatakuliahPerSemesterToolStripMenuItem.Enabled = False
                            .HelpToolStripMenuItem.Enabled = True
                            .LoginSistemToolStripMenuItem.Enabled = False
                            .LogoutToolStripMenuItem.Enabled = True
                        End With
                    Else
                        MsgBox("Anda tidak berhak menggunakan program ini!", vbCritical + vbOKOnly, "Informasi")
                        TxtUser.Text = ""
                        TxtPassword.Text = ""
                        Exit Sub
                    End If

                Else
                    If Hitung < 2 Then
                        MsgBox("Password masih salah, Silahkan ulangi lagi!!" & Hitung, vbCritical + vbOKOnly, "Warning")
                        TxtPassword.Text = ""
                        TxtPassword.Focus()
                        Hitung = Hitung + 1

                    Else
                        MsgBox("Anda bukan user yang berhak!", vbCritical + vbOKOnly, "Warning")
                        Me.Close()
                    End If
                End If

            Else
                MsgBox("Anda belum terdaftar!!", vbCritical + vbOKOnly, "Wraning")
                TxtUser.Text = ""
                TxtPassword.Text = ""
                TxtUser.Focus()
            End If
        End If
    End Sub

    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub BtnBatal_Click(sender As Object, e As EventArgs) Handles BtnBatal.Click
        Pesan = MsgBox("Anda yakin mau keluar?", vbQuestion + vbYesNo, "Question")
        If Pesan = vbYes Then
            Me.Close()

        Else
            TxtPassword.Focus()
        End If
    End Sub

End Class