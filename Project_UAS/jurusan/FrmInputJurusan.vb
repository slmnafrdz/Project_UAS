Imports MySql.Data.MySqlClient

Public Class FrmInputJurusan

    ' Poin 5: Instansiasi objek untuk memisahkan logika dari Form
    Private prodiObj As New ClsJurusan()

        Sub BuatKodeProdi()
            Call KoneksiDB()
            Try
                Dim SQL As String = "SELECT Kd_Prodi FROM tbl_prodi ORDER BY Kd_Prodi DESC LIMIT 1"
                CMD = New MySqlCommand(SQL, DbKoneksi)
                Dim DRX As MySqlDataReader = CMD.ExecuteReader()
                Dim Hitung As Integer = 1
                If DRX.Read() Then
                    Hitung = CInt(Microsoft.VisualBasic.Right(DRX.GetString(0), 3)) + 1
                End If
                DRX.Close()
                TxtKdprodi.Text = "PR" & Microsoft.VisualBasic.Right("000" & Hitung, 3)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub

        Sub Kosongkan()
            TxtKdprodi.Clear()
            TxtNamaProdi.Clear()
        End Sub

        Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
            ' Poin 5: Form hanya bertugas mengambil input dan mengirim ke Objek
            prodiObj.Kode = TxtKdprodi.Text
            prodiObj.Nama = TxtNamaProdi.Text

            If TxtNamaProdi.Text = "" Then
                MsgBox("Nama Jurusan tidak boleh kosong!", vbExclamation)
                Exit Sub
            End If

            Try
                Call KoneksiDB()
                If BtnSimpan.Text = "&SIMPAN" Then
                    SQLinsert = "INSERT INTO tbl_prodi VALUES (@kd, @nm)"
                    CMD = New MySqlCommand(SQLinsert, DbKoneksi)
                Else
                    SQLUpdate = "UPDATE tbl_prodi SET Nm_Prodi=@nm WHERE Kd_Prodi=@kd"
                    CMD = New MySqlCommand(SQLUpdate, DbKoneksi)
                End If

                CMD.Parameters.AddWithValue("@kd", prodiObj.Kode)
                CMD.Parameters.AddWithValue("@nm", prodiObj.Nama)
                CMD.ExecuteNonQuery()

                ' Memakai Fungsi Polimorfisme Info()
                MsgBox(prodiObj.Info() & " Berhasil Tersimpan!", vbInformation)

                Me.Close()
                FrmDataJurusan.RefreshPaging()
            Catch ex As Exception
                MsgBox("Gagal simpan: " & ex.Message)
            End Try
        End Sub

        Private Sub BtnHapus_Click(sender As Object, e As EventArgs) Handles BtnHapus.Click
            If MsgBox("Yakin hapus data ini?", vbQuestion + vbYesNo) = vbYes Then
                Call KoneksiDB()
                CMD = New MySqlCommand("DELETE FROM tbl_prodi WHERE Kd_Prodi=@kd", DbKoneksi)
                CMD.Parameters.AddWithValue("@kd", TxtKdprodi.Text)
                CMD.ExecuteNonQuery()
                Me.Close()
                FrmDataJurusan.RefreshPaging()
            End If
        End Sub

        Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
            Me.Close()
        End Sub

        Private Sub FrmJurusan_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        End Sub
    End Class