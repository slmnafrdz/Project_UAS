Imports MySql.Data.MySqlClient

Public Class FrmDataRuangan

    Public CurrentPage As Integer = 1
    Dim PageSize As Integer = 10
    Dim TotalData As Integer = 0
    Dim TotalPage As Integer = 0
    Dim Offset As Integer = 0

    ' Poin 5: List untuk menampung sekumpulan objek dalam memori [cite: 24]
    Dim daftarRuangan As New List(Of ClsRuangan)

    ' --- LOGIKA TAMPILAN GRID ---
    Public Sub TampilkanDataGridRuang()
        Try
            PageSize = Val(ComboBoxEntries.Text)
            Offset = (CurrentPage - 1) * PageSize
            Call KoneksiDB()

            ' Query hanya mengambil field yang diperlukan
            QUERY = "SELECT Kd_Ruangan, Nm_Ruangan, Jml_Kapasitas FROM tbl_ruangkelas WHERE 1=1"
            ' Penambahan Logika Cari
            If TxtCariRuangan.Text.Trim() <> "" Then QUERY &= " AND Nm_Ruangan LIKE @Nama"
            QUERY &= " ORDER BY Kd_Ruangan ASC LIMIT @Offset, @Limit"

            Using DA As New MySqlDataAdapter(QUERY, DbKoneksi)
                DA.SelectCommand.Parameters.AddWithValue("@Offset", Offset)
                DA.SelectCommand.Parameters.AddWithValue("@Limit", PageSize)
                ' Binding Parameter Cari
                If TxtCariRuangan.Text.Trim() <> "" Then DA.SelectCommand.Parameters.AddWithValue("@Nama", "%" & TxtCariRuangan.Text.Trim() & "%")

                Dim DT As New DataTable
                DA.Fill(DT)

                ' --- MANAJEMEN DATA: INSTANSIASI KE LIST (Poin 2c & 5) [cite: 24, 29] ---
                daftarRuangan.Clear()
                For Each row As DataRow In DT.Rows
                    Dim r As New ClsRuangan()
                    r.Kode = row("Kd_Ruangan").ToString()
                    r.Nama = row("Nm_Ruangan").ToString()
                    r.Kapasitas = Val(row("Jml_Kapasitas"))
                    daftarRuangan.Add(r)
                Next

                ' Binding Data
                DataGridRuang.DataSource = DT

                ' --- FORMATTING GRID (BOLD HEADER & NO EXTRA COLUMN) ---
                With DataGridRuang
                    .AllowUserToAddRows = False
                    .ReadOnly = True
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                    .ClearSelection()
                    .CurrentCell = Nothing
                    ' Membuat Header menjadi BOLD
                    Dim styleHeader As New DataGridViewCellStyle()
                    styleHeader.Font = New Font("Arial", 10, FontStyle.Bold)
                    styleHeader.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .ColumnHeadersDefaultCellStyle = styleHeader
                    .ColumnHeadersHeight = 40

                    ' Setting Header Text & Lebar Kolom
                    .Columns(0).HeaderText = "KODE RUANGAN"
                    .Columns(0).Width = 150
                    .Columns(1).HeaderText = "NAMA RUANGAN"
                    .Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .Columns(2).HeaderText = "KAPASITAS"
                    .Columns(2).Width = 120
                End With

                ' Penomoran Row Header
                For i As Integer = 0 To DataGridRuang.Rows.Count - 1
                    DataGridRuang.Rows(i).HeaderCell.Value = (Offset + i + 1).ToString()
                Next

                LbJumlahBaris.Text = "Jumlah Baris Entri: " & DT.Rows.Count
                UpdateNavigatorState()
            End Using
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    ' --- PAGING LOGIC ---
    Sub HitungTotalData()
        Try
            Call KoneksiDB()
            QUERY = "SELECT COUNT(*) FROM tbl_ruangkelas WHERE 1=1"
            ' Penambahan Logika Cari di Count
            If TxtCariRuangan.Text.Trim() <> "" Then QUERY &= " AND Nm_Ruangan LIKE @Nama"

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            If TxtCariRuangan.Text.Trim() <> "" Then CMD.Parameters.AddWithValue("@Nama", "%" & TxtCariRuangan.Text.Trim() & "%")

            TotalData = CInt(CMD.ExecuteScalar())
            TotalPage = Math.Max(1, Math.Ceiling(TotalData / PageSize))

            LbTotalBaris.Text = "Total Baris Halaman: " & TotalData
            LbHasilBagiHalaman.Text = "Jumlah Halaman: " & TotalPage
        Catch ex As Exception
        End Try
    End Sub

    Sub UpdateNavigatorState()
        BindingNavigatorPositionItem.Text = CurrentPage.ToString()
        BindingNavigatorCountItem.Text = "of " & TotalPage
    End Sub

    Public Sub RefreshPaging()
        CurrentPage = 1
        HitungTotalData()
        TampilkanDataGridRuang()
    End Sub

    ' --- EVENTS ---
    Private Sub FrmDataRuangan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBoxEntries.Items.Clear()
        ComboBoxEntries.Items.AddRange(New String() {"10", "20", "50"})
        ComboBoxEntries.SelectedIndex = 0
        RefreshPaging()
    End Sub

    ' --- FITUR CARI BUTTON ---
    Private Sub BtnCari_Click(sender As Object, e As EventArgs) Handles BtnCari.Click
        RefreshPaging()
    End Sub

    ' --- FITUR CARI KEYPRESS ENTER ---
    Private Sub TxtCariRuangan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtCariRuangan.KeyPress
        If e.KeyChar = Chr(13) Then
            e.Handled = True
            BtnCari.PerformClick()
        End If
    End Sub

    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        With FrmInputRuangan
            .KosongkanData()
            .BuatKodeRuanganOtomatis()
            .BtnSimpan.Text = "Simpan"
            .BtnHapus.Enabled = False
            .ShowDialog()
        End With
    End Sub

    Private Sub DataGridRuang_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridRuang.CellMouseDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridRuang.Rows(e.RowIndex)
            With FrmInputRuangan
                .TxtKdRuangan.Text = row.Cells(0).Value.ToString()
                .TxtNamaRuangan.Text = row.Cells(1).Value.ToString()
                .TxtKapasitas.Text = row.Cells(2).Value.ToString()
                .BtnSimpan.Text = "Ubah"
                .BtnHapus.Enabled = True
                .ShowDialog()
            End With
        End If
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        Me.Close()
    End Sub
