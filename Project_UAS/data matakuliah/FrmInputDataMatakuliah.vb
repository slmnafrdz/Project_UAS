Imports MySql.Data.MySqlClient

Public Class FrmInputDataMatakuliah

    Sub KosongkanData()
            TxtKodeMatkul.Clear()
            TxtNamaMatkul.Clear()
            CmbSks.SelectedIndex = -1
            TxtTeoriMatkul.Text = "0"
            TxtPraktekMatkul.Text = "0"
            CbNamaSemester.SelectedIndex = -1
            CmbSemester.Items.Clear()
            CmbSemester.Text = ""
            ' --- TAMBAHAN: Kosongin Jurusan ---
            CbJurusan.SelectedIndex = -1
            CbJurusan.Text = ""

            ' Reset kotak kuning hasil hitung
            TxtHasilTeori.Text = "0"
            TxtHasilPraktek.Text = "0"
            TxtTotalSemua.Text = "0"
        End Sub

        Sub FormNormal()
            Call KosongkanData()

        BtnSimpan.Text = "&SIMPAN"
            BtnSimpan.BackColor = Color.Red

            ' Tombol Keluar standar
            BtnKeluar.Text = "&KELUAR"
            BtnKeluar.BackColor = Color.CornflowerBlue

            ' Tombol Hapus MATI dan MERAH
            BtnHapus.Enabled = False
            BtnHapus.BackColor = Color.Red
        End Sub

        Sub HitungMenitOtomatis()
            Try
                Dim teori As Integer = If(IsNumeric(TxtTeoriMatkul.Text), CInt(TxtTeoriMatkul.Text), 0)
                Dim praktek As Integer = If(IsNumeric(TxtPraktekMatkul.Text), CInt(TxtPraktekMatkul.Text), 0)

                Dim mntTeori As Integer = teori * 50
                Dim mntPraktek As Integer = praktek * 120
                Dim total As Integer = mntTeori + mntPraktek

                TxtHasilTeori.Text = mntTeori.ToString()
                TxtHasilPraktek.Text = mntPraktek.ToString()
                TxtTotalSemua.Text = total.ToString()
            Catch ex As Exception
            End Try
        End Sub

        Private Sub FrmMatakuliah_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            If CbJurusan.Items.Count = 0 Then CbJurusan.Items.Add("TEKNOLOGI REKAYASA PERANGKAT LUNAK (D4)")
            If CbNamaSemester.Items.Count = 0 Then CbNamaSemester.Items.AddRange(New Object() {"GANJIL", "GENAP"})
            If CmbSks.Items.Count = 0 Then CmbSks.Items.AddRange(New Object() {"1", "2", "3", "4"})

            Call HitungMenitOtomatis()
        End Sub

        Private Sub CbNamaSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaSemester.SelectedIndexChanged
            ' Simpan nilai yang terpilih sekarang (kalau ada)
            Dim NilaiSkrg As String = CmbSemester.Text

            CmbSemester.Items.Clear()
            If CbNamaSemester.Text = "GANJIL" Then
                For i As Integer = 1 To 7 Step 2 : CmbSemester.Items.Add(i.ToString()) : Next
            ElseIf CbNamaSemester.Text = "GENAP" Then
                For i As Integer = 2 To 8 Step 2 : CmbSemester.Items.Add(i.ToString()) : Next
            End If

            ' Cek apakah nilai lama masih valid di list baru, kalau iya balikin
            If CmbSemester.Items.Contains(NilaiSkrg) Then
                CmbSemester.Text = NilaiSkrg
            End If
        End Sub

        Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
            If BtnKeluar.Text = "&BATAL" Then
                ' Kalau tulisannya BATAL, hapus semua inputan (Kosongin)
                Call FormNormal()
                TxtKodeMatkul.Focus()
            Else

                Me.Close()
            FrmDataMatakuliah.Enabled = True
        End If
        End Sub

        Private Sub TxtTeoriMatkul_TextChanged(sender As Object, e As EventArgs) Handles TxtTeoriMatkul.TextChanged
            Call HitungMenitOtomatis()
        End Sub

        Private Sub TxtPraktekMatkul_TextChanged(sender As Object, e As EventArgs) Handles TxtPraktekMatkul.TextChanged
            Call HitungMenitOtomatis()
        End Sub

    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
        ' 1. Validasi input: Jangan biarkan kosong
        If TxtKodeMatkul.Text = "" Or TxtNamaMatkul.Text = "" Or CmbSemester.Text = "" Then
            MsgBox("Data (Kode, Nama, Semester) harus diisi!", vbExclamation)
            Exit Sub
        End If

        Try
            Call KoneksiDB()

            ' 2. Deteksi aksi berdasarkan teks tombol (Ubah atau Simpan)
            Dim teksTombol As String = BtnSimpan.Text.Replace("&", "").ToUpper()

            ' Tentukan Query hanya SEKALI di sini
            If teksTombol = "SIMPAN" Then
                QUERY = "INSERT INTO tbl_matakuliah VALUES (@kd, @nm, @sks, @teori, @prak, @sem)"
            ElseIf teksTombol = "UBAH" Then
                ' Penting: WHERE menggunakan @kd (Kode Matakuliah)
                QUERY = "UPDATE tbl_matakuliah SET Nm_Matakuliah=@nm, Sks_Matakuliah=@sks, " &
                    "Teori_Matakuliah=@teori, Praktek_Matakuliah=@prak, Semester_Matakuliah=@sem " &
                    "WHERE Kd_Matakuliah=@kd"
            End If

            CMD = New MySqlCommand(QUERY, DbKoneksi)

            ' 3. Masukkan Parameter
            CMD.Parameters.AddWithValue("@kd", TxtKodeMatkul.Text)
            CMD.Parameters.AddWithValue("@nm", TxtNamaMatkul.Text)
            CMD.Parameters.AddWithValue("@sks", CmbSks.Text)
            CMD.Parameters.AddWithValue("@teori", TxtTeoriMatkul.Text)
            CMD.Parameters.AddWithValue("@prak", TxtPraktekMatkul.Text)
            CMD.Parameters.AddWithValue("@sem", CmbSemester.Text)

            ' 4. Eksekusi
            Dim barisTerpengaruh As Integer = CMD.ExecuteNonQuery()

            If barisTerpengaruh > 0 Then
                MsgBox("Data berhasil diproses!", vbInformation)
                FrmDataMatakuliah.TampilkanDataGridMatakuliah() ' Refresh Grid di Form Utama
                Me.Close() ' Tutup form input
            Else
                ' Jika 0 baris terpengaruh, berarti WHERE @kd tidak ditemukan
                MsgBox("Gagal! Data dengan Kode: " & TxtKodeMatkul.Text & " tidak ditemukan.", vbCritical)
            End If

        Catch ex As Exception
            MsgBox("Waduh Gagal: " & ex.Message, vbCritical)
        End Try
    End Sub
    Private Sub FrmMatakuliah_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        FrmDataMatakuliah.Enabled = True
        FrmDataMatakuliah.BringToFront()
    End Sub

        Public Sub IsiAngkaSemesterByStatus()
            CmbSemester.Items.Clear()
            If CbNamaSemester.Text = "GANJIL" Then
                CmbSemester.Items.AddRange({"1", "3", "5", "7"})
            ElseIf CbNamaSemester.Text = "GENAP" Then
                CmbSemester.Items.AddRange({"2", "4", "6", "8"})
            End If
        End Sub

        Private Sub BtnHapus_Click(sender As Object, e As EventArgs) Handles BtnHapus.Click
            If TxtKodeMatkul.Text = "" Then
                MsgBox("Pilih dulu data yang mau dihapus!", vbExclamation)
                Exit Sub
            End If

            Dim Pesan As DialogResult = MsgBox("Yakin mau hapus data: " & TxtNamaMatkul.Text & "?", vbQuestion + vbYesNo, "Konfirmasi Hapus")

            If Pesan = vbYes Then
                Try
                    Call KoneksiDB()
                    QUERY = "DELETE FROM tbl_matakuliah WHERE Kd_Matakuliah = @kd"
                    CMD = New MySqlCommand(QUERY, DbKoneksi)
                    CMD.Parameters.AddWithValue("@kd", TxtKodeMatkul.Text)
                    CMD.ExecuteNonQuery()

                    MsgBox("Data berhasil dihapus!", vbInformation)

                ' INI PENTING: Refresh grid dan balikin form ke kondisi normal
                FrmDataMatakuliah.TampilkanDataGridMatakuliah()
                Call FormNormal() ' Form jadi kosong lagi, tombol hapus jadi merah lagi

                Catch ex As Exception
                    MsgBox("Gagal Hapus: " & ex.Message, vbCritical)
                End Try
            End If
        End Sub


End Class
