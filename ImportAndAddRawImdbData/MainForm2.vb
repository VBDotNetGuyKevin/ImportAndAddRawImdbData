Imports System.Collections.Concurrent
Imports System.ComponentModel
Imports System.Data.Common
Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Reflection.Metadata.Ecma335
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.Data.SqlClient
Imports ImportAndAddRawImdbData.Constants
Imports AH25 = ImportAndAddRawImdbData.Constants.AdHoc2_5TableNameEnum
Imports C = ImportAndAddRawImdbData.Constants
Imports Comp = System.IO.Compression
Imports FT = ImportAndAddRawImdbData.RawFileInfo.FileTypeEnum
Imports PFT = ImportAndAddRawImdbData.CountOrInsertData.ProcessFileTypeEnum
Imports PT = ImportAndAddRawImdbData.CountOrInsertData.ProcessTypeEnum
Imports CAS = ImportAndAddRawImdbData.CountOrInsertData.ChooseAllOrSelectedEnum
Imports SCT = ImportAndAddRawImdbData.MainForm2.SqlCmdTypeEnum
Imports SP = ImportAndAddRawImdbData.CountOrInsertData.SequentialOrParallelEnum
Imports TS = ImportAndAddRawImdbData.ThreadSafeMethods
Imports RFI = ImportAndAddRawImdbData.RawFileInfo
Imports IT = ImportAndAddRawImdbData.MainForm2.ImportTypeEnum

