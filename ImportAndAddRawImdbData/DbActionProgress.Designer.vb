<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DbActionProgress
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
        Cancel_Button = New Button()
        ProgressLogTextBox = New TextBox()
        Label1 = New Label()
        SuspendLayout()
        ' 
        ' Cancel_Button
        ' 
        Cancel_Button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Cancel_Button.Location = New Point(59, 135)
        Cancel_Button.Name = "Cancel_Button"
        Cancel_Button.Size = New Size(377, 23)
        Cancel_Button.TabIndex = 0
        Cancel_Button.Text = "&Cancel"
        Cancel_Button.UseVisualStyleBackColor = True
        ' 
        ' ProgressLogTextBox
        ' 
        ProgressLogTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ProgressLogTextBox.BorderStyle = BorderStyle.FixedSingle
        ProgressLogTextBox.Location = New Point(12, 27)
        ProgressLogTextBox.Multiline = True
        ProgressLogTextBox.Name = "ProgressLogTextBox"
        ProgressLogTextBox.ReadOnly = True
        ProgressLogTextBox.ScrollBars = ScrollBars.Vertical
        ProgressLogTextBox.Size = New Size(471, 102)
        ProgressLogTextBox.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(65, 15)
        Label1.TabIndex = 2
        Label1.Text = "Status Log:"
        ' 
        ' DbActionProgress
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(495, 170)
        ControlBox = False
        Controls.Add(Label1)
        Controls.Add(ProgressLogTextBox)
        Controls.Add(Cancel_Button)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Name = "DbActionProgress"
        Text = "DbActionProgress"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Cancel_Button As Button
    Friend WithEvents ProgressLogTextBox As TextBox
    Friend WithEvents Label1 As Label
End Class
