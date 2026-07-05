
Imports PT = ImportAndAddRawImdbData.CountOrInsertData.ProcessTypeEnum
Imports SP = ImportAndAddRawImdbData.CountOrInsertData.SequentialOrParallelEnum
Imports PFT = ImportAndAddRawImdbData.CountOrInsertData.ProcessFileTypeEnum
Imports C = ImportAndAddRawImdbData.Constants
Imports CAS = ImportAndAddRawImdbData.CountOrInsertData.ChooseAllOrSpecificEnum
Imports System.IO

Public Class CountOrInsertData

    Private _ProcessType As PT = PT.CountData
    Private _SequentialOrParallel As SP = SP.Sequential

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(processType As PT,
                   sequentialOrParallel As SP,
                   processFileType As PFT)

        InitializeComponent()

        _ProcessType = processType
        _SequentialOrParallel = sequentialOrParallel
        _processFileType = processFileType

    End Sub

    Private Sub CountOrInsertData_Load(sender As Object, e As EventArgs) _
        Handles Me.Load

        ChooseAllOrSpecificComboBox.SelectedIndex = CInt(CAS.Unknown)

        If _ProcessType = PT.CountData Then
            Me.Text = "Count Data File Rows"
            Label1.Text = "Count All or Specific Data Files"

            ProcessFilesButton.Text = "Count &Files"

            ChooseSequentialOrParallelGroupBox.Visible = True

            Me.Size = New Size(271, 350)

        ElseIf _ProcessType = PT.InsertData Then
            Me.Text = "Insert Data Rows to IMDB DB"
            Label1.Text = "Insert All or Specific Data Files to IMDB DB"
            ProcessFilesButton.Text = "Insert &Files in DB"

            ChooseSequentialOrParallelGroupBox.Visible = False

            Me.Size = New Size(271, 267)

        End If

    End Sub

    Private _processFilesList As New List(Of String)()

    Public ReadOnly Property ProcessFilesList As List(Of String)
        Get
            If _processFilesList Is Nothing Then
                _processFilesList = New List(Of String)()
            End If

            Return _processFilesList
        End Get
    End Property

    Private _processFileType As PFT = PFT.Compressed
    Public ReadOnly Property ProcessFileType As PFT
        Get
            Return _processFileType
        End Get
    End Property

    Public Enum ProcessFileTypeEnum
        Compressed
        Decompressed
    End Enum

    Public Enum ProcessTypeEnum
        CountData
        InsertData
    End Enum

    Public Enum SequentialOrParallelEnum
        Sequential
        Parallel
    End Enum

    Public Enum ChooseAllOrSpecificEnum As Integer
        Unknown = -1
        AllFiles = 0
        SpecificFiles = 1
    End Enum

    Private _ChooseAllOrSpecific As CAS =
            CAS.Unknown

    Public ReadOnly Property ChooseAllOrSpecific As CAS
        Get
            Return _ChooseAllOrSpecific
        End Get
    End Property

    Public ReadOnly Property ProcessType As PT
        Get
            Return _ProcessType
        End Get
    End Property

    Public ReadOnly Property SequentialOrParallel As SP
        Get
            Return _SequentialOrParallel
        End Get
    End Property

    Private Sub ProcessInParallelRadioButton_CheckedChanged(sender As Object, e As EventArgs) _
        Handles ProcessInParallelRadioButton.CheckedChanged

        _SequentialOrParallel = SequentialOrParallelEnum.Parallel

    End Sub

    Private Sub ProcessSequentiallyRadioButton_CheckedChanged(sender As Object, e As EventArgs) _
        Handles ProcessSequentiallyRadioButton.CheckedChanged

        _SequentialOrParallel = SequentialOrParallelEnum.Sequential

    End Sub

    Private Sub ChooseAllOrSpecificComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles ChooseAllOrSpecificComboBox.SelectedIndexChanged

        Select Case CType(ChooseAllOrSpecificComboBox.SelectedIndex, CAS)
            Case CAS.AllFiles
                ProcessFilesList.Clear()

                Select Case Me.ProcessFileType
                    Case PFT.Compressed
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.NameBasicsCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleAkasCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleBasicsCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleCrewCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleEpisodeCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitlePrincipalsCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleRatingsCompressedFileName))

                    Case PFT.Decompressed
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.NameBasicsDecompFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleAkasDecompFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleBasicsDecompFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleCrewDecompFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleEpisodeDecompFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitlePrincipalsDecompFileName))
                        ProcessFilesList.Add(Path.Combine(MainForm2.FilesLocation, C.TitleRatingsDecompFileName))

                End Select

                _ChooseAllOrSpecific = CAS.AllFiles
                ChooseArchivesCheckedListBox.Enabled = False
                ProcessFilesButton.Enabled = True

            Case CAS.SpecificFiles
                _ChooseAllOrSpecific = CAS.SpecificFiles
                ChooseArchivesCheckedListBox.Enabled = True
                ProcessFilesButton.Enabled = True

            Case CAS.Unknown
                ProcessFilesButton.Enabled = False

        End Select

    End Sub

    Private Sub ProcessFilesButton_Click(sender As Object, e As EventArgs) _
        Handles ProcessFilesButton.Click

        DialogResult = DialogResult.OK

    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) _
        Handles ExitButton.Click

        DialogResult = DialogResult.Cancel

    End Sub

End Class