Public Class MainForm2

    ''' <summary>
    ''' Defines the types of raw files that can be processed, with each type associated with an integer value.
    ''' </summary>
    Public Enum RawFileTypeEnum As Integer
        UNKNOWN = -1
        NameBasics = 0
        TitleAkas = 1
        TitleBasics = 2
        TitleCrew = 3
        TitleEpisode = 4
        TitlePrincipals = 5
        TitleRatings = 6
    End Enum

    ''' <summary>
    ''' Defines the types of SQL commands that can be executed, with each type associated with an integer value.
    ''' </summary>
    Public Enum SqlCmdTypeEnum As Integer
        INSERT
        UPDATE
        TRUNCATE
        ADD_CONSTRAINT
        DROP_CONSTRAINT
    End Enum

    ''' <summary>
    ''' Defines an enumeration for the operations of dropping or adding table constraints in a database. 
    ''' The enumeration has two members: DROP and ADD, which represent the respective operations.
    ''' </summary>
    Private Enum DropAddEnum As Integer
        ''' <summary>
        ''' Represents the operation of dropping a table constraint.
        ''' </summary>
        DROP
        ''' <summary>
        ''' Represents the operation of adding a table constraint.
        ''' </summary>
        ADD
    End Enum

    Private Property FolderLocation As String = String.Empty
    Private Property LocationExists As Boolean = False

    Private Property ArchiveDownloadLocationsList As List(Of String)

    Private Property AllPreviouslyUploadedFilenamesAndRowCountsPlusCurrent As String = String.Empty
    Private Property CompiledPreviouslyUploadedFilenamesAndRowCounts As String = String.Empty
    Private Property CurrentUploadFilenameAndRowCount As String = String.Empty
    Private Property CurrentlyUploadingFilename As String = String.Empty

    Private Property DecompressedFileList As New List(Of String)

    ''' <summary>
    ''' This property holds the type of import being performed, which can 
    ''' be either Compressed, Decompressed, or Unknown. It is used to 
    ''' determine how to process the files during the import operation.
    ''' </summary>
    ''' <returns></returns>
    Private Property ImportType As IT = IT.Unknown

    ''' <summary>
    ''' Gets the raw file information for each file type.
    ''' </summary>
    Private Property MyRawFileInfo As New SortedList(Of FT, RawFileInfo) From
        {
            {FT.OVERALL, New RawFileInfo(FT.OVERALL)},
            {FT.NameBasics, New RawFileInfo(FT.NameBasics)},
            {FT.TitleAkas, New RawFileInfo(FT.TitleAkas)},
            {FT.TitleBasics, New RawFileInfo(FT.TitleBasics)},
            {FT.TitleCrew, New RawFileInfo(FT.TitleCrew)},
            {FT.TitleEpisode, New RawFileInfo(FT.TitleEpisode)},
            {FT.TitlePrincipals, New RawFileInfo(FT.TitlePrincipals)},
            {FT.TitleRatings, New RawFileInfo(FT.TitleRatings)}
        }

    ''' <summary>
    ''' Gets or sets the type of import being performed, which can be either Compressed, Decompressed, or Unknown.
    ''' It is used to determine how to process the files during the import operation.
    ''' </summary>
    Private Property ProcessFileType As PFT = PFT.Compressed

    Private Property _SequentialOrParallel As SP = SP.Sequential
    Public Property SequentialOrParallel As SP
        Get
            Return _SequentialOrParallel
        End Get
        Private Set(value As SP)
            _SequentialOrParallel = value
        End Set
    End Property

    Private Property _ChooseAllOrSelected As CAS = CAS.Unknown
    Public Property ChooseAllOrSelected As CAS
        Get
            Return _ChooseAllOrSelected
        End Get
        Private Set(value As CAS)
            _ChooseAllOrSelected = value
        End Set
    End Property

    Private _ProcessType As PT = PT.CountData
    Public Property ProcessType As PT
        Get
            Return _ProcessType
        End Get
        Private Set(value As PT)
            _ProcessType = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the list of files to be counted.
    ''' </summary>
    ''' <returns></returns>
    Private Property CountFilesList As New List(Of String)

    ''' <summary>
    ''' Gets or sets the list of files to be inserted into the database.
    ''' </summary>
    ''' <returns></returns>
    Private Property InsertDataFilesList As New List(Of String)

    Private Property CountTsvRowsButtonEnabled As Boolean = True
    Private Property CountArchiveRowsButtonEnabled As Boolean = True

    ''' <summary>
    ''' Gets or sets a value indicating whether any of the background worker 
    ''' operations have been cancelled. This property is used to track the 
    ''' cancellation state of the operations and to control the 
    ''' enabling/disabling of UI controls based on that state.
    ''' </summary>
    ''' <returns></returns>
    Private Property CancelledOperations As Boolean = False

    Private Property CountArchiveRowsEnabled As Boolean = False
    Private Property CountTsvRowsEnabled As Boolean = False
    Private Property DecompressAfterDownloadEnabled As Boolean = False

    ''' <summary>
    ''' Gets the location of the folder where the files are stored.
    ''' </summary>
    ''' <returns>The folder location as a string.</returns>
    Public ReadOnly Property FilesLocation As String
        Get
            Return FolderLocation
        End Get
    End Property

    Public Const CompressedFileExtension As String = ".tsv.gz"
    Public Const UnCompressedFileExtension As String = ".tsv"

    Public Enum ImportTypeEnum
        Unknown
        Compressed
        Decompressed
    End Enum

    ''' <summary>
    ''' Determines the file type based on the provided file name. It checks 
    ''' the file name against known IMDB data file names and returns the 
    ''' corresponding file type enumeration (FT). If the file name does 
    ''' not match any known types, it returns FT.Unknown.
    ''' </summary>
    ''' <param name="fileName">The name of the file to check.</param>
    ''' <returns>The file type enumeration (FT) corresponding to the file name.</returns>
    Public Shared Function GetFileTypeBasedOnFileName(fileName As String) As FT

        Dim result As FT = FT.Unknown

        Select Case fileName
            Case C.NameBasicsCompressedFileName,
                 C.NameBasicsDecompressedFileName
                result = FT.NameBasics

            Case C.TitleAkasCompressedFileName,
                 C.TitleAkasDecompressedFileName
                result = FT.TitleAkas

            Case C.TitleBasicsCompressedFileName,
                 C.TitleBasicsDecompressedFileName
                result = FT.TitleBasics

            Case C.TitleCrewCompressedFileName,
                 C.TitleCrewDecompressedFileName
                result = FT.TitleCrew

            Case C.TitleEpisodeCompressedFileName,
                 C.TitleEpisodeDecompressedFileName
                result = FT.TitleEpisode

            Case C.TitlePrincipalsCompressedFileName,
                 C.TitlePrincipalsDecompressedFileName
                result = FT.TitlePrincipals

            Case C.TitleRatingsCompressedFileName,
                 C.TitleRatingsDecompressedFileName
                result = FT.TitleRatings

            Case Else
                result = FT.Unknown

        End Select

        Return result

    End Function

    Public Shared Function GetFileTypeBasedOnFileName(fileName As String,
                                                ByRef importType As IT) As FT

        Dim result As FT = FT.Unknown

        Select Case fileName
            ' Name.Basics
            Case C.NameBasicsCompressedFileName,
                 C.NameBasicsDecompressedFileName

                result = FT.NameBasics
                importType = IT.Decompressed

                If fileName = C.NameBasicsCompressedFileName Then
                    importType = IT.Compressed
                End If


                ' Title.Akas
            Case C.TitleAkasCompressedFileName,
                 C.TitleAkasDecompressedFileName
                result = FT.TitleAkas
                importType = IT.Decompressed

                If fileName = C.TitleAkasCompressedFileName Then
                    importType = IT.Compressed
                End If


                ' Title.Basics
            Case C.TitleBasicsCompressedFileName,
                 C.TitleBasicsDecompressedFileName
                result = FT.TitleBasics
                importType = IT.Decompressed

                If fileName = C.TitleBasicsCompressedFileName Then
                    importType = IT.Compressed
                End If


                ' Title.Crew
            Case C.TitleCrewCompressedFileName,
                 C.TitleCrewDecompressedFileName
                result = FT.TitleCrew
                importType = IT.Decompressed

                If fileName = C.TitleCrewCompressedFileName Then
                    importType = IT.Compressed
                End If


                ' Title.Episode
            Case C.TitleEpisodeCompressedFileName,
                 C.TitleEpisodeDecompressedFileName
                result = FT.TitleEpisode
                importType = IT.Decompressed

                If fileName = C.TitleEpisodeCompressedFileName Then
                    importType = IT.Compressed
                End If


                ' Title.Principals
            Case C.TitlePrincipalsCompressedFileName,
                 C.TitlePrincipalsDecompressedFileName
                result = FT.TitlePrincipals
                importType = IT.Decompressed

                If fileName = C.TitlePrincipalsCompressedFileName Then
                    importType = IT.Compressed
                End If


                ' Title.Ratings
            Case C.TitleRatingsCompressedFileName,
                 C.TitleRatingsDecompressedFileName
                result = FT.TitleRatings
                importType = IT.Decompressed

                If fileName = C.TitleRatingsCompressedFileName Then
                    importType = IT.Compressed
                End If


            Case Else
                result = FT.Unknown
                importType = IT.Unknown

        End Select

        Return result

    End Function

    ''' <summary>
    ''' This event handler is triggered when the form is loaded. It initializes the form's controls
    ''' and loads the saved settings from the My.Settings file.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MainForm_Load(sender As Object, e As EventArgs) _
        Handles Me.Load

        With My.Settings
            .Reload()

            Me.FolderLocation = .FolderLocation

            If Me.ArchiveDownloadLocationsList Is Nothing Then
                Me.ArchiveDownloadLocationsList = New List(Of String)()
            Else
                Me.ArchiveDownloadLocationsList.Clear()
            End If

            For Each archiveUrl As String In .ArchiveList
                Me.ArchiveDownloadLocationsList.Add(archiveUrl)
            Next

            FolderLocationTextBox.Text = Me.FolderLocation

            C.ApproxRows5(AH25.Attributes) = .SavedCountAttributes
            C.ApproxRows5(AH25.Episodes) = .SavedCountEpisodes
            C.ApproxRows5(AH25.Genres) = .SavedCountGenres
            C.ApproxRows5(AH25.PrimaryProfessions) = .SavedCountPrimaryProfessions
            C.ApproxRows5(AH25.Principals) = .SavedCountPrincipals
            C.ApproxRows5(AH25.Professions) = .SavedCountProfessions
            C.ApproxRows5(AH25.TitleCharacters) = .SavedCountTitleCharacters
            C.ApproxRows5(AH25.TitleGenres) = .SavedCountTitleGenres
            C.ApproxRows5(AH25.TitleNameAttributes) = .SavedCountTitleNameAttributes
            C.ApproxRows5(AH25.TitleNames) = .SavedCountTitleNames
            C.ApproxRows5(AH25.TitlePrincipals) = .SavedCountTitlePrincipals
            C.ApproxRows5(AH25.Titles) = .SavedCountTitles
            C.ApproxRows5(AH25.TitleTypes) = .SavedCountTitleTypes

            C.ApproxRows4(1) = .SavedRowCount401 : C.ApproxRows4(2) = .SavedRowCount402
            C.ApproxRows4(3) = .SavedRowCount403 : C.ApproxRows4(4) = .SavedRowCount404
            C.ApproxRows4(5) = .SavedRowCount405 : C.ApproxRows4(6) = .SavedRowCount406
            C.ApproxRows4(7) = .SavedRowCount407 : C.ApproxRows4(8) = .SavedRowCount408
            C.ApproxRows4(9) = .SavedRowCount409 : C.ApproxRows4(10) = .SavedRowCount410
            C.ApproxRows4(11) = .SavedRowCount411 : C.ApproxRows4(12) = .SavedRowCount412
            C.ApproxRows4(13) = .SavedRowCount413 : C.ApproxRows4(14) = .SavedRowCount414
            C.ApproxRows4(15) = .SavedRowCount415 : C.ApproxRows4(16) = .SavedRowCount416
            C.ApproxRows4(17) = .SavedRowCount417 : C.ApproxRows4(18) = .SavedRowCount418
            C.ApproxRows4(19) = .SavedRowCount419 : C.ApproxRows4(20) = .SavedRowCount420
            C.ApproxRows4(21) = .SavedRowCount421 : C.ApproxRows4(22) = .SavedRowCount422
            C.ApproxRows4(23) = .SavedRowCount423 : C.ApproxRows4(24) = .SavedRowCount424
            C.ApproxRows4(25) = .SavedRowCount425 : C.ApproxRows4(26) = .SavedRowCount426

            C.TimeOut4List(1) = .TimeOut401 : C.TimeOut4List(2) = .TimeOut402
            C.TimeOut4List(3) = .TimeOut403 : C.TimeOut4List(4) = .TimeOut404
            C.TimeOut4List(5) = .TimeOut405 : C.TimeOut4List(6) = .TimeOut406
            C.TimeOut4List(7) = .TimeOut407 : C.TimeOut4List(8) = .TimeOut408
            C.TimeOut4List(9) = .TimeOut409 : C.TimeOut4List(10) = .TimeOut410
            C.TimeOut4List(11) = .TimeOut411 : C.TimeOut4List(12) = .TimeOut412
            C.TimeOut4List(13) = .TimeOut413 : C.TimeOut4List(14) = .TimeOut414
            C.TimeOut4List(15) = .TimeOut415 : C.TimeOut4List(16) = .TimeOut416
            C.TimeOut4List(17) = .TimeOut417 : C.TimeOut4List(18) = .TimeOut418
            C.TimeOut4List(19) = .TimeOut419 : C.TimeOut4List(20) = .TimeOut420
            C.TimeOut4List(21) = .TimeOut421 : C.TimeOut4List(22) = .TimeOut422
            C.TimeOut4List(23) = .TimeOut423 : C.TimeOut4List(24) = .TimeOut424
            C.TimeOut4List(25) = .TimeOut425 : C.TimeOut4List(26) = .TimeOut426

            MyRawFileInfo(FT.NameBasics).PreviousStartTime = .NameBasicsSavedStartTime
            MyRawFileInfo(FT.NameBasics).PreviousEndTime = .NameBasicsSavedEndTime
            MyRawFileInfo(FT.NameBasics).LastRowCount = .NameBasicsSavedRowCount

            MyRawFileInfo(FT.TitleAkas).PreviousStartTime = .TitleAkasSavedStartTime
            MyRawFileInfo(FT.TitleAkas).PreviousEndTime = .TitleAkasSavedEndTime
            MyRawFileInfo(FT.TitleAkas).LastRowCount = .TitleAkasSavedRowCount

            MyRawFileInfo(FT.TitleBasics).PreviousStartTime = .TitleBasicsSavedStartTime
            MyRawFileInfo(FT.TitleBasics).PreviousEndTime = .TitleBasicsSavedEndTime
            MyRawFileInfo(FT.TitleBasics).LastRowCount = .TitleBasicsSavedRowCount

            MyRawFileInfo(FT.TitleCrew).PreviousStartTime = .TitleCrewSavedStartTime
            MyRawFileInfo(FT.TitleCrew).PreviousEndTime = .TitleCrewSavedEndTime
            MyRawFileInfo(FT.TitleCrew).LastRowCount = .TitleCrewSavedRowCount

            MyRawFileInfo(FT.TitleEpisode).PreviousStartTime = .TitleEpisodeSavedStartTime
            MyRawFileInfo(FT.TitleEpisode).PreviousEndTime = .TitleEpisodeSavedEndTime
            MyRawFileInfo(FT.TitleEpisode).LastRowCount = .TitleEpisodeSavedRowCount

            MyRawFileInfo(FT.TitlePrincipals).PreviousStartTime = .TitlePrincipalsSavedStartTime
            MyRawFileInfo(FT.TitlePrincipals).PreviousEndTime = .TitlePrincipalsSavedEndTime
            MyRawFileInfo(FT.TitlePrincipals).LastRowCount = .TitlePrincipalsSavedRowCount

            MyRawFileInfo(FT.TitleRatings).PreviousStartTime = .TitleRatingsSavedStartTime
            MyRawFileInfo(FT.TitleRatings).PreviousEndTime = .TitleRatingsSavedEndTime
            MyRawFileInfo(FT.TitleRatings).LastRowCount = .TitleRatingsSavedRowCount

            MyRawFileInfo(FT.OVERALL).PreviousStartTime = .OverallSavedStartTime
            MyRawFileInfo(FT.OVERALL).PreviousEndTime = .OverallSavedEndTime
            MyRawFileInfo(FT.OVERALL).LastRowCount = (.NameBasicsSavedRowCount +
                                                      .TitleAkasSavedRowCount +
                                                      .TitleBasicsSavedRowCount +
                                                      .TitleCrewSavedRowCount +
                                                      .TitleEpisodeSavedRowCount +
                                                      .TitlePrincipalsSavedRowCount +
                                                      .TitleRatingsSavedRowCount)

            ' check if the files already exist in the folder location, 
            ' and if so, enable the button to count the rows in those 
            ' files, so the user can see how many rows are in each file 
            ' before they decide to download the updated files, since it 
            ' may take a while to download them, and they may not want to 
            ' download them if they already have them and they can see 
            ' how many rows are in each file

            NameBasicsPreviousRowCountTextBox.Text =
                MyRawFileInfo(FT.NameBasics).LastRowCount.ToString(C.COMMA_MASK)

            TitleAkasPreviousRowCountTextBox.Text =
                MyRawFileInfo(FT.TitleAkas).LastRowCount.ToString(C.COMMA_MASK)

            TitleBasicsPreviousRowCountTextBox.Text =
                MyRawFileInfo(FT.TitleBasics).LastRowCount.ToString(C.COMMA_MASK)

            TitleCrewPreviousRowCountTextBox.Text =
                MyRawFileInfo(FT.TitleCrew).LastRowCount.ToString(C.COMMA_MASK)

            TitleEpisodePreviousRowCountTextBox.Text =
                MyRawFileInfo(FT.TitleEpisode).LastRowCount.ToString(C.COMMA_MASK)

            TitlePrincipalsPreviousRowCountTextBox.Text =
                MyRawFileInfo(FT.TitlePrincipals).LastRowCount.ToString(C.COMMA_MASK)

            TitleRatingsPreviousRowCountTextBox.Text =
                MyRawFileInfo(FT.TitleRatings).LastRowCount.ToString(C.COMMA_MASK)

            CountTsvRowsButton.Enabled = ((File.Exists(Path.Combine(FolderLocation, C.NameBasicsDecompressedFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleAkasDecompressedFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleBasicsDecompressedFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleCrewDecompressedFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeDecompressedFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsDecompressedFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleRatingsDecompressedFileName))))

            CountTsvRowsButtonEnabled = CountTsvRowsButton.Enabled

            CountArchiveRowsButton.Enabled = ((File.Exists(Path.Combine(FolderLocation, C.NameBasicsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleAkasCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleBasicsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleCrewCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleRatingsCompressedFileName))))

            CountArchiveRowsButtonEnabled = CountArchiveRowsButton.Enabled

            If CountArchiveRowsButton.Enabled Then
                Dim nameBasicsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.NameBasicsCompressedFileName)).Length

                Dim titleAkasLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleAkasCompressedFileName)).Length

                Dim titleBasicsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleBasicsCompressedFileName)).Length

                Dim titleCrewLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleCrewCompressedFileName)).Length

                Dim titleEpisodeLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleEpisodeCompressedFileName)).Length

                Dim titlePrincipalsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitlePrincipalsCompressedFileName)).Length

                Dim titleRatingsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleRatingsCompressedFileName)).Length

                Dim nameBasicsDisplayLength As String =
                    GetFileDisplayLength(nameBasicsLength) & " " &
                    GetFileDisplayLengthString(nameBasicsLength)

                Dim titleAkasDisplayLength As String =
                    GetFileDisplayLength(titleAkasLength) & " " &
                    GetFileDisplayLengthString(titleAkasLength)

                Dim titleBasicsDisplayLength As String =
                    GetFileDisplayLength(titleBasicsLength) & " " &
                    GetFileDisplayLengthString(titleBasicsLength)

                Dim titleCrewDisplayLength As String =
                    GetFileDisplayLength(titleCrewLength) & " " &
                    GetFileDisplayLengthString(titleCrewLength)

                Dim titleEpisodeDisplayLength As String =
                    GetFileDisplayLength(titleEpisodeLength) & " " &
                    GetFileDisplayLengthString(titleEpisodeLength)

                Dim titlePrincipalsDisplayLength As String =
                    GetFileDisplayLength(titlePrincipalsLength) & " " &
                    GetFileDisplayLengthString(titlePrincipalsLength)

                Dim titleRatingsDisplayLength As String =
                    GetFileDisplayLength(titleRatingsLength) & " " &
                    GetFileDisplayLengthString(titleRatingsLength)

                NameBasicsSizeTextBox.Text = nameBasicsDisplayLength
                TitleAkasSizeTextBox.Text = titleAkasDisplayLength
                TitleBasicsSizeTextBox.Text = titleBasicsDisplayLength
                TitleCrewSizeTextBox.Text = titleCrewDisplayLength
                TitleEpisodeSizeTextBox.Text = titleEpisodeDisplayLength
                TitlePrincipalsSizeTextBox.Text = titlePrincipalsDisplayLength
                TitleRatingsSizeTextBox.Text = titleRatingsDisplayLength

                FileSizeHeader1Label.Text = "File Size .gz"
                FileSizeHeader2Label.Text = "File Size .gz"

            ElseIf CountTsvRowsButton.Enabled Then
                NameBasicsFilenameLabel.Text = C.NameBasicsDecompressedFileName
                TitleAkasFilenameLabel.Text = C.TitleAkasDecompressedFileName
                TitleBasicsFilenameLabel.Text = C.TitleBasicsDecompressedFileName
                TitleCrewFilenameLabel.Text = C.TitleCrewDecompressedFileName
                TitleEpisodeFilenameLabel.Text = C.TitleEpisodeDecompressedFileName
                TitlePrincipalsFilenameLabel.Text = C.TitlePrincipalsDecompressedFileName
                TitleRatingsFilenameLabel.Text = C.TitleRatingsDecompressedFileName

                Dim nameBasicsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.NameBasicsDecompressedFileName)).Length

                Dim titleAkasLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleAkasDecompressedFileName)).Length

                Dim titleBasicsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleBasicsDecompressedFileName)).Length

                Dim titleCrewLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleCrewDecompressedFileName)).Length

                Dim titleEpisodeLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleEpisodeDecompressedFileName)).Length

                Dim titlePrincipalsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitlePrincipalsDecompressedFileName)).Length

                Dim titleRatingsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleRatingsDecompressedFileName)).Length

                NameBasicsSizeTextBox.Text =
                    GetFileDisplayLength(nameBasicsLength) & " " &
                    GetFileDisplayLengthString(nameBasicsLength)

                TitleAkasSizeTextBox.Text =
                    GetFileDisplayLength(titleAkasLength) & " " &
                    GetFileDisplayLengthString(titleAkasLength)

                TitleBasicsSizeTextBox.Text =
                    GetFileDisplayLength(titleBasicsLength) & " " &
                    GetFileDisplayLengthString(titleBasicsLength)

                TitleCrewSizeTextBox.Text =
                    GetFileDisplayLength(titleCrewLength) & " " &
                    GetFileDisplayLengthString(titleCrewLength)

                TitleEpisodeSizeTextBox.Text =
                    GetFileDisplayLength(titleEpisodeLength) & " " &
                    GetFileDisplayLengthString(titleEpisodeLength)

                TitlePrincipalsSizeTextBox.Text =
                    GetFileDisplayLength(titlePrincipalsLength) & " " &
                    GetFileDisplayLengthString(titlePrincipalsLength)

                TitleRatingsSizeTextBox.Text =
                    GetFileDisplayLength(titleRatingsLength) & " " &
                    GetFileDisplayLengthString(titleRatingsLength)

                FileSizeHeader1Label.Text = "File Size .tsv"
                FileSizeHeader2Label.Text = "File Size .tsv"
            End If
        End With

    End Sub

    ''' <summary>
    ''' This event handler is triggered when the form is closed. It saves the current settings to the My.Settings file,
    ''' so that they can be reloaded the next time the application is run.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A FormClosedEventArgs that contains the event data.</param>
    Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs) _
        Handles Me.FormClosed

        ' save the current settings to the My.Settings file, so that they can be reloaded the next time the application is run
        With My.Settings
            .Reload()

            .FolderLocation = Me.FolderLocation

            .ArchiveList.Clear()
            .ArchiveList.AddRange(Me.ArchiveDownloadLocationsList.ToArray())

            .SavedCountAttributes = C.ApproxRows5(AH25.Attributes)
            .SavedCountEpisodes = C.ApproxRows5(AH25.Episodes)
            .SavedCountGenres = C.ApproxRows5(AH25.Genres)
            .SavedCountPrimaryProfessions = C.ApproxRows5(AH25.PrimaryProfessions)
            .SavedCountPrincipals = C.ApproxRows5(AH25.Principals)
            .SavedCountProfessions = C.ApproxRows5(AH25.Professions)
            .SavedCountTitleCharacters = C.ApproxRows5(AH25.TitleCharacters)
            .SavedCountTitleGenres = C.ApproxRows5(AH25.TitleGenres)
            .SavedCountTitleNameAttributes = C.ApproxRows5(AH25.TitleNameAttributes)
            .SavedCountTitleNames = C.ApproxRows5(AH25.TitleNames)
            .SavedCountTitlePrincipals = C.ApproxRows5(AH25.TitlePrincipals)
            .SavedCountTitles = C.ApproxRows5(AH25.Titles)
            .SavedCountTitleTypes = C.ApproxRows5(AH25.TitleTypes)

            .SavedRowCount401 = C.ApproxRows4(1) : .SavedRowCount402 = C.ApproxRows4(2)
            .SavedRowCount403 = C.ApproxRows4(3) : .SavedRowCount404 = C.ApproxRows4(4)
            .SavedRowCount405 = C.ApproxRows4(5) : .SavedRowCount406 = C.ApproxRows4(6)
            .SavedRowCount407 = C.ApproxRows4(7) : .SavedRowCount408 = C.ApproxRows4(8)
            .SavedRowCount409 = C.ApproxRows4(9) : .SavedRowCount410 = C.ApproxRows4(10)
            .SavedRowCount411 = C.ApproxRows4(11) : .SavedRowCount412 = C.ApproxRows4(12)
            .SavedRowCount413 = C.ApproxRows4(13) : .SavedRowCount414 = C.ApproxRows4(14)
            .SavedRowCount415 = C.ApproxRows4(15) : .SavedRowCount416 = C.ApproxRows4(16)
            .SavedRowCount417 = C.ApproxRows4(17) : .SavedRowCount418 = C.ApproxRows4(18)
            .SavedRowCount419 = C.ApproxRows4(19) : .SavedRowCount420 = C.ApproxRows4(20)
            .SavedRowCount421 = C.ApproxRows4(21) : .SavedRowCount422 = C.ApproxRows4(22)
            .SavedRowCount423 = C.ApproxRows4(23) : .SavedRowCount424 = C.ApproxRows4(24)
            .SavedRowCount425 = C.ApproxRows4(25) : .SavedRowCount426 = C.ApproxRows4(26)

            .TimeOut401 = C.TimeOut4List(1) : .TimeOut402 = C.TimeOut4List(2)
            .TimeOut403 = C.TimeOut4List(3) : .TimeOut404 = C.TimeOut4List(4)
            .TimeOut405 = C.TimeOut4List(5) : .TimeOut406 = C.TimeOut4List(6)
            .TimeOut407 = C.TimeOut4List(7) : .TimeOut408 = C.TimeOut4List(8)
            .TimeOut409 = C.TimeOut4List(9) : .TimeOut410 = C.TimeOut4List(10)
            .TimeOut411 = C.TimeOut4List(11) : .TimeOut412 = C.TimeOut4List(12)
            .TimeOut413 = C.TimeOut4List(13) : .TimeOut414 = C.TimeOut4List(14)
            .TimeOut415 = C.TimeOut4List(15) : .TimeOut416 = C.TimeOut4List(16)
            .TimeOut417 = C.TimeOut4List(17) : .TimeOut418 = C.TimeOut4List(18)
            .TimeOut419 = C.TimeOut4List(19) : .TimeOut420 = C.TimeOut4List(20)
            .TimeOut421 = C.TimeOut4List(21) : .TimeOut422 = C.TimeOut4List(22)
            .TimeOut423 = C.TimeOut4List(23) : .TimeOut424 = C.TimeOut4List(24)
            .TimeOut425 = C.TimeOut4List(25) : .TimeOut426 = C.TimeOut4List(26)

            If MyRawFileInfo(FT.NameBasics).CompletedProcessing Then
                .NameBasicsSavedStartTime = MyRawFileInfo(FT.NameBasics).CurrentStartTime
                .NameBasicsSavedEndTime = MyRawFileInfo(FT.NameBasics).CurrentEndTime
                .NameBasicsSavedRowCount = MyRawFileInfo(FT.NameBasics).CountedRows
            Else
                .NameBasicsSavedStartTime = MyRawFileInfo(FT.NameBasics).PreviousStartTime
                .NameBasicsSavedEndTime = MyRawFileInfo(FT.NameBasics).PreviousEndTime
                .NameBasicsSavedRowCount = MyRawFileInfo(FT.NameBasics).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleAkas).CompletedProcessing Then
                .TitleAkasSavedStartTime = MyRawFileInfo(FT.TitleAkas).CurrentStartTime
                .TitleAkasSavedEndTime = MyRawFileInfo(FT.TitleAkas).CurrentEndTime
                .TitleAkasSavedRowCount = MyRawFileInfo(FT.TitleAkas).CountedRows
            Else
                .TitleAkasSavedStartTime = MyRawFileInfo(FT.TitleAkas).PreviousStartTime
                .TitleAkasSavedEndTime = MyRawFileInfo(FT.TitleAkas).PreviousEndTime
                .TitleAkasSavedRowCount = MyRawFileInfo(FT.TitleAkas).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleBasics).CompletedProcessing Then
                .TitleBasicsSavedStartTime = MyRawFileInfo(FT.TitleBasics).CurrentStartTime
                .TitleBasicsSavedEndTime = MyRawFileInfo(FT.TitleBasics).CurrentEndTime
                .TitleBasicsSavedRowCount = MyRawFileInfo(FT.TitleBasics).CountedRows
            Else
                .TitleBasicsSavedStartTime = MyRawFileInfo(FT.TitleBasics).PreviousStartTime
                .TitleBasicsSavedEndTime = MyRawFileInfo(FT.TitleBasics).PreviousEndTime
                .TitleBasicsSavedRowCount = MyRawFileInfo(FT.TitleBasics).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleCrew).CompletedProcessing Then
                .TitleCrewSavedStartTime = MyRawFileInfo(FT.TitleCrew).CurrentStartTime
                .TitleCrewSavedEndTime = MyRawFileInfo(FT.TitleCrew).CurrentEndTime
                .TitleCrewSavedRowCount = MyRawFileInfo(FT.TitleCrew).CountedRows
            Else
                .TitleCrewSavedStartTime = MyRawFileInfo(FT.TitleCrew).PreviousStartTime
                .TitleCrewSavedEndTime = MyRawFileInfo(FT.TitleCrew).PreviousEndTime
                .TitleCrewSavedRowCount = MyRawFileInfo(FT.TitleCrew).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleEpisode).CompletedProcessing Then
                .TitleEpisodeSavedStartTime = MyRawFileInfo(FT.TitleEpisode).CurrentStartTime
                .TitleEpisodeSavedEndTime = MyRawFileInfo(FT.TitleEpisode).CurrentEndTime
                .TitleEpisodeSavedRowCount = MyRawFileInfo(FT.TitleEpisode).CountedRows
            Else
                .TitleEpisodeSavedStartTime = MyRawFileInfo(FT.TitleEpisode).PreviousStartTime
                .TitleEpisodeSavedEndTime = MyRawFileInfo(FT.TitleEpisode).PreviousEndTime
                .TitleEpisodeSavedRowCount = MyRawFileInfo(FT.TitleEpisode).LastRowCount
            End If

            If MyRawFileInfo(FT.TitlePrincipals).CompletedProcessing Then
                .TitlePrincipalsSavedStartTime = MyRawFileInfo(FT.TitlePrincipals).CurrentStartTime
                .TitlePrincipalsSavedEndTime = MyRawFileInfo(FT.TitlePrincipals).CurrentEndTime
                .TitlePrincipalsSavedRowCount = MyRawFileInfo(FT.TitlePrincipals).CountedRows
            Else
                .TitlePrincipalsSavedStartTime = MyRawFileInfo(FT.TitlePrincipals).PreviousStartTime
                .TitlePrincipalsSavedEndTime = MyRawFileInfo(FT.TitlePrincipals).PreviousEndTime
                .TitlePrincipalsSavedRowCount = MyRawFileInfo(FT.TitlePrincipals).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleRatings).CompletedProcessing Then
                .TitleRatingsSavedStartTime = MyRawFileInfo(FT.TitleRatings).CurrentStartTime
                .TitleRatingsSavedEndTime = MyRawFileInfo(FT.TitleRatings).CurrentEndTime
                .TitleRatingsSavedRowCount = MyRawFileInfo(FT.TitleRatings).CountedRows
            Else
                .TitleRatingsSavedStartTime = MyRawFileInfo(FT.TitleRatings).PreviousStartTime
                .TitleRatingsSavedEndTime = MyRawFileInfo(FT.TitleRatings).PreviousEndTime
                .TitleRatingsSavedRowCount = MyRawFileInfo(FT.TitleRatings).LastRowCount
            End If

            If MyRawFileInfo(FT.OVERALL).CompletedProcessing Then
                .OverallSavedStartTime = MyRawFileInfo(FT.OVERALL).CurrentStartTime
                .OverallSavedEndTime = MyRawFileInfo(FT.OVERALL).CurrentEndTime
                .OverallSavedRowCount = MyRawFileInfo(FT.OVERALL).CountedRows
            Else
                .OverallSavedStartTime = MyRawFileInfo(FT.OVERALL).PreviousStartTime
                .OverallSavedEndTime = MyRawFileInfo(FT.OVERALL).PreviousEndTime
                .OverallSavedRowCount = MyRawFileInfo(FT.OVERALL).LastRowCount
            End If

            .Save()
        End With

    End Sub

    ''' <summary>
    ''' This event handler is triggered when the "Choose Folder" button is clicked. It opens a folder
    ''' browser dialog to allow the user to select a folder. If a folder is selected, it updates the
    ''' FolderLocation property and the FolderLocationTextBox.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub ChooseFolderButton_Click(sender As Object, e As EventArgs) _
        Handles ChooseFolderButton.Click

        With ChooseFolderDialog
            .SelectedPath = FolderLocation

            If .ShowDialog() = DialogResult.OK Then
                FolderLocation = .SelectedPath
                FolderLocationTextBox.Text = FolderLocation
            End If
        End With

    End Sub

    ''' <summary>
    ''' This event handler is triggered when the text in the FolderLocationTextBox changes. 
    ''' It checks if the specified folder location exists and updates the UI accordingly. If 
    ''' the folder exists, it enables the DownloadUpdatedArchivesButton and other related 
    ''' controls, and sets the FolderLocation property. If the folder does not exist, it 
    ''' disables those controls and clears the FolderLocation property.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FolderLocationTextBox_TextChanged(sender As Object, e As EventArgs) _
        Handles FolderLocationTextBox.TextChanged

        LocationExists = Directory.Exists(FolderLocationTextBox.Text)

        DownloadUpdatedArchivesButton.Enabled = LocationExists
        DownloadFileNumberTextBox.Text = String.Empty

        ArchiveDownloadProgressBar.Enabled = LocationExists
        ArchiveDownloadProgressBar.Visible = LocationExists

        CurrentFileTextBox.Enabled = LocationExists
        CurrentFileTextBox.Visible = LocationExists

        CurrentFileLabel.Visible = LocationExists

        DownloadFileNumberTextBox.Visible = LocationExists

        If LocationExists Then
            FolderLocation = FolderLocationTextBox.Text
        Else
            FolderLocation = String.Empty
        End If

    End Sub

    ''' <summary>
    ''' This event handler is triggered when the "Load All Data Files" 
    ''' button is clicked. It checks which files exist in the specified 
    ''' folder location and sets the ProcessFileType accordingly 
    ''' (Compressed or Decompressed). It then opens a dialog to allow 
    ''' the user to choose whether to count rows or insert data, and 
    ''' if the user confirms, it starts a background worker to process 
    ''' the selected files.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LoadAllDataFilesButton_Click(sender As Object, e As EventArgs) _
        Handles LoadAllDataFilesButton.Click

        EndThingsButton.Text = "&Cancel"

        AcceptButton = EndThingsButton
        CancelButton = EndThingsButton

        CancelledOperations = False

        ' check which files exist, and then set PFT.Decompressed or PFT.Compressed
        ' if both exist, still use the decompressed files as long as the datechanged is the same
        ' if only compressed files used, then set to PFT.Compressed, and if only decompressed files exist
        ' then set to PFT.Decompressed

        ProcessFileType = PFT.Compressed    ' default to Compressed Files as they're the ones downloaded

        ' check if all files exist for compressed, decompressed, or both
        If File.Exists(Path.Combine(FolderLocation, C.NameBasicsDecompressedFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleAkasDecompressedFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleBasicsDecompressedFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleCrewDecompressedFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeDecompressedFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsDecompressedFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleRatingsDecompressedFileName)) Then

            ProcessFileType = PFT.Decompressed

        ElseIf File.Exists(Path.Combine(FolderLocation, C.NameBasicsCompressedFileName)) AndAlso
               File.Exists(Path.Combine(FolderLocation, C.TitleAkasCompressedFileName)) AndAlso
               File.Exists(Path.Combine(FolderLocation, C.TitleBasicsCompressedFileName)) AndAlso
               File.Exists(Path.Combine(FolderLocation, C.TitleCrewCompressedFileName)) AndAlso
               File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeCompressedFileName)) AndAlso
               File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsCompressedFileName)) AndAlso
               File.Exists(Path.Combine(FolderLocation, C.TitleRatingsCompressedFileName)) Then

            ProcessFileType = PFT.Compressed

        End If

        Using countOrInsertDataForm As New CountOrInsertData(PT.InsertData,
                                                             SP.Sequential,
                                                             ProcessFileType,
                                                             FolderLocation)

            If countOrInsertDataForm.ShowDialog <> DialogResult.OK Then
                EndThingsButton.Text = "E&xit"

                AcceptButton = LoadAllDataFilesButton
                CancelButton = EndThingsButton

                Exit Sub
            End If

            InsertDataFilesList.Clear()

            ' Retrieve the list of files to process from the dialog
            Dim filesToProcess = countOrInsertDataForm.ProcessFilesList

            For Each fileToProcess In filesToProcess
                InsertDataFilesList.Add(fileToProcess)
            Next

            ImportDataButton.Enabled = False

            CountArchiveRowsButtonEnabled = CountArchiveRowsButton.Enabled
            CountArchiveRowsButton.Enabled = False

            CountTsvRowsButtonEnabled = CountTsvRowsButton.Enabled
            CountTsvRowsButton.Enabled = False

            DecompressAfterDownloadCheckBox.Enabled = False
            DownloadUpdatedArchivesButton.Enabled = False
            LoadAllDataFilesButton.Enabled = False
            ChooseFolderButton.Enabled = False
            FolderLocationTextBox.Enabled = False

            CancelledOperations = False

            ' Launch the background worker to process the files
            SqlBackgroundWorker.RunWorkerAsync()
        End Using

    End Sub

    ' 7 files to download, each around 1.5GB, so we need to do this asynchronously and with progress reporting
    ' https://datasets.imdbws.com/name.basics.tsv.gz
    ' https://datasets.imdbws.com/title.akas.tsv.gz
    ' https://datasets.imdbws.com/title.basics.tsv.gz
    ' https://datasets.imdbws.com/title.crew.tsv.gz
    ' https://datasets.imdbws.com/title.episode.tsv.gz
    ' https://datasets.imdbws.com/title.principals.tsv.gz
    ' https://datasets.imdbws.com/title.ratings.tsv.gz

    ''' <summary>
    ''' Downloads a file from the specified URL to the specified destination path, while reporting progress. 
    ''' It uses HttpClient to send an asynchronous GET request and reads the response stream in chunks, writing them to a file. 
    ''' The progress is calculated based on the total bytes read and the total content length, and it updates a progress bar in the UI.
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="destinationPath"></param>
    ''' <returns></returns>
    Public Async Function DownloadFileWithProgress(url As String,
                                                   destinationPath As String) As Task

        Using client As New HttpClient()
            ' Get headers first without downloading the whole body

            Using response = Await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)
                response.EnsureSuccessStatusCode()

                Dim totalBytes = response.Content.Headers.ContentLength

                Dim totalBytesDisplay As String = String.Empty

                If totalBytes.HasValue Then
                    totalBytesDisplay = (GetFileDisplayLength(totalBytes) &
                                         " " &
                                         GetFileDisplayLengthString(totalBytes))

                Else
                    totalBytesDisplay = "Unknown Size"

                End If

                ' Handle file specific logic for each of the 7 files, and update the UI with 
                ' the file size info for each file so the user has a sense of how big the file 
                ' is before it starts downloading

                Select Case GetFileTypeBasedOnFileName(Path.GetFileName(destinationPath))
                    Case FT.NameBasics : TS.SetText(NameBasicsSizeTextBox, totalBytesDisplay)
                    Case FT.TitleAkas : TS.SetText(TitleAkasSizeTextBox, totalBytesDisplay)
                    Case FT.TitleBasics : TS.SetText(TitleBasicsSizeTextBox, totalBytesDisplay)
                    Case FT.TitleCrew : TS.SetText(TitleCrewSizeTextBox, totalBytesDisplay)
                    Case FT.TitleEpisode : TS.SetText(TitleEpisodeSizeTextBox, totalBytesDisplay)
                    Case FT.TitlePrincipals : TS.SetText(TitlePrincipalsSizeTextBox, totalBytesDisplay)
                    Case FT.TitleRatings : TS.SetText(TitleRatingsSizeTextBox, totalBytesDisplay)
                End Select

                Using contentStream = Await response.Content.ReadAsStreamAsync(),
                    fileStream = New FileStream(destinationPath,
                                                FileMode.Create,
                                                FileAccess.Write,
                                                FileShare.None,
                                                81919,
                                                True)

                    Dim buffer(81919) As Byte
                    Dim totalRead As Long = 0
                    Dim bytesRead As Integer

                    Do
                        bytesRead = Await contentStream.ReadAsync(buffer, 0, buffer.Length)

                        If bytesRead = 0 Then
                            Exit Do
                        End If

                        Await fileStream.WriteAsync(buffer, 0, bytesRead)

                        totalRead += bytesRead

                        ' Calculate and report progress
                        If totalBytes.HasValue Then
                            Dim progress = (totalRead / totalBytes.Value) * 100
                            'Console.WriteLine($"Progress: {progress:F2}%")

                            ' From inside your background thread:
                            TS.SetValue(ArchiveDownloadProgressBar, CInt(progress))
                        End If

                    Loop While True

                End Using
            End Using
        End Using

    End Function

    ''' <summary>
    ''' This event handler is triggered when the "Download Updated Archives" 
    ''' button is clicked. It disables other buttons and controls in the UI 
    ''' to prevent user interaction during the download process. It then iterates 
    ''' through a list of archive URLs, determines the file type based on the 
    ''' filename, and updates the UI to show the progress of each file download. 
    ''' The actual download is performed asynchronously, and progress is reported 
    ''' back to the UI.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub DownloadUpdatedArchivesButton_Click(sender As Object, e As EventArgs) _
        Handles DownloadUpdatedArchivesButton.Click

        ' disable the other buttons
        TS.SetEnabled(DownloadUpdatedArchivesButton, False)
        TS.SetEnabled(LoadAllDataFilesButton, False)
        TS.SetEnabled(ChooseFolderButton, False)
        TS.SetEnabled(FolderLocationTextBox, False)
        TS.SetEnabled(EndThingsButton, False)
        TS.SetEnabled(DecompressAfterDownloadCheckBox, False)

        CountArchiveRowsButtonEnabled = TS.GetEnabled(CountArchiveRowsButton)
        TS.SetEnabled(CountArchiveRowsButton, False)

        CountTsvRowsButtonEnabled = TS.GetEnabled(CountTsvRowsButton)
        TS.SetEnabled(CountTsvRowsButton, False)

        ' make all of the file size and row count info labels and textboxes not visible, 
        ' since we're going to use those same textboxes to show the progress of the file 
        ' downloads, and we don't want to show any row count info until after the files '
        ' are downloaded and we're processing them in the backgroundworker thread, so we'll 
        ' just hide those labels and textboxes for now, and then make them visible again in 
        ' the backgroundworker thread when we start processing the files

        TS.SetVisible(NameBasicsFilenameLabel, False)
        TS.SetVisible(NameBasicsCountTextBox, False)
        TS.SetVisible(NameBasicsSizeTextBox, False)

        TS.SetVisible(TitleAkasFilenameLabel, False)
        TS.SetVisible(TitleAkasCountTextBox, False)
        TS.SetVisible(TitleAkasSizeTextBox, False)

        TS.SetVisible(TitleBasicsFilenameLabel, False)
        TS.SetVisible(TitleBasicsCountTextBox, False)
        TS.SetVisible(TitleBasicsSizeTextBox, False)

        TS.SetVisible(TitleCrewFilenameLabel, False)
        TS.SetVisible(TitleCrewCountTextBox, False)
        TS.SetVisible(TitleCrewSizeTextBox, False)

        TS.SetVisible(TitleEpisodeFilenameLabel, False)
        TS.SetVisible(TitleEpisodeCountTextBox, False)
        TS.SetVisible(TitleEpisodeSizeTextBox, False)

        TS.SetVisible(TitlePrincipalsFilenameLabel, False)
        TS.SetVisible(TitlePrincipalsCountTextBox, False)
        TS.SetVisible(TitlePrincipalsSizeTextBox, False)

        TS.SetVisible(TitleRatingsFilenameLabel, False)
        TS.SetVisible(TitleRatingsCountTextBox, False)
        TS.SetVisible(TitleRatingsSizeTextBox, False)

        ' based on the current filename being downloaded, update the filesize and row count info 
        ' in the UI, and then update that info as the file is being downloaded, so the user has 
        ' a sense of how big the file is and how many rows it contains, and how far along the 
        ' download is for that file

        Dim currentFileNumber As Integer = 1
        Dim maxFileNumber As Integer = ArchiveDownloadLocationsList.Count

        For Each archiveUrl As String In ArchiveDownloadLocationsList

            Dim fileName As String = Path.GetFileName(New Uri(archiveUrl).LocalPath)

            Select Case GetFileTypeBasedOnFileName(fileName)
                Case FT.NameBasics
                    TS.SetVisible(NameBasicsFilenameLabel, True)
                    TS.SetVisible(NameBasicsCountTextBox, True)
                    TS.SetVisible(NameBasicsSizeTextBox, True)

                    TS.SetText(NameBasicsCountTextBox, String.Empty)
                    TS.SetText(NameBasicsSizeTextBox, String.Empty)

                Case FT.TitleAkas
                    TS.SetVisible(TitleAkasFilenameLabel, True)
                    TS.SetVisible(TitleAkasCountTextBox, True)
                    TS.SetVisible(TitleAkasSizeTextBox, True)

                    TS.SetText(TitleAkasCountTextBox, String.Empty)
                    TS.SetText(TitleAkasSizeTextBox, String.Empty)

                Case FT.TitleBasics
                    TS.SetVisible(TitleBasicsFilenameLabel, True)
                    TS.SetVisible(TitleBasicsCountTextBox, True)
                    TS.SetVisible(TitleBasicsSizeTextBox, True)

                    TS.SetText(TitleBasicsCountTextBox, String.Empty)
                    TS.SetText(TitleBasicsSizeTextBox, String.Empty)

                Case FT.TitleCrew
                    TS.SetVisible(TitleCrewFilenameLabel, True)
                    TS.SetVisible(TitleCrewCountTextBox, True)
                    TS.SetVisible(TitleCrewSizeTextBox, True)

                    TS.SetText(TitleCrewCountTextBox, String.Empty)
                    TS.SetText(TitleCrewSizeTextBox, String.Empty)

                Case FT.TitleEpisode
                    TS.SetVisible(TitleEpisodeFilenameLabel, True)
                    TS.SetVisible(TitleEpisodeCountTextBox, True)
                    TS.SetVisible(TitleEpisodeSizeTextBox, True)

                    TS.SetText(TitleEpisodeCountTextBox, String.Empty)
                    TS.SetText(TitleEpisodeSizeTextBox, String.Empty)

                Case FT.TitlePrincipals
                    TS.SetVisible(TitlePrincipalsFilenameLabel, True)
                    TS.SetVisible(TitlePrincipalsCountTextBox, True)
                    TS.SetVisible(TitlePrincipalsSizeTextBox, True)

                    TS.SetText(TitlePrincipalsCountTextBox, String.Empty)
                    TS.SetText(TitlePrincipalsSizeTextBox, String.Empty)

                Case FT.TitleRatings
                    TS.SetVisible(TitleRatingsFilenameLabel, True)
                    TS.SetVisible(TitleRatingsCountTextBox, True)
                    TS.SetVisible(TitleRatingsSizeTextBox, True)

                    TS.SetText(TitleRatingsCountTextBox, String.Empty)
                    TS.SetText(TitleRatingsSizeTextBox, String.Empty)

            End Select

            TS.SetText(CurrentFileTextBox,
                       Path.GetFileName(fileName))

            TS.SetText(DownloadFileNumberTextBox,
                       $"{currentFileNumber} of {maxFileNumber}")

            TS.SetMinimum(ArchiveDownloadProgressBar, 0)
            TS.SetMaximum(ArchiveDownloadProgressBar, 100)
            TS.SetValue(ArchiveDownloadProgressBar, 0)

            Await DownloadFileWithProgress(archiveUrl, Path.Combine(FolderLocation, fileName))

            currentFileNumber += 1
        Next

        ' launch separate task to count the rows in the file and update the UI with that info, 
        ' as it may take a while for the larger files, and it would be nice to have that info 
        ' available as soon as possible, rather than waiting until the file is fully downloaded 
        ' and then counting the rows in a separate step after the download is complete

        TS.SetText(DownloadFileNumberTextBox, String.Empty)
        TS.SetValue(ArchiveDownloadProgressBar, 0)

        Dim overwriteDecompressedFiles As Boolean = False
        Dim alreadyAskedIfOverwrite As Boolean = False
        Dim decompressedFileExists As Boolean = False

        DecompressedFileList.Clear()

        'DecompressAfterDownloadCheckBox
        If DecompressAfterDownloadCheckBox.Checked Then
            TS.SetEnabled(CountArchiveRowsButton, False)

            Dim compressedFileNumber As Integer = 0

            For Each compressedFile As String In Directory.GetFiles(FolderLocation, "*.gz")
                compressedFileNumber += 1
                decompressedFileExists = False

                Dim fileName As String = Path.GetFileName(compressedFile)
                Dim destinationPath As String = Path.Combine(FolderLocation, Path.GetFileNameWithoutExtension(fileName))

                If File.Exists(destinationPath) Then
                    decompressedFileExists = True

                    If Not overwriteDecompressedFiles AndAlso
                       Not alreadyAskedIfOverwrite AndAlso
                       MessageBox.Show("Decompressed Files already exist! Overwrite them?",
                                       "Overwrite Files?",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Information) = DialogResult.Yes Then

                        overwriteDecompressedFiles = True

                    End If

                    If Not alreadyAskedIfOverwrite Then
                        alreadyAskedIfOverwrite = True
                    End If

                    If overwriteDecompressedFiles Then
                        ' delete the existing output file
                        File.Delete(destinationPath)

                        decompressedFileExists = False
                    End If

                End If

                DecompressedFileList.Add(destinationPath)

                'Debug.Print("Decompress of " & fileName & " to " & Path.GetFileName(destinationPath))

                If overwriteDecompressedFiles OrElse
                   Not decompressedFileExists Then
                    If compressedFileNumber > 1 Then
                        TS.AppendText(ProgressLogTextBox, Environment.NewLine)
                    End If

                    Debug.Print("Decompress of " & fileName & " to " & Path.GetFileName(destinationPath))

                    TS.AppendText(ProgressLogTextBox,
                                  $"Decompressing File: {Path.GetFileName(compressedFile)} to File: {Path.GetFileName(destinationPath)}{Environment.NewLine}")

                    Dim destinationFileSize As Long =
                        Await DecompressDownloadedGZipFile(compressedFile, destinationPath)

                    'DecompressGZipFile(compressedFile, destinationPath)
                    Debug.Print("Completed decompress " &
                                " to File: " &
                                Path.GetFileName(destinationPath) & vbTab & " - " &
                                "Output File Size: " & destinationFileSize.ToString(C.COMMA_MASK))

                    TS.AppendText(ProgressLogTextBox,
                                  $"Completed decompress to File: {Path.GetFileName(destinationPath)}{vbTab} - Output File Size: {destinationFileSize.ToString(C.COMMA_MASK)}")
                End If
            Next
        End If

        CountTsvRowsButton.Enabled = ((File.Exists(Path.Combine(FolderLocation, C.NameBasicsDecompressedFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleAkasDecompressedFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleBasicsDecompressedFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleCrewDecompressedFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeDecompressedFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsDecompressedFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleRatingsDecompressedFileName))))
        CountTsvRowsButtonEnabled = CountTsvRowsButton.Enabled

        If CountTsvRowsButton.Enabled Then
            NameBasicsFilenameLabel.Text = C.NameBasicsDecompressedFileName
            TitleAkasFilenameLabel.Text = C.TitleAkasDecompressedFileName
            TitleBasicsFilenameLabel.Text = C.TitleBasicsDecompressedFileName
            TitleCrewFilenameLabel.Text = C.TitleCrewDecompressedFileName
            TitleEpisodeFilenameLabel.Text = C.TitleEpisodeDecompressedFileName
            TitlePrincipalsFilenameLabel.Text = C.TitlePrincipalsDecompressedFileName
            TitleRatingsFilenameLabel.Text = C.TitleRatingsDecompressedFileName

            Dim nameBasicsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.NameBasicsDecompressedFileName))).Length

            Dim titleAkasLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleAkasDecompressedFileName))).Length

            Dim titleBasicsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleBasicsDecompressedFileName))).Length

            Dim titleCrewLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleCrewDecompressedFileName))).Length

            Dim titleEpisodeLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleEpisodeDecompressedFileName))).Length

            Dim titlePrincipalsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitlePrincipalsDecompressedFileName))).Length

            Dim titleRatingsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleRatingsDecompressedFileName))).Length

            TS.SetText(NameBasicsSizeTextBox,
                       GetFileDisplayLength(nameBasicsLength) & " " &
                       GetFileDisplayLengthString(nameBasicsLength))

            TS.SetText(TitleAkasSizeTextBox,
                       GetFileDisplayLength(titleAkasLength) & " " &
                       GetFileDisplayLengthString(titleAkasLength))

            TS.SetText(TitleBasicsSizeTextBox,
                       GetFileDisplayLength(titleBasicsLength) & " " &
                       GetFileDisplayLengthString(titleBasicsLength))

            TS.SetText(TitleCrewSizeTextBox,
                       GetFileDisplayLength(titleCrewLength) & " " &
                       GetFileDisplayLengthString(titleCrewLength))

            TS.SetText(TitleEpisodeSizeTextBox,
                       GetFileDisplayLength(titleEpisodeLength) & " " &
                       GetFileDisplayLengthString(titleEpisodeLength))

            TS.SetText(TitlePrincipalsSizeTextBox,
                       GetFileDisplayLength(titlePrincipalsLength) & " " &
                       GetFileDisplayLengthString(titlePrincipalsLength))

            TS.SetText(TitleRatingsSizeTextBox,
                       GetFileDisplayLength(titleRatingsLength) & " " &
                       GetFileDisplayLengthString(titleRatingsLength))

            FileSizeHeader1Label.Text = "File Size .tsv"
            FileSizeHeader2Label.Text = "File Size .tsv"

        End If

        If Not DecompressAfterDownloadCheckBox.Checked Then
            CountArchiveRowsButton.Enabled = ((File.Exists(Path.Combine(FolderLocation, C.NameBasicsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleAkasCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleBasicsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleCrewCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleRatingsCompressedFileName))))

            CountArchiveRowsButtonEnabled = CountArchiveRowsButton.Enabled

            FileSizeHeader1Label.Text = "File Size .gz"
            FileSizeHeader2Label.Text = "File Size .gz"

        End If

        If CountArchiveRowsButton.Enabled Then
            ImportType = IT.Compressed

        ElseIf CountTsvRowsButton.Enabled Then
            ImportType = IT.Decompressed

        Else
            ImportType = IT.Unknown

        End If

        Dim completionMessage As String = "Completed downloading "

        If DecompressAfterDownloadCheckBox.Checked Then
            completionMessage &= "and decompressing "
        End If

        completionMessage &= "IMDB compressed data files"

        MessageBox.Show(completionMessage,
                        "Download Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)

        ' Re-enable the buttons

        TS.SetEnabled(LoadAllDataFilesButton, True)
        TS.SetEnabled(ChooseFolderButton, True)
        TS.SetEnabled(FolderLocationTextBox, True)
        TS.SetEnabled(EndThingsButton, True)
        TS.SetEnabled(DownloadUpdatedArchivesButton, True)
        TS.SetEnabled(DecompressAfterDownloadCheckBox, True)

        TS.SetEnabled(CountArchiveRowsButton, CountArchiveRowsButtonEnabled)
        TS.SetEnabled(CountTsvRowsButton, CountTsvRowsButtonEnabled)

        Me.CancelButton = EndThingsButton
        Me.AcceptButton = LoadAllDataFilesButton

    End Sub

    ''' <summary>
    ''' Gets the display string for the file length, either "GB" or "MB".
    ''' </summary>
    ''' <param name="fileLength">The length of the file in bytes.</param>
    ''' <returns>A string representing the file length unit.</returns>
    Private Function GetFileDisplayLengthString(fileLength As Long) As String

        Return IIf(FileIsGbOrLarger(fileLength), "GB", "MB")

    End Function

    ''' <summary>
    ''' Gets the display string for the file length value.
    ''' </summary>
    ''' <param name="fileLength">The length of the file in bytes.</param>
    ''' <returns>A string representing the file length value.</returns>
    Private Function GetFileDisplayLength(fileLength As Long) As String

        Return CType(IIf(FileIsGbOrLarger(fileLength),
                         GetGBDisplayLength(fileLength),
                         GetMBDisplayLength(fileLength)), Double).ToString("F2")


    End Function

    ''' <summary>
    ''' Determines if the file length is greater than or equal to 1 GB.
    ''' </summary>
    ''' <param name="fileLength">The length of the file in bytes.</param>
    ''' <returns>True if the file length is greater than or equal to 1 GB, otherwise False.</returns>
    Private Function FileIsGbOrLarger(fileLength As Long) As Boolean

        Return (GetMBDisplayLength(fileLength) >= 1024.0)

    End Function

    ''' <summary>
    ''' Gets the display length in gigabytes for the given file length in bytes.
    ''' </summary>
    ''' <param name="fileLength">The length of the file in bytes.</param>
    ''' <returns>The display length in gigabytes.</returns>
    Private Function GetGBDisplayLength(fileLength As Long) As Double

        Return CType((fileLength / (1024 * 1024 * 1024)), Double)

    End Function

    ''' <summary>
    ''' Gets the display length in megabytes for the given file length in bytes.
    ''' </summary>
    ''' <param name="fileLength">The length of the file in bytes.</param>
    ''' <returns>The display length in megabytes.</returns>
    Private Function GetMBDisplayLength(fileLength As Long) As Double

        Return CType((fileLength / (1024 * 1024)), Double)

    End Function

    ''' <summary>
    ''' Decompresses a downloaded GZip file asynchronously. It reads the 
    ''' compressed file, decompresses it, and writes the decompressed data 
    ''' to the specified output path. The function returns the size of the 
    ''' decompressed file in bytes.
    ''' </summary>
    ''' <param name="zipPath">The path to the compressed GZip file.</param>
    ''' <param name="outputPath">The path where the decompressed file will be written.</param>
    ''' <returns>The size of the decompressed file in bytes.</returns>
    Private Async Function DecompressDownloadedGZipFile(zipPath As String, outputPath As String) As Task(Of Long)

        Dim decompAsync =
            Async Function(compressedPath As String, decompressedPath As String) As Task(Of Long)
                ' Open the compressed file for reading
                Using compressedStream As New FileStream(compressedPath,
                                                         FileMode.Open,
                                                         FileAccess.Read)
                    ' Create the output file for writing
                    Using outputStream As New FileStream(decompressedPath,
                                                         FileMode.Create,
                                                         FileAccess.Write)
                        ' Wrap the compressed stream in a GZipStream set to Decompress mode
                        Using decompressor As New Comp.GZipStream(compressedStream,
                                                                  Comp.CompressionMode.Decompress)
                            Await Task.Delay(100)

                            ' Copy the decompressed data to the output file stream
                            decompressor.CopyTo(outputStream)
                        End Using
                    End Using
                End Using

                Dim myFileInfo As New FileInfo(outputPath)

                Dim result As Long = myFileInfo.Length
                myFileInfo = Nothing

                Return result
            End Function

        Return Await decompAsync(zipPath, outputPath)

    End Function

    ''' <summary>
    ''' Decompresses a GZip file. It reads the compressed file, decompresses 
    ''' it, and writes the decompressed data to the specified output path.
    ''' </summary>
    ''' <param name="zipPath">The path to the compressed GZip file.</param>
    ''' <param name="outputPath">The path where the decompressed file will be written.</param>
    Public Sub DecompressGZipFile(zipPath As String,
                                  outputPath As String)

        ' Open the compressed file for reading
        Using compressedStream As New FileStream(zipPath, FileMode.Open, FileAccess.Read)
            ' Create the output file for writing
            Using outputStream As New FileStream(outputPath, FileMode.Create, FileAccess.Write)
                ' Wrap the compressed stream in a GZipStream set to Decompress mode
                Using decompressor As New Comp.GZipStream(compressedStream, Comp.CompressionMode.Decompress)
                    ' Copy the decompressed data to the output file stream
                    decompressor.CopyTo(outputStream)
                End Using
            End Using
        End Using

    End Sub

    ''' <summary>
    ''' Handles the click event for the "End Things" button. Depending 
    ''' on the button's text, it either cancels any ongoing background 
    ''' worker operations or exits the application. If the button's text 
    ''' is "&Cancel", it checks which background worker is busy and 
    ''' requests cancellation. If the button's text is "E&xit", it 
    ''' closes the application.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The event data.</param>
    Private Sub EndThingsButton_Click(sender As Object, e As EventArgs) _
        Handles EndThingsButton.Click

        If EndThingsButton.Text = "&Cancel" Then
            ' end the backgroundworker thread

            If SqlBackgroundWorker.IsBusy Then
                SqlBackgroundWorker.CancelAsync()

            ElseIf SqlImportBackgroundWorker.IsBusy Then
                SqlImportBackgroundWorker.CancelAsync()

            ElseIf AllArchivesSequentialBackgroundWorker.IsBusy Then
                AllArchivesSequentialBackgroundWorker.CancelAsync()

            ElseIf NameBasicsBackgroundWorker.IsBusy Then
                NameBasicsBackgroundWorker.CancelAsync()

            ElseIf TitleAkasBackgroundWorker.IsBusy Then
                TitleAkasBackgroundWorker.CancelAsync()

            ElseIf TitleBasicsBackgroundWorker.IsBusy Then
                TitleBasicsBackgroundWorker.CancelAsync()

            ElseIf TitleCrewBackgroundWorker.IsBusy Then
                TitleCrewBackgroundWorker.CancelAsync()

            ElseIf TitleEpisodeBackgroundWorker.IsBusy Then
                TitleEpisodeBackgroundWorker.CancelAsync()

            ElseIf TitlePrincipalsBackgroundWorker.IsBusy Then
                TitlePrincipalsBackgroundWorker.CancelAsync()

            ElseIf TitleRatingsBackgroundWorker.IsBusy Then
                TitleRatingsBackgroundWorker.CancelAsync()

            End If

        ElseIf EndThingsButton.Text = "E&xit" Then
            ' exit the app
            Me.Close()

        End If

    End Sub

    ''' <summary>
    ''' Counts the number of rows in a specified file and updates the corresponding text box.
    ''' </summary>
    ''' <param name="localFileName">The name of the file to count rows for.</param>
    ''' <returns>The number of rows in the file.</returns>
    Private Function CountFileRows(localFolderLocation As String,
                                   localFileName As String) As Long

        Dim rowCount As Long = IO.File.ReadLines(Path.Combine(localFolderLocation, localFileName)).Count - 1

        Dim localImportType As IT = IT.Unknown
        Dim localFileType As FT = GetFileTypeBasedOnFileName(localFileName, localImportType)

        If localImportType = IT.Decompressed Then
            Select Case localFileType
                Case FT.NameBasics : TS.SetText(NameBasicsCountTextBox, rowCount.ToString(C.COMMA_MASK))
                Case FT.TitleAkas : TS.SetText(TitleAkasCountTextBox, rowCount.ToString(C.COMMA_MASK))
                Case FT.TitleBasics : TS.SetText(TitleBasicsCountTextBox, rowCount.ToString(C.COMMA_MASK))
                Case FT.TitleCrew : TS.SetText(TitleCrewCountTextBox, rowCount.ToString(C.COMMA_MASK))
                Case FT.TitleEpisode : TS.SetText(TitleEpisodeCountTextBox, rowCount.ToString(C.COMMA_MASK))
                Case FT.TitlePrincipals : TS.SetText(TitlePrincipalsCountTextBox, rowCount.ToString(C.COMMA_MASK))
                Case FT.TitleRatings : TS.SetText(TitleRatingsCountTextBox, rowCount.ToString(C.COMMA_MASK))
            End Select
        End If

        Return rowCount

    End Function

    ''' <summary>
    ''' Counts the number of rows in a specified compressed file and updates the corresponding text box.
    ''' </summary>
    ''' <param name="fileName">The name of the compressed file to count rows for.</param>
    ''' <returns>The number of rows in the compressed file.</returns>
    Private Function CountCompressedFileRows(ByVal folderLocation As String,
                                             ByVal fileName As String) As Long

        Dim rowCount As Long = 0
        Dim actualRowCount As Long = 0

        Dim fileInfoObj As New FileInfo(Path.Combine(folderLocation, fileName))
        Dim gzipFileStream As FileStream = IO.File.OpenRead(fileInfoObj.FullName)

        Using decompressionStream As New Comp.GZipStream(gzipFileStream,
                                                         Comp.CompressionMode.Decompress)

            ' Create a stream reader to read from the decompression stream
            Using myStreamReader As New StreamReader(decompressionStream)
                Try
                    Dim line As String =
                        myStreamReader.ReadLine()

                    Do While (line IsNot Nothing)
                        actualRowCount += 1

                        If actualRowCount > 1 Then
                            rowCount += 1
                        End If

                        Dim weShouldExitNow As Boolean = False

                        If AllArchivesSequentialBackgroundWorker.IsBusy Then
                            If AllArchivesSequentialBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True
                            End If

                        ElseIf (NameBasicsBackgroundWorker.IsBusy OrElse
                                TitleAkasBackgroundWorker.IsBusy OrElse
                                TitleBasicsBackgroundWorker.IsBusy OrElse
                                TitleCrewBackgroundWorker.IsBusy OrElse
                                TitleEpisodeBackgroundWorker.IsBusy OrElse
                                TitlePrincipalsBackgroundWorker.IsBusy OrElse
                                TitleRatingsBackgroundWorker.IsBusy) Then

                            If NameBasicsBackgroundWorker.IsBusy Then
                                If Not NameBasicsBackgroundWorker.CancellationPending Then
                                    NameBasicsBackgroundWorker.CancelAsync()
                                End If
                                weShouldExitNow = True

                            ElseIf NameBasicsBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True
                            End If

                            If TitleAkasBackgroundWorker.IsBusy Then
                                If Not TitleAkasBackgroundWorker.CancellationPending Then
                                    TitleAkasBackgroundWorker.CancelAsync()
                                End If

                                weShouldExitNow = True

                            ElseIf TitleAkasBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True

                            End If

                            If TitleBasicsBackgroundWorker.IsBusy Then
                                If Not TitleBasicsBackgroundWorker.CancellationPending Then
                                    TitleBasicsBackgroundWorker.CancelAsync()
                                End If

                                weShouldExitNow = True

                            ElseIf TitleBasicsBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True

                            End If

                            If TitleCrewBackgroundWorker.IsBusy Then
                                If Not TitleCrewBackgroundWorker.CancellationPending Then
                                    TitleCrewBackgroundWorker.CancelAsync()
                                End If

                                weShouldExitNow = True

                            ElseIf TitleCrewBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True

                            End If

                            If TitleEpisodeBackgroundWorker.IsBusy Then
                                If Not TitleEpisodeBackgroundWorker.CancellationPending Then
                                    TitleEpisodeBackgroundWorker.CancelAsync()
                                End If

                                weShouldExitNow = True

                            ElseIf TitleEpisodeBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True

                            End If

                            If TitlePrincipalsBackgroundWorker.IsBusy Then
                                If Not TitlePrincipalsBackgroundWorker.CancellationPending Then
                                    TitlePrincipalsBackgroundWorker.CancelAsync()
                                End If

                                weShouldExitNow = True

                            ElseIf TitlePrincipalsBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True

                            End If

                            If TitleRatingsBackgroundWorker.IsBusy Then
                                If Not TitleRatingsBackgroundWorker.CancellationPending Then
                                    TitleRatingsBackgroundWorker.CancelAsync()
                                End If

                                weShouldExitNow = True

                            ElseIf TitleRatingsBackgroundWorker.CancellationPending Then
                                weShouldExitNow = True

                            End If

                        End If

                        If weShouldExitNow Then
                            CancelledOperations = True

                            Exit Do
                        End If

                        If rowCount > 1 Then
                            ' set the textbox text to show the progress of counting the rows in the file, 
                            ' as this may take a while for the larger files, and it would be nice to have 
                            ' that info available as soon as possible, rather than waiting until the file 
                            ' is fully counted and then updating the UI with that info

                            'Debug.Print(rowCount.ToString())
                            Dim localImportType As IT = IT.Unknown
                            Dim localFileType As FT = GetFileTypeBasedOnFileName(fileName, localImportType)

                            If localImportType = IT.Compressed Then
                                Select Case localFileType
                                    Case FT.NameBasics
                                        TS.SetText(NameBasicsCountTextBox, rowCount.ToString(C.COMMA_MASK))

                                    Case FT.TitleAkas
                                        TS.SetText(TitleAkasCountTextBox, rowCount.ToString(C.COMMA_MASK))

                                    Case FT.TitleBasics
                                        TS.SetText(TitleBasicsCountTextBox, rowCount.ToString(C.COMMA_MASK))

                                    Case FT.TitleCrew
                                        TS.SetText(TitleCrewCountTextBox, rowCount.ToString(C.COMMA_MASK))

                                    Case FT.TitleEpisode
                                        TS.SetText(TitleEpisodeCountTextBox, rowCount.ToString(C.COMMA_MASK))

                                    Case FT.TitlePrincipals
                                        TS.SetText(TitlePrincipalsCountTextBox, rowCount.ToString(C.COMMA_MASK))

                                    Case FT.TitleRatings
                                        TS.SetText(TitleRatingsCountTextBox, rowCount.ToString(C.COMMA_MASK))

                                End Select
                            End If
                        End If

                        line = myStreamReader.ReadLine()
                    Loop

                Catch ex As Exception
                    Debug.Print(ex.Message)

                    LogErrorsToFile($"Exception: {ex.ToString()}")

                Finally
                    myStreamReader.Close()
                    decompressionStream.Close()
                    gzipFileStream.Close()

                End Try
            End Using
        End Using

        Return rowCount

    End Function

    ''' <summary>
    ''' Handles the click event for the "Count Archive Rows" button. It 
    ''' initializes the counting process for the rows in the compressed 
    ''' archive files. Depending on the user's choice, it can count the 
    ''' rows sequentially or in parallel. The method updates the UI with 
    ''' the row counts as they are calculated, providing feedback to the 
    ''' user on the progress of the counting operation.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The event data.</param>
    Private Sub CountArchiveRowsButton_Click(sender As Object, e As EventArgs) _
        Handles CountArchiveRowsButton.Click

        Dim localFileList As New List(Of String)

        EndThingsButton.Text = "&Cancel"

        Me.AcceptButton = EndThingsButton
        Me.CancelButton = EndThingsButton

        Me.ImportType = IT.Compressed

        For Each localFT As FT In [Enum].GetValues(Of FT)()
            If ((localFT = FT.OVERALL) OrElse
                (localFT = FT.Unknown)) Then
                Continue For
            End If

            With MyRawFileInfo(localFT)
                .ToBeCounted = False
                .IsBeingCounted = False
                .HasBeenCounted = False

                .CountedRows = 0
            End With
        Next

        Using countOrInsertDataForm As New CountOrInsertData(PT.CountData,
                                                             SP.Sequential,
                                                             PFT.Compressed,
                                                             FolderLocation)

            If countOrInsertDataForm.ShowDialog() <> DialogResult.OK Then
                EndThingsButton.Text = "E&xit"

                Me.AcceptButton = LoadAllDataFilesButton
                Me.CancelButton = EndThingsButton

                Exit Sub
            End If

            ' get list of files to process from the CountOrInsertData form, 
            ' then kick off the counting of rows in those files,
            ' then update the UI with that info as it becomes available, 
            ' so the user has a sense of how many rows are in each file, 
            ' and can see the progress of that counting as it happens, rather 
            ' than waiting until all the counting is done and then updating 
            ' the UI with that info
            Dim filesToProcess As List(Of String) = countOrInsertDataForm.ProcessFilesList
            Dim gzFilesToProcess As String() = countOrInsertDataForm.ProcessFilesList.ToArray

            Dim localFileType As FT

            For Each gzFile As String In gzFilesToProcess
                localFileType = GetFileTypeBasedOnFileName(Path.GetFileName(gzFile))

                ' This check shouldn't be needed, but I'm simply being careful to be robust
                If MyRawFileInfo(localFileType).ToBeCounted Then
                    MyRawFileInfo(localFileType).ToBeCounted = True
                    MyRawFileInfo(localFileType).IsBeingCounted = True
                End If
            Next

            Me.ProcessType = countOrInsertDataForm.ProcessType
            Me.SequentialOrParallel = countOrInsertDataForm.SequentialOrParallel
            Me.ChooseAllOrSelected = countOrInsertDataForm.ChooseAllOrSelected

            CountFilesList.Clear()

            For Each fileToProcess As String In filesToProcess
                CountFilesList.Add(fileToProcess)
            Next

            Select Case Me.SequentialOrParallel
                Case SP.Sequential
                    DownloadUpdatedArchivesButton.Enabled = False
                    LoadAllDataFilesButton.Enabled = False
                    ChooseFolderButton.Enabled = False
                    FolderLocationTextBox.Enabled = False

                    CountArchiveRowsButtonEnabled = CountArchiveRowsButton.Enabled
                    CountArchiveRowsButton.Enabled = False
                    CountTsvRowsButtonEnabled = CountTsvRowsButton.Enabled
                    CountTsvRowsButton.Enabled = False

                    CancelledOperations = False

                    AllArchivesSequentialBackgroundWorker.RunWorkerAsync()

                Case SP.Parallel

                    Dim fileLineCounts As New ConcurrentDictionary(Of String, Long)()

                    Dim watch As Stopwatch = Stopwatch.StartNew()

                    Parallel.ForEach(gzFilesToProcess,
                                 Sub(currentFile)
                                     Dim lines As Long = CountGZipLinesFast(currentFile)

                                     fileLineCounts.TryAdd(currentFile, lines)
                                 End Sub)

                    watch.Stop()

                    Dim elapsedMs As Long = watch.ElapsedMilliseconds

                    Debug.WriteLine($"Elapsed time: {elapsedMs} ms")

                    ' save the counted line amounts to the appropriate variables 
                    ' for each file, so that they can be used later in the program, 
                    ' and also update the UI with that info as it becomes available, 
                    ' so the user has a sense of how many rows are in each file, and 
                    ' can see the progress of that counting as it happens, rather 
                    ' than waiting until all the counting is done and then updating 
                    ' the UI with that info

                    For Each kvp As KeyValuePair(Of String, Long) In fileLineCounts
                        Dim fileName As String = kvp.Key
                        Dim lineCount As Long = kvp.Value

                        Dim localImportType As IT = IT.Unknown

                        localFileType = GetFileTypeBasedOnFileName(Path.GetFileName(fileName), localImportType)

                        If localImportType = IT.Compressed Then
                            With MyRawFileInfo(localFileType)
                                .CountedRows = lineCount
                                .HasBeenCounted = True
                            End With

                            Select Case localFileType
                                Case FT.NameBasics
                                    TS.SetText(NameBasicsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                                Case FT.TitleAkas
                                    TS.SetText(TitleAkasCountTextBox, lineCount.ToString(C.COMMA_MASK))

                                Case FT.TitleBasics
                                    TS.SetText(TitleBasicsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                                Case FT.TitleCrew
                                    TS.SetText(TitleCrewCountTextBox, lineCount.ToString(C.COMMA_MASK))

                                Case FT.TitleEpisode
                                    TS.SetText(TitleEpisodeCountTextBox, lineCount.ToString(C.COMMA_MASK))

                                Case FT.TitlePrincipals
                                    TS.SetText(TitlePrincipalsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                                Case FT.TitleRatings
                                    TS.SetText(TitleRatingsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                            End Select
                        End If

                    Next

                    Dim countedFiles As Integer = 0
                    Dim filesToCount As Integer = 0

                    For Each localFT As FT In [Enum].GetValues(Of FT)()
                        If ((localFT = FT.OVERALL) OrElse
                            (localFT = FT.Unknown)) Then
                            Continue For
                        End If

                        With MyRawFileInfo(localFT)
                            If .IsBeingCounted Then filesToCount += 1
                            If .HasBeenCounted Then countedFiles += 1
                        End With
                    Next

                    If countedFiles = filesToCount Then
                        ' we have counted all of the files that were selected
                        MyRawFileInfo(FT.OVERALL).CountedRows = 0

                        For Each localFT In [Enum].GetValues(Of FT)()
                            If ((localFT = FT.OVERALL) OrElse
                                (localFT = FT.Unknown)) Then
                                Continue For
                            End If

                            With MyRawFileInfo(localFT)
                                If .HasBeenCounted Then
                                    MyRawFileInfo(FT.OVERALL).CountedRows += .CountedRows
                                End If
                            End With
                        Next
                    End If

            End Select
        End Using

        ' kick off separate tasks to count the rows in each file using BackgroundWorker, 
        ' and then update the UI with that info as it becomes available, so the user has 
        ' a sense of how many rows are in each file, and can see the progress of that 
        ' counting as it happens, rather than waiting until all the counting is done and 
        ' then updating the UI with that info

    End Sub

    ''' <summary>
    ''' Handles the click event for the "Count TSV Rows" button. It 
    ''' initializes the counting process for the rows in the 
    ''' decompressed TSV files. Depending on the user's choice, it 
    ''' can count the rows sequentially or in parallel. The method 
    ''' updates the UI with the row counts as they are calculated, 
    ''' providing feedback to the user on the progress of the 
    ''' counting operation.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CountTsvRowsButton_Click(sender As Object, e As EventArgs) _
        Handles CountTsvRowsButton.Click

        EndThingsButton.Text = "&Cancel"

        Me.AcceptButton = EndThingsButton
        Me.CancelButton = EndThingsButton

        Me.ImportType = IT.Decompressed

        For Each localFT As FT In [Enum].GetValues(Of FT)()
            If ((localFT = FT.OVERALL) OrElse
                (localFT = FT.Unknown)) Then
                Continue For
            End If

            With MyRawFileInfo(localFT)
                .IsBeingCounted = False
                .HasBeenCounted = False
                .ToBeCounted = False

                .CountedRows = 0
            End With
        Next

        ' kick off separate tasks to count the rows in each file using BackgroundWorker, 
        ' and then update the UI with that info as it becomes available, so the user has 
        ' a sense of how many rows are in each file, and can see the progress of that 
        ' counting as it happens, rather than waiting until all the counting is done and 
        ' then updating the UI with that info

        Using countOrInsertDataForm As New CountOrInsertData(PT.CountData,
                                                             SP.Sequential,
                                                             PFT.Decompressed,
                                                             FolderLocation)

            If countOrInsertDataForm.ShowDialog() <> DialogResult.OK Then
                EndThingsButton.Text = "E&xit"

                Me.AcceptButton = LoadAllDataFilesButton
                Me.CancelButton = EndThingsButton

                Exit Sub
            End If

            ' get list of files to process from the CountOrInsertData form, and then kick off the counting of rows in those files,
            ' and then update the UI with that info as it becomes available, so the user has a sense of how many rows are in each file, and can see the progress of that counting as it happens, rather than waiting until all the counting is done and then updating the UI with that info
            Dim filesToProcess As List(Of String) = countOrInsertDataForm.ProcessFilesList

            Dim processType As PT = countOrInsertDataForm.ProcessType

            Me.SequentialOrParallel = countOrInsertDataForm.SequentialOrParallel
            Me.ChooseAllOrSelected = countOrInsertDataForm.ChooseAllOrSelected

            CountFilesList.Clear()

            For Each fileToProcess As String In filesToProcess
                Dim localFileType As FT = GetFileTypeBasedOnFileName(Path.GetFileName(fileToProcess))
                MyRawFileInfo(localFileType).IsBeingCounted = True

                CountFilesList.Add(fileToProcess)
            Next

            Select Case Me.SequentialOrParallel
                Case SP.Sequential
                    DownloadUpdatedArchivesButton.Enabled = False
                    LoadAllDataFilesButton.Enabled = False
                    ChooseFolderButton.Enabled = False
                    FolderLocationTextBox.Enabled = False

                    CancelledOperations = False

                    CountTsvRowsButtonEnabled = CountTsvRowsButton.Enabled
                    CountTsvRowsButton.Enabled = False

                    CountArchiveRowsButtonEnabled = CountArchiveRowsButton.Enabled
                    CountArchiveRowsButton.Enabled = False

                    AllArchivesSequentialBackgroundWorker.RunWorkerAsync()

                Case SP.Parallel
                    ' kick off the backgroundworker for each file to process

                    DownloadUpdatedArchivesButton.Enabled = False
                    LoadAllDataFilesButton.Enabled = False
                    ChooseFolderButton.Enabled = False
                    FolderLocationTextBox.Enabled = False

                    CountTsvRowsButtonEnabled = CountTsvRowsButton.Enabled
                    CountTsvRowsButton.Enabled = False

                    CountArchiveRowsButtonEnabled = CountArchiveRowsButton.Enabled
                    CountArchiveRowsButton.Enabled = False

                    CancelledOperations = False

                    ' kick off the backgroundworker for each selected file to process, 
                    ' and then update the UI with that info as it becomes available, so 
                    ' the user has a sense of how many rows are in each file, and can 
                    ' see the progress of that counting as it happens, rather than waiting 
                    ' until all the counting is done and then updating the UI with that info

                    For Each fileToProcess As String In CountFilesList
                        If fileToProcess.StartsWith(FolderLocation) Then
                            fileToProcess = Path.GetFileName(fileToProcess)
                        End If

                        Dim localImportType As IT = IT.Unknown
                        Dim localFileType As FT = GetFileTypeBasedOnFileName(fileToProcess, localImportType)

                        If localImportType = IT.Decompressed AndAlso
                           File.Exists(Path.Combine(FolderLocation, fileToProcess)) Then

                            MyRawFileInfo(localFileType).IsBeingCounted = True

                            Select Case localFileType
                                Case FT.NameBasics
                                    NameBasicsBackgroundWorker.RunWorkerAsync()

                                Case FT.TitleAkas
                                    TitleAkasBackgroundWorker.RunWorkerAsync()

                                Case FT.TitleBasics
                                    TitleBasicsBackgroundWorker.RunWorkerAsync()

                                Case FT.TitleCrew
                                    TitleCrewBackgroundWorker.RunWorkerAsync()

                                Case FT.TitleEpisode
                                    TitleEpisodeBackgroundWorker.RunWorkerAsync()

                                Case FT.TitlePrincipals
                                    TitlePrincipalsBackgroundWorker.RunWorkerAsync()

                                Case FT.TitleRatings
                                    TitleRatingsBackgroundWorker.RunWorkerAsync()
                            End Select

                        End If
                    Next
            End Select

        End Using

    End Sub

    ''' <summary>
    ''' Counts the number of lines in a GZip compressed file quickly by 
    ''' reading the file in chunks and counting newline characters. It 
    ''' uses a buffer to read the file in 64 KB chunks, which improves 
    ''' performance for large files. The method returns the total line 
    ''' count minus one to account for the header row.
    ''' </summary>
    ''' <param name="filePath">The path to the GZip compressed file.</param>
    ''' <returns>The number of lines in the file, excluding the header row.</returns>
    Private Function CountGZipLinesFast(filePath As String) As Long

        Dim lineCount As Long = 0
        Dim bufferSize As Integer = 65536 ' 64 KB
        Dim buffer(bufferSize - 1) As Byte

        Using fileStream As New FileStream(filePath,
                                           FileMode.Open,
                                           FileAccess.Read,
                                           FileShare.Read,
                                           bufferSize)

            Using gzipStream As New GZipStream(fileStream,
                                               CompressionMode.Decompress)

                Dim bytesRead As Integer = gzipStream.Read(buffer, 0, bufferSize)

                While bytesRead > 0

                    For i As Integer = 0 To bytesRead - 1
                        If buffer(i) = 10 Then
                            lineCount += 1
                        End If
                    Next

                    bytesRead = gzipStream.Read(buffer, 0, bufferSize)
                End While
            End Using
        End Using

        ' since each file has one first row that has the column names, 
        ' we subtract that one since it isn't part of the actual data

        Return lineCount - 1

    End Function

    ''' <summary>
    ''' Checks if all the data files have been counted and updates the UI controls accordingly. 
    ''' If all files are counted or if any operation has been cancelled, it re-enables the relevant 
    ''' UI controls (FolderLocationTextBox, ChooseFolderButton, LoadAllDataFilesButton, DownloadUpdatedArchivesButton). 
    ''' It also resets the EndThingsButton text to "E&xit" if it was previously set to "&Cancel".
    ''' </summary>
    Private Sub CheckAllCounted()

        Dim allCounted As Boolean = (MyRawFileInfo(FT.NameBasics).HasBeenCounted AndAlso
                                     MyRawFileInfo(FT.TitleAkas).HasBeenCounted AndAlso
                                     MyRawFileInfo(FT.TitleBasics).HasBeenCounted AndAlso
                                     MyRawFileInfo(FT.TitleCrew).HasBeenCounted AndAlso
                                     MyRawFileInfo(FT.TitleEpisode).HasBeenCounted AndAlso
                                     MyRawFileInfo(FT.TitlePrincipals).HasBeenCounted AndAlso
                                     MyRawFileInfo(FT.TitleRatings).HasBeenCounted)

        Dim reEnableControls As Boolean = (allCounted OrElse
                                           CancelledOperations)

        FolderLocationTextBox.Enabled = reEnableControls
        ChooseFolderButton.Enabled = reEnableControls
        LoadAllDataFilesButton.Enabled = reEnableControls
        DownloadUpdatedArchivesButton.Enabled = reEnableControls

        CountArchiveRowsButton.Enabled = CountArchiveRowsButtonEnabled
        CountTsvRowsButton.Enabled = CountTsvRowsButtonEnabled

        If reEnableControls Then
            ' reset the EndThingsButton
            If EndThingsButton.Text = "&Cancel" Then
                EndThingsButton.Text = "E&xit"

                Me.AcceptButton = LoadAllDataFilesButton
                Me.CancelButton = EndThingsButton
            End If
        End If

    End Sub

    ''' <summary>
    ''' Checks if the selected files in the provided list have been counted and updates the UI controls accordingly. 
    ''' If all selected files are counted or if any operation has been cancelled, it re-enables the relevant 
    ''' UI controls (FolderLocationTextBox, ChooseFolderButton, LoadAllDataFilesButton, DownloadUpdatedArchivesButton). 
    ''' It also resets the EndThingsButton text to "E&xit" if it was previously set to "&Cancel".
    ''' </summary>
    ''' <param name="countFilesList"></param>
    Private Sub CheckSelectedCounted(countFilesList As List(Of String))

        Dim allCounted As Boolean = False

        ' keep a list of all files being counted, and check if all of them have been counted, and if so, re-enable the controls

        Dim countingFiles As Integer = 0
        Dim countedFiles As Integer = 0

        For Each localFT As FT In [Enum].GetValues(Of FT)()
            If ((localFT = FT.OVERALL) OrElse
                (localFT = FT.Unknown)) Then
                Continue For
            End If

            With MyRawFileInfo(localFT)
                If .IsBeingCounted Then countingFiles += 1
                If .HasBeenCounted Then countedFiles += 1
            End With
        Next

        allCounted = (countedFiles = countingFiles)

        If CancelledOperations OrElse
           allCounted Then

            With MyRawFileInfo(FT.NameBasics)
                If .HasBeenCounted Then .IsBeingCounted = False
            End With

            With MyRawFileInfo(FT.TitleAkas)
                If .HasBeenCounted Then .IsBeingCounted = False
            End With

            With MyRawFileInfo(FT.TitleBasics)
                If .HasBeenCounted Then .IsBeingCounted = False
            End With

            With MyRawFileInfo(FT.TitleCrew)
                If .HasBeenCounted Then .IsBeingCounted = False
            End With

            With MyRawFileInfo(FT.TitleEpisode)
                If .HasBeenCounted Then .IsBeingCounted = False
            End With

            With MyRawFileInfo(FT.TitlePrincipals)
                If .HasBeenCounted Then .IsBeingCounted = False
            End With

            With MyRawFileInfo(FT.TitleRatings)
                If .HasBeenCounted Then .IsBeingCounted = False
            End With

            FolderLocationTextBox.Enabled = True
            ChooseFolderButton.Enabled = True
            LoadAllDataFilesButton.Enabled = True
            DownloadUpdatedArchivesButton.Enabled = True
            CountArchiveRowsButton.Enabled = CountArchiveRowsButtonEnabled
            CountTsvRowsButton.Enabled = CountTsvRowsButtonEnabled
            ' reset the EndThingsButton

            If EndThingsButton.Text = "&Cancel" Then
                EndThingsButton.Text = "E&xit"

                Me.AcceptButton = LoadAllDataFilesButton
                Me.CancelButton = EndThingsButton

            End If
        End If

    End Sub


#Region "BackgroundWorker Objects DoWork Event Handlers"
    ''' <summary>
    ''' Handles the DoWork event for the NameBasicsBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests from other background workers and
    ''' counts the rows in the NameBasics data file based on the import type.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub NameBasicsBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles NameBasicsBackgroundWorker.DoWork

        If TitleAkasBackgroundWorker.CancellationPending OrElse
           TitleBasicsBackgroundWorker.CancellationPending OrElse
           TitleCrewBackgroundWorker.CancellationPending OrElse
           TitleEpisodeBackgroundWorker.CancellationPending OrElse
           TitlePrincipalsBackgroundWorker.CancellationPending OrElse
           TitleRatingsBackgroundWorker.CancellationPending Then
            CancelledOperations = True

            NameBasicsBackgroundWorker.CancelAsync()
        End If

        Dim localFT As FT = FT.NameBasics
        Dim rowCount As Long = 0

        Select Case ImportType
            Case IT.Compressed
                rowCount = CountCompressedFileRows(FolderLocation, C.NameBasicsCompressedFileName)

            Case IT.Decompressed
                rowCount = CountFileRows(FolderLocation, C.NameBasicsDecompressedFileName)

        End Select

        With MyRawFileInfo(localFT)
            .CountedRows = rowCount

            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the TitleAkasBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests from other background workers and
    ''' counts the rows in the TitleAkas data file based on the import type.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param> 
    Private Sub TitleAkasBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles TitleAkasBackgroundWorker.DoWork

        If NameBasicsBackgroundWorker.CancellationPending OrElse
           TitleBasicsBackgroundWorker.CancellationPending OrElse
           TitleCrewBackgroundWorker.CancellationPending OrElse
           TitleEpisodeBackgroundWorker.CancellationPending OrElse
           TitlePrincipalsBackgroundWorker.CancellationPending OrElse
           TitleRatingsBackgroundWorker.CancellationPending Then
            CancelledOperations = True

            TitleAkasBackgroundWorker.CancelAsync()
        End If

        Dim localFT As FT = FT.TitleAkas
        Dim rowCount As Long = 0

        Select Case ImportType
            Case IT.Compressed
                rowCount = CountCompressedFileRows(FolderLocation, C.TitleAkasCompressedFileName)

            Case IT.Decompressed
                rowCount = CountFileRows(FolderLocation, C.TitleAkasDecompressedFileName)

        End Select

        With MyRawFileInfo(localFT)
            .CountedRows = rowCount

            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the TitleBasicsBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests from other background workers and
    ''' counts the rows in the TitleBasics data file based on the import type.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub TitleBasicsBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles TitleBasicsBackgroundWorker.DoWork

        If NameBasicsBackgroundWorker.CancellationPending OrElse
           TitleAkasBackgroundWorker.CancellationPending OrElse
           TitleCrewBackgroundWorker.CancellationPending OrElse
           TitleEpisodeBackgroundWorker.CancellationPending OrElse
           TitlePrincipalsBackgroundWorker.CancellationPending OrElse
           TitleRatingsBackgroundWorker.CancellationPending Then
            CancelledOperations = True

            TitleBasicsBackgroundWorker.CancelAsync()
        End If

        Dim localFT As FT = FT.TitleBasics
        Dim rowCount As Long = 0

        Select Case ImportType
            Case IT.Compressed
                rowCount = CountCompressedFileRows(FolderLocation, C.TitleBasicsCompressedFileName)

            Case IT.Decompressed
                rowCount = CountFileRows(FolderLocation, C.TitleBasicsDecompressedFileName)

        End Select

        With MyRawFileInfo(localFT)
            .CountedRows = rowCount

            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the TitleCrewBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests from other background workers and
    ''' counts the rows in the TitleCrew data file based on the import type.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub TitleCrewBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles TitleCrewBackgroundWorker.DoWork

        If NameBasicsBackgroundWorker.CancellationPending OrElse
           TitleAkasBackgroundWorker.CancellationPending OrElse
           TitleBasicsBackgroundWorker.CancellationPending OrElse
           TitleEpisodeBackgroundWorker.CancellationPending OrElse
           TitlePrincipalsBackgroundWorker.CancellationPending OrElse
           TitleRatingsBackgroundWorker.CancellationPending Then
            CancelledOperations = True

            TitleCrewBackgroundWorker.CancelAsync()
        End If

        Dim localFT As FT = FT.TitleCrew
        Dim rowCount As Long = 0

        Select Case ImportType
            Case IT.Compressed
                rowCount = CountCompressedFileRows(FolderLocation, C.TitleCrewCompressedFileName)

            Case IT.Decompressed
                rowCount = CountFileRows(FolderLocation, C.TitleCrewDecompressedFileName)

        End Select

        With MyRawFileInfo(localFT)
            .CountedRows = rowCount

            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the TitleEpisodeBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests from other background workers and
    ''' counts the rows in the TitleEpisode data file based on the import type.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub TitleEpisodeBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles TitleEpisodeBackgroundWorker.DoWork

        If NameBasicsBackgroundWorker.CancellationPending OrElse
           TitleAkasBackgroundWorker.CancellationPending OrElse
           TitleBasicsBackgroundWorker.CancellationPending OrElse
           TitleCrewBackgroundWorker.CancellationPending OrElse
           TitlePrincipalsBackgroundWorker.CancellationPending OrElse
           TitleRatingsBackgroundWorker.CancellationPending Then
            CancelledOperations = True

            TitleEpisodeBackgroundWorker.CancelAsync()
        End If

        Dim localFT As FT = FT.TitleEpisode
        Dim rowCount As Long = 0

        Select Case ImportType
            Case IT.Compressed
                rowCount = CountCompressedFileRows(FolderLocation, C.TitleEpisodeCompressedFileName)

            Case IT.Decompressed
                rowCount = CountFileRows(FolderLocation, C.TitleEpisodeDecompressedFileName)

        End Select

        With MyRawFileInfo(localFT)
            .CountedRows = rowCount

            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the TitlePrincipalsBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests from other background workers and
    ''' counts the rows in the TitlePrincipals data file based on the import type.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub TitlePrincipalsBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles TitlePrincipalsBackgroundWorker.DoWork

        If NameBasicsBackgroundWorker.CancellationPending OrElse
           TitleAkasBackgroundWorker.CancellationPending OrElse
           TitleBasicsBackgroundWorker.CancellationPending OrElse
           TitleCrewBackgroundWorker.CancellationPending OrElse
           TitleEpisodeBackgroundWorker.CancellationPending OrElse
           TitleRatingsBackgroundWorker.CancellationPending Then
            CancelledOperations = True

            TitlePrincipalsBackgroundWorker.CancelAsync()
        End If

        Dim localFT As FT = FT.TitlePrincipals
        Dim rowCount As Long = 0

        Select Case ImportType
            Case IT.Compressed
                rowCount = CountCompressedFileRows(FolderLocation, C.TitlePrincipalsCompressedFileName)

            Case IT.Decompressed
                rowCount = CountFileRows(FolderLocation, C.TitlePrincipalsDecompressedFileName)

        End Select

        With MyRawFileInfo(localFT)
            .CountedRows = rowCount

            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the TitleRatingsBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests from other background workers and
    ''' counts the rows in the TitleRatings data file based on the import type.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub TitleRatingsBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles TitleRatingsBackgroundWorker.DoWork

        If NameBasicsBackgroundWorker.CancellationPending OrElse
           TitleAkasBackgroundWorker.CancellationPending OrElse
           TitleBasicsBackgroundWorker.CancellationPending OrElse
           TitleCrewBackgroundWorker.CancellationPending OrElse
           TitleEpisodeBackgroundWorker.CancellationPending OrElse
           TitlePrincipalsBackgroundWorker.CancellationPending Then
            CancelledOperations = True

            TitleRatingsBackgroundWorker.CancelAsync()
        End If

        Dim localFT As FT = FT.TitleRatings
        Dim rowCount As Long = 0

        Select Case ImportType
            Case IT.Compressed
                rowCount = CountCompressedFileRows(FolderLocation, C.TitleRatingsCompressedFileName)

            Case IT.Decompressed
                rowCount = CountFileRows(FolderLocation, C.TitleRatingsDecompressedFileName)

        End Select

        With MyRawFileInfo(localFT)
            .CountedRows = rowCount

            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the AllArchivesBackgroundWorker.
    ''' This event is triggered when the background worker starts its operation.
    ''' It checks for cancellation requests and processes each file in the CountFilesList.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub AllArchivesBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles AllArchivesSequentialBackgroundWorker.DoWork

        Dim fileNumber As Integer = 0
        Dim localRowCount As Long = 0
        Dim fileCounted As Boolean = False
        Dim localFileType As FT = FT.Unknown

        For Each fileToProcess As String In CountFilesList
            fileNumber += 1

            If AllArchivesSequentialBackgroundWorker.CancellationPending Then
                CancelledOperations = True

                Exit For
            End If

            If fileToProcess.StartsWith(FolderLocation) Then
                fileToProcess = Path.GetFileName(fileToProcess)
            End If

            localFileType = GetFileTypeBasedOnFileName(fileToProcess)

            MyRawFileInfo(localFileType).ToBeCounted = True

            If File.Exists(Path.Combine(FolderLocation, fileToProcess)) Then

                Select Case ImportType

                    Case IT.Compressed
                        localRowCount = CountCompressedFileRows(FolderLocation, fileToProcess)

                    Case IT.Decompressed
                        localRowCount = CountFileRows(FolderLocation, fileToProcess)

                End Select

                With MyRawFileInfo(localFileType)
                    If .ToBeCounted Then
                        .IsBeingCounted = False
                        .HasBeenCounted = True

                        .CountedRows = localRowCount
                    End If

                    fileCounted = True
                End With

                ' update the log textbox with the row count for the current file, 
                ' as we want to show the user that we're making progress on counting 
                ' the rows in the files, rather than waiting until all the files are 
                ' counted and then updating the UI with that info

                If fileCounted Then
                    Dim rowCountMessage As String =
                        "Row count for " & fileToProcess &
                        ": " & localRowCount.ToString(C.COMMA_MASK) &
                        Environment.NewLine
                    ' 
                    If fileNumber = 1 Then
                        TS.AppendText(ProgressLogTextBox,
                                      Environment.NewLine &
                                      Environment.NewLine)
                    End If

                    TS.AppendText(ProgressLogTextBox,
                                  rowCountMessage)
                End If
            End If
        Next

    End Sub

    ''' <summary>
    ''' Handles the DoWork event for the SqlBackgroundWorker. This event 
    ''' is triggered when the background worker starts its operation. It 
    ''' processes each file in the InsertDataFilesList, reads the data 
    ''' line by line, and inserts it into the corresponding SQL Server 
    ''' table. The method also handles cancellation requests and updates 
    ''' the UI with progress information.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A DoWorkEventArgs that contains the event data.</param>
    Private Sub SqlBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) _
        Handles SqlBackgroundWorker.DoWork

        ' refer to the backgroundworker by its name: SqlBackgroundWorker

        Dim rowNumber As Integer = 0
        Dim dataRowNumber As Integer = 0

        TS.SetText(ProgressLogTextBox, String.Empty)
        TS.SetText(CurrentImportFileNumberTextBox, String.Empty)

        Dim rawFileType As FT = FT.Unknown

        Dim lastRowCount As Long = 0
        Dim countedRows As Long = 0

        TS.SetMinimum(ImportArchiveFileProgressBar, 0)
        TS.SetMaximum(ImportArchiveFileProgressBar, 100)

        With MyRawFileInfo(FT.OVERALL)
            .CurrentStartTime = Now
            .CurrentTime = .CurrentStartTime
        End With

        Dim currentTimeBetweenTransactions As TimeSpan = Nothing
        Dim previousTimeBetweenTransactions As TimeSpan = Nothing
        Dim currentTransactionTime As DateTime = Date.MinValue
        Dim previousTransactionTime As DateTime = Date.MinValue
        Dim myStreamReader As StreamReader = Nothing
        Dim fileInfo As FileInfo = Nothing ' New FileInfo(filePath)
        Dim gzipFileStream As FileStream = Nothing
        Dim decompressedFileStream As FileStream = Nothing
        Dim decompressionStream As Comp.GZipStream = Nothing
        Dim columnNamesList As New List(Of String)

        Dim curentFileNumber As Integer = 1
        Dim maxFileNumber As Integer = InsertDataFilesList.Count

        Using conn As New SqlConnection(C.IMDB_CONNECTION_STRING)

            conn.Open()

            ' break up the files into the file TYPES based on the 7 files...
            For Each fileToProcess As String In InsertDataFilesList
                If SqlBackgroundWorker.CancellationPending Then
                    CancelledOperations = True

                    Exit For
                End If

                TS.SetValue(ImportArchiveFileProgressBar, 0)

                If fileToProcess.StartsWith(FolderLocation) Then
                    fileToProcess = Path.GetFileName(fileToProcess)
                End If

                rawFileType = GetFileTypeBasedOnFileName(fileToProcess)

                TS.SetText(OverallEstimatedProcessingTimeTextBox,
                           MyRawFileInfo(FT.OVERALL).EstimatedTotalTimeString)

                With MyRawFileInfo(rawFileType)
                    lastRowCount = .LastRowCount
                    countedRows = .CountedRows

                    If .HasBeenCounted Then
                        TS.SetText(EstimatedOrCountedRowsTextBox, .CountedRows.ToString(C.COMMA_MASK))
                        TS.SetText(PreviousCountOrCountedHeaderLabel, "Total Row Count")

                    Else
                        TS.SetText(EstimatedOrCountedRowsTextBox, .LastRowCount.ToString(C.COMMA_MASK))
                        TS.SetText(PreviousCountOrCountedHeaderLabel, "Previous Total Row Count")

                    End If

                    TS.SetText(FileEstimatedProcessingTimeTextBox, .EstimatedTotalTimeString)

                    .CurrentStartTime = Now
                    .CurrentTime = .CurrentStartTime
                End With

                CurrentlyUploadingFilename =
                    Path.Combine(FolderLocation, fileToProcess)

                CurrentUploadFilenameAndRowCount =
                    "Processing file: " & CurrentlyUploadingFilename

                TS.SetText(ProgressLogTextBox,
                           CurrentUploadFilenameAndRowCount & Environment.NewLine)

                ' Clear the column names list for each file, as each file 
                ' has different column names, and we want to ensure we're 
                ' only working with the column names for the current file 
                ' as we create the tables in SQL Server and insert the data 
                ' into those tables.

                columnNamesList.Clear()

                TS.SetText(CurrentImportFileTextBox,
                           fileToProcess)

                TS.SetText(CurrentImportFileNumberTextBox,
                           $"{curentFileNumber} of {maxFileNumber}")

                fileInfo = New FileInfo(Path.Combine(FolderLocation, fileToProcess))

                Select Case ProcessFileType
                    Case PFT.Compressed
                        gzipFileStream = File.OpenRead(fileInfo.FullName)

                        decompressionStream = New GZipStream(gzipFileStream,
                                                             CompressionMode.Decompress)

                        myStreamReader = New StreamReader(decompressionStream)

                    Case PFT.Decompressed
                        decompressedFileStream = File.OpenRead(fileInfo.FullName)

                        myStreamReader = New StreamReader(decompressedFileStream)

                End Select

                Dim cmd As SqlCommand =
                    conn.CreateCommand()

                cmd.CommandTimeout = 120

                Dim transaction As SqlTransaction = conn.BeginTransaction()

                Dim cmdResult As Integer = 0

                cmd.Connection = conn
                cmd.Transaction = transaction

                Try
                    If SqlBackgroundWorker.CancellationPending Then
                        CancelledOperations = True

                        Exit Try
                    End If

                    Dim line As String = Nothing

                    Dim localTableFromFileName As String =
                        "[IMDB].[Raw].[" & fileInfo.Name

                    Select Case ProcessFileType
                        Case PFT.Compressed : localTableFromFileName &= "]"
                        Case PFT.Decompressed : localTableFromFileName &= ".gz]"
                    End Select

                    cmd.CommandText = "TRUNCATE TABLE " & localTableFromFileName & ";"
                    cmd.ExecuteNonQuery()

                    Dim insertCommandText As String = String.Empty
                    Dim insertValuesList As New List(Of String)

                    ' Read line by line from the files 

                    '   For Compressed Files (originally downloaded): *.tsv.gz 
                    '       or 
                    '   For Decompressed Files: *.tsv

                    rowNumber = 0
                    dataRowNumber = 0

                    line = myStreamReader.ReadLine()

                    Do
                        If String.IsNullOrEmpty(line) Then Exit Do

                        rowNumber += 1

                        ' get the column names from the first Row read from the file
                        If rowNumber = 1 Then
                            insertValuesList.Clear()

                            ' pull the column names from the first line of the file, 
                            ' and create a table in SQL Server with those column names, 
                            ' if it doesn't already exist
                            Dim columnNames As String() = line.Split(vbTab)

                            For Each columnName As String In columnNames
                                Select Case columnName
                                    Case "tconst", "[tconst]" : columnNamesList.Add("[TitleId]")
                                    Case "parentTconst", "[parentTconst]" : columnNamesList.Add("[ParentTitleId]")
                                    Case "nconst", "[nconst]" : columnNamesList.Add("[NameId]")
                                    Case Else : columnNamesList.Add("[" & columnName.Replace("[", "").Replace("]", "") & "]")
                                End Select
                            Next

                            insertCommandText = " INSERT INTO " & localTableFromFileName & " " & Environment.NewLine &
                                                "     ( " & String.Join(", ", columnNamesList.Select(Function(c) c)) & " )" & Environment.NewLine &
                                                " VALUES " & Environment.NewLine &
                                                "     ( YYYY );"
                        Else
                            dataRowNumber += 1

                            ' Parse the line and insert it into SQL Server table
                            If Not String.IsNullOrWhiteSpace(line.Replace("\N", "").Replace(vbTab, "")) Then

                                ' break the line into columns based on the tab delimiter, 
                                ' and then insert the columns into the table, replacing 
                                ' any \N with NULL and any ' with '' to escape them in SQL Server

                                Dim valuesList As New List(Of String)()
                                Dim valueIndex As Integer = 0

                                For Each value As String In line.Split(vbTab)
                                    If value = "\N" Then
                                        valuesList.Add("NULL")
                                    Else
                                        valuesList.Add("N'" & value.Replace("'", "''") & "'")
                                    End If

                                    valueIndex += 1
                                Next

                                Try
                                    cmd.CommandText =
                                        insertCommandText.Replace("YYYY", String.Join(", ", valuesList))

                                    cmdResult = cmd.ExecuteNonQuery()

                                    TS.SetText(CurrentRowNumberTextBox,
                                               rowNumber.ToString(C.COMMA_MASK))

                                Catch ex As Exception
                                    If Not conn.State = ConnectionState.Open Then
                                        Throw
                                    End If

                                    LogErrorsToFile("Row Number: " & rowNumber.ToString(C.COMMA_MASK))
                                    LogErrorsToFile(cmd.CommandText)
                                    LogErrorsToFile("cmdResult = " & cmdResult.ToString())

                                    TS.AppendText(ProgressLogTextBox,
                                                  "Data File Row Number: " & rowNumber.ToString(C.COMMA_MASK) & Environment.NewLine &
                                                  " Error inserting row: " & Environment.NewLine &
                                                  ex.Message & Environment.NewLine)
                                End Try
                            End If
                        End If

                        ' calculate the elapsed time and estimated time remaining for the current file being processed
                        ' display the information in the ElapsedTimeForFileTextBox and FileEstimatedTimeRemainingTextBox textboxes, respectively

                        ' calculate the elapsed time and estimated time remaining for the overall processing of all files
                        ' display the information in the OverallElapsedTimeTextBox and OverallEstimatedTimeRemainingTextBox textboxes, respectively

                        ' For every 10,000 rows: 
                        '   1) commit the transaction 
                        '   2) start a new transaction. 

                        ' This effectively batches the log writes, which improves the overall INSERT performance.
                        If ((dataRowNumber Mod 10000) = 0) AndAlso
                            (dataRowNumber > 0) Then
                            If SqlBackgroundWorker.CancellationPending Then
                                Exit Do
                            End If

                            If countedRows > 0 Then
                                CurrentUploadFilenameAndRowCount =
                                    Environment.NewLine &
                                    "Processing file: " & CurrentlyUploadingFilename & vbTab &
                                    "Rows Committed to Database: " & dataRowNumber.ToString(C.COMMA_MASK) & vbTab &
                                    "of " & countedRows.ToString(C.COMMA_MASK) &
                                    " rows"

                            Else
                                CurrentUploadFilenameAndRowCount =
                                    Environment.NewLine &
                                    "Processing file: " & CurrentlyUploadingFilename & vbTab &
                                    "Rows Committed to Database: " & dataRowNumber.ToString(C.COMMA_MASK) & vbTab &
                                    "of approximately " & lastRowCount.ToString(C.COMMA_MASK) &
                                    " rows"
                            End If

                            previousTransactionTime = currentTransactionTime
                            currentTransactionTime = Now

                            If previousTransactionTime <> Date.MinValue AndAlso
                               currentTransactionTime <> Date.MinValue Then
                                previousTimeBetweenTransactions = currentTimeBetweenTransactions
                                currentTimeBetweenTransactions = currentTransactionTime - previousTransactionTime

                            End If

                            Debug.Print(Now.ToLongTimeString() & vbTab &
                                        CurrentUploadFilenameAndRowCount)

                            If Not String.IsNullOrEmpty(CompiledPreviouslyUploadedFilenamesAndRowCounts) Then
                                AllPreviouslyUploadedFilenamesAndRowCountsPlusCurrent =
                                    CompiledPreviouslyUploadedFilenamesAndRowCounts & Environment.NewLine &
                                    CurrentUploadFilenameAndRowCount

                            Else
                                AllPreviouslyUploadedFilenamesAndRowCountsPlusCurrent =
                                    CurrentUploadFilenameAndRowCount

                            End If

                            ' perhaps update a progress bar here or something to show the progress of the file loading
                            TS.SetText(ProgressLogTextBox,
                                       AllPreviouslyUploadedFilenamesAndRowCountsPlusCurrent)

                            '   1 commit the transaction 
                            transaction.Commit()
                            transaction = conn.BeginTransaction()

                            '   2 start a new transaction. 
                            cmd.Connection = conn
                            cmd.Transaction = transaction

                            ' continuously update the number of seconds between each database transaction commit, 
                            ' and the estimated number of seconds remaining to process the current file, based on 
                            ' the average time per transaction commit and the number of transactions remaining for 
                            ' the current file and estimated number of transactions remaining for all files.

                            ' if  the row count = 10000, then this is the first transaction for the file
                            ' otherwise, we can compare against the previous transaction time

                            previousTimeBetweenTransactions = currentTimeBetweenTransactions
                            previousTransactionTime = currentTransactionTime

                            'MyRawFileInfo(rawFileType).CurrentTime = Now
                            MyRawFileInfo(FT.OVERALL).CurrentTime = currentTransactionTime

                            With MyRawFileInfo(rawFileType)
                                .CurrentTime = currentTransactionTime

                                ' the first commit will be the time between the current transaction time and the start time for the file,
                                ' and subsequent commits will be the time between the current transaction time and the previous transaction time
                                If dataRowNumber <= 10000 Then
                                    currentTimeBetweenTransactions =
                                        (currentTransactionTime - .CurrentStartTime)

                                Else
                                    currentTimeBetweenTransactions =
                                        (currentTransactionTime - previousTransactionTime)

                                End If

                                .CurrentRowNumber = dataRowNumber

                                TS.SetValue(ImportArchiveFileProgressBar, .ProgressCompleted)
                            End With

                            ' not sure about this one, but it should be close enough for the overall time remaining estimate
                            MyRawFileInfo(FT.OVERALL).CurrentRowNumber += 10000

                            ' update the textboxes with the elapsed time and 
                            ' estimated time remaining for the current file 
                            ' and overall processing of all files
                            TS.SetText(FileEstimatedTimeRemainingTextBox,
                                       MyRawFileInfo(rawFileType).EstimatedRemainingTimeString)

                            TS.SetText(ElapsedTimeForFileTextBox,
                                       MyRawFileInfo(rawFileType).ElapsedTimeString)


                            TS.SetText(OverallEstimatedTimeRemainingTextBox,
                                       MyRawFileInfo(FT.OVERALL).EstimatedRemainingTimeString)

                            TS.SetText(OverallElapsedTimeTextBox,
                                       MyRawFileInfo(FT.OVERALL).ElapsedTimeString)
                        End If

                        line = myStreamReader.ReadLine()

                    Loop While Not String.IsNullOrEmpty(line)

                    If SqlBackgroundWorker.CancellationPending Then
                        Exit Try
                    End If

                    transaction.Commit()

                    CurrentUploadFilenameAndRowCount =
                        "Completed Processing file: " & fileToProcess & vbTab &
                        "Total # of Rows Committed to Database: " & dataRowNumber.ToString(C.COMMA_MASK)

                    Debug.Print(Now.ToLongTimeString() & vbTab &
                                CurrentUploadFilenameAndRowCount)

                    AllPreviouslyUploadedFilenamesAndRowCountsPlusCurrent =
                        CompiledPreviouslyUploadedFilenamesAndRowCounts & Environment.NewLine &
                        Environment.NewLine &
                        CurrentUploadFilenameAndRowCount

                    ' On the very first file, I don't need the new line here
                    If curentFileNumber > 1 Then
                        CompiledPreviouslyUploadedFilenamesAndRowCounts &= Environment.NewLine
                    End If

                    CompiledPreviouslyUploadedFilenamesAndRowCounts &= CurrentUploadFilenameAndRowCount

                    TS.SetText(ProgressLogTextBox,
                               AllPreviouslyUploadedFilenamesAndRowCountsPlusCurrent)

                    MyRawFileInfo(rawFileType).CurrentRowNumber = dataRowNumber

                Catch ex As Exception
                    LogErrorsToFile($"Error processing file: {fileToProcess}")
                    LogErrorsToFile($"Command: {cmd.CommandText}")
                    LogErrorsToFile($"Exception: {ex.ToString()}")

                    TS.AppendText(ProgressLogTextBox, $"Error processing file: {fileToProcess}" & Environment.NewLine)
                    TS.AppendText(ProgressLogTextBox, $"Command: {cmd.CommandText}" & Environment.NewLine)
                    TS.AppendText(ProgressLogTextBox, $"Exception: {ex.ToString()}" & Environment.NewLine)

                Finally
                    myStreamReader.Close()

                    Select Case ProcessFileType
                        Case PFT.Compressed
                            decompressionStream.Close()
                            gzipFileStream.Close()

                        Case PFT.Decompressed
                            decompressedFileStream.Close()

                    End Select

                End Try

                If SqlBackgroundWorker.CancellationPending Then
                    Exit For
                End If

                MyRawFileInfo(rawFileType).CurrentEndTime = Now

                curentFileNumber += 1
            Next

            MyRawFileInfo(FT.OVERALL).CurrentEndTime = Now
        End Using

        ' calculate the TimeSpan values and save them to the Settings?

    End Sub
#End Region

    ''' <summary>
    ''' Logs error messages to a file named "error_log.txt" in the 
    ''' application's base directory. Each log entry includes a 
    ''' timestamp and the provided error message. If an exception 
    ''' occurs while attempting to write to the log file, it is 
    ''' caught and printed to the debug output.
    ''' </summary>
    ''' <param name="errorMessage">The error message to log.</param>
    Public Sub LogErrorsToFile(errorMessage As String)
        Try
            Dim logPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt")
            Dim appendToFile As Boolean = True

            Using writer As New StreamWriter(logPath, appendToFile)
                writer.WriteLine($"{DateTime.Now}: {errorMessage}")
            End Using

        Catch ex As Exception
            ' Handle any exceptions that occur while trying to log the error.
            Debug.Print($"Failed to log error to file: {ex.Message}")

        End Try
    End Sub

#Region "BackgroundWorker Objects RunWorkerCompleted Event Handlers"
    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the NameBasicsBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It sets the NameBasicsCounted flag to True and calls CheckAllCounted() to verify 
    ''' if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub NameBasicsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles NameBasicsBackgroundWorker.RunWorkerCompleted

        MyRawFileInfo(FT.NameBasics).HasBeenCounted = True

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the TitleAkasBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It sets the TitleAkasCounted flag to True and calls CheckAllCounted() to verify 
    ''' if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub TitleAkasBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleAkasBackgroundWorker.RunWorkerCompleted

        With MyRawFileInfo(FT.TitleAkas)
            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the TitleBasicsBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It sets the TitleBasicsCounted flag to True and calls CheckAllCounted() to verify 
    ''' if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub TitleBasicsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleBasicsBackgroundWorker.RunWorkerCompleted

        With MyRawFileInfo(FT.TitleBasics)
            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the TitleCrewBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It sets the TitleCrewCounted flag to True and calls CheckAllCounted() to verify 
    ''' if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub TitleCrewBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleCrewBackgroundWorker.RunWorkerCompleted

        With MyRawFileInfo(FT.TitleCrew)
            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the TitleEpisodeBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It sets the TitleEpisodeCounted flag to True and calls CheckAllCounted() to verify 
    ''' if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub TitleEpisodeBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleEpisodeBackgroundWorker.RunWorkerCompleted

        MyRawFileInfo(FT.TitleEpisode).HasBeenCounted = True

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the TitlePrincipalsBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It sets the TitlePrincipalsCounted flag to True and calls CheckAllCounted() to verify 
    ''' if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub TitlePrincipalsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitlePrincipalsBackgroundWorker.RunWorkerCompleted

        With MyRawFileInfo(FT.TitlePrincipals)
            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the TitleRatingsBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It sets the TitleRatingsCounted flag to True and calls CheckAllCounted() to verify 
    ''' if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub TitleRatingsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleRatingsBackgroundWorker.RunWorkerCompleted

        With MyRawFileInfo(FT.TitleRatings)
            .IsBeingCounted = False
            .HasBeenCounted = True
        End With

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the AllArchivesBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It calls CheckAllCounted() to verify if all counting operations are complete.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub AllArchivesBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles AllArchivesSequentialBackgroundWorker.RunWorkerCompleted

        Select Case Me.ChooseAllOrSelected

            Case CAS.AllFiles
                CheckAllCounted()

            Case CAS.SelectedFiles
                CheckSelectedCounted(CountFilesList)

        End Select

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event for the SqlBackgroundWorker. 
    ''' This event is triggered when the background worker has completed its operation. 
    ''' It updates the UI elements to reflect the completion of the SQL operations.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The RunWorkerCompletedEventArgs instance containing the event data.</param>
    Private Sub SqlBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles SqlBackgroundWorker.RunWorkerCompleted

        ' refer to the backgroundworker by its name: SqlBackgroundWorker
        EndThingsButton.Text = "E&xit"

        ImportDataButton.Enabled = True

        CountArchiveRowsButton.Enabled = CountArchiveRowsButtonEnabled
        CountTsvRowsButton.Enabled = CountTsvRowsButtonEnabled

        DecompressAfterDownloadCheckBox.Enabled = True

        DownloadUpdatedArchivesButton.Enabled = True
        LoadAllDataFilesButton.Enabled = True
        ChooseFolderButton.Enabled = True
        FolderLocationTextBox.Enabled = True

        Me.AcceptButton = LoadAllDataFilesButton
        Me.CancelButton = EndThingsButton

    End Sub

    ''' <summary>
    ''' Counts the number of rows in a specified table in the database. 
    ''' It executes a SQL command to retrieve the row count and handles 
    ''' potential SQL exceptions, including timeouts, with retry logic. 
    ''' The function updates the approximate row count for the specified 
    ''' table and logs progress messages to a progress log text box.
    ''' </summary>
    ''' <param name="currentTable">The table for which to count rows.</param>
    ''' <param name="sqlConn">The SQL connection to use for the operation.</param>
    ''' <param name="sqlCmd">The SQL command to execute.</param>
    ''' <param name="lastTable">The last table in the sequence, used for logging purposes.</param>
    ''' <param name="timeOutForExecution">The timeout value for the SQL command execution.</param>
    ''' <returns>True if the operation was successful; otherwise, False.</returns>
    Private Function CountTable(currentTable As AH25,
                          ByRef sqlConn As SqlConnection,
                          ByRef sqlCmd As SqlCommand,
                       Optional lastTable As AH25 = AH25.TitleTypes,
                       Optional timeOutForExecution As Integer = C.DEFAULT_TIMEOUT) As Boolean

        Dim rowCount As Long = 0
        Dim commandText As String = C.AdHoc5List(currentTable)

        Dim success As Boolean = True

        Dim retryAfterCommandTimeOut As Boolean = True
        Dim retryCount As Integer = 0
        Dim maxRetryAttempts As Integer = 3

        Dim stepLogMsg As String =
            "Step " & CInt(currentTable).ToString() &
            " of " & CInt(lastTable).ToString()

        Dim approximateRowCount As Long = C.ApproxRows5(currentTable)

        Dim logMessageHeader As String =
            C.EQUALSIGNS & Environment.NewLine &
            C.BASIC_LOG_MESSAGE_5 & Environment.NewLine &
            C.EQUALSIGNS & Environment.NewLine

        With sqlCmd
            .CommandTimeout = timeOutForExecution

            Do While retryAfterCommandTimeOut
                Try
                    .CommandText = commandText

                    TS.SetText(ProgressLogTextBox,
                               logMessageHeader &
                               "Executing T-SQL: " & stepLogMsg & Environment.NewLine &
                               C.DASHES & Environment.NewLine &
                               .CommandText & Environment.NewLine & Environment.NewLine &
                               "Expecting approximately: " & approximateRowCount.ToString(C.COMMA_MASK) & " Row(s)")

                    rowCount = Convert.ToInt64(.ExecuteScalar())

                    TS.AppendText(ProgressLogTextBox,
                                  Environment.NewLine &
                                  "                         " &
                                  rowCount.ToString(C.COMMA_MASK) & " Row(s)")

                    C.ApproxRows5(currentTable) = rowCount

                    Thread.Sleep(4000)

                    Exit Do

                Catch sqlEx As SqlException
                    Debug.Print(sqlEx.ToString)

                    LogErrorsToFile($"SQL Exception: {sqlEx.ToString()}")
                    LogErrorsToFile(.CommandText)

                    If sqlEx.Message.StartsWith("Execution Timeout Expired") Then
                        retryCount += 1

                        If retryCount >= maxRetryAttempts Then
                            retryAfterCommandTimeOut = False
                        End If

                        timeOutForExecution += (C.DEFAULT_TIMEOUT * 3)

                        .CommandTimeout = timeOutForExecution

                    Else
                        retryAfterCommandTimeOut = False
                        success = False
                        Throw

                        Exit Do
                    End If

                    Exit Try

                Catch ex As Exception
                    retryAfterCommandTimeOut = False
                    success = False

                    LogErrorsToFile($"Exception: {ex.ToString()}")

                    Throw

                    Exit Do

                End Try

            Loop
        End With

        Return success

    End Function

    ''' <summary>
    ''' Truncates the specified table in the database. It executes a 
    ''' SQL command to remove all rows from the table and handles 
    ''' potential SQL exceptions, including timeouts, with retry 
    ''' logic. The function logs progress messages to a progress 
    ''' log text box and returns a boolean indicating the success 
    ''' of the operation.
    ''' </summary>
    ''' <param name="currentTable">The table to truncate.</param>
    ''' <param name="sqlConn">The SQL connection object.</param>
    ''' <param name="sqlCmd">The SQL command object.</param>
    ''' <param name="lastTable">The last table in the process.</param>
    ''' <param name="timeOutForExecution">The timeout for the SQL command execution.</param>
    ''' <returns>True if the operation was successful, otherwise False.</returns>
    Private Function TruncateTable(currentTable As AH25,
                             ByRef sqlConn As SqlConnection,
                             ByRef sqlCmd As SqlCommand,
                          Optional lastTable As AH25 = AH25.TitleTypes,
                          Optional timeOutForExecution As Integer = C.DEFAULT_TIMEOUT) As Boolean

        ' This function handles the following steps:
        ' #2-01:  IMDB - #2-01 - TRUNCATE TABLE [IMDB].[dbo].[Attributes]
        ' #2-02:  IMDB - #2-02 - TRUNCATE TABLE [IMDB].[dbo].[Episodes]
        ' #2-03:  IMDB - #2-03 - TRUNCATE TABLE [IMDB].[dbo].[Genres]
        ' #2-04:  IMDB - #2-04 - TRUNCATE TABLE [IMDB].[dbo].[PrimaryProfessions]
        ' #2-05:  IMDB - #2-05 - TRUNCATE TABLE [IMDB].[dbo].[Principals]
        ' #2-06:  IMDB - #2-06 - TRUNCATE TABLE [IMDB].[dbo].[Professions]
        ' #2-07:  IMDB - #2-07 - TRUNCATE TABLE [IMDB].[dbo].[TitleCharacters]
        ' #2-08:  IMDB - #2-08 - TRUNCATE TABLE [IMDB].[dbo].[TitleGenres]
        ' #2-09:  IMDB - #2-09 - TRUNCATE TABLE [IMDB].[dbo].[TitleNameAttributes]
        ' #2-10:  IMDB - #2-10 - TRUNCATE TABLE [IMDB].[dbo].[TitleNames]
        ' #2-11:  IMDB - #2-11 - TRUNCATE TABLE [IMDB].[dbo].[TitlePrincipals]
        ' #2-12:  IMDB - #2-12 - TRUNCATE TABLE [IMDB].[dbo].[Titles]
        ' #2-13:  IMDB - #2-13 - TRUNCATE TABLE [IMDB].[dbo].[TitleTypes]

        Dim success As Boolean = True
        Dim retryAfterCommandTimeOut As Boolean = True
        Dim retryCount As Integer = 0
        Dim maxRetryAttempts As Integer = 3

        Dim stepLogMsg As String =
            "Step " & CInt(currentTable).ToString() &
            " of " & CInt(lastTable).ToString()

        Dim commandText As String = C.AdHoc2List(currentTable)

        Dim basicLogMessage =
            C.EQUALSIGNS & Environment.NewLine &
            C.BASIC_LOG_MESSAGE_2 & Environment.NewLine &
            C.EQUALSIGNS & Environment.NewLine

        With sqlCmd
            .CommandTimeout = timeOutForExecution

            Do While retryAfterCommandTimeOut
                Try
                    .CommandText = commandText

                    TS.SetText(ProgressLogTextBox,
                               basicLogMessage &
                               "Executing T-SQL: " & stepLogMsg & Environment.NewLine &
                               C.DASHES & Environment.NewLine &
                               commandText)

                    .ExecuteNonQuery()

                    TS.AppendText(ProgressLogTextBox,
                                  Environment.NewLine & Environment.NewLine &
                                  "Action Successful")

                    Thread.Sleep(1000)

                    Exit Do

                Catch sqlEx As SqlException
                    Debug.Print(sqlEx.ToString)

                    LogErrorsToFile($"SQL Exception: {sqlEx.ToString()}")
                    LogErrorsToFile($"Command: {commandText}")

                    ' Retry logic for SQL command timeout
                    If sqlEx.Message.StartsWith("Execution Timeout Expired") Then
                        retryCount += 1

                        If retryCount >= maxRetryAttempts Then
                            retryAfterCommandTimeOut = False
                        End If

                        timeOutForExecution += (C.DEFAULT_TIMEOUT * 3)

                        .CommandTimeout = timeOutForExecution

                    Else
                        retryAfterCommandTimeOut = False
                        success = False

                        Throw

                        Exit Do
                    End If

                    Exit Try

                Catch ex As Exception
                    retryAfterCommandTimeOut = False
                    success = False

                    LogErrorsToFile($"Exception: {ex.ToString()}")

                    Throw

                    Exit Do

                End Try
            Loop

        End With

        Return success

    End Function

    ''' <summary>
    ''' Adds or drops a table constraint in the database based on the specified operation (drop or add). 
    ''' It executes the appropriate SQL command to either drop or add the constraint and handles potential 
    ''' SQL exceptions, including timeouts, with retry logic. The function logs progress messages to a 
    ''' progress log text box and returns a boolean indicating the success of the operation.
    ''' </summary>
    ''' <param name="currentStep">The current step in the process.</param>
    ''' <param name="lastStep">The last step in the process.</param>
    ''' <param name="sqlConn">The SQL connection object.</param>
    ''' <param name="sqlCmd">The SQL command object.</param>
    ''' <param name="dropOrAdd">The operation to perform (drop or add).</param>
    ''' <param name="timeOutForExecution">The timeout for the SQL command execution.</param>
    ''' <returns>True if the operation was successful, otherwise False.</returns>
    Private Function AddOrDropTableConstraint(currentStep As Integer,
                                              lastStep As Integer,
                                        ByRef sqlConn As SqlConnection,
                                        ByRef sqlCmd As SqlCommand,
                                              dropOrAdd As DropAddEnum,
                                     Optional timeOutForExecution As Integer = C.DEFAULT_TIMEOUT) As Boolean

        ' This function takes care of the commands to DROP constraints 
        ' 01 #1-01:  IMDB - #1-01 - DROP PK_Episodes.sql
        ' 02 #1-02:  IMDB - #1-02 - DROP FK_PrimaryProfession_Principal
        ' 03 #1-03:  IMDB - #1-03 - DROP FK_PrimaryProfession_Profession
        ' 04 #1-04:  IMDB - #1-04 - DROP FK_TitleCharacters_Episode
        ' 05 #1-05:  IMDB - #1-05 - DROP FK_TitleCharacters_Parent
        ' 06 #1-06:  IMDB - #1-06 - DROP FK_TitleCharacters_Principal
        ' 07 #1-07:  IMDB - #1-07 - DROP FK_TitleCharacters_Title
        ' 08 #1-08:  IMDB - #1-08 - DROP IX_TitleCharacters
        ' 09 #1-09:  IMDB - #1-09 - DROP FK_TitleGenres_Genre
        ' 10 #1-10:  IMDB - #1-10 - DROP FK_TitleGenres_Title
        ' 11 #1-11:  IMDB - #1-11 - DROP FK_TitleNameAttributes_Attribute
        ' 12 #1-12:  IMDB - #1-12 - DROP FK_TitleNameAttributes_TitleName
        ' 13 #1-13:  IMDB - #1-13 - DROP FK_TitleNames_Title
        ' 14 #1-14:  IMDB - #1-14 - DROP FK_TitlePrincipals_Principal
        ' 15 #1-15:  IMDB - #1-15 - DROP FK_TitlePrincipals_Profession
        ' 16 #1-16:  IMDB - #1-16 - DROP FK_TitlePrincipals_Title
        ' 17 #1-17:  IMDB - #1-17 - DROP FK_Titles_TitleType
        ' 18 #1-18:  IMDB - #1-18 - DROP PK_TitleTypes
        ' 19 #1-19:  IMDB - #1-19 - DROP PK_Attributes
        ' 20 #1-20:  IMDB - #1-20 - DROP UQ_Attributes
        ' 21 #1-21:  IMDB - #1-21 - DROP PK_Genres
        ' 22 #1-22:  IMDB - #1-22 - DROP PK_PrimaryProfession
        ' 23 #1-23:  IMDB - #1-23 - DROP PK_Principals
        ' 24 #1-24:  IMDB - #1-24 - DROP PK_Professions
        ' 25 #1-25:  IMDB - #1-25 - DROP PK_TitleGenres
        ' 26 #1-26:  IMDB - #1-26 - DROP PK_TitleNameAttributes
        ' 27 #1-27:  IMDB - #1-27 - DROP IX_TitleNames_Original
        ' 28 #1-28:  IMDB - #1-28 - DROP PK_TitleNames
        ' 29 #1-29:  IMDB - #1-29 - DROP PK_TitlePrincipals
        ' 30 #1-30:  IMDB - #1-30 - DROP PK_Titles

        ' ... and the commands to ADD the constraints
        '--------------------------------------------------------------------
        ' 01  #3-01:  IMDB - #3-01 - CREATE PK_Principals
        ' 02  #3-02:  IMDB - #3-02 - CREATE PK_Professions
        ' 03  #3-03:  IMDB - #3-03 - CREATE PK_PrimaryProfession
        ' 04  #3-04:  IMDB - #3-04 - CREATE FK_PrimaryProfession_Principal
        ' 05  #3-05:  IMDB - #3-05 - CREATE FK_PrimaryProfession_Principal
        ' 06  #3-06:  IMDB - #3-06 - CREATE FK_PrimaryProfession_Profession
        ' 07  #3-07:  IMDB - #3-07 - CREATE FK_PrimaryProfession_Profession
        ' 08  #3-08:  IMDB - #3-08 - CREATE PK_Genres
        ' 09  #3-09:  IMDB - #3-09 - CREATE PK_TitleTypes
        ' 10  #3-10:  IMDB - #3-10 - CREATE PK_Titles
        ' 11  #3-11:  IMDB - #3-11 - CREATE FK_Titles_TitleType
        ' 12  #3-12:  IMDB - #3-12 - CREATE FK_Titles_TitleType
        ' 13  #3-13:  IMDB - #3-13 - CREATE PK_TitleGenres
        ' 14  #3-14:  IMDB - #3-14 - CREATE FK_TitleGenres_Title
        ' 15  #3-15:  IMDB - #3-15 - CREATE FK_TitleGenres_Title
        ' 16  #3-16:  IMDB - #3-16 - CREATE FK_TitleGenres_Genre
        ' 17  #3-17:  IMDB - #3-17 - CREATE FK_TitleGenres_Genre
        ' 18  #3-18:  IMDB - #3-18 - CREATE PK_TitleNames
        ' 19  #3-19:  IMDB - #3-19 - CREATE FK_TitleNames_Title
        ' 20  #3-20:  IMDB - #3-20 - CREATE FK_TitleNames_Title
        ' 21  #3-21:  IMDB - #3-21 - CREATE IX_TitleNames_Original
        ' 22  #3-22:  IMDB - #3-22 - CREATE PK_Attributes
        ' 23  #3-23:  IMDB - #3-23 - CREATE UQ_Attributes
        ' 24  #3-24:  IMDB - #3-24 - CREATE PK_TitleNameAttributes
        ' 25  #3-25:  IMDB - #3-25 - CREATE FK_TitleNameAttributes_TitleName
        ' 26  #3-26:  IMDB - #3-26 - CREATE FK_TitleNameAttributes_TitleName
        ' 27  #3-27:  IMDB - #3-27 - CREATE FK_TitleNameAttributes_Attribute
        ' 28  #3-28:  IMDB - #3-28 - CREATE FK_TitleNameAttributes_Attribute
        ' 29  #3-29:  IMDB - #3-29 - CREATE PK_TitlePrincipals
        ' 30  #3-30:  IMDB - #3-30 - CREATE FK_TitlePrincipals_Title
        ' 31  #3-31:  IMDB - #3-31 - CREATE FK_TitlePrincipals_Title
        ' 32  #3-32:  IMDB - #3-32 - CREATE FK_TitlePrincipals_Principal
        ' 33  #3-33:  IMDB - #3-33 - CREATE FK_TitlePrincipals_Principal
        ' 34  #3-34:  IMDB - #3-34 - CREATE FK_TitlePrincipals_Profession
        ' 35  #3-35:  IMDB - #3-35 - CREATE FK_TitlePrincipals_Profession
        ' 36  #3-36:  IMDB - #3-36 - CREATE FK_TitleCharacters_Title
        ' 37  #3-37:  IMDB - #3-37 - CREATE FK_TitleCharacters_Title
        ' 38  #3-38:  IMDB - #3-38 - CREATE FK_TitleCharacters_Principal
        ' 39  #3-39:  IMDB - #3-39 - CREATE FK_TitleCharacters_Principal
        ' 40  #3-40:  IMDB - #3-40 - CREATE IX_TitleCharacters
        ' 41  #3-41:  IMDB - #3-41 - CREATE PK_Episodes
        ' 42  #3-42:  IMDB - #3-42 - CREATE FK_TitleCharacters_Parent
        ' 43  #3-43:  IMDB - #3-43 - CREATE FK_TitleCharacters_Parent
        ' 44  #3-44:  IMDB - #3-44 - CREATE FK_TitleCharacters_Episode
        ' 45  #3-45:  IMDB - #3-45 - CREATE FK_TitleCharacters_Episode

        Dim success As Boolean = True

        Dim retryAfterCommandTimeOut As Boolean = True
        Dim retryCount As Integer = 0
        Dim maxRetryAttempts As Integer = 3

        Dim stepLogMsg As String =
            "Step " & currentStep.ToString() &
            " of " & lastStep.ToString()

        Dim basicLogMsg As String = String.Empty
        Dim commandText As String = ""

        Select Case dropOrAdd
            Case DropAddEnum.DROP
                basicLogMsg = C.BASIC_LOG_MESSAGE_1 &
                              Environment.NewLine
                commandText = C.AdHoc1List(currentStep)

            Case DropAddEnum.ADD
                basicLogMsg = C.BASIC_LOG_MESSAGE_3 &
                              Environment.NewLine
                commandText = C.AdHoc3List(currentStep)

        End Select

        basicLogMsg =
            C.EQUALSIGNS & Environment.NewLine &
            basicLogMsg &
            C.EQUALSIGNS & Environment.NewLine

        With sqlCmd
            .CommandTimeout = C.DEFAULT_TIMEOUT

            Do While retryAfterCommandTimeOut
                Try
                    .CommandText = commandText

                    TS.SetText(ProgressLogTextBox,
                               basicLogMsg &
                               "Executing T-SQL: " & stepLogMsg & Environment.NewLine &
                               C.DASHES & Environment.NewLine &
                               commandText)

                    .ExecuteNonQuery()

                    TS.AppendText(ProgressLogTextBox,
                                  Environment.NewLine & Environment.NewLine &
                                  "Action Successful")

                    Thread.Sleep(1000)

                    Exit Do

                Catch sqlEx As SqlException
                    Debug.Print(sqlEx.ToString)

                    LogErrorsToFile($"SQL Exception: {sqlEx.ToString()}")
                    LogErrorsToFile($"Command: {commandText}")

                    If sqlEx.Message.StartsWith("Execution Timeout Expired") Then
                        retryCount += 1

                        If retryCount >= maxRetryAttempts Then
                            retryAfterCommandTimeOut = False
                        End If

                        timeOutForExecution += C.DEFAULT_TIMEOUT

                        .CommandTimeout = timeOutForExecution

                    Else
                        retryAfterCommandTimeOut = False
                        success = False

                        Throw

                        Exit Do
                    End If

                    Exit Try

                Catch ex As Exception
                    retryAfterCommandTimeOut = False
                    success = False

                    LogErrorsToFile($"Exception: {ex.ToString()}")

                    Throw

                    Exit Do

                End Try
            Loop
        End With

        Return success

    End Function

    ''' <summary>
    ''' Inserts or updates data in a specified table in the database. It 
    ''' executes a SQL command to perform the insert or update operation 
    ''' and handles potential SQL exceptions, including timeouts, with 
    ''' retry logic. The function logs progress messages to a progress 
    ''' log text box and returns a boolean indicating the success of 
    ''' the operation.
    ''' </summary>
    ''' <param name="currentStep">The current step in the process.</param>
    ''' <param name="lastStep">The last step in the process.</param>
    ''' <param name="rowsAffected">The number of rows affected by the operation.</param>
    ''' <param name="sqlConn">The SQL connection object.</param>
    ''' <param name="sqlCmd">The SQL command object.</param>
    ''' <returns>True if the operation was successful, otherwise False.</returns>
    Private Function InsertOrUpdateTable(currentStep As Integer,
                                         lastStep As Integer,
                                   ByRef rowsAffected As Long,
                                   ByRef sqlConn As SqlConnection,
                                   ByRef sqlCmd As SqlCommand) As Boolean

        ' This function takes care of all of the import functionality:

        '== #4-01 - INSERT into [IMDB].[dbo].[Principals]
        '== #4-02 - INSERT into [IMDB].[dbo].[Professions]
        '== #4-03 - INSERT into [IMDB].[dbo].[Professions]
        '== #4-04 - INSERT into [IMDB].[dbo].[PrimaryProfessions]
        '== #4-05 - INSERT into [IMDB].[dbo].[Genres]
        '== #4-06 - INSERT into [IMDB].[dbo].[TitleTypes]
        '== #4-07 - INSERT into [IMDB].[dbo].[Titles]
        '== #4-08 - INSERT into [IMDB].[dbo].[TitleTypes]
        '== #4-09 - INSERT into [IMDB].[dbo].[Titles]
        '== #4-10 - INSERT into [IMDB].[dbo].[TitleGenres]
        '== #4-11 - INSERT into [IMDB].[dbo].[TitleNames]
        '== #4-12 - INSERT into [IMDB].[dbo].[Attributes]
        '== #4-13 - INSERT into [IMDB].[dbo].[TitleNameAttributes]
        '== #4-14 - INSERT into [IMDB].[dbo].[Attributes]
        '== #4-15 - INSERT into [IMDB].[dbo].[TitleNameAttributes]
        '== #4-16 - INSERT into [IMDB].[dbo].[Titles]
        '== #4-17 - INSERT into [IMDB].[dbo].[Principals]
        '== #4-18 - INSERT into [IMDB].[dbo].[TitlePrincipals]
        '== #4-19 - UPDATE [IMDB].[dbo].[TitlePrincipals]
        '== #4-20 - INSERT into [IMDB].[dbo].[TitleCharacters]
        '== #4-21 - INSERT into #writers_directors
        '== #4-22 - INSERT into [IMDB].[dbo].[Titles] from #writers_directors
        '== #4-23 - INSERT into [IMDB].[dbo].[Principals] from #writers_directors
        '== #4-24 - INSERT into [IMDB].[dbo].[TitlePrincipals] from #writers_directors
        '== #4-25 - INSERT into [IMDB].[dbo].[Episodes]
        '== #4-26 - UPDATE [IMDB].[dbo].[Titles] for Votes and Average Ratings

        Dim success As Boolean = True
        Dim retryAfterCommandTimeOut As Boolean = True

        Dim retryCount As Integer = 0
        Dim maxRetryAttempts As Integer = 3

        Dim basicLogMsg As String =
            "Step " & currentStep.ToString() &
            " of " & lastStep.ToString()

        Dim finalLogText As String = "INSERTED"

        Dim commandText As String = C.AdHoc4List(currentStep)
        Dim timeOutForExecution As Integer = C.TimeOut4List(currentStep)
        Dim approximateRowCount As Long = C.ApproxRows4(currentStep)

        If commandText.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) Then
            finalLogText = "UPDATED"
        End If

        Dim logMessageHeader As String =
            C.EQUALSIGNS & Environment.NewLine &
            C.BASIC_LOG_MESSAGE_4 & Environment.NewLine &
            C.EQUALSIGNS & Environment.NewLine

        With sqlCmd
            .CommandTimeout = timeOutForExecution

            Do While retryAfterCommandTimeOut
                Try
                    .CommandText = commandText

                    TS.SetText(ProgressLogTextBox,
                               logMessageHeader &
                               "Executing T-SQL: " & basicLogMsg & Environment.NewLine &
                               C.DASHES & Environment.NewLine &
                               .CommandText & Environment.NewLine & Environment.NewLine &
                               "Expecting approximately: " & approximateRowCount.ToString(C.COMMA_MASK) & " Row(s)")

                    If timeOutForExecution >= 60 Then
                        TS.AppendText(ProgressLogTextBox,
                                      Environment.NewLine &
                                      Environment.NewLine &
                                      "Command Timeout set to: " &
                                      RFI.GetTimeStringFromSeconds_General(timeOutForExecution) &
                                      " - PLEASE WAIT")
                    End If

                    rowsAffected = .ExecuteNonQuery()

                    If C.TimeOut4List(currentStep) <> .CommandTimeout Then
                        C.TimeOut4List(currentStep) = .CommandTimeout
                    End If

                    If timeOutForExecution < 60 Then
                        TS.AppendText(ProgressLogTextBox,
                                      Environment.NewLine &
                                      "                         " &
                                      rowsAffected.ToString(C.COMMA_MASK) & " Row(s) " & finalLogText)

                    Else
                        TS.AppendText(ProgressLogTextBox,
                                      Environment.NewLine &
                                      Environment.NewLine &
                                      "                         " &
                                      rowsAffected.ToString(C.COMMA_MASK) & " Row(s) " & finalLogText)
                    End If

                    C.ApproxRows4(currentStep) = rowsAffected

                    Thread.Sleep(4000)

                    Exit Do

                Catch sqlEx As SqlException
                    Debug.Print(sqlEx.ToString)

                    LogErrorsToFile($"SQL Exception: {sqlEx.ToString()}")
                    LogErrorsToFile($"Command: {commandText}")

                    If sqlEx.Message.StartsWith("Execution Timeout Expired") Then
                        retryCount += 1

                        If retryCount >= maxRetryAttempts Then
                            retryAfterCommandTimeOut = False
                        End If

                        timeOutForExecution += (C.DEFAULT_TIMEOUT * 3)

                        TS.AppendText(ProgressLogTextBox,
                                      Environment.NewLine &
                                      Environment.NewLine &
                                      "Command TIMEOUT occurred... " &
                                      "Increasing Timeout setting by " &
                                      RFI.GetTimeStringFromSeconds_General(C.DEFAULT_TIMEOUT * 3) &
                                      " to " & RFI.GetTimeStringFromSeconds_General(timeOutForExecution) &
                                      " and attempting Command again...")

                        .CommandTimeout = timeOutForExecution

                    Else
                        retryAfterCommandTimeOut = False
                        success = False

                        Throw

                        Exit Do
                    End If

                    Exit Try

                Catch ex As Exception
                    LogErrorsToFile($"Exception: {ex.ToString()}")
                    LogErrorsToFile($"Command: {commandText}")

                    retryAfterCommandTimeOut = False
                    success = False

                    Throw

                    Exit Do


                End Try
            Loop
        End With

        Return success

    End Function

    ''' <summary>
    ''' Handles the click event of the ImportDataButton. It prompts the 
    ''' user for confirmation before proceeding with the data import 
    ''' operation. If confirmed, it disables relevant controls, sets 
    ''' up the background worker for SQL import, and starts the 
    ''' asynchronous operation to import data into the database.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ImportDataButton_Click(sender As Object, e As EventArgs) _
        Handles ImportDataButton.Click

        If MessageBox.Show("Are you sure you want to import all the data into the database?" & Environment.NewLine &
                           "This operation will take a while and cannot be undone." & Environment.NewLine &
                           "Please make sure you have a backup of your database before proceeding." & Environment.NewLine &
                           "Do you want to continue?",
                           "Confirm Data Import",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning) = DialogResult.No Then
            Exit Sub
        End If

        ' launch the sqlimportbackgroundworker after disabling all of the controls needed
        EndThingsButton.Text = "&Cancel"

        AcceptButton = EndThingsButton
        CancelButton = EndThingsButton

        CancelledOperations = False

        CountArchiveRowsEnabled = CountArchiveRowsButton.Enabled
        CountTsvRowsEnabled = CountTsvRowsButton.Enabled
        DecompressAfterDownloadEnabled = DecompressAfterDownloadCheckBox.Enabled

        If CountArchiveRowsEnabled Then
            CountArchiveRowsButton.Enabled = False
        End If

        If CountTsvRowsEnabled Then
            CountTsvRowsButton.Enabled = False
        End If

        If DecompressAfterDownloadEnabled Then
            DecompressAfterDownloadCheckBox.Enabled = False
        End If

        DownloadUpdatedArchivesButton.Enabled = False
        LoadAllDataFilesButton.Enabled = False
        ChooseFolderButton.Enabled = False
        FolderLocationTextBox.Enabled = False
        ImportDataButton.Enabled = False
        CancelledOperations = False

        SqlImportBackgroundWorker.RunWorkerAsync()

    End Sub

    ''' <summary>
    ''' Handles the DoWork event of the SqlImportBackgroundWorker. It processes
    ''' the database commands to load the data into the proper tables from the
    ''' [Raw] Data Tables.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SqlImportBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles SqlImportBackgroundWorker.DoWork

        ' process the Database commands to load the data into the proper tables from the [Raw] Data Tables.

        Dim localCmd As SqlCommand = Nothing
        Dim localTransaction As SqlTransaction = Nothing
        Dim commandText As String = String.Empty
        Dim rowsAffected As Long = 0
        Dim approximateRowCount As Long = 0

        Dim basicLogMessage As String = String.Empty

        Using conn As New SqlConnection(C.IMDB_CONNECTION_STRING)
            conn.Open()

            localCmd = conn.CreateCommand()
            localCmd.CommandType = CommandType.Text


            localTransaction = conn.BeginTransaction(IsolationLevel.Serializable)
            localCmd.Transaction = localTransaction

            Try
                ' #1: We need to drop all of the constraints on the tables 
                ' #2: Truncate the tables 
                ' #3: Re-add the constraints.  

                ' This is much faster than deleting the data and then trying to insert it with the constraints in place.

                Do While Not CancelledOperations
                    With localCmd
                        '======================================================================================================
                        '======================================================================================================
                        '== #1 drop the constraints
                        '======================================================================================================
                        '======================================================================================================
                        For currentStep As Integer = 1 To C.ADHOC_COUNT_1_MAX
                            If Not AddOrDropTableConstraint(currentStep,
                                                            C.ADHOC_COUNT_1_MAX,
                                                            conn,
                                                            localCmd,
                                                            DropAddEnum.DROP,
                                                            C.DEFAULT_TIMEOUT) OrElse
                               SqlImportBackgroundWorker.CancellationPending Then

                                CancelledOperations = SqlImportBackgroundWorker.CancellationPending

                                localTransaction.Rollback()

                                Exit Do
                            End If
                        Next

                        '======================================================================================================
                        '======================================================================================================
                        '== #2 truncate the tables
                        '======================================================================================================
                        '======================================================================================================
                        For Each ah25Table As AH25 In [Enum].GetValues(Of AH25)()

                            If Not TruncateTable(ah25Table, conn, localCmd) OrElse
                               SqlImportBackgroundWorker.CancellationPending Then
                                CancelledOperations = SqlImportBackgroundWorker.CancellationPending

                                localTransaction.Rollback()

                                Exit Do
                            End If

                        Next

                        '======================================================================================================
                        '======================================================================================================
                        '== #3 re-add the constraints
                        '======================================================================================================
                        '======================================================================================================
                        For currentStep As Integer = 1 To C.ADHOC_COUNT_3_MAX

                            If Not AddOrDropTableConstraint(currentStep,
                                                            C.ADHOC_COUNT_3_MAX,
                                                            conn,
                                                            localCmd,
                                                            DropAddEnum.ADD,
                                                            C.DEFAULT_TIMEOUT) OrElse
                               SqlImportBackgroundWorker.CancellationPending Then

                                CancelledOperations = SqlImportBackgroundWorker.CancellationPending

                                localTransaction.Rollback()

                                Exit Do
                            End If

                        Next

                        '======================================================================================================
                        '======================================================================================================
                        '== #4 Import data into IMDB.dbo Db Tables from IMDB.Raw Db Tables
                        '======================================================================================================
                        '======================================================================================================
                        For currentStep As Integer =
                            C.ADHOC_COUNT_4_MIN To C.ADHOC_COUNT_4_MAX

                            If Not InsertOrUpdateTable(currentStep,
                                                       C.ADHOC_COUNT_4_MAX,
                                                       rowsAffected,
                                                       conn,
                                                       localCmd) OrElse
                               SqlImportBackgroundWorker.CancellationPending Then
                                CancelledOperations = SqlImportBackgroundWorker.CancellationPending

                                localTransaction.Rollback()

                                Exit Do
                            End If
                        Next

                        '======================================================================================================
                        '======================================================================================================
                        '== #5 Get Final Table Counts for the IMDB.dbo Db Tables
                        '======================================================================================================
                        '======================================================================================================
                        For Each ah25Table As AH25 In [Enum].GetValues(Of AH25)()

                            If Not CountTable(ah25Table, conn, localCmd) OrElse
                               SqlImportBackgroundWorker.CancellationPending Then

                                CancelledOperations = SqlImportBackgroundWorker.CancellationPending

                                localTransaction.Rollback()

                                Exit Do
                            End If

                        Next

                        Exit Do
                    End With
                Loop

                If Not CancelledOperations Then
                    localTransaction.Commit()
                End If

            Catch ex As Exception
                LogErrorsToFile($"Exception: {ex.ToString()}")

                If conn.State = ConnectionState.Open Then
                    localTransaction.Rollback()
                End If

            Finally
                If localCmd IsNot Nothing Then
                    localCmd.Dispose()
                End If
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' Handles the RunWorkerCompleted event of the SqlImportBackgroundWorker. It re-enables
    ''' the controls and updates the UI after the background worker has completed its operation.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SqlImportBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles SqlImportBackgroundWorker.RunWorkerCompleted

        ImportDataButton.Enabled = True

        ' refer to the backgroundworker by its name: SqlBackgroundWorker
        EndThingsButton.Text = "E&xit"

        CountArchiveRowsButton.Enabled = CountArchiveRowsEnabled
        CountTsvRowsButton.Enabled = CountTsvRowsEnabled
        DecompressAfterDownloadCheckBox.Enabled = DecompressAfterDownloadEnabled

        DownloadUpdatedArchivesButton.Enabled = True
        LoadAllDataFilesButton.Enabled = True
        ChooseFolderButton.Enabled = True
        FolderLocationTextBox.Enabled = True

        Me.AcceptButton = LoadAllDataFilesButton
        Me.CancelButton = EndThingsButton

    End Sub

#End Region

End Class