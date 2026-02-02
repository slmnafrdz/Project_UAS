Imports MySql.Data.MySqlClient
Public Class FrmDataDosenPengampuMatakul

    ' PAGING VARIABLES
    Dim CurrentPage As Integer = 1
    Dim PageSize As Integer = 10
    Dim TotalData As Integer = 0
    Dim TotalPage As Integer = 0
    Dim Offset As Integer = 0

    'Cache Paging
    Dim PageCache As New Dictionary(Of String, DataTable)
    Dim LastCacheKey As String = ""

    Private isloading As Boolean = True

    ' CACHE KEY
    Function GEtCacheKey(Page As Integer) As String
        Dim Prodi As String = If(CbNamaJurusan.SelectedValue Is Nothing, "", CbNamaJurusan.SelectedValue.ToString())
        Dim NamaSemester As String = CbNamaSemester.Text.Trim()
        Dim KdDosen As String = TxtKdDosen.Text.Trim()
        Dim Limit As String = ComboBoxEntries.Text
        Return $"{Prodi}|{NamaSemester}|{KdDosen}|{Limit}|PAGE:{Page}"
    End Function

    Sub ClearPagingCache()
        PageCache.Clear()
        LastCacheKey = ""
    End Sub

    ' ENTRIES PER PAGE
    Sub NumberEntriesHalaman()
        ComboBoxEntries.Items.Clear()
        ComboBoxEntries.Items.Add("10")
        ComboBoxEntries.Items.Add("15")
        ComboBoxEntries.Items.Add("20")
        ComboBoxEntries.Items.Add("25")
        ComboBoxEntries.Items.Add("50")
        ComboBoxEntries.Items.Add("100")
        ComboBoxEntries.SelectedIndex = 0
    End Sub

    ' ROW NUMBER (HEADER)
    Private Sub FormatRowHeader(grid As DataGridView, startIndex As Integer, pageSize As Integer)
        grid.SuspendLayout()
        grid.AllowUserToAddRows = False
        grid.RowHeadersVisible = True
        grid.RowHeadersWidth = 58
        grid.RowHeadersDefaultCellStyle.Font = New Font("Arial", 8, FontStyle.Bold)

        For i As Integer = 0 To grid.Rows.Count - 1
            grid.Rows(i).HeaderCell.Value = (startIndex + i + 1).ToString()
        Next
        grid.ResumeLayout()
    End Sub

    Private Sub DataGridPengampu_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DataGridPengampu.DataBindingComplete
        For i As Integer = 0 To DataGridPengampu.Rows.Count - 1
            DataGridPengampu.Rows(i).HeaderCell.Value = (Offset + i + 1).ToString()
        Next
    End Sub

    Sub AktifDataGridPengampu()
        With DataGridPengampu
            .EnableHeadersVisualStyles = False

            'mengatur propertis pada data grid header'
            .Font = New Font(DataGridPengampu.Font, FontStyle.Bold)
            DataGridPengampu.DefaultCellStyle.Font = New Font("Arial", 9)
            DataGridPengampu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            DataGridPengampu.ColumnHeadersHeight = 35

            'memberikan nama pada header KODE PENGAMPU'
            DataGridPengampu.Columns(0).Width = 80
            DataGridPengampu.Columns(0).HeaderText = "KODE PENGAMPU"
            DataGridPengampu.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header KODE DOSEN'
            DataGridPengampu.Columns(1).Width = 80
            DataGridPengampu.Columns(1).HeaderText = "KODE DOSEN"
            DataGridPengampu.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header NIDN'
            DataGridPengampu.Columns(2).Width = 110
            DataGridPengampu.Columns(2).HeaderText = "NIDN"
            DataGridPengampu.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header NAMA DOSEN'
            DataGridPengampu.Columns(3).Width = 170
            DataGridPengampu.Columns(3).HeaderText = "NAMA DOSEN"
            DataGridPengampu.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header KODE MATAKULIAH'
            DataGridPengampu.Columns(4).Width = 120
            DataGridPengampu.Columns(4).HeaderText = "KODE MATAKULIAH"
            DataGridPengampu.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header NAMA MATAKULIAH'
            DataGridPengampu.Columns(5).Width = 260
            DataGridPengampu.Columns(5).HeaderText = "NAMA MATAKULIAH"
            DataGridPengampu.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header SKS'
            DataGridPengampu.Columns(6).Width = 50
            DataGridPengampu.Columns(6).HeaderText = "SKS"
            DataGridPengampu.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header SKS TEORI'
            DataGridPengampu.Columns(7).Width = 80
            DataGridPengampu.Columns(7).HeaderText = "SKS TEORI"
            DataGridPengampu.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header NIM'
            DataGridPengampu.Columns(8).Width = 90
            DataGridPengampu.Columns(8).HeaderText = "SKS SEMESTER"
            DataGridPengampu.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header SEMESTER'
            DataGridPengampu.Columns(9).Width = 80
            DataGridPengampu.Columns(9).HeaderText = "SEMESTER"
            DataGridPengampu.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header KELAS'
            DataGridPengampu.Columns(10).Width = 90
            DataGridPengampu.Columns(10).HeaderText = "KELAS"
            DataGridPengampu.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header TAHUN AKADEMIK'
            DataGridPengampu.Columns(11).Width = 110
            DataGridPengampu.Columns(11).HeaderText = "TAHUN AKADEMIK"
            DataGridPengampu.Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            'memberikan nama pada header NAMA SEMESTER'
            DataGridPengampu.Columns(12).Width = 110
            DataGridPengampu.Columns(12).HeaderText = "NAMA SEMESTER"
            DataGridPengampu.Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

        End With
    End Sub

    ' SEMESTER COMBO
    Sub KosongkanComboSemester()
        isloading = True
        CbNamaSemester.DataSource = Nothing
        CbNamaSemester.Items.Clear()
        CbNamaSemester.Text = ""
        CbNamaSemester.SelectedIndex = -1
        CbNamaSemester.Enabled = False
        isloading = False
    End Sub

    Sub IsiComboSemesterPilihan()
        isloading = True

        CbNamaSemester.DataSource = Nothing
        CbNamaSemester.DisplayMember = ""
        CbNamaSemester.ValueMember = ""

        CbNamaSemester.Items.Clear()
        CbNamaSemester.Items.Add("ALL")
        CbNamaSemester.Items.Add("GANJIL")
        CbNamaSemester.Items.Add("GENAP")

        CbNamaSemester.Enabled = True
        CbNamaSemester.SelectedIndex = 0 'default ALL

        isloading = False
    End Sub


    Sub HitungTotalData()
        Try
            Call KoneksiDB()

            QUERY = "SELECT COUNT(*) " &
                "FROM tbl_pengampu_matakuliah pm " &
                "INNER JOIN tbl_dosen d ON pm.Kd_Dosen = d.Kd_Dosen " &
                "INNER JOIN tbl_matakuliah mk ON pm.Kd_Matakuliah = mk.Kd_Matakuliah " &
                "WHERE 1=1 "

            Dim kdProdi As String = If(CbNamaJurusan.SelectedValue Is Nothing, "", CbNamaJurusan.SelectedValue.ToString())
            Dim namaSemester As String = CbNamaSemester.Text.Trim()
            Dim kdDosen As String = TxtKdDosen.Text.Trim()

            If kdProdi <> "" Then
                QUERY &= " AND d.Kd_Prodi = @KdProdi "
            End If

            If namaSemester <> "" AndAlso namaSemester <> "ALL" Then
                QUERY &= " AND (CASE WHEN MOD(mk.Semester_Matakuliah,2)=1 THEN 'GANJIL' ELSE 'GENAP' END) = @NamaSemester "
            End If

            If kdDosen <> "" Then
                QUERY &= " AND d.Kd_Dosen = @KdDosen "
            End If

            Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                If kdProdi <> "" Then CMD.Parameters.AddWithValue("@KdProdi", kdProdi)
                If namaSemester <> "" AndAlso namaSemester <> "ALL" Then CMD.Parameters.AddWithValue("@NamaSemester", namaSemester)
                If kdDosen <> "" Then CMD.Parameters.AddWithValue("@KdDosen", kdDosen)

                TotalData = CInt(CMD.ExecuteScalar())
            End Using

            TotalPage = Math.Ceiling(TotalData / PageSize)
            If TotalPage < 1 Then TotalPage = 1

            LbTotalBaris.Text = "Total Baris Halaman Semester: " & TotalData
            LbHasilBagiHalaman.Text = TotalPage.ToString()

        Catch ex As Exception
            MessageBox.Show("Error HitungTotalData: " & ex.Message)
        End Try
    End Sub

    ' LOAD GRID (PAGING + FILTER)
    Sub TampilkanDataGridPengampu()
        Try
            PageSize = Val(ComboBoxEntries.Text)
            Offset = (CurrentPage - 1) * PageSize

            Dim CacheKey As String = GEtCacheKey(CurrentPage)

            If PageCache.ContainsKey(CacheKey) Then
                Dim dtCache As DataTable = PageCache(CacheKey)
                DataGridPengampu.DataSource = dtCache
                FormatRowHeader(DataGridPengampu, Offset, PageSize)
                LbJumlahBaris.Text = "Jumlah Baris Entri : " & dtCache.Rows.Count
                UpdateNavigatorState()
                AktifDataGridPengampu()
                Exit Sub
            End If

            Call KoneksiDB()

            QUERY = "SELECT " &
                "pm.KdPengampu AS KODE, " &
                "d.Kd_Dosen AS NIK, " &
                "d.Nidn_Dosen AS NIDN, " &
                "d.Nm_Dosen AS NamaDosen, " &
                "mk.Kd_Matakuliah AS KodeMatakuliah, " &
                "mk.Nm_Matakuliah AS NamaMatakuliah, " &
                "mk.Sks_Matakuliah AS SKS, " &
                "mk.Teori_Matakuliah AS SKSTeori, " &
                "mk.Praktek_Matakuliah AS SKSPraktek, " &
                "mk.Semester_Matakuliah AS Semester, " &
                "pm.Nama_Kelas AS Kelas, " &
                "pm.Tahun_Akademik AS TahunAkademik, " &
                "(CASE WHEN MOD(mk.Semester_Matakuliah,2)=1 THEN 'GANJIL' ELSE 'GENAP' END) AS NamaSemester " &
                "FROM tbl_pengampu_matakuliah pm " &
                "INNER JOIN tbl_dosen d ON pm.Kd_Dosen = d.Kd_Dosen " &
                "INNER JOIN tbl_matakuliah mk ON pm.Kd_Matakuliah = mk.Kd_Matakuliah " &
                "WHERE 1=1 "

            Dim kdProdi As String = If(CbNamaJurusan.SelectedValue Is Nothing, "", CbNamaJurusan.SelectedValue.ToString())
            Dim namaSemester As String = CbNamaSemester.Text.Trim()
            Dim kdDosen As String = TxtKdDosen.Text.Trim()

            If kdProdi <> "" Then
                QUERY &= " AND d.Kd_Prodi = @KdProdi "
            End If

            If namaSemester <> "" AndAlso namaSemester <> "ALL" Then
                QUERY &= " AND (CASE WHEN MOD(mk.Semester_Matakuliah,2)=1 THEN 'GANJIL' ELSE 'GENAP' END) = @NamaSemester "
            End If

            If kdDosen <> "" Then
                QUERY &= " AND d.Kd_Dosen = @KdDosen "
            End If

            QUERY &= " ORDER BY pm.KdPengampu ASC LIMIT @Offset, @Limit"

            Using DA As New MySqlDataAdapter(QUERY, DbKoneksi)
                If kdProdi <> "" Then DA.SelectCommand.Parameters.AddWithValue("@KdProdi", kdProdi)
                If namaSemester <> "" AndAlso namaSemester <> "ALL" Then DA.SelectCommand.Parameters.AddWithValue("@NamaSemester", namaSemester)
                If kdDosen <> "" Then DA.SelectCommand.Parameters.AddWithValue("@KdDosen", kdDosen)

                DA.SelectCommand.Parameters.AddWithValue("@Offset", Offset)
                DA.SelectCommand.Parameters.AddWithValue("@Limit", PageSize)

                Dim DT As New DataTable
                DA.Fill(DT)

                PageCache(CacheKey) = DT
                DataGridPengampu.DataSource = DT

                FormatRowHeader(DataGridPengampu, Offset, PageSize)
                LbJumlahBaris.Text = "Jumlah Baris Entri : " & DT.Rows.Count

                UpdateNavigatorState()
                AktifDataGridPengampu()
            End Using

        Catch ex As Exception
            MessageBox.Show("Error TampilkanDataGridPengampu: " & ex.Message)
        End Try
    End Sub

    ' NAVIGATOR
    Sub UpdateNavigatorState()
        BindingNavigatorMoveFirstItem.Enabled = (CurrentPage > 1)
        BindingNavigatorMovePreviousItem.Enabled = (CurrentPage > 1)

        BindingNavigatorMoveNextItem.Enabled = (CurrentPage < TotalPage)
        BindingNavigatorMoveLastItem.Enabled = (CurrentPage < TotalPage)

        BindingNavigatorPositionItem.Text = CurrentPage.ToString()
        BindingNavigatorCountItem.Text = $"of {TotalPage}"
    End Sub

    Sub RefreshPaging()
        CurrentPage = 1
        ClearPagingCache()
        HitungTotalData()
        TampilkanDataGridPengampu()
        UpdateNavigatorState()
    End Sub

    'buat kompatibel kalau kamu masih manggil yg typo
    Sub ReffreshPaging()
        RefreshPaging()
    End Sub

    ' FILTER PRODI
    Sub TampilkanFilterNamaProdi()
        Try
            Call KoneksiDB()
            QUERY = "SELECT Kd_Prodi, Nm_Prodi FROM tbl_prodi ORDER BY Kd_Prodi"
            DA = New MySqlDataAdapter(QUERY, DbKoneksi)

            Dim DT As New DataTable
            DA.Fill(DT)

            CbNamaJurusan.DataSource = DT
            CbNamaJurusan.DisplayMember = "Nm_Prodi"
            CbNamaJurusan.ValueMember = "Kd_Prodi"
            CbNamaJurusan.SelectedIndex = -1

            LbkdProdi.Text = ""
            KosongkanComboSemester()

        Catch ex As Exception
            MsgBox("Gagal memuat data Prodi: " & ex.Message, vbCritical)
        End Try
    End Sub

    Function ProdiBelumAdaPengampu(kdProdi As String) As Boolean
        Try
            Call KoneksiDB()
            QUERY = "SELECT COUNT(*) " &
                "FROM tbl_pengampu_matakuliah pm " &
                "INNER JOIN tbl_dosen d ON pm.Kd_Dosen = d.Kd_Dosen " &
                "WHERE d.Kd_Prodi = @KdProdi"

            Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                CMD.Parameters.AddWithValue("@KdProdi", kdProdi)
                Return CInt(CMD.ExecuteScalar()) = 0
            End Using

        Catch ex As Exception
            MessageBox.Show("Error validasi prodi: " & ex.Message)
            Return False
        End Try
    End Function

    Private Function AmbilProdiDosen(ByVal kdDosen As String) As (KdProdi As String, NmProdi As String)
        Try
            Call KoneksiDB()
            Dim kdProdi As String = ""
            Dim nmProdi As String = ""

            Dim q As String =
        "SELECT d.Kd_Prodi, p.Nm_Prodi " &
        "FROM tbl_dosen d " &
        "INNER JOIN tbl_prodi p ON d.Kd_Prodi = p.Kd_Prodi " &
        "WHERE d.Kd_Dosen = @KdDosen LIMIT 1"

            Using cmd As New MySqlCommand(q, DbKoneksi)
                cmd.Parameters.AddWithValue("@KdDosen", kdDosen)
                Using dr As MySqlDataReader = cmd.ExecuteReader()
                    If dr.Read() Then
                        kdProdi = dr("Kd_Prodi").ToString()
                        nmProdi = dr("Nm_Prodi").ToString()
                    End If
                End Using
            End Using

            Return (kdProdi, nmProdi)

        Catch
            Return ("", "")
        End Try
    End Function


    Private Sub CbNamaJurusan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaJurusan.SelectedIndexChanged
        If isloading Then Exit Sub

        'Jika belum pilih prodi / dikosongkan
        If CbNamaJurusan.SelectedIndex < 0 OrElse CbNamaJurusan.SelectedValue Is Nothing Then
            LbkdProdi.Text = ""
            KosongkanComboSemester()

            TxtKdDosen.Text = ""
            lbNidn.Text = ""
            LbNamaDosen.Text = ""
            Exit Sub
        End If

        Try
            'Kode prodi otomatis tampil di label
            Kode_Jurusan = CbNamaJurusan.SelectedValue.ToString()
            LbkdProdi.Text = Microsoft.VisualBasic.Right(Kode_Jurusan, 2)

            IsiComboSemesterPilihan()

            'VALIDASI: kalau prodi belum ada data pengampu
            If ProdiBelumAdaPengampu(Kode_Jurusan) Then
                Dim jawab As DialogResult = MessageBox.Show("Data Pengampu Matakuliah pada Prodi " & CbNamaJurusan.Text & " belum ada." & vbCrLf &
            "Apakah ingin menambahkan data baru?", "Informasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                If jawab = DialogResult.Yes Then
                    Try
                        ' WAJIB: ambil kode barunya, jangan cuma Call
                        Dim kodeBaru As String = BuatKodePengampu()

                        With FrmInputDataDosenPengampuMatakul
                            Try
                                .KosongkanData()
                            Catch
                            End Try

                            ' Mode tambah
                            .btnSimpan.Text = "Simpan"
                            .btnSimpan.Enabled = True
                            .btnSimpan.BackColor = Color.CornflowerBlue

                            .btnKeluar.Text = "Keluar"
                            .btnKeluar.Enabled = True
                            .btnKeluar.BackColor = Color.CornflowerBlue

                            .BtnHapus.Enabled = False
                            .BtnHapus.BackColor = Color.Red

                            ' Kode Pengampu
                            .LbKdPengampu.Text = kodeBaru

                            ' Jurusan otomatis
                            '.CbNamaJurusan.Text = CbNamaJurusan.Text
                            '.CbNamaJurusan.Enabled = False

                            .NamaProdiAwal = CbNamaJurusan.Text
                            .CbNamaJurusan.Enabled = False

                            Dim semAwal As String = CbNamaSemester.Text.Trim().ToUpper()
                            If semAwal = "" OrElse semAwal = "ALL" Then
                                .SemesterAwal = ""
                                .CbNamaSemester.Enabled = True
                            Else
                                .SemesterAwal = semAwal
                                .CbNamaSemester.Enabled = False
                            End If

                            ' Pastikan item combo sudah ada, baru set nilainya
                            .IsiComboSemesterInput()
                            .IsiComboKelasInput()

                            If .SemesterAwal <> "" Then
                                .CbNamaSemester.Text = .SemesterAwal
                            Else
                                .CbNamaSemester.SelectedIndex = -1
                                .CbNamaSemester.Text = ""
                            End If

                            .ShowDialog()
                        End With

                    Catch ex As Exception
                        MessageBox.Show("Gagal membuka FrmDosenPengampu: " & ex.Message)
                    End Try

                    ' Setelah tutup form input -> grid harus update terbaru
                    RefreshPaging()
                End If

                Exit Sub
            End If

            'Kalau prodi ada datanya -> langsung refresh grid
            RefreshPaging()

        Catch ex As Exception
            MsgBox("Error Memilih Prodi: " & ex.Message, vbCritical)
        End Try
    End Sub


    Private Sub CbNamaSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbNamaSemester.SelectedIndexChanged
        If isloading Then Exit Sub
        If CbNamaJurusan.SelectedIndex < 0 OrElse CbNamaJurusan.SelectedValue Is Nothing Then Exit Sub

        RefreshPaging()
    End Sub


    'MENAMPILKAN NIDN DAN NAMA DOSEN BERDASARKAN KODE DOSEN
    Sub TampilkanInfoDosenByKode()
        Try
            Call KoneksiDB()
            Dim kdDosen As String = TxtKdDosen.Text.Trim()

            If kdDosen = "" Then
                lbNidn.Text = ""
                LbNamaDosen.Text = ""
                Exit Sub
            End If

            QUERY = "SELECT Nidn_Dosen, Nm_Dosen FROM tbl_dosen WHERE Kd_Dosen = @KdDosen LIMIT 1"
            Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                CMD.Parameters.AddWithValue("@KdDosen", kdDosen)

                Using DR As MySqlDataReader = CMD.ExecuteReader()
                    If DR.Read() Then
                        lbNidn.Text = DR("Nidn_Dosen").ToString()
                        LbNamaDosen.Text = DR("Nm_Dosen").ToString()
                    Else
                        lbNidn.Text = ""
                        LbNamaDosen.Text = ""
                        MsgBox("Kode Dosen tidak ditemukan!", vbExclamation)
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error TampilkanInfoDosen: " & ex.Message)
        End Try
    End Sub

    Private Sub TxtKdDosen_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtKdDosen.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True

            TampilkanInfoDosenByKode()
            RefreshPaging()

            DataGridPengampu.Enabled = (TxtKdDosen.Text.Trim() <> "" AndAlso LbNamaDosen.Text.Trim() <> "")
        End If
    End Sub


    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles btnTambah.Click
        '1) Validasi prodi harus dipilih dulu
        If CbNamaJurusan.SelectedIndex = -1 OrElse CbNamaJurusan.SelectedValue Is Nothing Then
            MsgBox("Silahkan Pilih Nama Prodi terlebih Dahulu!!", vbCritical, "Peringatan")
            CbNamaJurusan.Focus()
            Exit Sub
        End If

        'Validasi semester: tidak boleh kosong dan tidak boleh "ALL"
        Dim sem As String = CbNamaSemester.Text.Trim().ToUpper()

        If sem = "" OrElse sem = "ALL" Then
            MsgBox("Silahkan pilih Semester terlebih dahulu (GANJIL / GENAP)!!", vbCritical, "Peringatan")
            CbNamaSemester.Focus()
            Exit Sub
        End If


        '2) Konfirmasi
        Dim pesan = MessageBox.Show("Apakah data Pengampu Matakuliah akan ditambah?", "Konfirmasi",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If pesan <> DialogResult.Yes Then Exit Sub

        Try
            Dim kodeBaru As String = BuatKodePengampu()

            With FrmInputDataDosenPengampuMatakul
                'WAJIB: bersihkan sisa data dari mode Ubah sebelumnya
                Try
                    .KosongkanData()
                Catch
                End Try

                'Pastikan mode Tambah
                .btnSimpan.Text = "Simpan"
                .btnKeluar.Text = "Keluar"

                'Pastikan combo terisi sebelum diset nilainya
                Try
                    .IsiComboSemesterInput()
                    .IsiComboKelasInput()
                Catch
                End Try

                .LbKdPengampu.Text = kodeBaru

                'Reset identitas dosen & matkul
                .TxtNoIdentitas.Text = ""
                .LbNIDN.Text = ""
                .LbNamaDosen.Text = ""
                .TxtKdMatakuliah.Text = ""
                .LbNamaMatakuliah.Text = ""
                .LbSKS.Text = ""
                .LbSKSTeori.Text = ""
                .LbSKSPraktek.Text = ""
                .LbSMTR.Text = ""
                .TxtTahunAkademik.Text = ""

                'Set prodi
                If .CbNamaJurusan.DataSource IsNot Nothing Then
                    .CbNamaJurusan.SelectedValue = CbNamaJurusan.SelectedValue
                Else
                    .CbNamaJurusan.Text = CbNamaJurusan.Text
                    .CbNamaJurusan.Enabled = False
                End If

                .SemesterAwal = CbNamaSemester.Text.Trim()
                .CbNamaSemester.Enabled = False
                .CbNamaSemester.Text = .SemesterAwal

                Dim kdDosenAuto As String = TxtKdDosen.Text.Trim()
                Dim nidnAuto As String = lbNidn.Text.Trim()
                Dim namaAuto As String = LbNamaDosen.Text.Trim()

                If kdDosenAuto <> "" AndAlso nidnAuto <> "" AndAlso namaAuto <> "" Then
                    .TxtNoIdentitas.Text = kdDosenAuto
                    .LbNIDN.Text = nidnAuto
                    .LbNamaDosen.Text = namaAuto

                End If

                .btnSimpan.Enabled = True
                .btnSimpan.BackColor = Color.CornflowerBlue

                .BtnHapus.Enabled = False
                .BtnHapus.BackColor = Color.Red

                .btnKeluar.Enabled = True
                .btnKeluar.BackColor = Color.CornflowerBlue

                .ShowDialog()
            End With


            RefreshPaging()

        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan saat membuka FrmDosenPengampu: " & ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Function BuatKodePengampu() As String
        Call KoneksiDB()
        Try
            Dim Prefix As String = "PMK" 'contoh PMK0001

            QUERY = "SELECT MAX(KdPengampu) 
                 FROM tbl_pengampu_matakuliah
                 WHERE LEFT(KdPengampu,3) = @Prefix"

            Using CMD As New MySqlCommand(QUERY, DbKoneksi)
                CMD.Parameters.AddWithValue("@Prefix", Prefix)

                Dim hasil As Object = CMD.ExecuteScalar()
                Dim noUrut As Integer = 1

                If hasil IsNot Nothing AndAlso Not IsDBNull(hasil) Then
                    noUrut = CInt(Microsoft.VisualBasic.Right(hasil.ToString(), 4)) + 1
                End If

                Return Prefix & noUrut.ToString("0000")
            End Using

        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan saat membuat Kode Pengampu: " & ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return PrefixDefaultPMK()
        End Try
    End Function

    Private Function PrefixDefaultPMK() As String
        Return "PMK0001"
    End Function


    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles btnKeluar.Click
        If btnKeluar.Text = "Keluar" Then
            Pesan = MsgBox("Anda yakin mau keluar dari Form Data Dosen Pengampu?", vbQuestion + vbYesNo, "Informasi")
            If Pesan = vbYes Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub ComboBoxEntries_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ComboBoxEntries.KeyPress
        e.Handled = True
    End Sub

    Private Sub ComboBoxEntries_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxEntries.SelectedIndexChanged
        If ComboBoxEntries.SelectedIndex = -1 Then Exit Sub
        PageSize = CInt(ComboBoxEntries.Text)
        RefreshPaging()
    End Sub

    Private Sub BindingNavigatorMoveFirstItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveFirstItem.Click
        CurrentPage = 1
        UpdateNavigatorState()
        TampilkanDataGridPengampu()
    End Sub

    Private Sub BindingNavigatorMovePreviousItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMovePreviousItem.Click
        If CurrentPage > 1 Then
            CurrentPage -= 1
            UpdateNavigatorState()
            TampilkanDataGridPengampu()
        End If
    End Sub

    Private Sub BindingNavigatorMoveNextItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveNextItem.Click
        If CurrentPage < TotalPage Then
            CurrentPage += 1
            UpdateNavigatorState()
            TampilkanDataGridPengampu()
        End If
    End Sub

    Private Sub BindingNavigatorMoveLastItem_Click(sender As Object, e As EventArgs) Handles BindingNavigatorMoveLastItem.Click
        CurrentPage = TotalPage
        UpdateNavigatorState()
        TampilkanDataGridPengampu()
    End Sub

    Private Sub BindingNavigatorPositionItem_Leave(sender As Object, e As EventArgs) Handles BindingNavigatorPositionItem.Leave
        If Val(BindingNavigatorPositionItem.Text) < 1 Then BindingNavigatorPositionItem.Text = "1"
        If Val(BindingNavigatorPositionItem.Text) > TotalPage Then BindingNavigatorPositionItem.Text = TotalPage.ToString()

        CurrentPage = Val(BindingNavigatorPositionItem.Text)
        UpdateNavigatorState()
        TampilkanDataGridPengampu()
    End Sub

    Private Sub FrmDataDosenPengampu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not SystemInformation.TerminalServerSession Then
            Dim dgvType As Type = DataGridPengampu.GetType
            Dim pi As Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            pi.SetValue(DataGridPengampu, True, Nothing)
        End If
        isloading = True
        Call KoneksiDB()

        NumberEntriesHalaman()
        TampilkanFilterNamaProdi()
        KosongkanComboSemester()

        TxtKdDosen.Text = ""
        lbNidn.Text = ""
        LbNamaDosen.Text = ""

        isloading = False

        RefreshPaging()
        DataGridPengampu.Enabled = False
    End Sub

    Private Sub DataGridPengampu_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridPengampu.CellMouseDoubleClick
        Try
            If e.RowIndex < 0 Then Exit Sub
            If DataGridPengampu.Rows.Count = 0 Then Exit Sub

            Dim kdPengampu As String = ""
            If DataGridPengampu.Columns.Contains("KODE") Then
                kdPengampu = If(DataGridPengampu.Rows(e.RowIndex).Cells("KODE").Value, "").ToString()
            Else
                kdPengampu = If(DataGridPengampu.Rows(e.RowIndex).Cells(0).Value, "").ToString()
            End If
            kdPengampu = kdPengampu.Trim()
            If kdPengampu = "" Then Exit Sub

            Try
                If DR IsNot Nothing AndAlso Not DR.IsClosed Then DR.Close()
            Catch
            End Try

            Call KoneksiDB()

            Dim dt As New DataTable()
            Dim sql As String =
            "SELECT " &
            "pm.KdPengampu, pm.Kd_Dosen, pm.Kd_Matakuliah, pm.Nama_Kelas, pm.Tahun_Akademik, " &
            "d.Nidn_Dosen, d.Nm_Dosen, p.Kd_Prodi, p.Nm_Prodi, " &
            "mk.Nm_Matakuliah, mk.Sks_Matakuliah, mk.Teori_Matakuliah, mk.Praktek_Matakuliah, mk.Semester_Matakuliah, " &
            "(CASE WHEN MOD(mk.Semester_Matakuliah,2)=1 THEN 'GANJIL' ELSE 'GENAP' END) AS Nama_Semester " &
            "FROM tbl_pengampu_matakuliah pm " &
            "INNER JOIN tbl_dosen d ON pm.Kd_Dosen = d.Kd_Dosen " &
            "INNER JOIN tbl_prodi p ON d.Kd_Prodi = p.Kd_Prodi " &
            "INNER JOIN tbl_matakuliah mk ON pm.Kd_Matakuliah = mk.Kd_Matakuliah " &
            "WHERE pm.KdPengampu = @KdPengampu LIMIT 1"

            Using da As New MySqlDataAdapter(sql, DbKoneksi)
                da.SelectCommand.Parameters.AddWithValue("@KdPengampu", kdPengampu)
                da.Fill(dt)
            End Using

            If dt.Rows.Count = 0 Then
                MsgBox("Data Pengampu tidak ditemukan!", vbExclamation, "Informasi")
                Exit Sub
            End If

            Dim row As DataRow = dt.Rows(0)

            'ambil data aman dari DBNull
            Dim vKdPengampu As String = SafeStr(row("KdPengampu"))
            Dim vKdDosen As String = SafeStr(row("Kd_Dosen"))
            Dim vNidn As String = SafeStr(row("Nidn_Dosen"))
            Dim vNamaDosen As String = SafeStr(row("Nm_Dosen"))
            Dim vKdProdi As String = SafeStr(row("Kd_Prodi"))
            Dim vNmProdi As String = SafeStr(row("Nm_Prodi"))

            Dim vKdMatkul As String = SafeStr(row("Kd_Matakuliah"))
            Dim vNmMatkul As String = SafeStr(row("Nm_Matakuliah"))
            Dim vSks As String = SafeStr(row("Sks_Matakuliah"))
            Dim vSksTeori As String = SafeStr(row("Teori_Matakuliah"))
            Dim vSksPraktek As String = SafeStr(row("Praktek_Matakuliah"))
            Dim vSmtrMatkul As String = SafeStr(row("Semester_Matakuliah"))

            Dim vKelas As String = SafeStr(row("Nama_Kelas"))
            Dim vThnAkademik As String = SafeStr(row("Tahun_Akademik"))
            Dim vNamaSemester As String = SafeStr(row("Nama_Semester"))

            With FrmInputDataDosenPengampuMatakul
                'isi list dulu biar bisa dipilih
                .TampilkanFilterNamaProdi()
                .IsiComboSemesterInput()
                .IsiComboKelasInput()

                'Mode edit
                .btnSimpan.Text = "Ubah"
                .btnSimpan.Enabled = True
                .btnSimpan.BackColor = Color.CornflowerBlue

                .btnKeluar.Text = "Batal"
                .btnKeluar.Enabled = True
                .btnKeluar.BackColor = Color.CornflowerBlue

                .BtnHapus.Enabled = True
                .BtnHapus.BackColor = Color.CornflowerBlue

                'Pastikan semester bisa dipilih saat mode Ubah
                .CbNamaSemester.Enabled = True

                'Isi data utama
                .LbKdPengampu.Text = vKdPengampu

                'Jurusan
                If .CbNamaJurusan.DataSource IsNot Nothing AndAlso vKdProdi <> "" Then
                    .CbNamaJurusan.SelectedValue = vKdProdi
                Else
                    .CbNamaJurusan.Text = vNmProdi
                End If

                .TxtNoIdentitas.Text = vKdDosen
                .LbNIDN.Text = vNidn
                .LbNamaDosen.Text = vNamaDosen

                .TxtKdMatakuliah.Text = vKdMatkul
                .LbNamaMatakuliah.Text = vNmMatkul
                .LbSKS.Text = vSks
                .LbSKSTeori.Text = vSksTeori
                .LbSKSPraktek.Text = vSksPraktek
                .LbSMTR.Text = vSmtrMatkul

                .SemesterAwal = vNamaSemester.Trim().ToUpper()
                .KelasAwal = vKelas.Trim().ToUpper()

                SetComboTextSafe(.CbNamaSemester, .SemesterAwal)
                SetComboTextSafe(.CbKelas, .KelasAwal)

                .TxtTahunAkademik.Text = vThnAkademik

                Me.Enabled = False
                Dim hasilDialog As DialogResult = .ShowDialog()
                Me.Enabled = True
                Me.Activate()

                'Kalau sukses Ubah/Simpan -> update filter semester di grid sesuai pilihan terakhir di FrmDosenPengampu
                If hasilDialog = DialogResult.OK Then
                    Dim semBaru As String = .CbNamaSemester.Text.Trim().ToUpper()

                    If semBaru <> "" AndAlso semBaru <> "ALL" Then
                        isloading = True
                        Dim idx As Integer = CbNamaSemester.FindStringExact(semBaru)
                        If idx >= 0 Then
                            CbNamaSemester.SelectedIndex = idx
                        Else
                            CbNamaSemester.Text = semBaru
                        End If
                        isloading = False
                    End If
                End If

            End With

            RefreshPaging()

        Catch ex As Exception
            MessageBox.Show("Terjadi Kesalahan Saat menampilkan dari datagrid: " & ex.Message)
            Me.Enabled = True
        End Try
    End Sub

    Private Function SafeStr(val As Object) As String
        If val Is Nothing OrElse IsDBNull(val) Then Return ""
        Return val.ToString().Trim()
    End Function

    Private Sub SetComboTextSafe(cb As ComboBox, value As String)
        Dim v As String = If(value, "").Trim()

        If v = "" OrElse v.ToUpper() = "ALL" Then
            cb.SelectedIndex = -1
            cb.Text = ""
            Exit Sub
        End If

        'Kalau item belum ada, tambahkan dulu
        If Not cb.Items.Contains(v) Then
            cb.Items.Add(v)
        End If

        Dim idx As Integer = cb.FindStringExact(v)
        If idx >= 0 Then
            cb.SelectedIndex = idx
        Else
            cb.SelectedIndex = -1
            cb.Text = v
        End If
    End Sub

End Class
