Imports MySql.Data.MySqlClient

Public Class FrmInputHari

    Private objH As New ClsHari()

    Sub KosongkanData()
        TxtKodeHari.Clear()
        TxtNamaHari.Clear()
    End Sub

    Sub BuatKodeHariOtomatis()
        Try
            Call KoneksiDB()
            QUERY = "SELECT Id_Hari FROM tbl_hari ORDER BY Id_Hari DESC LIMIT 1"
            CMD = New MySqlCommand(QUERY, DbKoneksi)
            DR = CMD.ExecuteReader()
            Dim nomor As Integer = 1
            If DR.Read() Then
                nomor = CInt(Microsoft.VisualBasic.Right(DR("Id_Hari").ToString(), 3)) + 1
            End If
            DR.Close()
            TxtKodeHari.Text = "HR" & Microsoft.VisualBasic.Right("000" & nomor, 3)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
        ' Masukkan input ke objek sesuai poin 5 UAS
        objH.Id = TxtKodeHari.Text
        objH.Nama = TxtNamaHari.Text

        If objH.Nama = "ERROR" Then
            MsgBox("Nama Hari tidak valid!", vbCritical)
            Exit Sub
        End If

        Try
            Call KoneksiDB()

            ' LOGIKA: Cek apakah Tombolnya SIMPAN atau UBAH
            If BtnSimpan.Text = "&SIMPAN" Then
                QUERY = "INSERT INTO tbl_hari (Id_Hari, Nm_Hari) VALUES (@id, @nm)"
            Else
                QUERY = "UPDATE tbl_hari SET Nm_Hari=@nm WHERE Id_Hari=@id"
            End If

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            CMD.Parameters.AddWithValue("@id", objH.Id)
            CMD.Parameters.AddWithValue("@nm", objH.Nama)
            CMD.ExecuteNonQuery()

            MsgBox("Berhasil! " & objH.Info(), vbInformation)
            Me.Close()
            FrmDataHari.RefreshPaging()
        Catch ex As Exception
            MsgBox("Gagal Simpan/Update: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnHapus_Click(sender As Object, e As EventArgs) Handles BtnHapus.Click
        If MsgBox("Yakin ingin menghapus data ini?", vbYesNo + vbQuestion) = vbYes Then
            Try
                Call KoneksiDB()
                QUERY = "DELETE FROM tbl_hari WHERE Id_Hari=@id"
                CMD = New MySqlCommand(QUERY, DbKoneksi)
                CMD.Parameters.AddWithValue("@id", TxtKodeHari.Text)
                CMD.ExecuteNonQuery()

                Me.Close()
                FrmDataHari.RefreshPaging()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        Me.Close()
    End Sub
End Class
