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
'Imports System.IO.Compression
Imports Comp = System.IO.Compression
Imports FT = ImportAndAddRawImdbData.RawFileInfo.FileTypeEnum
Imports PFT = ImportAndAddRawImdbData.CountOrInsertData.ProcessFileTypeEnum
Imports PT = ImportAndAddRawImdbData.CountOrInsertData.ProcessTypeEnum
Imports SCT = ImportAndAddRawImdbData.MainForm2.SqlCmdTypeEnum
Imports SP = ImportAndAddRawImdbData.CountOrInsertData.SequentialOrParallelEnum
Imports TS = ImportAndAddRawImdbData.ThreadSafeMethods
Imports RFI = ImportAndAddRawImdbData.RawFileInfo

Public Class MainForm2

    Private Property FolderLocation As String = String.Empty
    Private Property LocationExists As Boolean = False

    Private Property ArchiveDownloadLocationsList As List(Of String)

    Private Property AllPreviouslyUploadedFilenamesAndRowCountsPlusCurrent As String = String.Empty
    Private Property CompiledPreviouslyUploadedFilenamesAndRowCounts As String = String.Empty
    Private Property CurrentUploadFilenameAndRowCount As String = String.Empty
    Private Property CurrentlyUploadingFilename As String = String.Empty

    Private Property DecompressedFileList As New List(Of String)

    Public Const CompressedFileExtension As String = ".tsv.gz"
    Public Const UnCompressedFileExtension As String = ".tsv"

    Public Enum ImportTypeEnum
        Unknown
        Compressed
        Decompressed
    End Enum

    Private Property ImportType As ImportTypeEnum =
                                   ImportTypeEnum.Unknown

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

            TS.SetText(NameBasicsPreviousRowCountTextBox,
                       MyRawFileInfo(FT.NameBasics).LastRowCount.ToString(C.COMMA_MASK))

            TS.SetText(TitleAkasPreviousRowCountTextBox,
                       MyRawFileInfo(FT.TitleAkas).LastRowCount.ToString(C.COMMA_MASK))

            TS.SetText(TitleBasicsPreviousRowCountTextBox,
                       MyRawFileInfo(FT.TitleBasics).LastRowCount.ToString(C.COMMA_MASK))

            TS.SetText(TitleCrewPreviousRowCountTextBox,
                       MyRawFileInfo(FT.TitleCrew).LastRowCount.ToString(C.COMMA_MASK))

            TS.SetText(TitleEpisodePreviousRowCountTextBox,
                       MyRawFileInfo(FT.TitleEpisode).LastRowCount.ToString(C.COMMA_MASK))

            TS.SetText(TitlePrincipalsPreviousRowCountTextBox,
                       MyRawFileInfo(FT.TitlePrincipals).LastRowCount.ToString(C.COMMA_MASK))

            TS.SetText(TitleRatingsPreviousRowCountTextBox,
                       MyRawFileInfo(FT.TitleRatings).LastRowCount.ToString(C.COMMA_MASK))

            CountTsvRowsButton.Enabled = ((File.Exists(Path.Combine(FolderLocation, C.NameBasicsDecompFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleAkasDecompFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleBasicsDecompFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleCrewDecompFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeDecompFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsDecompFileName))) AndAlso
                                          (File.Exists(Path.Combine(FolderLocation, C.TitleRatingsDecompFileName))))

            CountArchiveRowsButton.Enabled = ((File.Exists(Path.Combine(FolderLocation, C.NameBasicsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleAkasCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleBasicsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleCrewCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsCompressedFileName))) AndAlso
                                              (File.Exists(Path.Combine(FolderLocation, C.TitleRatingsCompressedFileName))))

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
                NameBasicsFilenameLabel.Text = C.NameBasicsDecompFileName
                TitleAkasFilenameLabel.Text = C.TitleAkasDecompFileName
                TitleBasicsFilenameLabel.Text = C.TitleBasicsDecompFileName
                TitleCrewFilenameLabel.Text = C.TitleCrewDecompFileName
                TitleEpisodeFilenameLabel.Text = C.TitleEpisodeDecompFileName
                TitlePrincipalsFilenameLabel.Text = C.TitlePrincipalsDecompFileName
                TitleRatingsFilenameLabel.Text = C.TitleRatingsDecompFileName

                Dim nameBasicsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.NameBasicsDecompFileName)).Length

                Dim titleAkasLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleAkasDecompFileName)).Length

                Dim titleBasicsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleBasicsDecompFileName)).Length

                Dim titleCrewLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleCrewDecompFileName)).Length

                Dim titleEpisodeLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleEpisodeDecompFileName)).Length

                Dim titlePrincipalsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitlePrincipalsDecompFileName)).Length

                Dim titleRatingsLength As Long =
                    New FileInfo(Path.Combine(FolderLocation,
                                              C.TitleRatingsDecompFileName)).Length

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
                .NameBasicsSavedRowCount = MyRawFileInfo(FT.NameBasics).CurrentRowCount
            Else
                .NameBasicsSavedStartTime = MyRawFileInfo(FT.NameBasics).PreviousStartTime
                .NameBasicsSavedEndTime = MyRawFileInfo(FT.NameBasics).PreviousEndTime
                .NameBasicsSavedRowCount = MyRawFileInfo(FT.NameBasics).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleAkas).CompletedProcessing Then
                .TitleAkasSavedStartTime = MyRawFileInfo(FT.TitleAkas).CurrentStartTime
                .TitleAkasSavedEndTime = MyRawFileInfo(FT.TitleAkas).CurrentEndTime
                .TitleAkasSavedRowCount = MyRawFileInfo(FT.TitleAkas).CurrentRowCount
            Else
                .TitleAkasSavedStartTime = MyRawFileInfo(FT.TitleAkas).PreviousStartTime
                .TitleAkasSavedEndTime = MyRawFileInfo(FT.TitleAkas).PreviousEndTime
                .TitleAkasSavedRowCount = MyRawFileInfo(FT.TitleAkas).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleBasics).CompletedProcessing Then
                .TitleBasicsSavedStartTime = MyRawFileInfo(FT.TitleBasics).CurrentStartTime
                .TitleBasicsSavedEndTime = MyRawFileInfo(FT.TitleBasics).CurrentEndTime
                .TitleBasicsSavedRowCount = MyRawFileInfo(FT.TitleBasics).CurrentRowCount
            Else
                .TitleBasicsSavedStartTime = MyRawFileInfo(FT.TitleBasics).PreviousStartTime
                .TitleBasicsSavedEndTime = MyRawFileInfo(FT.TitleBasics).PreviousEndTime
                .TitleBasicsSavedRowCount = MyRawFileInfo(FT.TitleBasics).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleCrew).CompletedProcessing Then
                .TitleCrewSavedStartTime = MyRawFileInfo(FT.TitleCrew).CurrentStartTime
                .TitleCrewSavedEndTime = MyRawFileInfo(FT.TitleCrew).CurrentEndTime
                .TitleCrewSavedRowCount = MyRawFileInfo(FT.TitleCrew).CurrentRowCount
            Else
                .TitleCrewSavedStartTime = MyRawFileInfo(FT.TitleCrew).PreviousStartTime
                .TitleCrewSavedEndTime = MyRawFileInfo(FT.TitleCrew).PreviousEndTime
                .TitleCrewSavedRowCount = MyRawFileInfo(FT.TitleCrew).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleEpisode).CompletedProcessing Then
                .TitleEpisodeSavedStartTime = MyRawFileInfo(FT.TitleEpisode).CurrentStartTime
                .TitleEpisodeSavedEndTime = MyRawFileInfo(FT.TitleEpisode).CurrentEndTime
                .TitleEpisodeSavedRowCount = MyRawFileInfo(FT.TitleEpisode).CurrentRowCount
            Else
                .TitleEpisodeSavedStartTime = MyRawFileInfo(FT.TitleEpisode).PreviousStartTime
                .TitleEpisodeSavedEndTime = MyRawFileInfo(FT.TitleEpisode).PreviousEndTime
                .TitleEpisodeSavedRowCount = MyRawFileInfo(FT.TitleEpisode).LastRowCount
            End If

            If MyRawFileInfo(FT.TitlePrincipals).CompletedProcessing Then
                .TitlePrincipalsSavedStartTime = MyRawFileInfo(FT.TitlePrincipals).CurrentStartTime
                .TitlePrincipalsSavedEndTime = MyRawFileInfo(FT.TitlePrincipals).CurrentEndTime
                .TitlePrincipalsSavedRowCount = MyRawFileInfo(FT.TitlePrincipals).CurrentRowCount
            Else
                .TitlePrincipalsSavedStartTime = MyRawFileInfo(FT.TitlePrincipals).PreviousStartTime
                .TitlePrincipalsSavedEndTime = MyRawFileInfo(FT.TitlePrincipals).PreviousEndTime
                .TitlePrincipalsSavedRowCount = MyRawFileInfo(FT.TitlePrincipals).LastRowCount
            End If

            If MyRawFileInfo(FT.TitleRatings).CompletedProcessing Then
                .TitleRatingsSavedStartTime = MyRawFileInfo(FT.TitleRatings).CurrentStartTime
                .TitleRatingsSavedEndTime = MyRawFileInfo(FT.TitleRatings).CurrentEndTime
                .TitleRatingsSavedRowCount = MyRawFileInfo(FT.TitleRatings).CurrentRowCount
            Else
                .TitleRatingsSavedStartTime = MyRawFileInfo(FT.TitleRatings).PreviousStartTime
                .TitleRatingsSavedEndTime = MyRawFileInfo(FT.TitleRatings).PreviousEndTime
                .TitleRatingsSavedRowCount = MyRawFileInfo(FT.TitleRatings).LastRowCount
            End If

            If MyRawFileInfo(FT.OVERALL).CompletedProcessing Then
                .OverallSavedStartTime = MyRawFileInfo(FT.OVERALL).CurrentStartTime
                .OverallSavedEndTime = MyRawFileInfo(FT.OVERALL).CurrentEndTime
                .OverallSavedRowCount = MyRawFileInfo(FT.OVERALL).CurrentRowCount
            Else
                .OverallSavedStartTime = MyRawFileInfo(FT.OVERALL).PreviousStartTime
                .OverallSavedEndTime = MyRawFileInfo(FT.OVERALL).PreviousEndTime
                .OverallSavedRowCount = MyRawFileInfo(FT.OVERALL).LastRowCount
            End If

            .Save()
        End With

    End Sub

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

    Private Property ProcessFileType As PFT = PFT.Compressed

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
        If File.Exists(Path.Combine(FolderLocation, C.NameBasicsDecompFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleAkasDecompFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleBasicsDecompFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleCrewDecompFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeDecompFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsDecompFileName)) AndAlso
           File.Exists(Path.Combine(FolderLocation, C.TitleRatingsDecompFileName)) Then

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
                                                             ProcessFileType)

            If countOrInsertDataForm.ShowDialog <> DialogResult.OK Then
                EndThingsButton.Text = "E&xit"

                AcceptButton = LoadAllDataFilesButton
                CancelButton = EndThingsButton

                Exit Sub
            End If

            InsertDataFilesList.Clear()

            Dim filesToProcess = countOrInsertDataForm.ProcessFilesList

            For Each fileToProcess In filesToProcess
                InsertDataFilesList.Add(fileToProcess)
            Next

            ImportDataButton.Enabled = False
            CountArchiveRowsButton.Enabled = False
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

    Public ReadOnly Property FilesLocation As String
        Get
            Return FolderLocation
        End Get
    End Property

    Private Async Sub DownloadUpdatedArchivesButton_Click(sender As Object, e As EventArgs) _
        Handles DownloadUpdatedArchivesButton.Click

        ' disable the other buttons
        TS.SetEnabled(DownloadUpdatedArchivesButton, False)
        TS.SetEnabled(LoadAllDataFilesButton, False)
        TS.SetEnabled(ChooseFolderButton, False)
        TS.SetEnabled(FolderLocationTextBox, False)
        TS.SetEnabled(EndThingsButton, False)
        TS.SetEnabled(DecompressAfterDownloadCheckBox, False)
        TS.SetEnabled(CountArchiveRowsButton, False)

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

            Dim destinationPath As String = Path.Combine(FolderLocation, fileName)

            Select Case GetFileTypeBasedOnFileName(Path.GetFileName(destinationPath))
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

            Await DownloadFileWithProgress(archiveUrl, destinationPath)

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

        CountTsvRowsButton.Enabled = ((File.Exists(Path.Combine(FolderLocation, C.NameBasicsDecompFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleAkasDecompFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleBasicsDecompFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleCrewDecompFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleEpisodeDecompFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitlePrincipalsDecompFileName))) AndAlso
                                      (File.Exists(Path.Combine(FolderLocation, C.TitleRatingsDecompFileName))))

        If CountTsvRowsButton.Enabled Then
            NameBasicsFilenameLabel.Text = C.NameBasicsDecompFileName
            TitleAkasFilenameLabel.Text = C.TitleAkasDecompFileName
            TitleBasicsFilenameLabel.Text = C.TitleBasicsDecompFileName
            TitleCrewFilenameLabel.Text = C.TitleCrewDecompFileName
            TitleEpisodeFilenameLabel.Text = C.TitleEpisodeDecompFileName
            TitlePrincipalsFilenameLabel.Text = C.TitlePrincipalsDecompFileName
            TitleRatingsFilenameLabel.Text = C.TitleRatingsDecompFileName


            Dim nameBasicsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.NameBasicsDecompFileName))).Length

            Dim titleAkasLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleAkasDecompFileName))).Length

            Dim titleBasicsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleBasicsDecompFileName))).Length

            Dim titleCrewLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleCrewDecompFileName))).Length

            Dim titleEpisodeLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleEpisodeDecompFileName))).Length

            Dim titlePrincipalsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitlePrincipalsDecompFileName))).Length

            Dim titleRatingsLength As Long =
                (New FileInfo(Path.Combine(FolderLocation, C.TitleRatingsDecompFileName))).Length

            TS.SetText(NameBasicsSizeTextBox,
                       GetFileDisplayLength(nameBasicsLength) & " " & GetFileDisplayLengthString(nameBasicsLength))
            TS.SetText(TitleAkasSizeTextBox,
                       GetFileDisplayLength(titleAkasLength) & " " & GetFileDisplayLengthString(titleAkasLength))
            TS.SetText(TitleBasicsSizeTextBox,
                       GetFileDisplayLength(titleBasicsLength) & " " & GetFileDisplayLengthString(titleBasicsLength))
            TS.SetText(TitleCrewSizeTextBox,
                       GetFileDisplayLength(titleCrewLength) & " " & GetFileDisplayLengthString(titleCrewLength))
            TS.SetText(TitleEpisodeSizeTextBox,
                       GetFileDisplayLength(titleEpisodeLength) & " " & GetFileDisplayLengthString(titleEpisodeLength))
            TS.SetText(TitlePrincipalsSizeTextBox,
                       GetFileDisplayLength(titlePrincipalsLength) & " " & GetFileDisplayLengthString(titlePrincipalsLength))
            TS.SetText(TitleRatingsSizeTextBox,
                       GetFileDisplayLength(titleRatingsLength) & " " & GetFileDisplayLengthString(titleRatingsLength))

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

            FileSizeHeader1Label.Text = "File Size .gz"
            FileSizeHeader2Label.Text = "File Size .gz"

        End If

        If CountArchiveRowsButton.Enabled Then
            ImportType = ImportTypeEnum.Compressed

        ElseIf CountTsvRowsButton.Enabled Then
            ImportType = ImportTypeEnum.Decompressed

        Else
            ImportType = ImportTypeEnum.Unknown

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

        Me.CancelButton = EndThingsButton
        Me.AcceptButton = LoadAllDataFilesButton

    End Sub

    Private Function GetFileDisplayLengthString(fileLength As Long) As String

        Return IIf(FileIsGbOrLarger(fileLength), "GB", "MB")

    End Function

    Private Function GetFileDisplayLength(fileLength As Long) As String

        Return CType(IIf(FileIsGbOrLarger(fileLength),
                         GetGBDisplayLength(fileLength),
                         GetMBDisplayLength(fileLength)), Double).ToString("F2")


    End Function

    Private Function FileIsGbOrLarger(fileLength As Long) As Boolean

        Return (GetMBDisplayLength(fileLength) >= 1024.0)

    End Function

    Private Function GetGBDisplayLength(fileLength As Long) As Double

        Return CType((fileLength / (1024 * 1024 * 1024)), Double)

    End Function

    Private Function GetMBDisplayLength(fileLength As Long) As Double

        Return CType((fileLength / (1024 * 1024)), Double)

    End Function

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

    Private Property NameBasicsRowCount As Long = 0
    Private Property TitleAkasRowCount As Long = 0
    Private Property TitleBasicsRowCount As Long = 0
    Private Property TitleCrewRowCount As Long = 0
    Private Property TitleEpisodeRowCount As Long = 0
    Private Property TitlePrincipalsRowCount As Long = 0
    Private Property TitleRatingsRowCount As Long = 0

    Private Property NameBasicsCounted As Boolean = False
    Private Property TitleAkasCounted As Boolean = False
    Private Property TitleBasicsCounted As Boolean = False
    Private Property TitleCrewCounted As Boolean = False
    Private Property TitleEpisodeCounted As Boolean = False
    Private Property TitlePrincipalsCounted As Boolean = False
    Private Property TitleRatingsCounted As Boolean = False

    Private Sub EndThingsButton_Click(sender As Object, e As EventArgs) _
        Handles EndThingsButton.Click

        If EndThingsButton.Text = "&Cancel" Then
            ' end the backgroundworker thread

            If SqlBackgroundWorker.IsBusy Then
                SqlBackgroundWorker.CancelAsync()

            ElseIf SqlImportBackgroundWorker.IsBusy Then
                SqlImportBackgroundWorker.CancelAsync()

            ElseIf AllArchivesBackgroundWorker.IsBusy Then
                AllArchivesBackgroundWorker.CancelAsync()

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

    Private Function CountFileRows(ByVal fileName As String) As Long

        Dim rowCount As Long = IO.File.ReadLines(Path.Combine(FolderLocation, fileName)).Count - 1

        Select Case GetFileTypeBasedOnFileName(fileName)
            Case FT.NameBasics : TS.SetText(NameBasicsCountTextBox, rowCount.ToString(C.COMMA_MASK))
            Case FT.TitleAkas : TS.SetText(TitleAkasCountTextBox, rowCount.ToString(C.COMMA_MASK))
            Case FT.TitleBasics : TS.SetText(TitleBasicsCountTextBox, rowCount.ToString(C.COMMA_MASK))
            Case FT.TitleCrew : TS.SetText(TitleCrewCountTextBox, rowCount.ToString(C.COMMA_MASK))
            Case FT.TitleEpisode : TS.SetText(TitleEpisodeCountTextBox, rowCount.ToString(C.COMMA_MASK))
            Case FT.TitlePrincipals : TS.SetText(TitlePrincipalsCountTextBox, rowCount.ToString(C.COMMA_MASK))
            Case FT.TitleRatings : TS.SetText(TitleRatingsCountTextBox, rowCount.ToString(C.COMMA_MASK))
        End Select

        Return rowCount

    End Function

    Private Function CountCompressedFileRows(ByVal fileName As String) As Long

        Dim rowCount As Long = 0

        Dim fileInfoObj As New FileInfo(Path.Combine(FolderLocation, fileName))
        Dim gzipFileStream As FileStream = IO.File.OpenRead(fileInfoObj.FullName)

        Using decompressionStream As New Comp.GZipStream(gzipFileStream,
                                                         Comp.CompressionMode.Decompress)

            ' Create a stream reader to read from the decompression stream
            Using myStreamReader As New StreamReader(decompressionStream)
                Try
                    Dim line As String =
                        myStreamReader.ReadLine()

                    Do While (line IsNot Nothing)
                        rowCount += 1

                        Dim weShouldExitNow As Boolean = False

                        If AllArchivesBackgroundWorker.IsBusy Then
                            If AllArchivesBackgroundWorker.CancellationPending Then
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

                            Select Case GetFileTypeBasedOnFileName(fileName)
                                Case FT.NameBasics
                                    TS.SetText(NameBasicsCountTextBox,
                                               rowCount.ToString(C.COMMA_MASK))
                                    'Debug.Print(C.TitleAkasCompressedFileName & " row count: " & rowCount.ToString(C.COMMA_MASK))

                                Case FT.TitleAkas
                                    TS.SetText(TitleAkasCountTextBox,
                                               rowCount.ToString(C.COMMA_MASK))
                                    'Debug.Print(C.TitleAkasCompressedFileName & " row count: " & rowCount.ToString(C.COMMA_MASK))

                                Case FT.TitleBasics
                                    TS.SetText(TitleBasicsCountTextBox,
                                               rowCount.ToString(C.COMMA_MASK))
                                    'Debug.Print(C.TitleBasicsCompressedFileName & " row count: " & rowCount.ToString(C.COMMA_MASK))

                                Case FT.TitleCrew
                                    TS.SetText(TitleCrewCountTextBox,
                                               rowCount.ToString(C.COMMA_MASK))
                                    'Debug.Print(C.TitleCrewCompressedFileName & " row count: " & rowCount.ToString(C.COMMA_MASK))

                                Case FT.TitleEpisode
                                    TS.SetText(TitleEpisodeCountTextBox,
                                               rowCount.ToString(C.COMMA_MASK))
                                    'Debug.Print(C.TitleEpisodeCompressedFileName & " row count: " & rowCount.ToString(C.COMMA_MASK))

                                Case FT.TitlePrincipals
                                    TS.SetText(TitlePrincipalsCountTextBox,
                                               rowCount.ToString(C.COMMA_MASK))
                                    'Debug.Print(C.TitlePrincipalsCompressedFileName & " row count: " & rowCount.ToString(C.COMMA_MASK))

                                Case FT.TitleRatings
                                    TS.SetText(TitleRatingsCountTextBox,
                                               rowCount.ToString(C.COMMA_MASK))
                                    'Debug.Print(C.TitleRatingsCompressedFileName & " row count: " & rowCount.ToString(C.COMMA_MASK))

                            End Select
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

    Private Sub CountArchiveRowsButton_Click(sender As Object, e As EventArgs) _
        Handles CountArchiveRowsButton.Click

        Dim localFileList As New List(Of String)

        EndThingsButton.Text = "&Cancel"

        Me.AcceptButton = EndThingsButton
        Me.CancelButton = EndThingsButton

        Me.ImportType = ImportTypeEnum.Compressed

        NameBasicsRowCount = 0
        TitleAkasRowCount = 0
        TitleBasicsRowCount = 0
        TitleCrewRowCount = 0
        TitleEpisodeRowCount = 0
        TitlePrincipalsRowCount = 0
        TitleRatingsRowCount = 0

        NameBasicsCounted = False
        TitleAkasCounted = False
        TitleBasicsCounted = False
        TitleCrewCounted = False
        TitleEpisodeCounted = False
        TitlePrincipalsCounted = False
        TitleRatingsCounted = False

        Using countOrInsertDataForm As New CountOrInsertData(PT.CountData,
                                                             SP.Sequential,
                                                             PFT.Compressed)

            If countOrInsertDataForm.ShowDialog() <> DialogResult.OK Then
                EndThingsButton.Text = "E&xit"

                Me.AcceptButton = LoadAllDataFilesButton
                Me.CancelButton = EndThingsButton

                Exit Sub
            End If

            ' get list of files to process from the CountOrInsertData form, and then kick off the counting of rows in those files,
            ' and then update the UI with that info as it becomes available, so the user has a sense of how many rows are in each file, and can see the progress of that counting as it happens, rather than waiting until all the counting is done and then updating the UI with that info
            Dim filesToProcess As List(Of String) = countOrInsertDataForm.ProcessFilesList
            Dim gzFilesToProcess As String() = countOrInsertDataForm.ProcessFilesList.ToArray

            Dim processType As PT = countOrInsertDataForm.ProcessType
            Dim sequentialOrParallel As SP = countOrInsertDataForm.SequentialOrParallel

            CountFilesList.Clear()

            For Each fileToProcess As String In filesToProcess
                CountFilesList.Add(fileToProcess)
            Next

            If sequentialOrParallel = SP.Sequential Then
                DownloadUpdatedArchivesButton.Enabled = False
                LoadAllDataFilesButton.Enabled = False
                ChooseFolderButton.Enabled = False
                FolderLocationTextBox.Enabled = False

                CancelledOperations = False

                AllArchivesBackgroundWorker.RunWorkerAsync()

            ElseIf sequentialOrParallel = SP.Parallel Then

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

                    Select Case GetFileTypeBasedOnFileName(Path.GetFileName(fileName))
                        Case FT.NameBasics
                            MyRawFileInfo(FT.NameBasics).CompressedCountedRowCount = lineCount

                            NameBasicsRowCount = MyRawFileInfo(FT.NameBasics).CompressedCountedRowCount
                            NameBasicsCounted = True

                            TS.SetText(NameBasicsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                        Case FT.TitleAkas
                            MyRawFileInfo(FT.TitleAkas).CompressedCountedRowCount = lineCount

                            TitleAkasRowCount = MyRawFileInfo(FT.TitleAkas).CompressedCountedRowCount
                            TitleAkasCounted = True

                            TS.SetText(TitleAkasCountTextBox, lineCount.ToString(C.COMMA_MASK))

                        Case FT.TitleBasics
                            MyRawFileInfo(FT.TitleBasics).CompressedCountedRowCount = lineCount

                            TitleBasicsRowCount = MyRawFileInfo(FT.TitleBasics).CompressedCountedRowCount
                            TitleBasicsCounted = True

                            TS.SetText(TitleBasicsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                        Case FT.TitleCrew
                            MyRawFileInfo(FT.TitleCrew).CompressedCountedRowCount = lineCount

                            TitleCrewRowCount = MyRawFileInfo(FT.TitleCrew).CompressedCountedRowCount
                            TitleCrewCounted = True

                            TS.SetText(TitleCrewCountTextBox, lineCount.ToString(C.COMMA_MASK))

                        Case FT.TitleEpisode
                            MyRawFileInfo(FT.TitleEpisode).CompressedCountedRowCount = lineCount

                            TitleEpisodeRowCount = MyRawFileInfo(FT.TitleEpisode).CompressedCountedRowCount
                            TitleEpisodeCounted = True

                            TS.SetText(TitleEpisodeCountTextBox, lineCount.ToString(C.COMMA_MASK))

                        Case FT.TitlePrincipals
                            MyRawFileInfo(FT.TitlePrincipals).CompressedCountedRowCount = lineCount

                            TitlePrincipalsRowCount = MyRawFileInfo(FT.TitlePrincipals).CompressedCountedRowCount
                            TitlePrincipalsCounted = True

                            TS.SetText(TitlePrincipalsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                        Case FT.TitleRatings
                            MyRawFileInfo(FT.TitleRatings).CompressedCountedRowCount = lineCount

                            TitleRatingsRowCount = MyRawFileInfo(FT.TitleRatings).CompressedCountedRowCount
                            TitleRatingsCounted = True

                            TS.SetText(TitleRatingsCountTextBox, lineCount.ToString(C.COMMA_MASK))

                    End Select

                Next

                If (NameBasicsCounted AndAlso
                    TitleAkasCounted AndAlso
                    TitleBasicsCounted AndAlso
                    TitleCrewCounted AndAlso
                    TitleEpisodeCounted AndAlso
                    TitlePrincipalsCounted AndAlso
                    TitleRatingsCounted) Then

                    MyRawFileInfo(FT.OVERALL).CompressedCountedRowCount =
                        (MyRawFileInfo(FT.NameBasics).CompressedCountedRowCount +
                         MyRawFileInfo(FT.TitleAkas).CompressedCountedRowCount +
                         MyRawFileInfo(FT.TitleBasics).CompressedCountedRowCount +
                         MyRawFileInfo(FT.TitleCrew).CompressedCountedRowCount +
                         MyRawFileInfo(FT.TitleEpisode).CompressedCountedRowCount +
                         MyRawFileInfo(FT.TitlePrincipals).CompressedCountedRowCount +
                         MyRawFileInfo(FT.TitleRatings).CompressedCountedRowCount)

                End If
            End If
        End Using

        ' kick off separate tasks to count the rows in each file using BackgroundWorker, 
        ' and then update the UI with that info as it becomes available, so the user has 
        ' a sense of how many rows are in each file, and can see the progress of that 
        ' counting as it happens, rather than waiting until all the counting is done and 
        ' then updating the UI with that info

    End Sub

    Private Property CountFilesList As New List(Of String)
    Private Property InsertDataFilesList As New List(Of String)

    Private Sub CountTsvRowsButton_Click(sender As Object, e As EventArgs) _
        Handles CountTsvRowsButton.Click

        EndThingsButton.Text = "&Cancel"

        Me.AcceptButton = EndThingsButton
        Me.CancelButton = EndThingsButton

        Me.ImportType = ImportTypeEnum.Decompressed

        NameBasicsRowCount = 0
        TitleAkasRowCount = 0
        TitleBasicsRowCount = 0
        TitleCrewRowCount = 0
        TitleEpisodeRowCount = 0
        TitlePrincipalsRowCount = 0
        TitleRatingsRowCount = 0

        NameBasicsCounted = False
        TitleAkasCounted = False
        TitleBasicsCounted = False
        TitleCrewCounted = False
        TitleEpisodeCounted = False
        TitlePrincipalsCounted = False
        TitleRatingsCounted = False

        ' kick off separate tasks to count the rows in each file using BackgroundWorker, 
        ' and then update the UI with that info as it becomes available, so the user has 
        ' a sense of how many rows are in each file, and can see the progress of that 
        ' counting as it happens, rather than waiting until all the counting is done and 
        ' then updating the UI with that info

        Using countOrInsertDataForm As New CountOrInsertData(PT.CountData,
                                                             SP.Sequential,
                                                             PFT.Decompressed)

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
            Dim sequentialOrParallel As SP = countOrInsertDataForm.SequentialOrParallel

            CountFilesList.Clear()

            For Each fileToProcess As String In filesToProcess
                CountFilesList.Add(fileToProcess)
            Next

            If sequentialOrParallel = SP.Sequential Then

                DownloadUpdatedArchivesButton.Enabled = False
                LoadAllDataFilesButton.Enabled = False
                ChooseFolderButton.Enabled = False
                FolderLocationTextBox.Enabled = False

                CancelledOperations = False

                AllArchivesBackgroundWorker.RunWorkerAsync()

            ElseIf sequentialOrParallel = SP.Parallel Then
                ' kick off the backgroundworker for each file to process

                DownloadUpdatedArchivesButton.Enabled = False
                LoadAllDataFilesButton.Enabled = False
                ChooseFolderButton.Enabled = False
                FolderLocationTextBox.Enabled = False

                CancelledOperations = False

                For Each fileToProcess As String In CountFilesList
                    If fileToProcess.StartsWith(FolderLocation) Then
                        fileToProcess = Path.GetFileName(fileToProcess)
                    End If

                    If File.Exists(Path.Combine(FolderLocation, fileToProcess)) Then
                        Select Case fileToProcess
                            Case C.NameBasicsDecompFileName : NameBasicsBackgroundWorker.RunWorkerAsync(fileToProcess)
                            Case C.TitleAkasDecompFileName : TitleAkasBackgroundWorker.RunWorkerAsync(fileToProcess)
                            Case C.TitleBasicsDecompFileName : TitleBasicsBackgroundWorker.RunWorkerAsync(fileToProcess)
                            Case C.TitleCrewDecompFileName : TitleCrewBackgroundWorker.RunWorkerAsync(fileToProcess)
                            Case C.TitleEpisodeDecompFileName : TitleEpisodeBackgroundWorker.RunWorkerAsync(fileToProcess)
                            Case C.TitlePrincipalsDecompFileName : TitlePrincipalsBackgroundWorker.RunWorkerAsync(fileToProcess)
                            Case C.TitleRatingsDecompFileName : TitleRatingsBackgroundWorker.RunWorkerAsync(fileToProcess)
                        End Select
                    End If
                Next
            End If

        End Using

        'CheckAllCounted()

    End Sub

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

    Private Property CancelledOperations As Boolean = False

    Private Sub CheckAllCounted()

        Dim allCounted As Boolean = (NameBasicsCounted AndAlso
                                     TitleAkasCounted AndAlso
                                     TitleBasicsCounted AndAlso
                                     TitleCrewCounted AndAlso
                                     TitleEpisodeCounted AndAlso
                                     TitlePrincipalsCounted AndAlso
                                     TitleRatingsCounted)

        Dim reEnableControls As Boolean = (allCounted OrElse
                                           CancelledOperations)

        FolderLocationTextBox.Enabled = reEnableControls
        ChooseFolderButton.Enabled = reEnableControls
        LoadAllDataFilesButton.Enabled = reEnableControls
        DownloadUpdatedArchivesButton.Enabled = reEnableControls

        If reEnableControls Then
            ' reset the EndThingsButton
            If EndThingsButton.Text = "&Cancel" Then
                EndThingsButton.Text = "E&xit"

                Me.AcceptButton = LoadAllDataFilesButton
                Me.CancelButton = EndThingsButton
            End If
        End If

    End Sub

#Region "BackgroundWorker Objects DoWork Event Handlers"
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

        If ImportType = ImportTypeEnum.Compressed Then
            NameBasicsRowCount = CountCompressedFileRows(C.NameBasicsCompressedFileName)

        ElseIf ImportType = ImportTypeEnum.Decompressed Then
            NameBasicsRowCount = CountFileRows(C.NameBasicsDecompFileName)

        End If

    End Sub

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

        If ImportType = ImportTypeEnum.Compressed Then
            TitleAkasRowCount = CountCompressedFileRows(C.TitleAkasCompressedFileName)

        ElseIf ImportType = ImportTypeEnum.Decompressed Then
            TitleAkasRowCount = CountFileRows(C.TitleAkasDecompFileName)

        End If

    End Sub

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

        If ImportType = ImportTypeEnum.Compressed Then
            TitleBasicsRowCount = CountCompressedFileRows(C.TitleBasicsCompressedFileName)

        ElseIf ImportType = ImportTypeEnum.Decompressed Then
            TitleBasicsRowCount = CountFileRows(C.TitleBasicsDecompFileName)

        End If

    End Sub

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

        If ImportType = ImportTypeEnum.Compressed Then
            TitleCrewRowCount = CountCompressedFileRows(C.TitleCrewCompressedFileName)

        ElseIf ImportType = ImportTypeEnum.Decompressed Then
            TitleCrewRowCount = CountFileRows(C.TitleCrewDecompFileName)

        End If

    End Sub

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

        If ImportType = ImportTypeEnum.Compressed Then
            TitleEpisodeRowCount = CountCompressedFileRows(C.TitleEpisodeCompressedFileName)

        ElseIf ImportType = ImportTypeEnum.Decompressed Then
            TitleEpisodeRowCount = CountFileRows(C.TitleEpisodeDecompFileName)

        End If

    End Sub

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

        If ImportType = ImportTypeEnum.Compressed Then
            TitlePrincipalsRowCount = CountCompressedFileRows(C.TitlePrincipalsCompressedFileName)

        ElseIf ImportType = ImportTypeEnum.Decompressed Then
            TitlePrincipalsRowCount = CountFileRows(C.TitlePrincipalsDecompFileName)

        End If

    End Sub

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

        If ImportType = ImportTypeEnum.Compressed Then
            TitleRatingsRowCount = CountCompressedFileRows(C.TitleRatingsCompressedFileName)

        ElseIf ImportType = ImportTypeEnum.Decompressed Then
            TitleRatingsRowCount = CountFileRows(C.TitleRatingsDecompFileName)

        End If

    End Sub

    Private Sub AllArchivesBackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) _
        Handles AllArchivesBackgroundWorker.DoWork

        For Each fileToProcess As String In CountFilesList
            If AllArchivesBackgroundWorker.CancellationPending Then
                CancelledOperations = True

                Exit For
            End If

            If fileToProcess.StartsWith(FolderLocation) Then
                fileToProcess = Path.GetFileName(fileToProcess)
            End If

            If File.Exists(Path.Combine(FolderLocation, fileToProcess)) Then
                Select Case ImportType
                    Case ImportTypeEnum.Compressed
                        Select Case fileToProcess
                            Case C.NameBasicsCompressedFileName
                                MyRawFileInfo(FT.NameBasics).CountedRowCount = CountCompressedFileRows(fileToProcess) - 1

                                NameBasicsRowCount = MyRawFileInfo(FT.NameBasics).CountedRowCount
                                NameBasicsCounted = True

                                ' update the log textbox with the row count for the NameBasics file, 
                                ' as this is the first file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {NameBasicsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleAkasCompressedFileName
                                MyRawFileInfo(FT.TitleAkas).CountedRowCount = CountCompressedFileRows(fileToProcess) - 1

                                TitleAkasRowCount = MyRawFileInfo(FT.TitleAkas).CountedRowCount
                                TitleAkasCounted = True

                                ' update the log textbox with the row count for the TitleAkas file, 
                                ' as this is the second file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleAkasRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleBasicsCompressedFileName
                                MyRawFileInfo(FT.TitleBasics).CountedRowCount = CountCompressedFileRows(fileToProcess) - 1

                                TitleBasicsRowCount = MyRawFileInfo(FT.TitleBasics).CountedRowCount
                                TitleBasicsCounted = True

                                ' update the log textbox with the row count for the TitleBasics file, 
                                ' as this is the third file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleBasicsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleCrewCompressedFileName
                                MyRawFileInfo(FT.TitleCrew).CountedRowCount = CountCompressedFileRows(fileToProcess) - 1

                                TitleCrewRowCount = MyRawFileInfo(FT.TitleCrew).CountedRowCount
                                TitleCrewCounted = True

                                ' update the log textbox with the row count for the TitleCrew file, 
                                ' as this is the fourth file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleCrewRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleEpisodeCompressedFileName
                                MyRawFileInfo(FT.TitleEpisode).CountedRowCount = CountCompressedFileRows(fileToProcess) - 1

                                TitleEpisodeRowCount = MyRawFileInfo(FT.TitleEpisode).CountedRowCount
                                TitleEpisodeCounted = True

                                ' update the log textbox with the row count for the TitleEpisode file, 
                                ' as this is the fifth file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleEpisodeRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitlePrincipalsCompressedFileName
                                MyRawFileInfo(FT.TitlePrincipals).CountedRowCount = CountCompressedFileRows(fileToProcess) - 1

                                TitlePrincipalsRowCount = MyRawFileInfo(FT.TitlePrincipals).CountedRowCount
                                TitlePrincipalsCounted = True

                                ' update the log textbox with the row count for the TitlePrincipals file, 
                                ' as this is the sixth file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitlePrincipalsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleRatingsCompressedFileName
                                MyRawFileInfo(FT.TitleRatings).CountedRowCount = CountCompressedFileRows(fileToProcess) - 1

                                TitleRatingsRowCount = MyRawFileInfo(FT.TitleRatings).CountedRowCount
                                TitleRatingsCounted = True

                                ' update the log textbox with the row count for the TitleRatings file, 
                                ' as this is the seventh file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleRatingsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                        End Select

                    Case ImportTypeEnum.Decompressed
                        Select Case fileToProcess
                            Case C.NameBasicsDecompFileName
                                MyRawFileInfo(FT.NameBasics).CountedRowCount = CountFileRows(fileToProcess)

                                NameBasicsRowCount = MyRawFileInfo(FT.NameBasics).CountedRowCount
                                NameBasicsCounted = True

                                ' update the log textbox with the row count for the NameBasics file, 
                                ' as this is the first file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {NameBasicsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleAkasDecompFileName
                                MyRawFileInfo(FT.TitleAkas).CountedRowCount = CountFileRows(fileToProcess)

                                TitleAkasRowCount = MyRawFileInfo(FT.TitleAkas).CountedRowCount
                                TitleAkasCounted = True

                                ' update the log textbox with the row count for the TitleAkas file, 
                                ' as this is the second file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleAkasRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleBasicsDecompFileName
                                MyRawFileInfo(FT.TitleBasics).CountedRowCount = CountFileRows(fileToProcess)

                                TitleBasicsRowCount = MyRawFileInfo(FT.TitleBasics).CountedRowCount
                                TitleBasicsCounted = True

                                ' update the log textbox with the row count for the TitleBasics file, 
                                ' as this is the third file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleBasicsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleCrewDecompFileName
                                MyRawFileInfo(FT.TitleCrew).CountedRowCount = CountFileRows(fileToProcess)

                                TitleCrewRowCount = MyRawFileInfo(FT.TitleCrew).CountedRowCount
                                TitleCrewCounted = True

                                ' update the log textbox with the row count for the TitleCrew file, 
                                ' as this is the fourth file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleCrewRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleEpisodeDecompFileName
                                MyRawFileInfo(FT.TitleEpisode).CountedRowCount = CountFileRows(fileToProcess)

                                TitleEpisodeRowCount = MyRawFileInfo(FT.TitleEpisode).CountedRowCount
                                TitleEpisodeCounted = True

                                ' update the log textbox with the row count for the TitleEpisode file, 
                                ' as this is the fifth file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleEpisodeRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitlePrincipalsDecompFileName
                                MyRawFileInfo(FT.TitlePrincipals).CountedRowCount = CountFileRows(fileToProcess)

                                TitlePrincipalsRowCount = MyRawFileInfo(FT.TitlePrincipals).CountedRowCount
                                TitlePrincipalsCounted = True

                                ' update the log textbox with the row count for the TitlePrincipals file, 
                                ' as this is the sixth file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitlePrincipalsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                            Case C.TitleRatingsDecompFileName
                                MyRawFileInfo(FT.TitleRatings).CountedRowCount = CountFileRows(fileToProcess)

                                TitleRatingsRowCount = MyRawFileInfo(FT.TitleRatings).CountedRowCount
                                TitleRatingsCounted = True

                                ' update the log textbox with the row count for the TitleRatings file, 
                                ' as this is the seventh file to be processed, and we want to show the 
                                ' user that we're making progress on counting the rows in the files, 
                                ' rather than waiting until all the files are counted and then updating 
                                ' the UI with that info

                                TS.AppendText(ProgressLogTextBox,
                                              $"Row count for {fileToProcess}: {TitleRatingsRowCount.ToString(C.COMMA_MASK)}" & Environment.NewLine)

                        End Select
                End Select
            End If
        Next

    End Sub

    Private Function GetFileTypeBasedOnFileName(fileName As String) As FT

        Dim result As FT = FT.Unknown

        Select Case fileName
            Case C.NameBasicsCompressedFileName,
                 C.NameBasicsDecompFileName
                result = FT.NameBasics

            Case C.TitleAkasCompressedFileName,
                 C.TitleAkasDecompFileName
                result = FT.TitleAkas

            Case C.TitleBasicsCompressedFileName,
                 C.TitleBasicsDecompFileName
                result = FT.TitleBasics

            Case C.TitleCrewCompressedFileName,
                 C.TitleCrewDecompFileName
                result = FT.TitleCrew

            Case C.TitleEpisodeCompressedFileName,
                 C.TitleEpisodeDecompFileName
                result = FT.TitleEpisode

            Case C.TitlePrincipalsCompressedFileName,
                 C.TitlePrincipalsDecompFileName
                result = FT.TitlePrincipals

            Case C.TitleRatingsCompressedFileName,
                 C.TitleRatingsDecompFileName
                result = FT.TitleRatings

            Case Else
                result = FT.Unknown

        End Select

        Return result

    End Function

    Private Sub SqlBackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) _
        Handles SqlBackgroundWorker.DoWork

        ' refer to the backgroundworker by its name: SqlBackgroundWorker

        Dim rowNumber As Integer = 0
        Dim dataRowNumber As Integer = 0

        TS.SetText(ProgressLogTextBox, String.Empty)
        TS.SetText(CurrentImportFileNumberTextBox, String.Empty)

        Dim rawFileType As FT = FT.Unknown

        Dim lastRowCount As Long = 0
        Dim countedRowCount As Long = 0

        With MyRawFileInfo(FT.OVERALL)
            .CurrentStartTime = Now
            .CurrentTime = .CurrentStartTime
        End With

        Dim currentTimeBetweenTransactions As TimeSpan = Nothing
        Dim previousTimeBetweenTransactions As TimeSpan = Nothing
        Dim currentTransactionTime As DateTime = Date.MinValue
        Dim previousTransactionTime As DateTime = Date.MinValue

        Using conn As New SqlConnection(C.IMDB_CONNECTION_STRING)

            conn.Open()

            ' break up the files into the file TYPES based on the 7 files...

            Dim fileInfo As FileInfo = Nothing ' New FileInfo(filePath)
            Dim gzipFileStream As FileStream = Nothing
            Dim decompressedFileStream As FileStream = Nothing
            Dim decompressionStream As Comp.GZipStream = Nothing

            Dim columnNamesList As New List(Of String)

            Dim curentFileNumber As Integer = 1
            Dim maxFileNumber As Integer = InsertDataFilesList.Count

            For Each fileToProcess As String In InsertDataFilesList
                If SqlBackgroundWorker.CancellationPending Then
                    CancelledOperations = True

                    Exit For
                End If

                If fileToProcess.StartsWith(FolderLocation) Then
                    fileToProcess = Path.GetFileName(fileToProcess)
                End If

                rawFileType = GetFileTypeBasedOnFileName(fileToProcess)

                'Select Case fileToProcess
                '    Case C.NameBasicsCompressedFileName,
                '         C.NameBasicsDecompFileName
                '        rawFileType = FT.NameBasics

                '    Case C.TitleAkasCompressedFileName,
                '         C.TitleAkasDecompFileName
                '        rawFileType = FT.TitleAkas

                '    Case C.TitleBasicsCompressedFileName,
                '         C.TitleBasicsDecompFileName
                '        rawFileType = FT.TitleBasics

                '    Case C.TitleCrewCompressedFileName,
                '         C.TitleCrewDecompFileName
                '        rawFileType = FT.TitleCrew

                '    Case C.TitleEpisodeCompressedFileName,
                '         C.TitleEpisodeDecompFileName
                '        rawFileType = FT.TitleEpisode

                '    Case C.TitlePrincipalsCompressedFileName,
                '         C.TitlePrincipalsDecompFileName
                '        rawFileType = FT.TitlePrincipals

                '    Case C.TitleRatingsCompressedFileName,
                '         C.TitleRatingsDecompFileName
                '        rawFileType = FT.TitleRatings

                'End Select

                TS.SetText(OverallEstimatedProcessingTimeTextBox,
                           MyRawFileInfo(FT.OVERALL).EstimatedTotalTimeString)

                With MyRawFileInfo(rawFileType)
                    lastRowCount = .LastRowCount
                    countedRowCount = .CountedRowCount

                    TS.SetText(FileEstimatedProcessingTimeTextBox,
                               .EstimatedTotalTimeString)

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

                Dim myStreamReader As StreamReader = Nothing

                Select Case ProcessFileType
                    Case PFT.Compressed
                        gzipFileStream = IO.File.OpenRead(fileInfo.FullName)

                        decompressionStream = New Comp.GZipStream(gzipFileStream,
                                                                  Comp.CompressionMode.Decompress)

                        myStreamReader = New StreamReader(decompressionStream)

                    Case PFT.Decompressed
                        decompressedFileStream = IO.File.OpenRead(fileInfo.FullName)

                        myStreamReader = New StreamReader(decompressedFileStream)

                End Select

                'Dim myStreamReader As New StreamReader(decompressionStream)
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

                    Select Case ProcessFileType
                        Case PFT.Compressed
                            cmd.CommandText =
                                "TRUNCATE TABLE [Raw].[" & fileInfo.Name & "];"

                        Case PFT.Decompressed
                            cmd.CommandText =
                                "TRUNCATE TABLE [Raw].[" & fileInfo.Name & ".gz];"

                    End Select

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

                            Select Case ProcessFileType
                                Case PFT.Compressed
                                    insertCommandText = " INSERT INTO [Raw].[" & fileInfo.Name & "] " & Environment.NewLine &
                                                        "     ( " & String.Join(", ", columnNamesList.Select(Function(c) c)) & " )" & Environment.NewLine &
                                                        " VALUES " & Environment.NewLine &
                                                        "     ( YYYY );"

                                Case PFT.Decompressed
                                    insertCommandText = " INSERT INTO [Raw].[" & fileInfo.Name & ".gz] " & Environment.NewLine &
                                                        "     ( " & String.Join(", ", columnNamesList.Select(Function(c) c)) & " )" & Environment.NewLine &
                                                        " VALUES " & Environment.NewLine &
                                                        "     ( YYYY );"

                            End Select

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

                                    'If cmdResult <> -1 Then
                                    '    Debug.Print(cmd.CommandText)
                                    '    Debug.Print("cmdResult = " & cmdResult.ToString())
                                    'End If

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

                                    'TS.AppendText(ProgressLogTextBox,
                                    '              rowNumber.ToString() &
                                    '              " Cannot insert Duplicate Key: " &
                                    '              columnNamesList(0) & " = '" &
                                    '              valuesList(0) & "'" & Environment.NewLine)
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
                        If dataRowNumber = 0 Then
                            'Debug.Print("here we are!")
                        End If

                        If ((dataRowNumber Mod 10000) = 0) AndAlso
                            (dataRowNumber > 0) Then
                            If SqlBackgroundWorker.CancellationPending Then
                                Exit Do
                            End If

                            If countedRowCount > 0 Then
                                CurrentUploadFilenameAndRowCount =
                                    Environment.NewLine &
                                    "Processing file: " & CurrentlyUploadingFilename & vbTab &
                                    "Rows Committed to Database: " & dataRowNumber.ToString(C.COMMA_MASK) & vbTab &
                                    "of " & countedRowCount.ToString(C.COMMA_MASK) &
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
                            'Debug.Print(CurrentUploadFilenameAndRowCount)

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

                            MyRawFileInfo(rawFileType).CurrentTime = Now
                            MyRawFileInfo(FT.OVERALL).CurrentTime = MyRawFileInfo(rawFileType).CurrentTime

                            currentTransactionTime = MyRawFileInfo(rawFileType).CurrentTime

                            ' the first commit will be the time between the current transaction time and the start time for the file,
                            ' and subsequent commits will be the time between the current transaction time and the previous transaction time
                            If dataRowNumber <= 10000 Then
                                currentTimeBetweenTransactions =
                                    (currentTransactionTime - MyRawFileInfo(rawFileType).CurrentStartTime)

                            Else
                                currentTimeBetweenTransactions =
                                    (currentTransactionTime - previousTransactionTime)

                            End If

                            MyRawFileInfo(rawFileType).CurrentRowCount = dataRowNumber
                            MyRawFileInfo(FT.OVERALL).CurrentRowCount += 10000          ' not sure about this one, but it should be close enough for the overall time remaining estimate

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

                        'If Not String.IsNullOrEmpty(line) Then
                        '    dataRowNumber += 1
                        '    rowNumber += 1
                        'End If

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

                    MyRawFileInfo(rawFileType).CurrentRowCount = dataRowNumber

                Catch ex As Exception
                    LogErrorsToFile($"Error processing file: {fileToProcess}")
                    LogErrorsToFile($"Command: {cmd.CommandText}")
                    LogErrorsToFile($"Exception: {ex.ToString()}")

                    TS.AppendText(ProgressLogTextBox,
                                  $"Error processing file: {fileToProcess}" & Environment.NewLine)

                    TS.AppendText(ProgressLogTextBox,
                                  $"Command: {cmd.CommandText}" & Environment.NewLine)

                    TS.AppendText(ProgressLogTextBox,
                                  $"Exception: {ex.ToString()}" & Environment.NewLine)

                    'transaction.Rollback()

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
    Private Sub NameBasicsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles NameBasicsBackgroundWorker.RunWorkerCompleted

        NameBasicsCounted = True

        CheckAllCounted()

    End Sub

    Private Sub TitleAkasBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleAkasBackgroundWorker.RunWorkerCompleted

        TitleAkasCounted = True

        CheckAllCounted()

    End Sub

    Private Sub TitleBasicsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleBasicsBackgroundWorker.RunWorkerCompleted

        TitleBasicsCounted = True

        CheckAllCounted()

    End Sub

    Private Sub TitleCrewBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleCrewBackgroundWorker.RunWorkerCompleted

        TitleCrewCounted = True

        CheckAllCounted()

    End Sub

    Private Sub TitleEpisodeBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleEpisodeBackgroundWorker.RunWorkerCompleted

        TitleEpisodeCounted = True

        CheckAllCounted()

    End Sub

    Private Sub TitlePrincipalsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitlePrincipalsBackgroundWorker.RunWorkerCompleted

        TitlePrincipalsCounted = True

        CheckAllCounted()

    End Sub

    Private Sub TitleRatingsBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles TitleRatingsBackgroundWorker.RunWorkerCompleted

        TitleRatingsCounted = True

        CheckAllCounted()

    End Sub

    Private Sub AllArchivesBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles AllArchivesBackgroundWorker.RunWorkerCompleted

        CheckAllCounted()

    End Sub

    Private Sub SqlBackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) _
        Handles SqlBackgroundWorker.RunWorkerCompleted

        ' refer to the backgroundworker by its name: SqlBackgroundWorker
        EndThingsButton.Text = "E&xit"

        ImportDataButton.Enabled = True
        CountArchiveRowsButton.Enabled = True
        CountTsvRowsButton.Enabled = True
        DecompressAfterDownloadCheckBox.Enabled = True

        DownloadUpdatedArchivesButton.Enabled = True
        LoadAllDataFilesButton.Enabled = True
        ChooseFolderButton.Enabled = True
        FolderLocationTextBox.Enabled = True

        Me.AcceptButton = LoadAllDataFilesButton
        Me.CancelButton = EndThingsButton

    End Sub

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

    Public Enum SqlCmdTypeEnum As Integer
        INSERT
        UPDATE
        TRUNCATE
        ADD_CONSTRAINT
        DROP_CONSTRAINT
    End Enum

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

    Private Enum DropAddEnum As Integer
        DROP
        ADD
    End Enum

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

        Dim stepLogMsg As String = "Step " & currentStep.ToString() & " of " & lastStep.ToString()
        Dim basicLogMsg As String = String.Empty
        Dim commandText As String = ""

        Select Case dropOrAdd
            Case DropAddEnum.DROP
                basicLogMsg = "#1 Dropping All Constraints from the IMDB Db Tables" &
                              Environment.NewLine
                commandText = C.AdHoc1List(currentStep)

            Case DropAddEnum.ADD
                basicLogMsg = "#3 Re-Adding all of the Constraints (Keys, Foreign Keys, and Indexes to the IMDB Db Tables" &
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
        '== #4-26 - UPDATE data in [IMDB].[dbo].[Titles] for Votes and Average Ratings

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
                                      "Command Timeout set to: " & RFI.GetTimeStringFromSeconds_General(timeOutForExecution) & " - PLEASE WAIT")
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

    Private Property CountArchiveRowsEnabled As Boolean = False
    Private Property CountTsvRowsEnabled As Boolean = False
    Private Property DecompressAfterDownloadEnabled As Boolean = False

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