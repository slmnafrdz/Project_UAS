<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmInputDataDosen
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
        Me.Lbkdprodi = New System.Windows.Forms.Label()
        Me.TxtNama = New System.Windows.Forms.TextBox()
        Me.LbKdDosen = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnSimpan = New System.Windows.Forms.Button()
        Me.BtnKeluar = New System.Windows.Forms.Button()
        Me.CbJenisKelamin = New System.Windows.Forms.ComboBox()
        Me.TxtNidn = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TxtEmail = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtNoHp = New System.Windows.Forms.TextBox()
        Me.CbJurusan = New System.Windows.Forms.ComboBox()
        Me.BtnHapus = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Lbkdprodi
        '
        Me.Lbkdprodi.BackColor = System.Drawing.Color.Lime
        Me.Lbkdprodi.ForeColor = System.Drawing.Color.Transparent
        Me.Lbkdprodi.Location = New System.Drawing.Point(213, 359)
        Me.Lbkdprodi.Name = "Lbkdprodi"
        Me.Lbkdprodi.Size = New System.Drawing.Size(66, 28)
        Me.Lbkdprodi.TabIndex = 18
        '
        'TxtNama
        '
        Me.TxtNama.Location = New System.Drawing.Point(214, 193)
        Me.TxtNama.Name = "TxtNama"
        Me.TxtNama.Size = New System.Drawing.Size(184, 28)
        Me.TxtNama.TabIndex = 16
        '
        'LbKdDosen
        '
        Me.LbKdDosen.BackColor = System.Drawing.Color.Lime
        Me.LbKdDosen.ForeColor = System.Drawing.Color.White
        Me.LbKdDosen.Location = New System.Drawing.Point(214, 110)
        Me.LbKdDosen.Name = "LbKdDosen"
        Me.LbKdDosen.Size = New System.Drawing.Size(184, 28)
        Me.LbKdDosen.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(29, 364)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 21)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Jurusan "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(30, 276)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(68, 21)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "No Hp"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(27, 193)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(118, 21)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Nama Dosen"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(26, 236)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(123, 21)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Jenis Kelamin"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 157)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 21)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "NIDN"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(26, 114)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 21)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Kode Dosen"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Georgia", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(665, 88)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = ":: INPUT DATA DOSEN ::"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnSimpan
        '
        Me.BtnSimpan.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnSimpan.Location = New System.Drawing.Point(30, 31)
        Me.BtnSimpan.Name = "BtnSimpan"
        Me.BtnSimpan.Size = New System.Drawing.Size(127, 57)
        Me.BtnSimpan.TabIndex = 3
        Me.BtnSimpan.Text = "Simpan"
        Me.BtnSimpan.UseVisualStyleBackColor = True
        '
        'BtnKeluar
        '
        Me.BtnKeluar.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnKeluar.ForeColor = System.Drawing.Color.Black
        Me.BtnKeluar.Location = New System.Drawing.Point(513, 31)
        Me.BtnKeluar.Name = "BtnKeluar"
        Me.BtnKeluar.Size = New System.Drawing.Size(125, 57)
        Me.BtnKeluar.TabIndex = 2
        Me.BtnKeluar.Text = "Keluar"
        Me.BtnKeluar.UseVisualStyleBackColor = True
        '
        'CbJenisKelamin
        '
        Me.CbJenisKelamin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbJenisKelamin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbJenisKelamin.FormattingEnabled = True
        Me.CbJenisKelamin.Location = New System.Drawing.Point(214, 231)
        Me.CbJenisKelamin.Name = "CbJenisKelamin"
        Me.CbJenisKelamin.Size = New System.Drawing.Size(165, 29)
        Me.CbJenisKelamin.TabIndex = 10
        '
        'TxtNidn
        '
        Me.TxtNidn.Location = New System.Drawing.Point(214, 153)
        Me.TxtNidn.Name = "TxtNidn"
        Me.TxtNidn.Size = New System.Drawing.Size(282, 28)
        Me.TxtNidn.TabIndex = 9
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.GroupBox1.Controls.Add(Me.TxtEmail)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.TxtNoHp)
        Me.GroupBox1.Controls.Add(Me.Lbkdprodi)
        Me.GroupBox1.Controls.Add(Me.TxtNama)
        Me.GroupBox1.Controls.Add(Me.LbKdDosen)
        Me.GroupBox1.Controls.Add(Me.CbJurusan)
        Me.GroupBox1.Controls.Add(Me.CbJenisKelamin)
        Me.GroupBox1.Controls.Add(Me.TxtNidn)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Font = New System.Drawing.Font("Garamond", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(665, 402)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = ":: Input Data :"
        '
        'TxtEmail
        '
        Me.TxtEmail.Location = New System.Drawing.Point(214, 317)
        Me.TxtEmail.Name = "TxtEmail"
        Me.TxtEmail.Size = New System.Drawing.Size(184, 28)
        Me.TxtEmail.TabIndex = 21
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(30, 317)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 21)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Email"
        '
        'TxtNoHp
        '
        Me.TxtNoHp.Location = New System.Drawing.Point(214, 276)
        Me.TxtNoHp.Name = "TxtNoHp"
        Me.TxtNoHp.Size = New System.Drawing.Size(184, 28)
        Me.TxtNoHp.TabIndex = 19
        '
        'CbJurusan
        '
        Me.CbJurusan.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbJurusan.FormattingEnabled = True
        Me.CbJurusan.Location = New System.Drawing.Point(285, 359)
        Me.CbJurusan.Name = "CbJurusan"
        Me.CbJurusan.Size = New System.Drawing.Size(353, 29)
        Me.CbJurusan.TabIndex = 13
        '
        'BtnHapus
        '
        Me.BtnHapus.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.BtnHapus.Location = New System.Drawing.Point(272, 31)
        Me.BtnHapus.Name = "BtnHapus"
        Me.BtnHapus.Size = New System.Drawing.Size(126, 57)
        Me.BtnHapus.TabIndex = 1
        Me.BtnHapus.Text = "Hapus"
        Me.BtnHapus.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.GroupBox2.Controls.Add(Me.BtnSimpan)
        Me.GroupBox2.Controls.Add(Me.BtnKeluar)
        Me.GroupBox2.Controls.Add(Me.BtnHapus)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox2.Font = New System.Drawing.Font("Georgia", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(0, 402)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(665, 113)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        '
        'FrmInputDataDosen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(665, 515)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Name = "FrmInputDataDosen"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmInputDataDosen"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Lbkdprodi As Label
    Friend WithEvents TxtNama As TextBox
    Friend WithEvents LbKdDosen As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnSimpan As Button
    Friend WithEvents BtnKeluar As Button
    Friend WithEvents CbJenisKelamin As ComboBox
    Friend WithEvents TxtNidn As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents CbJurusan As ComboBox
    Friend WithEvents BtnHapus As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents TxtNoHp As TextBox
    Friend WithEvents TxtEmail As TextBox
    Friend WithEvents Label8 As Label
End Class
