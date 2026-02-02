<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmInputDataDosenPengampuMatakul
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TxtNoIdentitas = New System.Windows.Forms.TextBox()
        Me.CbNamaJurusan = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TxtTahunAkademik = New System.Windows.Forms.TextBox()
        Me.CbKelas = New System.Windows.Forms.ComboBox()
        Me.CbNamaSemester = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.LbSMTR = New System.Windows.Forms.Label()
        Me.LbSKSPraktek = New System.Windows.Forms.Label()
        Me.LbSKSTeori = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LbSKS = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.LbNamaMatakuliah = New System.Windows.Forms.Label()
        Me.BtnCariKodeMatkul = New System.Windows.Forms.Button()
        Me.TxtKdMatakuliah = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.LbNamaDosen = New System.Windows.Forms.Label()
        Me.LbNIDN = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.BtnCari = New System.Windows.Forms.Button()
        Me.LbKdPengampu = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnHapus = New System.Windows.Forms.Button()
        Me.btnSimpan = New System.Windows.Forms.Button()
        Me.btnKeluar = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtNoIdentitas
        '
        Me.TxtNoIdentitas.Location = New System.Drawing.Point(261, 188)
        Me.TxtNoIdentitas.Name = "TxtNoIdentitas"
        Me.TxtNoIdentitas.Size = New System.Drawing.Size(146, 28)
        Me.TxtNoIdentitas.TabIndex = 21
        '
        'CbNamaJurusan
        '
        Me.CbNamaJurusan.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbNamaJurusan.FormattingEnabled = True
        Me.CbNamaJurusan.Location = New System.Drawing.Point(261, 153)
        Me.CbNamaJurusan.Name = "CbNamaJurusan"
        Me.CbNamaJurusan.Size = New System.Drawing.Size(465, 29)
        Me.CbNamaJurusan.TabIndex = 19
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.GreenYellow
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Georgia", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(743, 88)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = ":: INPUT DATA PENGAMPU MATAKULIAH ::"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.GroupBox1.Controls.Add(Me.Label23)
        Me.GroupBox1.Controls.Add(Me.Label22)
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.TxtTahunAkademik)
        Me.GroupBox1.Controls.Add(Me.CbKelas)
        Me.GroupBox1.Controls.Add(Me.CbNamaSemester)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.LbSMTR)
        Me.GroupBox1.Controls.Add(Me.LbSKSPraktek)
        Me.GroupBox1.Controls.Add(Me.LbSKSTeori)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.LbSKS)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.LbNamaMatakuliah)
        Me.GroupBox1.Controls.Add(Me.BtnCariKodeMatkul)
        Me.GroupBox1.Controls.Add(Me.TxtKdMatakuliah)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.LbNamaDosen)
        Me.GroupBox1.Controls.Add(Me.LbNIDN)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.BtnCari)
        Me.GroupBox1.Controls.Add(Me.LbKdPengampu)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.TxtNoIdentitas)
        Me.GroupBox1.Controls.Add(Me.CbNamaJurusan)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Font = New System.Drawing.Font("Garamond", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(743, 634)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = ":: Input Data :"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(27, 591)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(173, 19)
        Me.Label23.TabIndex = 53
        Me.Label23.Text = "TAHUN AKADEMIK"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(27, 557)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(68, 19)
        Me.Label22.TabIndex = 52
        Me.Label22.Text = "KELAS "
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(27, 522)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(103, 19)
        Me.Label21.TabIndex = 51
        Me.Label21.Text = "SEMESTER"
        '
        'TxtTahunAkademik
        '
        Me.TxtTahunAkademik.Location = New System.Drawing.Point(261, 582)
        Me.TxtTahunAkademik.Name = "TxtTahunAkademik"
        Me.TxtTahunAkademik.Size = New System.Drawing.Size(161, 28)
        Me.TxtTahunAkademik.TabIndex = 50
        '
        'CbKelas
        '
        Me.CbKelas.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbKelas.FormattingEnabled = True
        Me.CbKelas.Location = New System.Drawing.Point(261, 547)
        Me.CbKelas.Name = "CbKelas"
        Me.CbKelas.Size = New System.Drawing.Size(146, 29)
        Me.CbKelas.TabIndex = 49
        '
        'CbNamaSemester
        '
        Me.CbNamaSemester.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbNamaSemester.FormattingEnabled = True
        Me.CbNamaSemester.Location = New System.Drawing.Point(261, 512)
        Me.CbNamaSemester.Name = "CbNamaSemester"
        Me.CbNamaSemester.Size = New System.Drawing.Size(122, 29)
        Me.CbNamaSemester.TabIndex = 48
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(27, 451)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(108, 19)
        Me.Label12.TabIndex = 47
        Me.Label12.Text = "SKS - Praktek"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(27, 486)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(58, 19)
        Me.Label11.TabIndex = 46
        Me.Label11.Text = "SMTR"
        '
        'LbSMTR
        '
        Me.LbSMTR.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbSMTR.Location = New System.Drawing.Point(261, 476)
        Me.LbSMTR.Name = "LbSMTR"
        Me.LbSMTR.Size = New System.Drawing.Size(63, 29)
        Me.LbSMTR.TabIndex = 45
        '
        'LbSKSPraktek
        '
        Me.LbSKSPraktek.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbSKSPraktek.Location = New System.Drawing.Point(261, 441)
        Me.LbSKSPraktek.Name = "LbSKSPraktek"
        Me.LbSKSPraktek.Size = New System.Drawing.Size(63, 29)
        Me.LbSKSPraktek.TabIndex = 44
        '
        'LbSKSTeori
        '
        Me.LbSKSTeori.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbSKSTeori.Location = New System.Drawing.Point(261, 406)
        Me.LbSKSTeori.Name = "LbSKSTeori"
        Me.LbSKSTeori.Size = New System.Drawing.Size(63, 29)
        Me.LbSKSTeori.TabIndex = 43
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(27, 414)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(93, 19)
        Me.Label7.TabIndex = 42
        Me.Label7.Text = "SKS - Teori"
        '
        'LbSKS
        '
        Me.LbSKS.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbSKS.Location = New System.Drawing.Point(261, 372)
        Me.LbSKS.Name = "LbSKS"
        Me.LbSKS.Size = New System.Drawing.Size(63, 29)
        Me.LbSKS.TabIndex = 41
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(27, 382)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 19)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "SKS"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(27, 344)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(139, 19)
        Me.Label20.TabIndex = 39
        Me.Label20.Text = "Nama Matakuliah"
        '
        'LbNamaMatakuliah
        '
        Me.LbNamaMatakuliah.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbNamaMatakuliah.Location = New System.Drawing.Point(261, 334)
        Me.LbNamaMatakuliah.Name = "LbNamaMatakuliah"
        Me.LbNamaMatakuliah.Size = New System.Drawing.Size(351, 29)
        Me.LbNamaMatakuliah.TabIndex = 38
        '
        'BtnCariKodeMatkul
        '
        Me.BtnCariKodeMatkul.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnCariKodeMatkul.BackColor = System.Drawing.Color.Lime
        Me.BtnCariKodeMatkul.Font = New System.Drawing.Font("Garamond", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCariKodeMatkul.Location = New System.Drawing.Point(418, 300)
        Me.BtnCariKodeMatkul.Name = "BtnCariKodeMatkul"
        Me.BtnCariKodeMatkul.Size = New System.Drawing.Size(201, 28)
        Me.BtnCariKodeMatkul.TabIndex = 37
        Me.BtnCariKodeMatkul.Text = "CARI KODE MATAKULIAH"
        Me.BtnCariKodeMatkul.UseVisualStyleBackColor = False
        '
        'TxtKdMatakuliah
        '
        Me.TxtKdMatakuliah.Location = New System.Drawing.Point(261, 299)
        Me.TxtKdMatakuliah.Name = "TxtKdMatakuliah"
        Me.TxtKdMatakuliah.Size = New System.Drawing.Size(146, 28)
        Me.TxtKdMatakuliah.TabIndex = 36
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(27, 269)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(104, 19)
        Me.Label18.TabIndex = 35
        Me.Label18.Text = "Nama Dosen"
        '
        'LbNamaDosen
        '
        Me.LbNamaDosen.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbNamaDosen.Location = New System.Drawing.Point(261, 259)
        Me.LbNamaDosen.Name = "LbNamaDosen"
        Me.LbNamaDosen.Size = New System.Drawing.Size(351, 29)
        Me.LbNamaDosen.TabIndex = 34
        '
        'LbNIDN
        '
        Me.LbNIDN.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbNIDN.Location = New System.Drawing.Point(261, 224)
        Me.LbNIDN.Name = "LbNIDN"
        Me.LbNIDN.Size = New System.Drawing.Size(146, 29)
        Me.LbNIDN.TabIndex = 33
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(27, 234)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(59, 19)
        Me.Label15.TabIndex = 32
        Me.Label15.Text = "NIDN"
        '
        'BtnCari
        '
        Me.BtnCari.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnCari.BackColor = System.Drawing.Color.Lime
        Me.BtnCari.Font = New System.Drawing.Font("Garamond", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCari.Location = New System.Drawing.Point(418, 188)
        Me.BtnCari.Name = "BtnCari"
        Me.BtnCari.Size = New System.Drawing.Size(52, 28)
        Me.BtnCari.TabIndex = 4
        Me.BtnCari.Text = "CARI"
        Me.BtnCari.UseVisualStyleBackColor = False
        '
        'LbKdPengampu
        '
        Me.LbKdPengampu.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LbKdPengampu.Location = New System.Drawing.Point(261, 116)
        Me.LbKdPengampu.Name = "LbKdPengampu"
        Me.LbKdPengampu.Size = New System.Drawing.Size(146, 29)
        Me.LbKdPengampu.TabIndex = 31
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(27, 126)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(127, 19)
        Me.Label13.TabIndex = 30
        Me.Label13.Text = "Kode Pengampu"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(27, 197)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 19)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "No Identitas"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(27, 306)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(132, 19)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Kode Matakuliah"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Garamond", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 163)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 19)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = " Jurusan "
        '
        'BtnHapus
        '
        Me.BtnHapus.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnHapus.Location = New System.Drawing.Point(311, 31)
        Me.BtnHapus.Name = "BtnHapus"
        Me.BtnHapus.Size = New System.Drawing.Size(126, 57)
        Me.BtnHapus.TabIndex = 1
        Me.BtnHapus.Text = "Hapus"
        Me.BtnHapus.UseVisualStyleBackColor = True
        '
        'btnSimpan
        '
        Me.btnSimpan.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnSimpan.Location = New System.Drawing.Point(69, 31)
        Me.btnSimpan.Name = "btnSimpan"
        Me.btnSimpan.Size = New System.Drawing.Size(127, 57)
        Me.btnSimpan.TabIndex = 3
        Me.btnSimpan.Text = "Simpan"
        Me.btnSimpan.UseVisualStyleBackColor = True
        '
        'btnKeluar
        '
        Me.btnKeluar.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnKeluar.ForeColor = System.Drawing.Color.Black
        Me.btnKeluar.Location = New System.Drawing.Point(552, 31)
        Me.btnKeluar.Name = "btnKeluar"
        Me.btnKeluar.Size = New System.Drawing.Size(125, 57)
        Me.btnKeluar.TabIndex = 2
        Me.btnKeluar.Text = "Keluar"
        Me.btnKeluar.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.GreenYellow
        Me.GroupBox2.Controls.Add(Me.btnSimpan)
        Me.GroupBox2.Controls.Add(Me.btnKeluar)
        Me.GroupBox2.Controls.Add(Me.BtnHapus)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox2.Font = New System.Drawing.Font("Georgia", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(0, 634)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(743, 113)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        '
        'FrmInputDataDosenPengampuMatakul
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(743, 747)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "FrmInputDataDosenPengampuMatakul"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmInputDataDosenPengampuMatakul"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtNoIdentitas As TextBox
    Friend WithEvents CbNamaJurusan As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LbNIDN As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents BtnCari As Button
    Friend WithEvents LbKdPengampu As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnHapus As Button
    Friend WithEvents btnSimpan As Button
    Friend WithEvents btnKeluar As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label18 As Label
    Friend WithEvents LbNamaDosen As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents TxtTahunAkademik As TextBox
    Friend WithEvents CbKelas As ComboBox
    Friend WithEvents CbNamaSemester As ComboBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents LbSMTR As Label
    Friend WithEvents LbSKSPraktek As Label
    Friend WithEvents LbSKSTeori As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents LbSKS As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents LbNamaMatakuliah As Label
    Friend WithEvents BtnCariKodeMatkul As Button
    Friend WithEvents TxtKdMatakuliah As TextBox
End Class
