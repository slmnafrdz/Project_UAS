Imports MySql.Data.MySqlClient
Module MDBKoneksi
    'memebuat variable
    Public DbKoneksi As MySqlConnection         'Menyimpan objek koneksi ke database MySQL.
    Public DA As MySqlDataAdapter               '(DataAdapter) jembatan untuk mengambil/mengirim data antara aplikasi dan database.
    Public DS As DataSet                        '(DataSet) berfungsi sebagai Menampung data sementara hasil query (bisa banyak tabel).
    Public CMD As MySqlCommand                  ' Menampung dan menjalankan perintah SQL (INSERT, UPDATE, DELETE, SELECT).
    Public DR As MySqlDataReader                'digunakan untuk Membaca data langsung dari database secara forward-only, read-only.
    Public Kode_Jurusan As String
    Public Kode_Semester As String
    Public Nomor As Integer
    Public Batas As String
    Public Hitung As Integer
    Public KodeTahunAngkatan As String
    Public LokasiDatabase As String             'Menyimpan string koneksi (server, user, password, database).
    Public SQLinsert As String
    Public SQLUpdate As String
    Public SQLHapus As String
    Public QUERY As String
    Public Isloading As String
    Public Pesan As String
    Public PesanMk1 As String
    Public PesanSmtr As String

    'membuat prosedure Public untuk koneksi database
    Public Sub KoneksiDB()
        Try
            'lokasi Database
            LokasiDatabase = "Server=localhost;Uid=root;Pwd=salman;Database=dbpenjadwalanmatkul"
            DbKoneksi = New MySqlConnection(LokasiDatabase)
            If DbKoneksi.State = ConnectionState.Closed Then
                DbKoneksi.Open()
            End If
        Catch ex As Exception
            MsgBox("KONEKSI GAGAL", vbExclamation, "KONEKSI GAGAL")
        End Try
    End Sub

    Function Diskonek()
        DbKoneksi.Close()
        Return DbKoneksi
    End Function
End Module