End Class

' ============================================================
' IMPLEMENTASI OOP (POIN 1-4) [cite: 9, 13, 16, 19]
' ============================================================
Public Interface IEntitasRuangan ' Poin 4: Abstraksi 
    Property Kode As String
    Property Nama As String
End Interface

Public MustInherit Class BaseRuangan ' Poin 4: Abstract Class 
    Implements IEntitasRuangan
    Private _kode As String ' Poin 1: Enkapsulasi (Private Access Specifier) 
    Private _nama As String

    Public Property Kode As String Implements IEntitasRuangan.Kode ' Poin 1: Property 
        Get
            Return _kode
        End Get
        Set(value As String)
            _kode = value
        End Set
    End Property

    Public Property Nama As String Implements IEntitasRuangan.Nama
        Get
            Return _nama
        End Get
        Set(value As String)
            ' Poin 1: Validasi input dilakukan di Property 
            If String.IsNullOrEmpty(value) Then _nama = "TANPA NAMA" Else _nama = value.ToUpper()
        End Set
    End Property

    Public Overridable Function Info() As String ' Poin 3: Polimorfisme (Overridable) 
        Return "Kode Ruangan: " & _kode
    End Function
End Class

Public Class ClsRuangan ' Poin 2: Pewarisan (Inheritance) 
    Inherits BaseRuangan
    Public Property Kapasitas As Integer

    Public Overrides Function Info() As String ' Poin 3: Overriding 
        Return "[DETAIL RUANGAN] " & MyBase.Info() & " - Kapasitas: " & Kapasitas & " Orang"
    End Function
End Class
