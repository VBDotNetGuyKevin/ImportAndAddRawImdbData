Imports FT = ImportAndAddRawImdbData.RawFileInfo.FileTypeEnum
Imports PT = ImportAndAddRawImdbData.CountOrInsertData.ProcessTypeEnum
Imports SP = ImportAndAddRawImdbData.CountOrInsertData.SequentialOrParallelEnum
Imports PFT = ImportAndAddRawImdbData.CountOrInsertData.ProcessFileTypeEnum
Imports C = ImportAndAddRawImdbData.Constants
Imports CAS = ImportAndAddRawImdbData.CountOrInsertData.ChooseAllOrSelectedEnum
Imports System.IO

Public Class CountOrInsertData

    Private _ProcessType As PT = PT.CountData
    Public Property ProcessType As PT
        Get
            Return _ProcessType
        End Get
        Private Set(value As PT)
            _ProcessType = value
        End Set
    End Property

    Private _SequentialOrParallel As SP = SP.Sequential
    Public Property SequentialOrParallel As SP
        Get
            Return _SequentialOrParallel
        End Get
        Private Set(value As SP)
            _SequentialOrParallel = value
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(processType As PT,
                   sequentialOrParallel As SP,
                   processFileType As PFT,
                   filesDirectoryLocation As String)

        Me.New()

        Me.ProcessType = processType
        Me.SequentialOrParallel = sequentialOrParallel
        Me.ProcessFileType = processFileType

        Me.FilesLocation = filesDirectoryLocation

    End Sub

    Private Property _filesLocation As String = String.Empty
    Private Property FilesLocation As String
        Get
            Return _filesLocation
        End Get
        Set(value As String)
            _filesLocation = value
        End Set
    End Property

    Private Sub CountOrInsertData_Load(sender As Object, e As EventArgs) _
        Handles Me.Load

        ChooseAllOrSelectedComboBox.SelectedIndex = CInt(CAS.Unknown)

        If Me.ProcessType = PT.CountData Then
            Me.Text = "Count Data File Rows"
            Label1.Text = "Count All or Specific Data Files"

            ProcessFilesButton.Text = "Count &Files"

            ChooseSequentialOrParallelGroupBox.Visible = True

            Me.Size = New Size(271, 350)

        ElseIf Me.ProcessType = PT.InsertData Then
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
    Public Property ProcessFileType As PFT
        Get
            Return _processFileType
        End Get
        Private Set(value As PFT)
            _processFileType = value
        End Set
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

    Public Enum ChooseAllOrSelectedEnum As Integer
        Unknown = -1
        AllFiles = 0
        SelectedFiles = 1
    End Enum

    Private _ChooseAllOrSelected As CAS = CAS.Unknown

    Public Property ChooseAllOrSelected As CAS
        Get
            Return _ChooseAllOrSelected
        End Get
        Private Set(value As CAS)
            _ChooseAllOrSelected = value
        End Set
    End Property

    Private Sub ProcessInParallelRadioButton_CheckedChanged(sender As Object, e As EventArgs) _
        Handles ProcessInParallelRadioButton.CheckedChanged

        Me.SequentialOrParallel = SequentialOrParallelEnum.Parallel

    End Sub

    Private Sub ProcessSequentiallyRadioButton_CheckedChanged(sender As Object, e As EventArgs) _
        Handles ProcessSequentiallyRadioButton.CheckedChanged

        Me.SequentialOrParallel = SequentialOrParallelEnum.Sequential

    End Sub

    Private Sub ChooseAllOrSelectedComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles ChooseAllOrSelectedComboBox.SelectedIndexChanged

        Select Case CType(ChooseAllOrSelectedComboBox.SelectedIndex, CAS)
            Case CAS.AllFiles
                ChooseAllOrSelected = CAS.AllFiles

                ProcessFilesList.Clear()

                Select Case Me.ProcessFileType
                    Case PFT.Compressed
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.NameBasicsCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleAkasCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleBasicsCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleCrewCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleEpisodeCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitlePrincipalsCompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleRatingsCompressedFileName))

                    Case PFT.Decompressed
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.NameBasicsDecompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleAkasDecompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleBasicsDecompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleCrewDecompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleEpisodeDecompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitlePrincipalsDecompressedFileName))
                        ProcessFilesList.Add(Path.Combine(Me.FilesLocation, C.TitleRatingsDecompressedFileName))

                End Select

                ChooseArchivesCheckedListBox.Enabled = False
                ProcessFilesButton.Enabled = True

            Case CAS.SelectedFiles
                ChooseAllOrSelected = CAS.SelectedFiles

                ChooseArchivesCheckedListBox.Enabled = True
                ProcessFilesButton.Enabled = True

                ProcessFilesList.Clear()

                ' I need code to populate the ChooseArchivesCheckedListBox with the available files for selection. and to 
                ' save the selected files to the ProcessFilesList when the user clicks the ProcessFilesButton.

                For Each checkedItem As String In ChooseArchivesCheckedListBox.CheckedItems
                    Dim fileName As String = checkedItem

                    Select Case Me.ProcessFileType
                        Case PFT.Compressed : fileName &= C.CompressedFileExtension
                        Case PFT.Decompressed : fileName &= C.DecompressedFileExtension
                    End Select

                    fileName = Path.Combine(Me.FilesLocation, fileName)

                    If Not MainForm2.GetFileTypeBasedOnFileName(fileName) = FT.Unknown Then
                        ProcessFilesList.Add(fileName)
                    End If
                Next

                ProcessFilesButton.Enabled =
                    (ChooseArchivesCheckedListBox.CheckedItems.Count > 0)

            Case CAS.Unknown
                ProcessFilesButton.Enabled = False

        End Select

    End Sub

    Private Sub ChooseArchivesCheckedListBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles ChooseArchivesCheckedListBox.SelectedIndexChanged

        ProcessFilesList.Clear()

        ' I need code to populate the ChooseArchivesCheckedListBox with the available files for selection. and to 
        ' save the selected files to the ProcessFilesList when the user clicks the ProcessFilesButton.


        Dim checkedItemsList As List(Of String) =
            ChooseArchivesCheckedListBox.CheckedItems.Cast(Of Object)().Select(Function(x) x.ToString()).ToList()

        For Each checkedItem As String In checkedItemsList ' allCheckedItems ' ChooseArchivesCheckedListBox.CheckedItems

            Dim fileName As String = checkedItem

            Select Case Me.ProcessFileType
                Case PFT.Compressed : fileName &= C.CompressedFileExtension
                Case PFT.Decompressed : fileName &= C.DecompressedFileExtension
            End Select


            If Not MainForm2.GetFileTypeBasedOnFileName(fileName) = FT.Unknown Then
                fileName = Path.Combine(Me.FilesLocation, fileName)

                ProcessFilesList.Add(fileName)
            End If
        Next

        ProcessFilesButton.Enabled =
            (ChooseArchivesCheckedListBox.CheckedItems.Count > 0)

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