<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm2
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
        ChooseFolderButton = New Button()
        Label1 = New Label()
        FolderLocationTextBox = New TextBox()
        DownloadFileNumberTextBox = New TextBox()
        CurrentFileLabel = New Label()
        CurrentFileTextBox = New TextBox()
        ArchiveDownloadProgressBar = New ProgressBar()
        DownloadUpdatedArchivesButton = New Button()
        LoadAllDataFilesButton = New Button()
        ChooseFolderDialog = New FolderBrowserDialog()
        ProgressLogTextBox = New TextBox()
        CurrentImportFileLabel = New Label()
        CurrentImportFileTextBox = New TextBox()
        CurrentImportFileNumberTextBox = New TextBox()
        CurrentRowNumberLabel = New Label()
        CurrentRowNumberTextBox = New TextBox()
        SqlBackgroundWorker = New ComponentModel.BackgroundWorker()
        EndThingsButton = New Button()
        FileCountsPanel = New Panel()
        TitleRatingsPreviousRowCountTextBox = New TextBox()
        TitlePrincipalsPreviousRowCountTextBox = New TextBox()
        TitleEpisodePreviousRowCountTextBox = New TextBox()
        Label6 = New Label()
        Label5 = New Label()
        TitleCrewPreviousRowCountTextBox = New TextBox()
        TitleBasicsPreviousRowCountTextBox = New TextBox()
        TitleAkasPreviousRowCountTextBox = New TextBox()
        NameBasicsPreviousRowCountTextBox = New TextBox()
        CountTsvRowsButton = New Button()
        CountArchiveRowsButton = New Button()
        FileSizeHeader2Label = New Label()
        TitleRatingsSizeTextBox = New TextBox()
        TitlePrincipalsSizeTextBox = New TextBox()
        TitleEpisodeSizeTextBox = New TextBox()
        CompressedFilenamesHeader2Label = New Label()
        RowCountsHeader2Label = New Label()
        TitleRatingsFilenameLabel = New Label()
        TitleRatingsCountTextBox = New TextBox()
        TitlePrincipalsFilenameLabel = New Label()
        TitlePrincipalsCountTextBox = New TextBox()
        TitleEpisodeFilenameLabel = New Label()
        TitleEpisodeCountTextBox = New TextBox()
        FileSizeHeader1Label = New Label()
        TitleCrewSizeTextBox = New TextBox()
        TitleBasicsSizeTextBox = New TextBox()
        TitleAkasSizeTextBox = New TextBox()
        NameBasicsSizeTextBox = New TextBox()
        CompressedFilenamesHeader1Label = New Label()
        RowCountsHeader1Label = New Label()
        TitleCrewFilenameLabel = New Label()
        TitleCrewCountTextBox = New TextBox()
        TitleBasicsFilenameLabel = New Label()
        TitleBasicsCountTextBox = New TextBox()
        TitleAkasFilenameLabel = New Label()
        TitleAkasCountTextBox = New TextBox()
        NameBasicsFilenameLabel = New Label()
        NameBasicsCountTextBox = New TextBox()
        NameBasicsBackgroundWorker = New ComponentModel.BackgroundWorker()
        TitleAkasBackgroundWorker = New ComponentModel.BackgroundWorker()
        TitleBasicsBackgroundWorker = New ComponentModel.BackgroundWorker()
        TitleCrewBackgroundWorker = New ComponentModel.BackgroundWorker()
        TitleEpisodeBackgroundWorker = New ComponentModel.BackgroundWorker()
        TitlePrincipalsBackgroundWorker = New ComponentModel.BackgroundWorker()
        TitleRatingsBackgroundWorker = New ComponentModel.BackgroundWorker()
        DecompressAfterDownloadCheckBox = New CheckBox()
        AllArchivesBackgroundWorker = New ComponentModel.BackgroundWorker()
        ImportDataButton = New Button()
        SqlImportBackgroundWorker = New ComponentModel.BackgroundWorker()
        Label2 = New Label()
        FileEstimatedTimeRemainingTextBox = New TextBox()
        OverallEstimatedTimeRemainingTextBox = New TextBox()
        Label3 = New Label()
        Label4 = New Label()
        Label7 = New Label()
        Label8 = New Label()
        ElapsedTimeForFileTextBox = New TextBox()
        OverallElapsedTimeTextBox = New TextBox()
        Label9 = New Label()
        Label10 = New Label()
        FileEstimatedProcessingTimeTextBox = New TextBox()
        OverallEstimatedProcessingTimeTextBox = New TextBox()
        ImportArchiveFileProgressBar = New ProgressBar()
        DownloadPanel = New Panel()
        LoadAllDataFilesPanel = New Panel()
        FileCountsPanel.SuspendLayout()
        DownloadPanel.SuspendLayout()
        LoadAllDataFilesPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' ChooseFolderButton
        ' 
        ChooseFolderButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ChooseFolderButton.Location = New Point(859, 26)
        ChooseFolderButton.Name = "ChooseFolderButton"
        ChooseFolderButton.Size = New Size(25, 23)
        ChooseFolderButton.TabIndex = 11
        ChooseFolderButton.Text = "..."
        ChooseFolderButton.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(12, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(138, 15)
        Label1.TabIndex = 10
        Label1.Text = "IMDB Data Files Location"
        ' 
        ' FolderLocationTextBox
        ' 
        FolderLocationTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        FolderLocationTextBox.Location = New Point(12, 27)
        FolderLocationTextBox.Name = "FolderLocationTextBox"
        FolderLocationTextBox.Size = New Size(841, 23)
        FolderLocationTextBox.TabIndex = 9
        ' 
        ' DownloadFileNumberTextBox
        ' 
        DownloadFileNumberTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        DownloadFileNumberTextBox.Location = New Point(939, 7)
        DownloadFileNumberTextBox.Name = "DownloadFileNumberTextBox"
        DownloadFileNumberTextBox.ReadOnly = True
        DownloadFileNumberTextBox.Size = New Size(74, 23)
        DownloadFileNumberTextBox.TabIndex = 17
        DownloadFileNumberTextBox.Text = "1 of 7"
        DownloadFileNumberTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' CurrentFileLabel
        ' 
        CurrentFileLabel.AutoSize = True
        CurrentFileLabel.Location = New Point(263, 10)
        CurrentFileLabel.Name = "CurrentFileLabel"
        CurrentFileLabel.Size = New Size(197, 15)
        CurrentFileLabel.TabIndex = 16
        CurrentFileLabel.Text = "Currently Downloading Archive File:"
        ' 
        ' CurrentFileTextBox
        ' 
        CurrentFileTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        CurrentFileTextBox.Location = New Point(466, 7)
        CurrentFileTextBox.Name = "CurrentFileTextBox"
        CurrentFileTextBox.ReadOnly = True
        CurrentFileTextBox.Size = New Size(467, 23)
        CurrentFileTextBox.TabIndex = 15
        ' 
        ' ArchiveDownloadProgressBar
        ' 
        ArchiveDownloadProgressBar.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ArchiveDownloadProgressBar.Location = New Point(263, 36)
        ArchiveDownloadProgressBar.Name = "ArchiveDownloadProgressBar"
        ArchiveDownloadProgressBar.Size = New Size(750, 23)
        ArchiveDownloadProgressBar.TabIndex = 14
        ' 
        ' DownloadUpdatedArchivesButton
        ' 
        DownloadUpdatedArchivesButton.Location = New Point(3, 6)
        DownloadUpdatedArchivesButton.Name = "DownloadUpdatedArchivesButton"
        DownloadUpdatedArchivesButton.Size = New Size(254, 23)
        DownloadUpdatedArchivesButton.TabIndex = 13
        DownloadUpdatedArchivesButton.Text = "#1 &Download Updated Archive Files"
        DownloadUpdatedArchivesButton.UseVisualStyleBackColor = True
        ' 
        ' LoadAllDataFilesButton
        ' 
        LoadAllDataFilesButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        LoadAllDataFilesButton.Location = New Point(3, 3)
        LoadAllDataFilesButton.Name = "LoadAllDataFilesButton"
        LoadAllDataFilesButton.Size = New Size(254, 23)
        LoadAllDataFilesButton.TabIndex = 12
        LoadAllDataFilesButton.Text = "#2 &Import Archive files into IMDB Raw Tables"
        LoadAllDataFilesButton.UseVisualStyleBackColor = True
        ' 
        ' ProgressLogTextBox
        ' 
        ProgressLogTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        ProgressLogTextBox.BorderStyle = BorderStyle.FixedSingle
        ProgressLogTextBox.Font = New Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ProgressLogTextBox.Location = New Point(12, 489)
        ProgressLogTextBox.Multiline = True
        ProgressLogTextBox.Name = "ProgressLogTextBox"
        ProgressLogTextBox.ReadOnly = True
        ProgressLogTextBox.ScrollBars = ScrollBars.Vertical
        ProgressLogTextBox.Size = New Size(1018, 330)
        ProgressLogTextBox.TabIndex = 18
        ' 
        ' CurrentImportFileLabel
        ' 
        CurrentImportFileLabel.AutoSize = True
        CurrentImportFileLabel.Location = New Point(3, 29)
        CurrentImportFileLabel.Name = "CurrentImportFileLabel"
        CurrentImportFileLabel.Size = New Size(179, 15)
        CurrentImportFileLabel.TabIndex = 19
        CurrentImportFileLabel.Text = "Currently Importing Archive File:"
        ' 
        ' CurrentImportFileTextBox
        ' 
        CurrentImportFileTextBox.Location = New Point(188, 26)
        CurrentImportFileTextBox.Name = "CurrentImportFileTextBox"
        CurrentImportFileTextBox.ReadOnly = True
        CurrentImportFileTextBox.Size = New Size(240, 23)
        CurrentImportFileTextBox.TabIndex = 20
        ' 
        ' CurrentImportFileNumberTextBox
        ' 
        CurrentImportFileNumberTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CurrentImportFileNumberTextBox.Location = New Point(735, 26)
        CurrentImportFileNumberTextBox.Name = "CurrentImportFileNumberTextBox"
        CurrentImportFileNumberTextBox.ReadOnly = True
        CurrentImportFileNumberTextBox.Size = New Size(74, 23)
        CurrentImportFileNumberTextBox.TabIndex = 21
        CurrentImportFileNumberTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' CurrentRowNumberLabel
        ' 
        CurrentRowNumberLabel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CurrentRowNumberLabel.AutoSize = True
        CurrentRowNumberLabel.Location = New Point(815, 29)
        CurrentRowNumberLabel.Name = "CurrentRowNumberLabel"
        CurrentRowNumberLabel.Size = New Size(43, 15)
        CurrentRowNumberLabel.TabIndex = 22
        CurrentRowNumberLabel.Text = "Row #:"
        ' 
        ' CurrentRowNumberTextBox
        ' 
        CurrentRowNumberTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        CurrentRowNumberTextBox.Location = New Point(864, 26)
        CurrentRowNumberTextBox.Name = "CurrentRowNumberTextBox"
        CurrentRowNumberTextBox.ReadOnly = True
        CurrentRowNumberTextBox.Size = New Size(149, 23)
        CurrentRowNumberTextBox.TabIndex = 23
        CurrentRowNumberTextBox.Text = "0"
        ' 
        ' SqlBackgroundWorker
        ' 
        SqlBackgroundWorker.WorkerReportsProgress = True
        SqlBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' EndThingsButton
        ' 
        EndThingsButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        EndThingsButton.Location = New Point(919, 26)
        EndThingsButton.Name = "EndThingsButton"
        EndThingsButton.Size = New Size(111, 23)
        EndThingsButton.TabIndex = 24
        EndThingsButton.Text = "E&xit"
        EndThingsButton.UseVisualStyleBackColor = True
        ' 
        ' FileCountsPanel
        ' 
        FileCountsPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        FileCountsPanel.BorderStyle = BorderStyle.FixedSingle
        FileCountsPanel.Controls.Add(TitleRatingsPreviousRowCountTextBox)
        FileCountsPanel.Controls.Add(TitlePrincipalsPreviousRowCountTextBox)
        FileCountsPanel.Controls.Add(TitleEpisodePreviousRowCountTextBox)
        FileCountsPanel.Controls.Add(Label6)
        FileCountsPanel.Controls.Add(Label5)
        FileCountsPanel.Controls.Add(TitleCrewPreviousRowCountTextBox)
        FileCountsPanel.Controls.Add(TitleBasicsPreviousRowCountTextBox)
        FileCountsPanel.Controls.Add(TitleAkasPreviousRowCountTextBox)
        FileCountsPanel.Controls.Add(NameBasicsPreviousRowCountTextBox)
        FileCountsPanel.Controls.Add(CountTsvRowsButton)
        FileCountsPanel.Controls.Add(CountArchiveRowsButton)
        FileCountsPanel.Controls.Add(FileSizeHeader2Label)
        FileCountsPanel.Controls.Add(TitleRatingsSizeTextBox)
        FileCountsPanel.Controls.Add(TitlePrincipalsSizeTextBox)
        FileCountsPanel.Controls.Add(TitleEpisodeSizeTextBox)
        FileCountsPanel.Controls.Add(CompressedFilenamesHeader2Label)
        FileCountsPanel.Controls.Add(RowCountsHeader2Label)
        FileCountsPanel.Controls.Add(TitleRatingsFilenameLabel)
        FileCountsPanel.Controls.Add(TitleRatingsCountTextBox)
        FileCountsPanel.Controls.Add(TitlePrincipalsFilenameLabel)
        FileCountsPanel.Controls.Add(TitlePrincipalsCountTextBox)
        FileCountsPanel.Controls.Add(TitleEpisodeFilenameLabel)
        FileCountsPanel.Controls.Add(TitleEpisodeCountTextBox)
        FileCountsPanel.Controls.Add(FileSizeHeader1Label)
        FileCountsPanel.Controls.Add(TitleCrewSizeTextBox)
        FileCountsPanel.Controls.Add(TitleBasicsSizeTextBox)
        FileCountsPanel.Controls.Add(TitleAkasSizeTextBox)
        FileCountsPanel.Controls.Add(NameBasicsSizeTextBox)
        FileCountsPanel.Controls.Add(CompressedFilenamesHeader1Label)
        FileCountsPanel.Controls.Add(RowCountsHeader1Label)
        FileCountsPanel.Controls.Add(TitleCrewFilenameLabel)
        FileCountsPanel.Controls.Add(TitleCrewCountTextBox)
        FileCountsPanel.Controls.Add(TitleBasicsFilenameLabel)
        FileCountsPanel.Controls.Add(TitleBasicsCountTextBox)
        FileCountsPanel.Controls.Add(TitleAkasFilenameLabel)
        FileCountsPanel.Controls.Add(TitleAkasCountTextBox)
        FileCountsPanel.Controls.Add(NameBasicsFilenameLabel)
        FileCountsPanel.Controls.Add(NameBasicsCountTextBox)
        FileCountsPanel.Location = New Point(12, 139)
        FileCountsPanel.Name = "FileCountsPanel"
        FileCountsPanel.Size = New Size(1018, 140)
        FileCountsPanel.TabIndex = 25
        ' 
        ' TitleRatingsPreviousRowCountTextBox
        ' 
        TitleRatingsPreviousRowCountTextBox.Location = New Point(747, 78)
        TitleRatingsPreviousRowCountTextBox.Name = "TitleRatingsPreviousRowCountTextBox"
        TitleRatingsPreviousRowCountTextBox.ReadOnly = True
        TitleRatingsPreviousRowCountTextBox.Size = New Size(93, 23)
        TitleRatingsPreviousRowCountTextBox.TabIndex = 40
        ' 
        ' TitlePrincipalsPreviousRowCountTextBox
        ' 
        TitlePrincipalsPreviousRowCountTextBox.Location = New Point(747, 53)
        TitlePrincipalsPreviousRowCountTextBox.Name = "TitlePrincipalsPreviousRowCountTextBox"
        TitlePrincipalsPreviousRowCountTextBox.ReadOnly = True
        TitlePrincipalsPreviousRowCountTextBox.Size = New Size(93, 23)
        TitlePrincipalsPreviousRowCountTextBox.TabIndex = 39
        ' 
        ' TitleEpisodePreviousRowCountTextBox
        ' 
        TitleEpisodePreviousRowCountTextBox.Location = New Point(747, 28)
        TitleEpisodePreviousRowCountTextBox.Name = "TitleEpisodePreviousRowCountTextBox"
        TitleEpisodePreviousRowCountTextBox.ReadOnly = True
        TitleEpisodePreviousRowCountTextBox.Size = New Size(93, 23)
        TitleEpisodePreviousRowCountTextBox.TabIndex = 38
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        Label6.Location = New Point(747, 9)
        Label6.Name = "Label6"
        Label6.Size = New Size(95, 15)
        Label6.TabIndex = 37
        Label6.Text = "Last Row Counts"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(319, 9)
        Label5.Name = "Label5"
        Label5.Size = New Size(95, 15)
        Label5.TabIndex = 36
        Label5.Text = "Last Row Counts"
        ' 
        ' TitleCrewPreviousRowCountTextBox
        ' 
        TitleCrewPreviousRowCountTextBox.Location = New Point(319, 103)
        TitleCrewPreviousRowCountTextBox.Name = "TitleCrewPreviousRowCountTextBox"
        TitleCrewPreviousRowCountTextBox.ReadOnly = True
        TitleCrewPreviousRowCountTextBox.Size = New Size(93, 23)
        TitleCrewPreviousRowCountTextBox.TabIndex = 35
        ' 
        ' TitleBasicsPreviousRowCountTextBox
        ' 
        TitleBasicsPreviousRowCountTextBox.Location = New Point(319, 78)
        TitleBasicsPreviousRowCountTextBox.Name = "TitleBasicsPreviousRowCountTextBox"
        TitleBasicsPreviousRowCountTextBox.ReadOnly = True
        TitleBasicsPreviousRowCountTextBox.Size = New Size(93, 23)
        TitleBasicsPreviousRowCountTextBox.TabIndex = 34
        ' 
        ' TitleAkasPreviousRowCountTextBox
        ' 
        TitleAkasPreviousRowCountTextBox.Location = New Point(319, 53)
        TitleAkasPreviousRowCountTextBox.Name = "TitleAkasPreviousRowCountTextBox"
        TitleAkasPreviousRowCountTextBox.ReadOnly = True
        TitleAkasPreviousRowCountTextBox.Size = New Size(93, 23)
        TitleAkasPreviousRowCountTextBox.TabIndex = 33
        ' 
        ' NameBasicsPreviousRowCountTextBox
        ' 
        NameBasicsPreviousRowCountTextBox.Location = New Point(319, 28)
        NameBasicsPreviousRowCountTextBox.Name = "NameBasicsPreviousRowCountTextBox"
        NameBasicsPreviousRowCountTextBox.ReadOnly = True
        NameBasicsPreviousRowCountTextBox.Size = New Size(93, 23)
        NameBasicsPreviousRowCountTextBox.TabIndex = 32
        ' 
        ' CountTsvRowsButton
        ' 
        CountTsvRowsButton.Enabled = False
        CountTsvRowsButton.Location = New Point(604, 104)
        CountTsvRowsButton.Name = "CountTsvRowsButton"
        CountTsvRowsButton.Size = New Size(177, 27)
        CountTsvRowsButton.TabIndex = 31
        CountTsvRowsButton.Text = "Count .ts&v Rows"
        CountTsvRowsButton.UseVisualStyleBackColor = True
        ' 
        ' CountArchiveRowsButton
        ' 
        CountArchiveRowsButton.Enabled = False
        CountArchiveRowsButton.Location = New Point(421, 104)
        CountArchiveRowsButton.Name = "CountArchiveRowsButton"
        CountArchiveRowsButton.Size = New Size(177, 27)
        CountArchiveRowsButton.TabIndex = 30
        CountArchiveRowsButton.Text = "Count .&gz Archive Rows"
        CountArchiveRowsButton.UseVisualStyleBackColor = True
        ' 
        ' FileSizeHeader2Label
        ' 
        FileSizeHeader2Label.AutoSize = True
        FileSizeHeader2Label.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        FileSizeHeader2Label.Location = New Point(550, 9)
        FileSizeHeader2Label.Name = "FileSizeHeader2Label"
        FileSizeHeader2Label.Size = New Size(48, 15)
        FileSizeHeader2Label.TabIndex = 29
        FileSizeHeader2Label.Text = "File Size"
        ' 
        ' TitleRatingsSizeTextBox
        ' 
        TitleRatingsSizeTextBox.Location = New Point(550, 78)
        TitleRatingsSizeTextBox.Name = "TitleRatingsSizeTextBox"
        TitleRatingsSizeTextBox.ReadOnly = True
        TitleRatingsSizeTextBox.Size = New Size(96, 23)
        TitleRatingsSizeTextBox.TabIndex = 27
        ' 
        ' TitlePrincipalsSizeTextBox
        ' 
        TitlePrincipalsSizeTextBox.Location = New Point(550, 53)
        TitlePrincipalsSizeTextBox.Name = "TitlePrincipalsSizeTextBox"
        TitlePrincipalsSizeTextBox.ReadOnly = True
        TitlePrincipalsSizeTextBox.Size = New Size(96, 23)
        TitlePrincipalsSizeTextBox.TabIndex = 26
        ' 
        ' TitleEpisodeSizeTextBox
        ' 
        TitleEpisodeSizeTextBox.Location = New Point(550, 28)
        TitleEpisodeSizeTextBox.Name = "TitleEpisodeSizeTextBox"
        TitleEpisodeSizeTextBox.ReadOnly = True
        TitleEpisodeSizeTextBox.Size = New Size(96, 23)
        TitleEpisodeSizeTextBox.TabIndex = 25
        ' 
        ' CompressedFilenamesHeader2Label
        ' 
        CompressedFilenamesHeader2Label.AutoSize = True
        CompressedFilenamesHeader2Label.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        CompressedFilenamesHeader2Label.Location = New Point(421, 9)
        CompressedFilenamesHeader2Label.Name = "CompressedFilenamesHeader2Label"
        CompressedFilenamesHeader2Label.Size = New Size(88, 15)
        CompressedFilenamesHeader2Label.TabIndex = 24
        CompressedFilenamesHeader2Label.Text = "GZip Filenames"
        ' 
        ' RowCountsHeader2Label
        ' 
        RowCountsHeader2Label.AutoSize = True
        RowCountsHeader2Label.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        RowCountsHeader2Label.Location = New Point(658, 9)
        RowCountsHeader2Label.Name = "RowCountsHeader2Label"
        RowCountsHeader2Label.Size = New Size(71, 15)
        RowCountsHeader2Label.TabIndex = 23
        RowCountsHeader2Label.Text = "Row Counts"
        ' 
        ' TitleRatingsFilenameLabel
        ' 
        TitleRatingsFilenameLabel.AutoSize = True
        TitleRatingsFilenameLabel.Location = New Point(421, 81)
        TitleRatingsFilenameLabel.Name = "TitleRatingsFilenameLabel"
        TitleRatingsFilenameLabel.Size = New Size(99, 15)
        TitleRatingsFilenameLabel.TabIndex = 20
        TitleRatingsFilenameLabel.Text = "title.ratings.tsv.gz"
        ' 
        ' TitleRatingsCountTextBox
        ' 
        TitleRatingsCountTextBox.Location = New Point(658, 78)
        TitleRatingsCountTextBox.Name = "TitleRatingsCountTextBox"
        TitleRatingsCountTextBox.ReadOnly = True
        TitleRatingsCountTextBox.Size = New Size(83, 23)
        TitleRatingsCountTextBox.TabIndex = 19
        ' 
        ' TitlePrincipalsFilenameLabel
        ' 
        TitlePrincipalsFilenameLabel.AutoSize = True
        TitlePrincipalsFilenameLabel.Location = New Point(421, 56)
        TitlePrincipalsFilenameLabel.Name = "TitlePrincipalsFilenameLabel"
        TitlePrincipalsFilenameLabel.Size = New Size(114, 15)
        TitlePrincipalsFilenameLabel.TabIndex = 18
        TitlePrincipalsFilenameLabel.Text = "title.principals.tsv.gz"
        ' 
        ' TitlePrincipalsCountTextBox
        ' 
        TitlePrincipalsCountTextBox.Location = New Point(658, 53)
        TitlePrincipalsCountTextBox.Name = "TitlePrincipalsCountTextBox"
        TitlePrincipalsCountTextBox.ReadOnly = True
        TitlePrincipalsCountTextBox.Size = New Size(83, 23)
        TitlePrincipalsCountTextBox.TabIndex = 17
        ' 
        ' TitleEpisodeFilenameLabel
        ' 
        TitleEpisodeFilenameLabel.AutoSize = True
        TitleEpisodeFilenameLabel.Location = New Point(421, 31)
        TitleEpisodeFilenameLabel.Name = "TitleEpisodeFilenameLabel"
        TitleEpisodeFilenameLabel.Size = New Size(104, 15)
        TitleEpisodeFilenameLabel.TabIndex = 16
        TitleEpisodeFilenameLabel.Text = "title.episode.tsv.gz"
        ' 
        ' TitleEpisodeCountTextBox
        ' 
        TitleEpisodeCountTextBox.Location = New Point(658, 28)
        TitleEpisodeCountTextBox.Name = "TitleEpisodeCountTextBox"
        TitleEpisodeCountTextBox.ReadOnly = True
        TitleEpisodeCountTextBox.Size = New Size(83, 23)
        TitleEpisodeCountTextBox.TabIndex = 15
        ' 
        ' FileSizeHeader1Label
        ' 
        FileSizeHeader1Label.AutoSize = True
        FileSizeHeader1Label.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        FileSizeHeader1Label.Location = New Point(128, 9)
        FileSizeHeader1Label.Name = "FileSizeHeader1Label"
        FileSizeHeader1Label.Size = New Size(66, 15)
        FileSizeHeader1Label.TabIndex = 14
        FileSizeHeader1Label.Text = "File Size .gz"
        ' 
        ' TitleCrewSizeTextBox
        ' 
        TitleCrewSizeTextBox.Location = New Point(128, 103)
        TitleCrewSizeTextBox.Name = "TitleCrewSizeTextBox"
        TitleCrewSizeTextBox.ReadOnly = True
        TitleCrewSizeTextBox.Size = New Size(96, 23)
        TitleCrewSizeTextBox.TabIndex = 13
        ' 
        ' TitleBasicsSizeTextBox
        ' 
        TitleBasicsSizeTextBox.Location = New Point(128, 78)
        TitleBasicsSizeTextBox.Name = "TitleBasicsSizeTextBox"
        TitleBasicsSizeTextBox.ReadOnly = True
        TitleBasicsSizeTextBox.Size = New Size(96, 23)
        TitleBasicsSizeTextBox.TabIndex = 12
        ' 
        ' TitleAkasSizeTextBox
        ' 
        TitleAkasSizeTextBox.Location = New Point(128, 53)
        TitleAkasSizeTextBox.Name = "TitleAkasSizeTextBox"
        TitleAkasSizeTextBox.ReadOnly = True
        TitleAkasSizeTextBox.Size = New Size(96, 23)
        TitleAkasSizeTextBox.TabIndex = 11
        ' 
        ' NameBasicsSizeTextBox
        ' 
        NameBasicsSizeTextBox.Location = New Point(128, 28)
        NameBasicsSizeTextBox.Name = "NameBasicsSizeTextBox"
        NameBasicsSizeTextBox.ReadOnly = True
        NameBasicsSizeTextBox.Size = New Size(96, 23)
        NameBasicsSizeTextBox.TabIndex = 10
        ' 
        ' CompressedFilenamesHeader1Label
        ' 
        CompressedFilenamesHeader1Label.AutoSize = True
        CompressedFilenamesHeader1Label.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        CompressedFilenamesHeader1Label.Location = New Point(2, 9)
        CompressedFilenamesHeader1Label.Name = "CompressedFilenamesHeader1Label"
        CompressedFilenamesHeader1Label.Size = New Size(88, 15)
        CompressedFilenamesHeader1Label.TabIndex = 9
        CompressedFilenamesHeader1Label.Text = "GZip Filenames"
        ' 
        ' RowCountsHeader1Label
        ' 
        RowCountsHeader1Label.AutoSize = True
        RowCountsHeader1Label.Font = New Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, CByte(0))
        RowCountsHeader1Label.Location = New Point(230, 9)
        RowCountsHeader1Label.Name = "RowCountsHeader1Label"
        RowCountsHeader1Label.Size = New Size(71, 15)
        RowCountsHeader1Label.TabIndex = 8
        RowCountsHeader1Label.Text = "Row Counts"
        ' 
        ' TitleCrewFilenameLabel
        ' 
        TitleCrewFilenameLabel.AutoSize = True
        TitleCrewFilenameLabel.Location = New Point(2, 106)
        TitleCrewFilenameLabel.Name = "TitleCrewFilenameLabel"
        TitleCrewFilenameLabel.Size = New Size(88, 15)
        TitleCrewFilenameLabel.TabIndex = 7
        TitleCrewFilenameLabel.Text = "title.crew.tsv.gz"
        ' 
        ' TitleCrewCountTextBox
        ' 
        TitleCrewCountTextBox.Location = New Point(230, 103)
        TitleCrewCountTextBox.Name = "TitleCrewCountTextBox"
        TitleCrewCountTextBox.ReadOnly = True
        TitleCrewCountTextBox.Size = New Size(83, 23)
        TitleCrewCountTextBox.TabIndex = 6
        ' 
        ' TitleBasicsFilenameLabel
        ' 
        TitleBasicsFilenameLabel.AutoSize = True
        TitleBasicsFilenameLabel.Location = New Point(2, 81)
        TitleBasicsFilenameLabel.Name = "TitleBasicsFilenameLabel"
        TitleBasicsFilenameLabel.Size = New Size(95, 15)
        TitleBasicsFilenameLabel.TabIndex = 5
        TitleBasicsFilenameLabel.Text = "title.basics.tsv.gz"
        ' 
        ' TitleBasicsCountTextBox
        ' 
        TitleBasicsCountTextBox.Location = New Point(230, 78)
        TitleBasicsCountTextBox.Name = "TitleBasicsCountTextBox"
        TitleBasicsCountTextBox.ReadOnly = True
        TitleBasicsCountTextBox.Size = New Size(83, 23)
        TitleBasicsCountTextBox.TabIndex = 4
        ' 
        ' TitleAkasFilenameLabel
        ' 
        TitleAkasFilenameLabel.AutoSize = True
        TitleAkasFilenameLabel.Location = New Point(2, 56)
        TitleAkasFilenameLabel.Name = "TitleAkasFilenameLabel"
        TitleAkasFilenameLabel.Size = New Size(86, 15)
        TitleAkasFilenameLabel.TabIndex = 3
        TitleAkasFilenameLabel.Text = "title.akas.tsv.gz"
        ' 
        ' TitleAkasCountTextBox
        ' 
        TitleAkasCountTextBox.Location = New Point(230, 53)
        TitleAkasCountTextBox.Name = "TitleAkasCountTextBox"
        TitleAkasCountTextBox.ReadOnly = True
        TitleAkasCountTextBox.Size = New Size(83, 23)
        TitleAkasCountTextBox.TabIndex = 2
        ' 
        ' NameBasicsFilenameLabel
        ' 
        NameBasicsFilenameLabel.AutoSize = True
        NameBasicsFilenameLabel.Location = New Point(2, 31)
        NameBasicsFilenameLabel.Name = "NameBasicsFilenameLabel"
        NameBasicsFilenameLabel.Size = New Size(105, 15)
        NameBasicsFilenameLabel.TabIndex = 1
        NameBasicsFilenameLabel.Text = "name.basics.tsv.gz"
        ' 
        ' NameBasicsCountTextBox
        ' 
        NameBasicsCountTextBox.Location = New Point(230, 28)
        NameBasicsCountTextBox.Name = "NameBasicsCountTextBox"
        NameBasicsCountTextBox.ReadOnly = True
        NameBasicsCountTextBox.Size = New Size(83, 23)
        NameBasicsCountTextBox.TabIndex = 0
        ' 
        ' NameBasicsBackgroundWorker
        ' 
        NameBasicsBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' TitleAkasBackgroundWorker
        ' 
        TitleAkasBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' TitleBasicsBackgroundWorker
        ' 
        TitleBasicsBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' TitleCrewBackgroundWorker
        ' 
        TitleCrewBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' TitleEpisodeBackgroundWorker
        ' 
        TitleEpisodeBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' TitlePrincipalsBackgroundWorker
        ' 
        TitlePrincipalsBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' TitleRatingsBackgroundWorker
        ' 
        TitleRatingsBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' DecompressAfterDownloadCheckBox
        ' 
        DecompressAfterDownloadCheckBox.AutoSize = True
        DecompressAfterDownloadCheckBox.Checked = True
        DecompressAfterDownloadCheckBox.CheckState = CheckState.Checked
        DecompressAfterDownloadCheckBox.Location = New Point(3, 35)
        DecompressAfterDownloadCheckBox.Name = "DecompressAfterDownloadCheckBox"
        DecompressAfterDownloadCheckBox.Size = New Size(236, 19)
        DecompressAfterDownloadCheckBox.TabIndex = 26
        DecompressAfterDownloadCheckBox.Text = "Decom&press and Delete After Download"
        DecompressAfterDownloadCheckBox.UseVisualStyleBackColor = True
        ' 
        ' AllArchivesBackgroundWorker
        ' 
        AllArchivesBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' ImportDataButton
        ' 
        ImportDataButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        ImportDataButton.Location = New Point(12, 443)
        ImportDataButton.Name = "ImportDataButton"
        ImportDataButton.Size = New Size(338, 25)
        ImportDataButton.TabIndex = 27
        ImportDataButton.Text = "#3 &Transform [IMDB].[Raw] data into [IMDB].[dbo] Tables "
        ImportDataButton.UseVisualStyleBackColor = True
        ' 
        ' SqlImportBackgroundWorker
        ' 
        SqlImportBackgroundWorker.WorkerReportsProgress = True
        SqlImportBackgroundWorker.WorkerSupportsCancellation = True
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Location = New Point(667, 87)
        Label2.Name = "Label2"
        Label2.Size = New Size(191, 15)
        Label2.TabIndex = 28
        Label2.Text = "Estimated Time Remaining for File:"
        ' 
        ' FileEstimatedTimeRemainingTextBox
        ' 
        FileEstimatedTimeRemainingTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        FileEstimatedTimeRemainingTextBox.Location = New Point(864, 84)
        FileEstimatedTimeRemainingTextBox.Name = "FileEstimatedTimeRemainingTextBox"
        FileEstimatedTimeRemainingTextBox.ReadOnly = True
        FileEstimatedTimeRemainingTextBox.Size = New Size(149, 23)
        FileEstimatedTimeRemainingTextBox.TabIndex = 29
        ' 
        ' OverallEstimatedTimeRemainingTextBox
        ' 
        OverallEstimatedTimeRemainingTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        OverallEstimatedTimeRemainingTextBox.Location = New Point(864, 113)
        OverallEstimatedTimeRemainingTextBox.Name = "OverallEstimatedTimeRemainingTextBox"
        OverallEstimatedTimeRemainingTextBox.ReadOnly = True
        OverallEstimatedTimeRemainingTextBox.Size = New Size(149, 23)
        OverallEstimatedTimeRemainingTextBox.TabIndex = 31
        ' 
        ' Label3
        ' 
        Label3.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label3.AutoSize = True
        Label3.Location = New Point(668, 116)
        Label3.Name = "Label3"
        Label3.Size = New Size(192, 15)
        Label3.TabIndex = 30
        Label3.Text = "Overall Estimated Time Remaining:"
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label4.AutoSize = True
        Label4.Location = New Point(12, 471)
        Label4.Name = "Label4"
        Label4.Size = New Size(75, 15)
        Label4.TabIndex = 32
        Label4.Text = "Progress Log"
        ' 
        ' Label7
        ' 
        Label7.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label7.AutoSize = True
        Label7.Location = New Point(387, 87)
        Label7.Name = "Label7"
        Label7.Size = New Size(119, 15)
        Label7.TabIndex = 33
        Label7.Text = "Elapsed Time for File:"
        ' 
        ' Label8
        ' 
        Label8.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label8.AutoSize = True
        Label8.Location = New Point(387, 116)
        Label8.Name = "Label8"
        Label8.Size = New Size(120, 15)
        Label8.TabIndex = 34
        Label8.Text = "Overall Elapsed Time:"
        ' 
        ' ElapsedTimeForFileTextBox
        ' 
        ElapsedTimeForFileTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ElapsedTimeForFileTextBox.Location = New Point(512, 84)
        ElapsedTimeForFileTextBox.Name = "ElapsedTimeForFileTextBox"
        ElapsedTimeForFileTextBox.ReadOnly = True
        ElapsedTimeForFileTextBox.Size = New Size(149, 23)
        ElapsedTimeForFileTextBox.TabIndex = 35
        ' 
        ' OverallElapsedTimeTextBox
        ' 
        OverallElapsedTimeTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        OverallElapsedTimeTextBox.Location = New Point(513, 113)
        OverallElapsedTimeTextBox.Name = "OverallElapsedTimeTextBox"
        OverallElapsedTimeTextBox.ReadOnly = True
        OverallElapsedTimeTextBox.Size = New Size(149, 23)
        OverallElapsedTimeTextBox.TabIndex = 36
        ' 
        ' Label9
        ' 
        Label9.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label9.AutoSize = True
        Label9.Location = New Point(3, 87)
        Label9.Name = "Label9"
        Label9.Size = New Size(191, 15)
        Label9.TabIndex = 37
        Label9.Text = "Estimated Processing Time for File:"
        ' 
        ' Label10
        ' 
        Label10.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label10.AutoSize = True
        Label10.Location = New Point(3, 116)
        Label10.Name = "Label10"
        Label10.Size = New Size(239, 15)
        Label10.TabIndex = 38
        Label10.Text = "Overall Estimated Processing Time (all files):"
        ' 
        ' FileEstimatedProcessingTimeTextBox
        ' 
        FileEstimatedProcessingTimeTextBox.Location = New Point(248, 84)
        FileEstimatedProcessingTimeTextBox.Name = "FileEstimatedProcessingTimeTextBox"
        FileEstimatedProcessingTimeTextBox.ReadOnly = True
        FileEstimatedProcessingTimeTextBox.Size = New Size(123, 23)
        FileEstimatedProcessingTimeTextBox.TabIndex = 39
        ' 
        ' OverallEstimatedProcessingTimeTextBox
        ' 
        OverallEstimatedProcessingTimeTextBox.Location = New Point(248, 113)
        OverallEstimatedProcessingTimeTextBox.Name = "OverallEstimatedProcessingTimeTextBox"
        OverallEstimatedProcessingTimeTextBox.ReadOnly = True
        OverallEstimatedProcessingTimeTextBox.Size = New Size(123, 23)
        OverallEstimatedProcessingTimeTextBox.TabIndex = 40
        ' 
        ' ImportArchiveFileProgressBar
        ' 
        ImportArchiveFileProgressBar.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        ImportArchiveFileProgressBar.Location = New Point(3, 55)
        ImportArchiveFileProgressBar.Name = "ImportArchiveFileProgressBar"
        ImportArchiveFileProgressBar.Size = New Size(1010, 23)
        ImportArchiveFileProgressBar.TabIndex = 41
        ' 
        ' DownloadPanel
        ' 
        DownloadPanel.BorderStyle = BorderStyle.FixedSingle
        DownloadPanel.Controls.Add(ArchiveDownloadProgressBar)
        DownloadPanel.Controls.Add(DownloadFileNumberTextBox)
        DownloadPanel.Controls.Add(CurrentFileTextBox)
        DownloadPanel.Controls.Add(CurrentFileLabel)
        DownloadPanel.Controls.Add(DownloadUpdatedArchivesButton)
        DownloadPanel.Controls.Add(DecompressAfterDownloadCheckBox)
        DownloadPanel.Location = New Point(12, 56)
        DownloadPanel.Name = "DownloadPanel"
        DownloadPanel.Size = New Size(1018, 73)
        DownloadPanel.TabIndex = 43
        ' 
        ' LoadAllDataFilesPanel
        ' 
        LoadAllDataFilesPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        LoadAllDataFilesPanel.BorderStyle = BorderStyle.FixedSingle
        LoadAllDataFilesPanel.Controls.Add(LoadAllDataFilesButton)
        LoadAllDataFilesPanel.Controls.Add(CurrentImportFileLabel)
        LoadAllDataFilesPanel.Controls.Add(OverallElapsedTimeTextBox)
        LoadAllDataFilesPanel.Controls.Add(OverallEstimatedTimeRemainingTextBox)
        LoadAllDataFilesPanel.Controls.Add(OverallEstimatedProcessingTimeTextBox)
        LoadAllDataFilesPanel.Controls.Add(Label3)
        LoadAllDataFilesPanel.Controls.Add(Label8)
        LoadAllDataFilesPanel.Controls.Add(ImportArchiveFileProgressBar)
        LoadAllDataFilesPanel.Controls.Add(Label10)
        LoadAllDataFilesPanel.Controls.Add(FileEstimatedProcessingTimeTextBox)
        LoadAllDataFilesPanel.Controls.Add(CurrentImportFileTextBox)
        LoadAllDataFilesPanel.Controls.Add(ElapsedTimeForFileTextBox)
        LoadAllDataFilesPanel.Controls.Add(CurrentImportFileNumberTextBox)
        LoadAllDataFilesPanel.Controls.Add(Label9)
        LoadAllDataFilesPanel.Controls.Add(Label7)
        LoadAllDataFilesPanel.Controls.Add(FileEstimatedTimeRemainingTextBox)
        LoadAllDataFilesPanel.Controls.Add(CurrentRowNumberLabel)
        LoadAllDataFilesPanel.Controls.Add(Label2)
        LoadAllDataFilesPanel.Controls.Add(CurrentRowNumberTextBox)
        LoadAllDataFilesPanel.Location = New Point(12, 290)
        LoadAllDataFilesPanel.Name = "LoadAllDataFilesPanel"
        LoadAllDataFilesPanel.Size = New Size(1018, 147)
        LoadAllDataFilesPanel.TabIndex = 44
        ' 
        ' MainForm2
        ' 
        AcceptButton = LoadAllDataFilesButton
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = EndThingsButton
        ClientSize = New Size(1042, 831)
        Controls.Add(LoadAllDataFilesPanel)
        Controls.Add(DownloadPanel)
        Controls.Add(Label4)
        Controls.Add(ImportDataButton)
        Controls.Add(FileCountsPanel)
        Controls.Add(EndThingsButton)
        Controls.Add(ProgressLogTextBox)
        Controls.Add(ChooseFolderButton)
        Controls.Add(Label1)
        Controls.Add(FolderLocationTextBox)
        MinimumSize = New Size(1058, 870)
        Name = "MainForm2"
        Text = "Download Compressed, RAW IMDB Data Files and Upload/Transform into SQL Server DB Tables"
        FileCountsPanel.ResumeLayout(False)
        FileCountsPanel.PerformLayout()
        DownloadPanel.ResumeLayout(False)
        DownloadPanel.PerformLayout()
        LoadAllDataFilesPanel.ResumeLayout(False)
        LoadAllDataFilesPanel.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents ChooseFolderButton As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents FolderLocationTextBox As TextBox
    Friend WithEvents DownloadFileNumberTextBox As TextBox
    Friend WithEvents CurrentFileLabel As Label
    Friend WithEvents CurrentFileTextBox As TextBox
    Friend WithEvents ArchiveDownloadProgressBar As ProgressBar
    Friend WithEvents DownloadUpdatedArchivesButton As Button
    Friend WithEvents LoadAllDataFilesButton As Button
    Friend WithEvents ChooseFolderDialog As FolderBrowserDialog
    Friend WithEvents ProgressLogTextBox As TextBox
    Friend WithEvents CurrentImportFileLabel As Label
    Friend WithEvents CurrentImportFileTextBox As TextBox
    Friend WithEvents CurrentImportFileNumberTextBox As TextBox
    Friend WithEvents CurrentRowNumberLabel As Label
    Friend WithEvents CurrentRowNumberTextBox As TextBox
    Friend WithEvents SqlBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents EndThingsButton As Button
    Friend WithEvents FileCountsPanel As Panel
    Friend WithEvents NameBasicsCountTextBox As TextBox
    Friend WithEvents NameBasicsFilenameLabel As Label
    Friend WithEvents TitleAkasFilenameLabel As Label
    Friend WithEvents TitleAkasCountTextBox As TextBox
    Friend WithEvents TitleBasicsFilenameLabel As Label
    Friend WithEvents TitleBasicsCountTextBox As TextBox
    Friend WithEvents TitleCrewFilenameLabel As Label
    Friend WithEvents TitleCrewCountTextBox As TextBox
    Friend WithEvents RowCountsHeader1Label As Label
    Friend WithEvents CompressedFilenamesHeader1Label As Label
    Friend WithEvents FileSizeHeader1Label As Label
    Friend WithEvents TitleCrewSizeTextBox As TextBox
    Friend WithEvents TitleBasicsSizeTextBox As TextBox
    Friend WithEvents TitleAkasSizeTextBox As TextBox
    Friend WithEvents NameBasicsSizeTextBox As TextBox
    Friend WithEvents FileSizeHeader2Label As Label
    Friend WithEvents TitleRatingsSizeTextBox As TextBox
    Friend WithEvents TitlePrincipalsSizeTextBox As TextBox
    Friend WithEvents TitleEpisodeSizeTextBox As TextBox
    Friend WithEvents CompressedFilenamesHeader2Label As Label
    Friend WithEvents RowCountsHeader2Label As Label
    Friend WithEvents TitleRatingsFilenameLabel As Label
    Friend WithEvents TitleRatingsCountTextBox As TextBox
    Friend WithEvents TitlePrincipalsFilenameLabel As Label
    Friend WithEvents TitlePrincipalsCountTextBox As TextBox
    Friend WithEvents TitleEpisodeFilenameLabel As Label
    Friend WithEvents TitleEpisodeCountTextBox As TextBox
    Friend WithEvents CountArchiveRowsButton As Button
    Friend WithEvents NameBasicsBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents TitleAkasBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents TitleBasicsBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents TitleCrewBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents TitleEpisodeBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents TitlePrincipalsBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents TitleRatingsBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents DecompressAfterDownloadCheckBox As CheckBox
    Friend WithEvents CountTsvRowsButton As Button
    Friend WithEvents AllArchivesBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents ImportDataButton As Button
    Friend WithEvents SqlImportBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label2 As Label
    Friend WithEvents FileEstimatedTimeRemainingTextBox As TextBox
    Friend WithEvents OverallEstimatedTimeRemainingTextBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents TitleCrewPreviousRowCountTextBox As TextBox
    Friend WithEvents TitleBasicsPreviousRowCountTextBox As TextBox
    Friend WithEvents TitleAkasPreviousRowCountTextBox As TextBox
    Friend WithEvents NameBasicsPreviousRowCountTextBox As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TitleEpisodePreviousRowCountTextBox As TextBox
    Friend WithEvents TitlePrincipalsPreviousRowCountTextBox As TextBox
    Friend WithEvents TitleRatingsPreviousRowCountTextBox As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents ElapsedTimeForFileTextBox As TextBox
    Friend WithEvents OverallElapsedTimeTextBox As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents FileEstimatedProcessingTimeTextBox As TextBox
    Friend WithEvents OverallEstimatedProcessingTimeTextBox As TextBox
    Friend WithEvents ImportArchiveFileProgressBar As ProgressBar
    Friend WithEvents DownloadPanel As Panel
    Friend WithEvents LoadAllDataFilesPanel As Panel
End Class
