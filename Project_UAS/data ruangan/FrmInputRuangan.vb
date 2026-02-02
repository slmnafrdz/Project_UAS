Imports MySql.Data.MySqlClient

Public Class FrmInputRuangan

    ' Poin 5: Interaksi Objek - Memisahkan logika bisnis dari GUI [cite: 22, 23, 28]
    Private objRuang As New ClsRuangan()

    Sub BuatKodeRuanganOtomatis()
        Try
            Call KoneksiDB()
            QUERY = "SELECT Kd_Ruangan FROM tbl_ruangkelas ORDER BY Kd_Ruangan DESC LIMIT 1"
            CMD = New MySqlCommand(QUERY, DbKoneksi)
            DR = CMD.ExecuteReader()
            Dim nomor As Integer = 1
            If DR.Read() Then
                nomor = CInt(Microsoft.VisualBasic.Right(DR("Kd_Ruangan").ToString(), 4)) + 1
            End If
            DR.Close()
            TxtKdRuangan.Text = "R" & Microsoft.VisualBasic.Right("0000" & nomor, 4)
        Catch ex As Exception
        End Try
    End Sub

    Sub KosongkanData()
        TxtKdRuangan.Clear()
        TxtNamaRuangan.Clear()
        TxtKapasitas.Clear()
    End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
        ' --- POIN 5: MENGIRIM INPUT KE OBJEK --- [cite: 23]
        objRuang.Kode = TxtKdRuangan.Text
        objRuang.Nama = TxtNamaRuangan.Text
        objRuang.Kapasitas = Val(TxtKapasitas.Text)

        If TxtNamaRuangan.Text = "" Then
            MsgBox("Nama ruangan wajib diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            Call KoneksiDB()
            If BtnSimpan.Text = "&SIMPAN" Then
                QUERY = "INSERT INTO tbl_ruangkelas VALUES (@kd, @nm, @kp)"
            Else
                QUERY = "UPDATE tbl_ruangkelas SET Nm_Ruangan=@nm, Jml_Kapasitas=@kp WHERE Kd_Ruangan=@kd"
            End If

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            CMD.Parameters.AddWithValue("@kd", objRuang.Kode)
            CMD.Parameters.AddWithValue("@nm", objRuang.Nama)
            CMD.Parameters.AddWithValue("@kp", objRuang.Kapasitas)
            CMD.ExecuteNonQuery()

            ' Menggunakan Fungsi Overriding Info() [cite: 18]
            MsgBox(objRuang.Info() & " Berhasil Disimpan!", vbInformation)

            Me.Close()
            FrmDataRuangan.RefreshPaging()
        Catch ex As Exception
            MsgBox("Gagal: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnHapus_Click(sender As Object, e As EventArgs) Handles BtnHapus.Click
        If MsgBox("Hapus data ini?", vbQuestion + vbYesNo) = vbYes Then
            Call KoneksiDB()
            CMD = New MySqlCommand("DELETE FROM tbl_ruangkelas WHERE Kd_Ruangan=@kd", DbKoneksi)
            CMD.Parameters.AddWithValue("@kd", TxtKdRuangan.Text)
            CMD.ExecuteNonQuery()
            Me.Close()
            FrmDataRuangan.RefreshPaging()
        End If
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        Me.Close()
    End Sub
End Class


