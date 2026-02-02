Imports MySql.Data.MySqlClient

Public Class FrmInputDataDosenPengampuMatakul
    Sub FormAktif()
        CbNamaJurusan.Enabled = True
        TxtNoIdentitas.Enabled = True
        TxtKdMatakuliah.Enabled = True
        LbSMTR.Enabled = True
        CbNamaSemester.Enabled = True
        CbKelas.Enabled = True
        TxtTahunAkademik.Enabled = True

        BtnCari.Enabled = True
        BtnCariKodeMatkul.Enabled = True
    End Sub

    Sub FormNonAktif()
        CbNamaJurusan.Enabled = False
        TxtNoIdentitas.Enabled = False
        TxtKdMatakuliah.Enabled = False
        LbSMTR.Enabled = False
        CbNamaSemester.Enabled = False
        CbKelas.Enabled = False
        TxtTahunAkademik.Enabled = False

        BtnCari.Enabled = False
        BtnCariKodeMatkul.Enabled = False
    End Sub

    Sub KosongkanData()
        LbKdPengampu.Text = ""

        CbNamaJurusan.SelectedIndex = -1
        CbNamaJurusan.Text = ""

        TxtNoIdentitas.Text = ""
        LbNIDN.Text = ""
        LbNamaDosen.Text = ""

        TxtKdMatakuliah.Text = ""
        LbNamaMatakuliah.Text = ""

        LbSKS.Text = ""
        LbSKSTeori.Text = ""
        LbSKSPraktek.Text = ""

        LbSMTR.Text = ""
        CbNamaSemester.SelectedIndex = -1
        CbNamaSemester.Text = ""
        CbKelas.SelectedIndex = -1
        CbKelas.Text = ""
        TxtTahunAkademik.Text = ""
    End Sub

    Sub FormNormal()
        Call FormAktif()

        btnSimpan.Text = "Simpan"
        btnSimpan.Enabled = True
        btnSimpan.BackColor = Color.CornflowerBlue

        BtnHapus.Enabled = False
        BtnHapus.BackColor = Color.Red

        btnKeluar.Text = "Keluar"
    End Sub


    Sub KosongkanCombobox()
        CbNamaJurusan.Items.Clear()
        CbNamaSemester.Items.Clear()
        CbKelas.Items.Clear()
    End Sub

    Sub TampilkanFilterNamaProdi()
        Call KoneksiDB()
        CMD = New MySqlCommand("SELECT Nm_Prodi FROM tbl_prodi ORDER BY Kd_Prodi", DbKoneksi)
        DR = CMD.ExecuteReader

        CbNamaJurusan.Items.Clear()
        Do While DR.Read
            CbNamaJurusan.Items.Add(DR.Item("Nm_Prodi"))
        Loop
        DR.Close()

        CbNamaJurusan.SelectedIndex = -1
        'LbKdProdi.Text = ""
    End Sub

    Public Property SemesterAwal As String = ""
    Public Property KelasAwal As String = ""
    Public Property KdProdiAwal As String = ""
    Public Property NamaProdiAwal As String = ""
    Public Property KodePengampuAwal As String = ""

    Sub IsiComboSemesterInput()
        'INI yang bikin list semester muncul
        CbNamaSemester.Items.Clear()
        CbNamaSemester.Items.Add("GANJIL")
        CbNamaSemester.Items.Add("GENAP")
        CbNamaSemester.SelectedIndex = -1
    End Sub

    Sub IsiComboKelasInput()
        CbKelas.Items.Clear()
        CbKelas.Items.Add("Reguler")
        CbKelas.Items.Add("Karyawan")
        CbKelas.Items.Add("Malam")
        CbKelas.SelectedIndex = -1
    End Sub


    Function BuatKodePengampu() As String
        Call KoneksiDB()
        Try
            Dim Prefix As String = "PMK"
            Dim hasil As Object

            'Ambil kode terbesar
            CMD = New MySqlCommand("SELECT MAX(KdPengampu) FROM tbl_pengampu_matakuliah WHERE LEFT(KdPengampu,3)='" & Prefix & "'", DbKoneksi)
            hasil = CMD.ExecuteScalar()

            Dim noUrut As Integer = 1
            If hasil IsNot Nothing AndAlso Not IsDBNull(hasil) Then
                noUrut = CInt(Microsoft.VisualBasic.Right(hasil.ToString(), 4)) + 1
            End If

            Return Prefix & noUrut.ToString("0000")

        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan saat membuat Kode Pengampu: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return "PMK0001"
        End Try
    End Function


    Sub CariDosenByNoIdentitas()
        If TxtNoIdentitas.Text.Trim() = "" Then Exit Sub

        Call KoneksiDB()
        Try
            Dim nmProdi As String = CbNamaJurusan.Text.Trim()

            Dim q As String =
        "SELECT d.Nidn_Dosen, d.Nm_Dosen " &
        "FROM tbl_dosen d " &
        "INNER JOIN tbl_prodi p ON d.Kd_Prodi = p.Kd_Prodi " &
        "WHERE d.Kd_Dosen = @KdDosen AND p.Nm_Prodi = @NmProdi LIMIT 1"

            Using cmd As New MySqlCommand(q, DbKoneksi)
                cmd.Parameters.AddWithValue("@KdDosen", TxtNoIdentitas.Text.Trim())
                cmd.Parameters.AddWithValue("@NmProdi", nmProdi)

                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        LbNIDN.Text = dr("Nidn_Dosen").ToString()
                        LbNamaDosen.Text = dr("Nm_Dosen").ToString()
                    Else
                        LbNIDN.Text = ""
                        LbNamaDosen.Text = ""
                        MsgBox("Kode dosen tidak ditemukan / bukan dosen Prodi " & nmProdi, vbExclamation)
                    End If
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat cari dosen: " & ex.Message, vbCritical)
        End Try
    End Sub


    Private Sub CariMatkulByKode()
        If TxtKdMatakuliah.Text.Trim() = "" Then
            MsgBox("Kode Matakuliah belum diisi!", vbExclamation, "Validasi")
            TxtKdMatakuliah.Focus()
            Exit Sub
        End If

        Try
            Call KoneksiDB()

            Dim query As String =
        "SELECT Nm_Matakuliah, Sks_Matakuliah, Teori_Matakuliah, Praktek_Matakuliah, Semester_Matakuliah " &
        "FROM tbl_matakuliah " &
        "WHERE Kd_Matakuliah = @KdMatkul " &
        "LIMIT 1"

            Using cmd As New MySqlCommand(query, DbKoneksi)
                cmd.Parameters.AddWithValue("@KdMatkul", TxtKdMatakuliah.Text.Trim())

                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        LbNamaMatakuliah.Text = dr("Nm_Matakuliah").ToString()
                        LbSKS.Text = dr("Sks_Matakuliah").ToString()
                        LbSKSTeori.Text = dr("Teori_Matakuliah").ToString()
                        LbSKSPraktek.Text = dr("Praktek_Matakuliah").ToString()
                        LbSMTR.Text = dr("Semester_Matakuliah").ToString()
                    Else
                        LbNamaMatakuliah.Text = ""
                        LbSKS.Text = ""
                        LbSKSTeori.Text = ""
                        LbSKSPraktek.Text = ""
                        LbSMTR.Text = ""
                        MsgBox("Data matakuliah tidak ditemukan!", vbInformation, "Informasi")
                    End If
                End Using
            End Using

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat cari matakuliah: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub FrmDosenPengampu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KoneksiDB()

        Call TampilkanFilterNamaProdi()
        Call IsiComboSemesterInput()
        Call IsiComboKelasInput()

        If LbKdPengampu.Text.Trim() = "" Then
            LbKdPengampu.Text = BuatKodePengampu()
        End If

        If NamaProdiAwal.Trim() <> "" Then
            CbNamaJurusan.Text = NamaProdiAwal
            CbNamaJurusan.Enabled = False
        End If

        Dim sem As String = SemesterAwal.Trim().ToUpper()
        If sem = "" OrElse sem = "ALL" Then
            CbNamaSemester.SelectedIndex = -1
            CbNamaSemester.Text = ""
        Else
            CbNamaSemester.Text = SemesterAwal.Trim()
        End If

        'Default tombol
        'Call FormNormal()
    End Sub

    Private Sub CbNamaJurusan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaJurusan.SelectedIndexChanged
        If CbNamaJurusan.SelectedIndex = -1 Then
            'LbKdProdi.Text = ""
            Exit Sub
        End If

        Call KoneksiDB()
        Try
            CMD = New MySqlCommand("SELECT Kd_Prodi FROM tbl_prodi WHERE Nm_Prodi='" & CbNamaJurusan.Text & "'", DbKoneksi)
            DR = CMD.ExecuteReader
            If DR.Read() Then
                'LbKdProdi.Text = DR("Kd_Prodi").ToString()
            Else
                'LbKdProdi.Text = ""
            End If
            DR.Close()
        Catch ex As Exception
            Try
                If DR IsNot Nothing AndAlso Not DR.IsClosed Then DR.Close()
            Catch
            End Try
            MsgBox("Terjadi kesalahan saat ambil kode prodi: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub

    Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles BtnCari.Click
        Call CariDosenByNoIdentitas()
    End Sub

    Private Sub TxtNoIdentitas_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtNoIdentitas.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            CariDosenByNoIdentitas()
        End If
    End Sub

    Private Sub BtnCariKodeMatkul_Click(sender As Object, e As EventArgs) Handles BtnCariKodeMatkul.Click
        Call CariMatkulByKode()
    End Sub

    Private Sub TxtKdMatakuliah_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtKdMatakuliah.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Call CariMatkulByKode()
        End If
    End Sub

    Private Sub CbNamaSemester_DropDown(sender As Object, e As EventArgs) Handles CbNamaSemester.DropDown
        'Kalau list sempat kosong, isi ulang
        If CbNamaSemester.Items.Count = 0 Then
            Call IsiComboSemesterInput()
        End If
    End Sub


    Private Sub BtnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        Call KoneksiDB()

        'Validasi
        If CbNamaJurusan.Text.Trim() = "" Then
            MsgBox("Jurusan wajib dipilih!", vbExclamation, "Validasi")
            Exit Sub
        End If

        If TxtNoIdentitas.Text.Trim() = "" OrElse LbNIDN.Text.Trim() = "" OrElse LbNamaDosen.Text.Trim() = "" Then
            MsgBox("Data dosen belum lengkap!!.", vbExclamation, "Validasi")
            Exit Sub
        End If

        If TxtKdMatakuliah.Text.Trim() = "" OrElse LbNamaMatakuliah.Text.Trim() = "" Then
            MsgBox("Matakuliah belum dipilih!!.", vbExclamation, "Validasi")
            Exit Sub
        End If

        If CbNamaSemester.Text.Trim() = "" Then
            MsgBox("Nama Semester wajib dipilih!", vbExclamation, "Validasi")
            Exit Sub
        End If

        If CbKelas.Text.Trim() = "" Then
            MsgBox("Kelas wajib dipilih!", vbExclamation, "Validasi")
            Exit Sub
        End If

        If TxtTahunAkademik.Text.Trim() = "" Then
            MsgBox("Tahun Akademik wajib diisi!", vbExclamation, "Validasi")
            Exit Sub
        End If

        Dim cekQ As String = "SELECT COUNT(*) FROM tbl_dosen d " &
                        "INNER JOIN tbl_prodi p ON d.Kd_Prodi = p.Kd_Prodi " &
                        "WHERE d.Kd_Dosen=@KdDosen AND p.Nm_Prodi=@NmProdi"

        Using cekCmd As New MySqlCommand(cekQ, DbKoneksi)
            cekCmd.Parameters.AddWithValue("@KdDosen", TxtNoIdentitas.Text.Trim())
            cekCmd.Parameters.AddWithValue("@NmProdi", CbNamaJurusan.Text.Trim())

            If CInt(cekCmd.ExecuteScalar()) = 0 Then
                MsgBox("Kode dosen ini bukan milik Prodi " & CbNamaJurusan.Text & ". Simpan dibatalkan.", vbExclamation)
                Exit Sub
            End If
        End Using


        Try
            If btnSimpan.Text = "Simpan" Then
                Dim q As String = "INSERT INTO tbl_pengampu_matakuliah " &
                               "(KdPengampu, Kd_Dosen, Kd_Matakuliah, Nama_Kelas, Tahun_Akademik) " &
                               "VALUES (@KdPengampu, @KdDosen, @KdMatkul, @Kelas, @TahunAkademik)"

                Using cmd As New MySqlCommand(q, DbKoneksi)
                    cmd.Parameters.AddWithValue("@KdPengampu", LbKdPengampu.Text.Trim())
                    cmd.Parameters.AddWithValue("@KdDosen", TxtNoIdentitas.Text.Trim())
                    cmd.Parameters.AddWithValue("@KdMatkul", TxtKdMatakuliah.Text.Trim())
                    cmd.Parameters.AddWithValue("@Kelas", CbKelas.Text.Trim())
                    cmd.Parameters.AddWithValue("@TahunAkademik", TxtTahunAkademik.Text.Trim())
                    cmd.ExecuteNonQuery()
                End Using

                MsgBox("Data pengampu berhasil ditambahkan!", vbInformation, "Sukses")
                Me.Close()

            ElseIf btnSimpan.Text = "Ubah" Then
                Call KoneksiDB()

                Dim SQLUpdate As String = "UPDATE tbl_pengampu_matakuliah SET " &
                                          "Kd_Dosen = @KdDosen, " &
                                          "Kd_Matakuliah = @KdMatakuliah, " &
                                          "Nama_Kelas = @NamaKelas, " &
                                          "Tahun_Akademik = @TahunAkademik " &
                                          "WHERE KdPengampu = @KdPengampu"

                CMD = New MySqlCommand(SQLUpdate, DbKoneksi)
                CMD.Parameters.Clear()

                CMD.Parameters.AddWithValue("@KdDosen", TxtNoIdentitas.Text.Trim())
                CMD.Parameters.AddWithValue("@KdMatakuliah", TxtKdMatakuliah.Text.Trim())
                CMD.Parameters.AddWithValue("@NamaKelas", CbKelas.Text.Trim())
                CMD.Parameters.AddWithValue("@TahunAkademik", TxtTahunAkademik.Text.Trim())
                CMD.Parameters.AddWithValue("@KdPengampu", LbKdPengampu.Text.Trim())

                Dim rows As Integer = CMD.ExecuteNonQuery()

                If rows > 0 Then
                    MsgBox("Data berhasil diubah!", vbInformation)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    MsgBox("Data tidak berubah. Pastikan Kode Pengampu benar atau nilai yang diubah memang berbeda.", vbExclamation)
                End If

                Exit Sub
            End If


        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat menyimpan data: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub


    Private Sub BtnHapus_Click(sender As Object, e As EventArgs) Handles BtnHapus.Click
        Dim konfirmasi As String = MsgBox("Anda yakin akan menghapus data ini?", vbYesNo + vbQuestion, "Konfirmasi Hapus")
        If konfirmasi <> vbYes Then Exit Sub

        Try
            If DR IsNot Nothing AndAlso Not DR.IsClosed Then DR.Close()
        Catch
        End Try

        Call KoneksiDB()

        Try
            Dim q As String = "DELETE FROM tbl_pengampu_matakuliah WHERE KdPengampu = @KdPengampu"
            Using cmd As New MySqlCommand(q, DbKoneksi)
                cmd.Parameters.AddWithValue("@KdPengampu", LbKdPengampu.Text.Trim())
                cmd.ExecuteNonQuery()
            End Using

            MsgBox("Data pengampu berhasil dihapus!", vbInformation, "Sukses")

            Try
                With FrmDataDosenPengampuMatakul
                    .TxtKdDosen.Text = ""
                    .lbNidn.Text = ""
                    .LbNamaDosen.Text = ""

                    .DataGridPengampu.Enabled = False

                    'refresh data grid terbaru
                    .RefreshPaging()
                End With
            Catch
            End Try

            'tutup form input
            Me.Close()

        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat menghapus data: " & ex.Message, vbCritical, "Error")
        End Try
    End Sub


    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        If btnKeluar.Text = "Keluar" Then
            Dim pesan = MsgBox("Apakah Anda yakin ingin keluar?", vbQuestion + vbYesNo, "Informasi")
            If pesan = vbYes Then
                Me.Close()
            End If
            Exit Sub
        End If

        If btnKeluar.Text = "Batal" Then
            Dim pesan = MsgBox("Batalkan perubahan data ini?", vbQuestion + vbYesNo, "Konfirmasi")
            If pesan = vbYes Then
                Call FormNormal()
                Call KosongkanData()
                btnKeluar.Text = "Keluar"
            End If
        End If
    End Sub

End Class
