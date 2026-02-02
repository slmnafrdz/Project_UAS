<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLaporanPenjadMatakuliah
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
        Me.CrystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CbJenisKelas = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CbTahunAkademik = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CbNamaSemester = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnKeluar = New System.Windows.Forms.Button()
        Me.BtnCetak = New System.Windows.Forms.Button()
        Me.CbNamaProdi = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CrystalReportViewer1
        '
        Me.CrystalReportViewer1.ActiveViewIndex = -1
        Me.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default
        Me.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrystalReportViewer1.Location = New System.Drawing.Point(0, 204)
        Me.CrystalReportViewer1.Name = "CrystalReportViewer1"
        Me.CrystalReportViewer1.ShowCloseButton = False
        Me.CrystalReportViewer1.ShowCopyButton = False
        Me.CrystalReportViewer1.ShowGotoPageButton = False
        Me.CrystalReportViewer1.ShowGroupTreeButton = False
        Me.CrystalReportViewer1.ShowLogo = False
        Me.CrystalReportViewer1.ShowParameterPanelButton = False
        Me.CrystalReportViewer1.ShowTextSearchButton = False
        Me.CrystalReportViewer1.Size = New System.Drawing.Size(1095, 418)
        Me.CrystalReportViewer1.TabIndex = 11
        Me.CrystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Orange
        Me.Panel1.Controls.Add(Me.CbJenisKelas)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.CbTahunAkademik)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.CbNamaSemester)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.BtnKeluar)
        Me.Panel1.Controls.Add(Me.BtnCetak)
        Me.Panel1.Controls.Add(Me.CbNamaProdi)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1095, 204)
        Me.Panel1.TabIndex = 10
        '
        'CbJenisKelas
        '
        Me.CbJenisKelas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbJenisKelas.Font = New System.Drawing.Font("Times New Roman", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbJenisKelas.FormattingEnabled = True
        Me.CbJenisKelas.Location = New System.Drawing.Point(473, 160)
        Me.CbJenisKelas.Name = "CbJenisKelas"
        Me.CbJenisKelas.Size = New System.Drawing.Size(213, 25)
        Me.CbJenisKelas.TabIndex = 14
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(469, 129)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(50, 19)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Kelas"
        '
        'CbTahunAkademik
        '
        Me.CbTahunAkademik.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbTahunAkademik.Font = New System.Drawing.Font("Times New Roman", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbTahunAkademik.FormattingEnabled = True
        Me.CbTahunAkademik.Location = New System.Drawing.Point(473, 91)
        Me.CbTahunAkademik.Name = "CbTahunAkademik"
        Me.CbTahunAkademik.Size = New System.Drawing.Size(213, 25)
        Me.CbTahunAkademik.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(469, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(129, 19)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Tahun Akademik"
        '
        'CbNamaSemester
        '
        Me.CbNamaSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbNamaSemester.Font = New System.Drawing.Font("Times New Roman", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbNamaSemester.FormattingEnabled = True
        Me.CbNamaSemester.Location = New System.Drawing.Point(42, 160)
        Me.CbNamaSemester.Name = "CbNamaSemester"
        Me.CbNamaSemester.Size = New System.Drawing.Size(179, 25)
        Me.CbNamaSemester.TabIndex = 10
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(38, 129)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(123, 19)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Nama Semester"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Lime
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1095, 47)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "LAPORAN DATA PENJADWALAN MATKUL"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnKeluar
        '
        Me.BtnKeluar.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BtnKeluar.AutoSize = True
        Me.BtnKeluar.Font = New System.Drawing.Font("Times New Roman", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnKeluar.ForeColor = System.Drawing.Color.Red
        Me.BtnKeluar.Location = New System.Drawing.Point(968, 121)
        Me.BtnKeluar.Name = "BtnKeluar"
        Me.BtnKeluar.Size = New System.Drawing.Size(100, 35)
        Me.BtnKeluar.TabIndex = 6
        Me.BtnKeluar.Text = "Keluar"
        Me.BtnKeluar.UseVisualStyleBackColor = True
        '
        'BtnCetak
        '
        Me.BtnCetak.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BtnCetak.AutoSize = True
        Me.BtnCetak.Font = New System.Drawing.Font("Times New Roman", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCetak.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.BtnCetak.Location = New System.Drawing.Point(794, 121)
        Me.BtnCetak.Name = "BtnCetak"
        Me.BtnCetak.Size = New System.Drawing.Size(159, 35)
        Me.BtnCetak.TabIndex = 7
        Me.BtnCetak.Text = "Cetak Data"
        Me.BtnCetak.UseVisualStyleBackColor = True
        '
        'CbNamaProdi
        '
        Me.CbNamaProdi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbNamaProdi.Font = New System.Drawing.Font("Times New Roman", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CbNamaProdi.FormattingEnabled = True
        Me.CbNamaProdi.Location = New System.Drawing.Point(42, 91)
        Me.CbNamaProdi.Name = "CbNamaProdi"
        Me.CbNamaProdi.Size = New System.Drawing.Size(384, 25)
        Me.CbNamaProdi.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(38, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 19)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Cari jurusan"
        '
        'FrmLaporanPenjadMatakuliah
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1095, 622)
        Me.Controls.Add(Me.CrystalReportViewer1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "FrmLaporanPenjadMatakuliah"
        Me.Text = "FrmLaporanPenjadMatakuliah"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CrystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents Panel1 As Panel
    Friend WithEvents CbJenisKelas As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents CbTahunAkademik As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents CbNamaSemester As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnKeluar As Button
    Friend WithEvents BtnCetak As Button
    Friend WithEvents CbNamaProdi As ComboBox
    Friend WithEvents Label2 As Label
End Class
