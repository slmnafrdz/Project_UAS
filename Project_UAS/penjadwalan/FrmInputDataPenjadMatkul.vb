Imports MySql.Data.MySqlClient

Public Class FrmInputDataPenjadMatkul


    ' Taruh koding ini di dalam FrmPenjaMatkul
    Sub ResetForm()
            ' 1. Bersihkan TextBox
            TxtKodePengampu.Clear()
            TxtKodePengampu.ReadOnly = False
            TxtJamAwal.Clear()
            TxtJamAkhir.Clear()

            ' 2. Bersihkan Label Detail (Ganti strip jadi kosong biar beneran ilang)
            LbKdDosen.Text = ""
            LbNidnDosen.Text = ""
            LbNamaDosen.Text = ""
            LbKodeMatkul.Text = ""
            LbNamaMatkul.Text = ""
            LbSKS.Text = "0"
            LbSKSTeori.Text = "0"
            LbSKSPraktek.Text = "0"
            LbSemester.Text = "0"

            ' 3. SAPU BERSIH ComboBox (Reset Index DAN Teksnya)
            ' Ini kunci biar datanya nggak nyangkut lagi bre
            Dim combos() As ComboBox = {CbNamaHari, CbRuangKelas, CmbTahunAkademik, CmbSemester, CbNamaJurusan}
            For Each cb In combos
                cb.SelectedIndex = -1
                cb.Text = "" ' Tambahin ini biar teks yang nangkring ilang total
            Next

            ' 4. Balikin status tombol
            btnSimpan.Enabled = False
            btnSimpan.Text = "&SIMPAN"
            btnSimpan.BackColor = Color.Red
            btnKeluar.Text = "&KELUAR"

            btnHapus.Enabled = False
            btnHapus.BackColor = Color.Red
        End Sub

        ' --- 1. PROSEDUR LOAD FORM ---
        Private Sub FrmInputDataPenjadMatkul_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        End Sub


        Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles BtnCari.Click
            If TxtKodePengampu.Text = "" Then
                MsgBox("Isi dulu Kode Pengampunya bre!", vbExclamation)
                Exit Sub
            End If

            Try
                Call KoneksiDB()

                ' 1. Validasi biar gak double input jadwal untuk pengampu yang sama
                Dim sqlCek As String = "SELECT COUNT(*) FROM tbl_jadwal_matkul WHERE KdPengampu = @kd"
                Dim cmdCek As New MySqlCommand(sqlCek, DbKoneksi)
                cmdCek.Parameters.AddWithValue("@kd", TxtKodePengampu.Text)

                If Convert.ToInt32(cmdCek.ExecuteScalar()) > 0 Then
                    MsgBox("Kode Ini SUDAH TERJADWAL!", vbInformation, "Informasi")
                    Exit Sub
                End If

                ' 2. Query Detail (Pastikan nama tabel & kolom sesuai sama yang di Navicat lo)
                Dim sqlAmbil As String = "SELECT d.Kd_Dosen, d.Nidn_Dosen, d.Nm_Dosen, m.Kd_Matakuliah, " &
                                 "m.Nm_Matakuliah, m.Sks_Matakuliah, m.Teori_Matakuliah, " &
                                 "m.Praktek_Matakuliah, m.Semester_Matakuliah " &
                                 "FROM tbl_pengampu_matakuliah dp " &
                                 "JOIN tbl_dosen d ON dp.Kd_Dosen = d.Kd_Dosen " &
                                 "JOIN tbl_matakuliah m ON dp.Kd_Matakuliah = m.Kd_Matakuliah " &
                                 "WHERE dp.KdPengampu = @kd"

                Dim cmdAmbil As New MySqlCommand(sqlAmbil, DbKoneksi)
                cmdAmbil.Parameters.AddWithValue("@kd", TxtKodePengampu.Text)

                Dim dr As MySqlDataReader = cmdAmbil.ExecuteReader
                If dr.Read() Then
                    ' Ngisi Label Otomatis (Sesuai image_ab8df7.jpg)
                    LbKdDosen.Text = dr("Kd_Dosen").ToString()
                    LbNidnDosen.Text = dr("Nidn_Dosen").ToString()
                    LbNamaDosen.Text = dr("Nm_Dosen").ToString()
                    LbKodeMatkul.Text = dr("Kd_Matakuliah").ToString()
                    LbNamaMatkul.Text = dr("Nm_Matakuliah").ToString()
                    LbSKS.Text = dr("Sks_Matakuliah").ToString()
                    LbSKSTeori.Text = dr("Teori_Matakuliah").ToString()
                    LbSKSPraktek.Text = dr("Praktek_Matakuliah").ToString()
                    LbSemester.Text = dr("Semester_Matakuliah").ToString()

                    MsgBox("Data Pengampu Ditemukan!", vbInformation)
                Else
                    MsgBox("Kode Pengampu tidak ditemukan!", vbExclamation)
                    Call BersihkanKotakBesar()
                End If
                dr.Close()
            Catch ex As Exception
                MsgBox("Error SQL: " & ex.Message)
            End Try
        End Sub


        ' Sub tambahan biar rapi
        Sub BersihkanKotakBesar()
            LbKdDosen.Text = ""
            LbNidnDosen.Text = ""
            LbNamaDosen.Text = ""
            LbKodeMatkul.Text = ""
            LbNamaMatkul.Text = ""
            LbSKS.Text = ""
            LbSKSTeori.Text = ""
            LbSKSPraktek.Text = ""
            LbSemester.Text = ""
        End Sub

        ' --- 4. LOGIKA CEK BENTROK (MENGIKUTI LOGIKA TEMAN ANDA) ---
        Function CekBentrokJadwal() As Boolean
            Try
                Call KoneksiDB()

                ' 1. DEFINISIKAN QUERY DASAR (sqlBase)
                ' Query ini buat nyari jadwal yang HARInya sama dan JAMnya bersinggungan/overlap
                Dim sqlBase As String = "SELECT j.JamAwal, j.JamAkhir, m.Nm_Matakuliah, r.Nm_Ruangan " &
                                "FROM tbl_jadwal_matkul j " &
                                "JOIN tbl_pengampu_matakuliah p ON j.KdPengampu = p.KdPengampu " &
                                "JOIN tbl_matakuliah m ON p.Kd_Matakuliah = m.Kd_Matakuliah " &
                                "JOIN tbl_ruangkelas r ON j.Kd_Ruangan = r.Kd_Ruangan " &
                                "WHERE j.Id_Hari = (SELECT Id_Hari FROM tbl_hari WHERE Nm_Hari=@hari LIMIT 1) " &
                                "AND (@awal < j.JamAkhir AND @akhir > j.JamAwal) " &
                                "AND j.KdPengampu <> @kd "

                ' A. CEK BENTROK RUANGAN (Ruangan Sama + Jam Tabrakan)
                Dim sqlRuang As String = sqlBase & " AND r.Nm_Ruangan = @ruang"
                Using cmdR As New MySqlCommand(sqlRuang, DbKoneksi)
                    cmdR.Parameters.AddWithValue("@hari", CbNamaHari.Text)
                    cmdR.Parameters.AddWithValue("@awal", TxtJamAwal.Text)
                    cmdR.Parameters.AddWithValue("@akhir", TxtJamAkhir.Text)
                    cmdR.Parameters.AddWithValue("@ruang", CbRuangKelas.Text)
                    cmdR.Parameters.AddWithValue("@kd", TxtKodePengampu.Text)

                    Using drR As MySqlDataReader = cmdR.ExecuteReader()
                        If drR.Read() Then
                            MsgBox("⚠ RUANGAN BENTROK!" & vbCrLf &
                           "Ruangan " & CbRuangKelas.Text & " sedang dipakai matkul: " & vbCrLf &
                           drR("Nm_Matakuliah").ToString(), vbCritical)
                            Return True
                        End If
                    End Using
                End Using

                ' B. CEK BENTROK DOSEN (Dosen Sama + Jam Tabrakan)
                ' Gunakan Label NIDN atau Kode Dosen lo di sini
                Dim sqlDosen As String = sqlBase & " AND p.Kd_Dosen = (SELECT Kd_Dosen FROM tbl_dosen WHERE Nm_Dosen=@nmDosen LIMIT 1)"
                Using cmdD As New MySqlCommand(sqlDosen, DbKoneksi)
                    cmdD.Parameters.AddWithValue("@hari", CbNamaHari.Text)
                    cmdD.Parameters.AddWithValue("@awal", TxtJamAwal.Text)
                    cmdD.Parameters.AddWithValue("@akhir", TxtJamAkhir.Text)
                    cmdD.Parameters.AddWithValue("@nmDosen", LbNamaDosen.Text)
                    cmdD.Parameters.AddWithValue("@kd", TxtKodePengampu.Text)

                    Using drD As MySqlDataReader = cmdD.ExecuteReader()
                        If drD.Read() Then
                            MsgBox("⚠ DOSEN BENTROK!" & vbCrLf &
                           "Dosen " & LbNamaDosen.Text & " sedang mengajar matkul: " & vbCrLf &
                           drD("Nm_Matakuliah").ToString(), vbCritical)
                            Return True
                        End If
                    End Using
                End Using

                Return False ' Kalau sampe sini berarti aman bre
            Catch ex As Exception
                MsgBox("Error Cek Bentrok: " & ex.Message)
                Return True
            End Try
        End Function

        ' --- 5. TOMBOL SIMPAN ---
        ' --- 5. TOMBOL SIMPAN / UBAH ---
        Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles BtnSimpan.Click
            Try
                If TxtKodePengampu.Text = "" Or CbRuangKelas.Text = "" Or CbNamaHari.Text = "" Then
                    MsgBox("Data belum lengkap bre, lengkapi dulu!", MsgBoxStyle.Exclamation)
                    Exit Sub
                End If

                If CekBentrokJadwal() = True Then Exit Sub
                Call KoneksiDB()

                If btnSimpan.Text = "SIMPAN" Or btnSimpan.Text = "&SIMPAN" Then
                    ' LOGIKA SIMPAN BARU
                    QUERY = "INSERT INTO tbl_jadwal_matkul (KdPengampu, Kd_Ruangan, Id_Hari, JamAwal, JamAkhir) " &
                    "VALUES (@kd, (SELECT Kd_Ruangan FROM tbl_ruangkelas WHERE Nm_Ruangan=@nmr LIMIT 1), " &
                    "(SELECT Id_Hari FROM tbl_hari WHERE Nm_Hari=@hari LIMIT 1), @awal, @akhir)"
                Else
                    ' LOGIKA UBAH DATA (INI YANG PENTING BRE!)
                    QUERY = "UPDATE tbl_jadwal_matkul SET " &
                    "Kd_Ruangan = (SELECT Kd_Ruangan FROM tbl_ruangkelas WHERE Nm_Ruangan=@nmr LIMIT 1), " &
                    "Id_Hari = (SELECT Id_Hari FROM tbl_hari WHERE Nm_Hari=@hari LIMIT 1), " &
                    "JamAwal = @awal, JamAkhir = @akhir " &
                    "WHERE KdPengampu = @kd"
                End If

                Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                    CMD.Parameters.AddWithValue("@kd", TxtKodePengampu.Text)
                    CMD.Parameters.AddWithValue("@nmr", CbRuangKelas.Text)
                    CMD.Parameters.AddWithValue("@hari", CbNamaHari.Text) ' Sekarang aman karena dihandle SELECT Id_Hari
                    CMD.Parameters.AddWithValue("@awal", TxtJamAwal.Text)
                    CMD.Parameters.AddWithValue("@akhir", TxtJamAkhir.Text)

                    CMD.ExecuteNonQuery()
                    MsgBox("Data Berhasil di " & btnSimpan.Text & " bre!", vbInformation)

                    ' Tutup form setelah berhasil biar balik ke Grid Utama
                    Me.Close()
                End Using

            Catch ex As Exception
                MsgBox("Error Simpan/Ubah: " & ex.Message)
            End Try
        End Sub

        Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
            If btnKeluar.Text = "&BATAL" Or btnKeluar.Text = "BATAL" Then
                Call ResetForm()
                TxtKodePengampu.Focus()
            Else
                Me.Close()
            End If
        End Sub

End Class