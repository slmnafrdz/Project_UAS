Imports MySql.Data.MySqlClient

Public Class FrmDataJurusan

    Public CurrentPage As Integer = 1
    Dim PageSize As Integer = 10
    Dim TotalData As Integer = 0
    Dim TotalPage As Integer = 0
    Dim Offset As Integer = 0

    ' Poin 5 Soal UAS: Menggunakan List(Of T) untuk menampung objek
    Dim daftarJurusan As New List(Of ClsJurusan)

    ' --- LOGIKA TAMPILAN GRID ---
    Sub TampilkanDataGridJurusan()
        Try
            PageSize = Val(ComboBoxEntries.Text)
            Offset = (CurrentPage - 1) * PageSize
            Call KoneksiDB()

            QUERY = "SELECT Kd_Prodi, Nm_Prodi FROM tbl_prodi WHERE 1=1"
            If TxtJurusan.Text <> "" Then QUERY &= " AND Nm_Prodi LIKE @NamaProdi"
            QUERY &= " ORDER BY Kd_Prodi ASC LIMIT @Offset, @Limit"

            Using DA As New MySqlDataAdapter(QUERY, DbKoneksi)
                DA.SelectCommand.Parameters.AddWithValue("@Offset", Offset)
                DA.SelectCommand.Parameters.AddWithValue("@Limit", PageSize)
                If TxtJurusan.Text <> "" Then DA.SelectCommand.Parameters.AddWithValue("@NamaProdi", "%" & TxtJurusan.Text & "%")

                Dim DT As New DataTable
                DA.Fill(DT)

                ' Instansiasi Objek ke List (Poin 5 & Tugas 2b Soal UAS)
                daftarJurusan.Clear()
                For Each row As DataRow In DT.Rows
                    Dim j As New ClsJurusan()
                    j.Kode = row("Kd_Prodi").ToString()
                    j.Nama = row("Nm_Prodi").ToString()
                    daftarJurusan.Add(j)
                Next

                DataGridJurusan.DataSource = DT

                ' --- STYLING GRID & HEADER ---
                With DataGridJurusan
                    .AllowUserToAddRows = False
                    .RowHeadersVisible = True
                    .RowHeadersWidth = 60

                    Dim styleHeader As New DataGridViewCellStyle()
                    styleHeader.Font = New Font("Arial", 12, FontStyle.Bold)
                    styleHeader.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .ColumnHeadersDefaultCellStyle = styleHeader
                    .ColumnHeadersHeight = 45

                    .Columns(0).HeaderText = "KODE PRODI"
                    .Columns(0).Width = 130
                    .Columns(1).HeaderText = "NAMA JURUSAN"
                    .Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End With

                ' Nomor Urut Row Header
                For i As Integer = 0 To DataGridJurusan.Rows.Count - 1
                    DataGridJurusan.Rows(i).HeaderCell.Value = (Offset + i + 1).ToString()
                Next

                LbJumlahBaris.Text = "Baris Entri: " & DT.Rows.Count
            End Using
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    ' --- EVENT KEYPRESS ENTER PADA PENCARIAN ---
    Private Sub TxtJurusan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtJurusan.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            RefreshPaging()
        End If
    End Sub

    ' --- EVENT DOUBLE CLICK UNTUK UPDATE ---
    Private Sub DataGridJurusan_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridJurusan.CellMouseDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridJurusan.Rows(e.RowIndex)
            With FrmInputJurusan
                .TxtKdprodi.Text = row.Cells(0).Value.ToString()
                .TxtNamaProdi.Text = row.Cells(1).Value.ToString()

                .BtnSimpan.Text = "Ubah"
                .BtnSimpan.BackColor = Color.CornflowerBlue
                .BtnHapus.Enabled = True
                .BtnHapus.BackColor = Color.CornflowerBlue
                .ShowDialog()
            End With
        End If
    End Sub

    ' --- NAVIGASI & PAGING ---
    Sub RefreshPaging()
        CurrentPage = 1
        HitungTotalData()
        TampilkanDataGridJurusan()
    End Sub

    Sub HitungTotalData()
        Try
            Call KoneksiDB()
            QUERY = "SELECT COUNT(*) FROM tbl_prodi WHERE 1=1"
            If TxtJurusan.Text <> "" Then QUERY &= " AND Nm_Prodi LIKE @NamaProdi"

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            If TxtJurusan.Text <> "" Then CMD.Parameters.AddWithValue("@NamaProdi", "%" & TxtJurusan.Text & "%")

            TotalData = CInt(CMD.ExecuteScalar())
            TotalPage = Math.Max(1, Math.Ceiling(TotalData / PageSize))

            ' Update Label sesuai Screenshot
            LbTotalBaris.Text = "Total Baris Halaman: " & TotalData
            LbHasilBagiHalaman.Text = "Jumlah Halaman: " & TotalPage
        Catch ex As Exception
            MsgBox("Error Count: " & ex.Message)
        End Try
    End Sub

    Private Sub FrmDataJurusan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NumberEntriesHalaman()
        RefreshPaging()
    End Sub

    Sub NumberEntriesHalaman()
        ComboBoxEntries.Items.Clear()
        ComboBoxEntries.Items.AddRange(New String() {"10", "20", "50", "100"})
        ComboBoxEntries.SelectedIndex = 0
    End Sub

    Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles BtnCari.Click
        RefreshPaging()
    End Sub

    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        With FrmInputJurusan
            .Kosongkan()
            .BuatKodeProdi()
            .BtnSimpan.Text = "Simpan"
            .BtnSimpan.BackColor = Color.CornflowerBlue
            .BtnHapus.Enabled = False
            .BtnHapus.BackColor = Color.Red
            .BtnKeluar.BackColor = Color.CornflowerBlue
            .ShowDialog()
        End With
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        Me.Close()
    End Sub
End Class

' ============================================================
' STRUKTUR OOP (DI BAWAH END CLASS FORM) - POIN 1-4 SOAL UAS
' ============================================================
Public Interface IEntitasJurusan ' Poin 4: Interface
        Property Kode As String
        Property Nama As String
    End Interface

    Public MustInherit Class BaseJurusan ' Poin 4: MustInherit
        Implements IEntitasJurusan
        Private _kode As String ' Poin 1: Private Access Specifier
        Private _nama As String

        Public Property Kode As String Implements IEntitasJurusan.Kode ' Poin 1: Property
            Get
                Return _kode
            End Get
            Set(value As String)
                _kode = value
            End Set
        End Property

        Public Property Nama As String Implements IEntitasJurusan.Nama
            Get
                Return _nama
            End Get
            Set(value As String)
                ' Poin 1: Validasi Input
                If String.IsNullOrEmpty(value) Then _nama = "DATA KOSONG" Else _nama = value.ToUpper()
            End Set
        End Property

        Public Overridable Function Info() As String ' Poin 3: Overridable
            Return "Kode: " & _kode
        End Function
    End Class

    Public Class ClsJurusan ' Poin 2: Inheritance
        Inherits BaseJurusan
        Public Overrides Function Info() As String ' Poin 3: Overriding
            Return "[SISTEM AKADEMIK] " & MyBase.Info() & " - " & MyBase.Nama
        End Function
    End Class