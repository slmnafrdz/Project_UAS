Imports MySql.Data.MySqlClient

Public Class FrmDataHari

    Dim CurrentPage As Integer = 1
    Dim PageSize As Integer = 10
    Dim TotalData As Integer = 0
    Dim TotalPage As Integer = 0
    Dim Offset As Integer = 0

    Dim daftarHari As New List(Of ClsHari)

    ' --- LOGIKA TAMPILAN GRID ---
    Sub TampilkanDataGridHari()
        Try
            PageSize = Val(ComboBoxEntries.Text)
            Offset = (CurrentPage - 1) * PageSize
            Call KoneksiDB()

            QUERY = "SELECT Id_Hari, Nm_Hari FROM tbl_hari WHERE 1=1"
            If TxtCariHari.Text <> "" Then QUERY &= " AND Nm_Hari LIKE @NamaHari"
            QUERY &= " ORDER BY Id_Hari ASC LIMIT @Offset, @Limit"

            Using DA As New MySqlDataAdapter(QUERY, DbKoneksi)
                DA.SelectCommand.Parameters.AddWithValue("@Offset", Offset)
                DA.SelectCommand.Parameters.AddWithValue("@Limit", PageSize)
                If TxtCariHari.Text <> "" Then DA.SelectCommand.Parameters.AddWithValue("@NamaHari", "%" & TxtCariHari.Text & "%")

                Dim DT As New DataTable
                DA.Fill(DT)

                ' Instansiasi Objek ke List (Poin 2b & 5 Soal UAS)
                daftarHari.Clear()
                For Each row As DataRow In DT.Rows
                    Dim h As New ClsHari()
                    h.Id = row("Id_Hari").ToString()
                    h.Nama = row("Nm_Hari").ToString()
                    daftarHari.Add(h)
                Next

                DataGridHari.DataSource = DT

                ' --- STYLING HEADER (GEDE & BOLD) & NO BARIS KOSONG ---
                With DataGridHari
                    .AllowUserToAddRows = False
                    .RowHeadersVisible = True
                    .RowHeadersWidth = 60

                    Dim styleHeader As New DataGridViewCellStyle()
                    styleHeader.Font = New Font("Arial", 12, FontStyle.Bold) ' GEDE & BOLD
                    styleHeader.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .ColumnHeadersDefaultCellStyle = styleHeader
                    .ColumnHeadersHeight = 45

                    .Columns(0).HeaderText = "KODE HARI"
                    .Columns(0).Width = 150
                    .Columns(1).HeaderText = "NAMA HARI"
                    .Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End With

                ' --- NOMOR URUT PADA ROW HEADER ---
                For i As Integer = 0 To DataGridHari.Rows.Count - 1
                    DataGridHari.Rows(i).HeaderCell.Value = (Offset + i + 1).ToString()
                Next

                LbJumlahBaris.Text = "Baris Entri: " & DT.Rows.Count
            End Using
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
    End Sub

    ' --- EVENT DOUBLE CLICK UNTUK UPDATE ---
    Private Sub DataGridHari_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridHari.CellMouseDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridHari.Rows(e.RowIndex)

            With FrmInputHari
                .TxtKodeHari.Text = row.Cells(0).Value.ToString()
                .TxtNamaHari.Text = row.Cells(1).Value.ToString()

                .BtnSimpan.Text = "Ubah"
                .BtnSimpan.BackColor = Color.CornflowerBlue
                .BtnHapus.Enabled = True
                .BtnHapus.BackColor = Color.CornflowerBlue
                .BtnKeluar.BackColor = Color.CornflowerBlue
                .ShowDialog()
            End With
        End If
    End Sub

    ' --- EVENT LAINNYA ---
    Sub RefreshPaging()
        CurrentPage = 1
        HitungTotalData()
        TampilkanDataGridHari()
    End Sub

    Sub HitungTotalData()
        Try
            Call KoneksiDB()
            QUERY = "SELECT COUNT(*) FROM tbl_hari"
            If TxtCariHari.Text <> "" Then QUERY &= " WHERE Nm_Hari LIKE @NamaHari"

            CMD = New MySqlCommand(QUERY, DbKoneksi)
            If TxtCariHari.Text <> "" Then CMD.Parameters.AddWithValue("@NamaHari", "%" & TxtCariHari.Text & "%")

            TotalData = CInt(CMD.ExecuteScalar())
            TotalPage = Math.Max(1, Math.Ceiling(TotalData / PageSize))

            ' --- UPDATE LABEL TOTAL BARIS & HAL ---
            LbTotalBaris.Text = "Total Baris: " & TotalData
            LbHasilBagiHalaman.Text = "Total Hal: " & TotalPage
        Catch ex As Exception
            MsgBox("Error Count: " & ex.Message)
        End Try
    End Sub

    Private Sub FrmDataHari_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        NumberEntriesHalaman()
        RefreshPaging()
    End Sub

    Sub NumberEntriesHalaman()
        ComboBoxEntries.Items.Clear()
        ComboBoxEntries.Items.AddRange(New String() {"10", "20", "50"})
        ComboBoxEntries.SelectedIndex = 0
    End Sub

    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        With FrmInputHari
            .KosongkanData()
            .BuatKodeHariOtomatis()
            .BtnSimpan.Text = "Simpan"
            .BtnSimpan.BackColor = Color.CornflowerBlue
            .BtnHapus.Enabled = False
            .ShowDialog()
        End With
    End Sub

    Private Sub BtnKeluar_Click(sender As Object, e As EventArgs) Handles BtnKeluar.Click
        Me.Close()
    End Sub
End Class

' ============================================================
' STRUKTUR OOP (DI BAWAH END CLASS FORM)
' ============================================================
Public Interface IEntitas
        Property Id As String
        Property Nama As String
    End Interface

    Public MustInherit Class BaseHari
        Implements IEntitas
        Private _id As String
        Private _nama As String

        Public Property Id As String Implements IEntitas.Id
            Get
                Return _id
            End Get
            Set(value As String)
                _id = value
            End Set
        End Property

        Public Property Nama As String Implements IEntitas.Nama
            Get
                Return _nama
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then _nama = "ERROR" Else _nama = value.ToUpper()
            End Set
        End Property

        Public Overridable Function Info() As String
            Return "ID: " & _id
        End Function
    End Class

    Public Class ClsHari
        Inherits BaseHari
        Public Overrides Function Info() As String
            Return "[SISTEM] " & MyBase.Info() & " - " & MyBase.Nama
        End Function
    End Class
