<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CountOrInsertData
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
        ChooseArchivesCheckedListBox = New CheckedListBox()
        ChooseAllOrSelectedComboBox = New ComboBox()
        Label1 = New Label()
        ProcessFilesButton = New Button()
        ExitButton = New Button()
        ProcessSequentiallyRadioButton = New RadioButton()
        ProcessInParallelRadioButton = New RadioButton()
        ChooseSequentialOrParallelGroupBox = New GroupBox()
        ChooseSequentialOrParallelGroupBox.SuspendLayout()
        SuspendLayout()
        ' 
        ' ChooseArchivesCheckedListBox
        ' 
        ChooseArchivesCheckedListBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ChooseArchivesCheckedListBox.Enabled = False
        ChooseArchivesCheckedListBox.FormattingEnabled = True
        ChooseArchivesCheckedListBox.Items.AddRange(New Object() {"name.basics", "title.akas", "title.basics", "title.crew", "title.episode", "title.principals", "title.ratings"})
        ChooseArchivesCheckedListBox.Location = New Point(13, 56)
        ChooseArchivesCheckedListBox.Name = "ChooseArchivesCheckedListBox"
        ChooseArchivesCheckedListBox.Size = New Size(237, 130)
        ChooseArchivesCheckedListBox.TabIndex = 0
        ' 
        ' ChooseAllOrSelectedComboBox
        ' 
        ChooseAllOrSelectedComboBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ChooseAllOrSelectedComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        ChooseAllOrSelectedComboBox.FormattingEnabled = True
        ChooseAllOrSelectedComboBox.Items.AddRange(New Object() {"All Available Archives", "Specific Archives"})
        ChooseAllOrSelectedComboBox.Location = New Point(12, 27)
        ChooseAllOrSelectedComboBox.Name = "ChooseAllOrSelectedComboBox"
        ChooseAllOrSelectedComboBox.Size = New Size(238, 23)
        ChooseAllOrSelectedComboBox.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(169, 15)
        Label1.TabIndex = 2
        Label1.Text = "Count All or Only Specific Files"
        ' 
        ' ProcessFilesButton
        ' 
        ProcessFilesButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        ProcessFilesButton.Enabled = False
        ProcessFilesButton.Location = New Point(12, 276)
        ProcessFilesButton.Name = "ProcessFilesButton"
        ProcessFilesButton.Size = New Size(108, 23)
        ProcessFilesButton.TabIndex = 4
        ProcessFilesButton.Text = "Process &Files"
        ProcessFilesButton.UseVisualStyleBackColor = True
        ' 
        ' ExitButton
        ' 
        ExitButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        ExitButton.Location = New Point(142, 276)
        ExitButton.Name = "ExitButton"
        ExitButton.Size = New Size(108, 23)
        ExitButton.TabIndex = 5
        ExitButton.Text = "&Cancel"
        ExitButton.UseVisualStyleBackColor = True
        ' 
        ' ProcessSequentiallyRadioButton
        ' 
        ProcessSequentiallyRadioButton.AutoSize = True
        ProcessSequentiallyRadioButton.Checked = True
        ProcessSequentiallyRadioButton.Location = New Point(6, 22)
        ProcessSequentiallyRadioButton.Name = "ProcessSequentiallyRadioButton"
        ProcessSequentiallyRadioButton.Size = New Size(132, 19)
        ProcessSequentiallyRadioButton.TabIndex = 0
        ProcessSequentiallyRadioButton.TabStop = True
        ProcessSequentiallyRadioButton.Text = "Process &Sequentially"
        ProcessSequentiallyRadioButton.UseVisualStyleBackColor = True
        ' 
        ' ProcessInParallelRadioButton
        ' 
        ProcessInParallelRadioButton.AutoSize = True
        ProcessInParallelRadioButton.Location = New Point(6, 47)
        ProcessInParallelRadioButton.Name = "ProcessInParallelRadioButton"
        ProcessInParallelRadioButton.Size = New Size(119, 19)
        ProcessInParallelRadioButton.TabIndex = 1
        ProcessInParallelRadioButton.TabStop = True
        ProcessInParallelRadioButton.Text = "&Process in Parallel"
        ProcessInParallelRadioButton.UseVisualStyleBackColor = True
        ' 
        ' ChooseSequentialOrParallelGroupBox
        ' 
        ChooseSequentialOrParallelGroupBox.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ChooseSequentialOrParallelGroupBox.Controls.Add(ProcessInParallelRadioButton)
        ChooseSequentialOrParallelGroupBox.Controls.Add(ProcessSequentiallyRadioButton)
        ChooseSequentialOrParallelGroupBox.Location = New Point(12, 192)
        ChooseSequentialOrParallelGroupBox.Name = "ChooseSequentialOrParallelGroupBox"
        ChooseSequentialOrParallelGroupBox.Size = New Size(238, 78)
        ChooseSequentialOrParallelGroupBox.TabIndex = 3
        ChooseSequentialOrParallelGroupBox.TabStop = False
        ChooseSequentialOrParallelGroupBox.Text = "Process Files Sequentially or in Parallel?"
        ' 
        ' CountOrInsertData
        ' 
        AcceptButton = ProcessFilesButton
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = ExitButton
        ClientSize = New Size(262, 311)
        ControlBox = False
        Controls.Add(ExitButton)
        Controls.Add(ProcessFilesButton)
        Controls.Add(ChooseSequentialOrParallelGroupBox)
        Controls.Add(Label1)
        Controls.Add(ChooseAllOrSelectedComboBox)
        Controls.Add(ChooseArchivesCheckedListBox)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximumSize = New Size(278, 350)
        MinimumSize = New Size(278, 267)
        Name = "CountOrInsertData"
        Text = "CountOrInsertData"
        ChooseSequentialOrParallelGroupBox.ResumeLayout(False)
        ChooseSequentialOrParallelGroupBox.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ChooseArchivesCheckedListBox As CheckedListBox
    Friend WithEvents ChooseAllOrSelectedComboBox As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ProcessFilesButton As Button
    Friend WithEvents ExitButton As Button
    Friend WithEvents ProcessSequentiallyRadioButton As RadioButton
    Friend WithEvents ProcessInParallelRadioButton As RadioButton
    Friend WithEvents ChooseSequentialOrParallelGroupBox As GroupBox
End Class
