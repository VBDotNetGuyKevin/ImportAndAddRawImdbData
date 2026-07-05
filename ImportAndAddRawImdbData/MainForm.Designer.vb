<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        FolderLocationTextBox = New TextBox()
        Label1 = New Label()
        ChooseFolderButton = New Button()
        ChooseFolderDialog = New FolderBrowserDialog()
        LoadAllDataFilesButton = New Button()
        DownloadUpdatedArchivesButton = New Button()
        ArchiveDownloadProgressBar = New ProgressBar()
        CurrentFileTextBox = New TextBox()
        CurrentFileLabel = New Label()
        DownloadFileNumberTextBox = New TextBox()
        SuspendLayout()
        ' 
        ' FolderLocationTextBox
        ' 
        FolderLocationTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        FolderLocationTextBox.Location = New Point(12, 106)
        FolderLocationTextBox.Name = "FolderLocationTextBox"
        FolderLocationTextBox.Size = New Size(541, 23)
        FolderLocationTextBox.TabIndex = 0
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 88)
        Label1.Name = "Label1"
        Label1.Size = New Size(138, 15)
        Label1.TabIndex = 1
        Label1.Text = "IMDB Data Files Location"
        ' 
        ' ChooseFolderButton
        ' 
        ChooseFolderButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ChooseFolderButton.Location = New Point(559, 106)
        ChooseFolderButton.Name = "ChooseFolderButton"
        ChooseFolderButton.Size = New Size(25, 23)
        ChooseFolderButton.TabIndex = 2
        ChooseFolderButton.Text = "..."
        ChooseFolderButton.UseVisualStyleBackColor = True
        ' 
        ' LoadAllDataFilesButton
        ' 
        LoadAllDataFilesButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        LoadAllDataFilesButton.Location = New Point(590, 106)
        LoadAllDataFilesButton.Name = "LoadAllDataFilesButton"
        LoadAllDataFilesButton.Size = New Size(198, 23)
        LoadAllDataFilesButton.TabIndex = 3
        LoadAllDataFilesButton.Text = "Load All Data to IMDB Raw Tables"
        LoadAllDataFilesButton.UseVisualStyleBackColor = True
        ' 
        ' DownloadUpdatedArchivesButton
        ' 
        DownloadUpdatedArchivesButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        DownloadUpdatedArchivesButton.Location = New Point(590, 54)
        DownloadUpdatedArchivesButton.Name = "DownloadUpdatedArchivesButton"
        DownloadUpdatedArchivesButton.Size = New Size(198, 23)
        DownloadUpdatedArchivesButton.TabIndex = 4
        DownloadUpdatedArchivesButton.Text = "&Download Updated Archive Files"
        DownloadUpdatedArchivesButton.UseVisualStyleBackColor = True
        ' 
        ' ArchiveDownloadProgressBar
        ' 
        ArchiveDownloadProgressBar.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ArchiveDownloadProgressBar.Location = New Point(12, 54)
        ArchiveDownloadProgressBar.Name = "ArchiveDownloadProgressBar"
        ArchiveDownloadProgressBar.Size = New Size(572, 23)
        ArchiveDownloadProgressBar.TabIndex = 5
        ' 
        ' CurrentFileTextBox
        ' 
        CurrentFileTextBox.Location = New Point(215, 25)
        CurrentFileTextBox.Name = "CurrentFileTextBox"
        CurrentFileTextBox.ReadOnly = True
        CurrentFileTextBox.Size = New Size(289, 23)
        CurrentFileTextBox.TabIndex = 6
        ' 
        ' CurrentFileLabel
        ' 
        CurrentFileLabel.AutoSize = True
        CurrentFileLabel.Location = New Point(12, 28)
        CurrentFileLabel.Name = "CurrentFileLabel"
        CurrentFileLabel.Size = New Size(197, 15)
        CurrentFileLabel.TabIndex = 7
        CurrentFileLabel.Text = "Currently Downloading Archive File:"
        ' 
        ' DownloadFileNumberTextBox
        ' 
        DownloadFileNumberTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        DownloadFileNumberTextBox.Location = New Point(510, 25)
        DownloadFileNumberTextBox.Name = "DownloadFileNumberTextBox"
        DownloadFileNumberTextBox.ReadOnly = True
        DownloadFileNumberTextBox.Size = New Size(74, 23)
        DownloadFileNumberTextBox.TabIndex = 8
        DownloadFileNumberTextBox.Text = "1 of 7"
        DownloadFileNumberTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(DownloadFileNumberTextBox)
        Controls.Add(CurrentFileLabel)
        Controls.Add(CurrentFileTextBox)
        Controls.Add(ArchiveDownloadProgressBar)
        Controls.Add(DownloadUpdatedArchivesButton)
        Controls.Add(LoadAllDataFilesButton)
        Controls.Add(ChooseFolderButton)
        Controls.Add(Label1)
        Controls.Add(FolderLocationTextBox)
        Name = "MainForm"
        Text = "Main Form"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents FolderLocationTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ChooseFolderButton As Button
    Friend WithEvents ChooseFolderDialog As FolderBrowserDialog
    Friend WithEvents LoadAllDataFilesButton As Button
    Friend WithEvents DownloadUpdatedArchivesButton As Button
    Friend WithEvents ArchiveDownloadProgressBar As ProgressBar
    Friend WithEvents CurrentFileTextBox As TextBox
    Friend WithEvents CurrentFileLabel As Label
    Friend WithEvents DownloadFileNumberTextBox As TextBox

End Class
