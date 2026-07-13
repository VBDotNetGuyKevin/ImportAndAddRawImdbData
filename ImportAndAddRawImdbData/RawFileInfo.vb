Imports FT = ImportAndAddRawImdbData.RawFileInfo.FileTypeEnum

''' <summary>
''' This class is used to store information about the 
''' current file being processed, including the file type, 
''' start and end times, estimated commit counts, and 
''' estimated time remaining. It also provides methods 
''' to calculate elapsed time and format time strings.
''' </summary>
Public Class RawFileInfo

    ''' <summary>
    ''' This enumeration defines the different types of files that can be processed.
    ''' </summary>
    Public Enum FileTypeEnum As Integer
        Unknown = -1
        OVERALL = 0
        NameBasics = 1
        TitleAkas = 2
        TitleBasics = 3
        TitleCrew = 4
        TitleEpisode = 5
        TitlePrincipals = 6
        TitleRatings = 7
    End Enum

    ''' <summary>
    ''' This property indicates whether the processing of the current file has been completed.
    ''' </summary>
    ''' <returns></returns>
    Public Property CompletedProcessing As Boolean = False

    ''' <summary>
    ''' This is the type of the current file being processed
    ''' </summary>
    Private _FileType As FT
    ''' <summary>
    ''' This property gets or sets the type of the current file being processed
    ''' </summary>
    ''' <returns></returns>
    Public Property FileType As FT
        Get
            Return _FileType
        End Get
        Set(value As FT)
            _FileType = value
        End Set
    End Property

    ''' <summary>
    ''' This is the start time of the current run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    Private _CurrentStartTime As Date = Date.MinValue
    ''' <summary>
    ''' This property gets or sets the start time of the current run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentStartTime As Date
        Get
            Return _CurrentStartTime
        End Get
        Set(value As Date)
            _CurrentStartTime = value
        End Set
    End Property

    ''' <summary>
    ''' This is the end time of the current run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    Private _CurrentEndTime As Date = Date.MinValue
    ''' <summary>
    ''' This property gets or sets the end time of the current run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentEndTime As Date
        Get
            Return _CurrentEndTime
        End Get
        Set(value As Date)
            _CurrentEndTime = value
            CompletedProcessing = True
        End Set
    End Property

    ''' <summary>
    ''' This is the start time of the previous run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    Private _PreviousStartTime As Date = Date.MinValue
    ''' <summary>
    ''' This property gets or sets the start time of the previous run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property PreviousStartTime As Date
        Get
            Return _PreviousStartTime
        End Get
        Set(value As Date)
            _PreviousStartTime = value
        End Set
    End Property

    ''' <summary>
    ''' This is the end time of the previous run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    Private _PreviousEndTime As Date = Date.MinValue
    ''' <summary>
    ''' This property gets or sets the end time of the previous run, which is used to calculate 
    ''' the estimated time to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property PreviousEndTime As Date
        Get
            Return _PreviousEndTime
        End Get
        Set(value As Date)
            _PreviousEndTime = value
        End Set
    End Property

    ''' <summary>
    ''' This is the estimated total number of commits to 
    ''' complete processing the current file
    ''' </summary>
    Private _EstimatedCommitCount As Integer
    ''' <summary>
    ''' This property gets or sets the estimated total number of commits to 
    ''' complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property EstimatedCommitCount As Integer
        Get
            Return _EstimatedCommitCount
        End Get
        Private Set(value As Integer)
            _EstimatedCommitCount = value
        End Set
    End Property

    ''' <summary>
    ''' This is the estimated remaining number of commits to complete 
    ''' processing the current file
    ''' </summary>
    Private _EstimatedRemainingCommitCount As Integer
    ''' <summary>
    ''' This property gets or sets the estimated remaining number of 
    ''' commits to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property EstimatedRemainingCommitCount As Integer
        Get
            Return _EstimatedRemainingCommitCount
        End Get
        Private Set(value As Integer)
            _EstimatedRemainingCommitCount = value
        End Set
    End Property

    ''' <summary>
    ''' This is the time of the current commit, which is used to 
    ''' calculate the estimated time to complete processing the 
    ''' current file
    ''' </summary>
    Private _CurrentCommitTime As Date = Date.MinValue
    ''' <summary>
    ''' This property gets or sets the time of the current commit, which is used to 
    ''' calculate the estimated time to complete processing the 
    ''' current file
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentCommitTime As Date
        Get
            Return _CurrentCommitTime
        End Get
        Set(value As Date)
            _CurrentCommitTime = value

            ' this is the first commit, so we will set the 
            ' previous commit time to the current commit time
            If Not Initializing Then
                If (PreviousCommitTime = Date.MinValue) Then
                    PreviousCommitTime = value
                End If

                If ((Not PreviousCommitTime = Date.MinValue) AndAlso
                    (Not value = Date.MinValue)) Then
                    ' we know that there has been at least 2 commits performed, so we can estimate 
                    ' the total time based on the amount of time between commits
                    AmountOfSecondsPerCommit =
                        CInt(Math.Truncate(CType((value - PreviousCommitTime), TimeSpan).TotalSeconds))
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' This is the estimated amount of time in seconds to complete processing the current file
    ''' </summary>
    Dim _AmountOfSecondsPerCommit As Integer = 4 ' this is an initial guestimate based on my own system
    ''' <summary>
    ''' This property gets or sets the estimated amount of time in seconds to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property AmountOfSecondsPerCommit As Integer
        Get
            Return _AmountOfSecondsPerCommit
        End Get
        Private Set(value As Integer)
            _AmountOfSecondsPerCommit = value
        End Set
    End Property

    ''' <summary>
    ''' This is the time of the previous commit, which is 
    ''' used to calculate the estimated time to complete 
    ''' processing the current file
    ''' </summary>
    Private _PreviousCommitTime As Date = Date.MinValue
    ''' <summary>
    ''' This property gets or sets the time of the previous commit, which is 
    ''' used to calculate the estimated time to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property PreviousCommitTime As Date
        Get
            Return _PreviousCommitTime
        End Get
        Set(value As Date)
            _PreviousCommitTime = value
        End Set
    End Property

    ''' <summary>
    ''' This is the estimated total time in the format of HH:MM:SS
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property EstimatedTotalTimeString As String
        Get
            Return GetTimeStringFromSeconds(EstimatedNumberOfSeconds)
        End Get
    End Property


    ''' <summary>
    ''' This is the estimated total time in seconds to complete 
    ''' processing the current file
    ''' </summary>
    Private _EstimatedNumberOfSeconds As Integer
    ''' <summary>
    ''' This property gets or sets the estimated total time in 
    ''' seconds to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property EstimatedNumberOfSeconds As Integer
        Get
            Return _EstimatedNumberOfSeconds
        End Get
        Private Set(value As Integer)
            _EstimatedNumberOfSeconds = value
        End Set
    End Property

    ''' <summary>
    ''' This is the estimated remaining time in the format of HH:MM:SS
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property EstimatedRemainingTimeString As String
        Get
            Return GetTimeStringFromSeconds(EstimatedRemainingSeconds)
        End Get
    End Property

    ''' <summary>
    ''' This is the estimated remaining time in seconds to 
    ''' complete processing the current file
    ''' </summary>
    Private _EstimatedRemainingSeconds As Integer
    ''' <summary>
    ''' This property gets or sets the estimated remaining time 
    ''' in seconds to complete processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property EstimatedRemainingSeconds As Integer
        Get
            Return _EstimatedRemainingSeconds
        End Get
        Private Set(value As Integer)
            _EstimatedRemainingSeconds = value
        End Set
    End Property

    ''' <summary>
    ''' This is the elapsed time in seconds since the start of processing the current file
    ''' </summary>
    Private _ElapsedSeconds As Integer
    ''' <summary>
    ''' This property gets or sets the elapsed time in seconds since the start of processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property ElapsedSeconds As Integer
        Get
            Return _ElapsedSeconds
        End Get
        Private Set(value As Integer)
            _ElapsedSeconds = value
        End Set
    End Property

    ''' <summary>
    ''' This is the elapsed time in the format of HH:MM:SS
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ElapsedTimeString As String
        Get
            Return GetTimeStringFromSeconds(ElapsedSeconds)
        End Get
    End Property

    ''' <summary>
    ''' This is the current time, which is used to calculate the elapsed time 
    ''' since the start of processing the current file
    ''' </summary>
    Private _CurrentTime As Date = Date.MinValue
    ''' <summary>
    ''' This property gets or sets the current time, which is used to calculate 
    ''' the elapsed time since the start of processing the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentTime As Date
        Get
            Return _CurrentTime
        End Get
        Set(value As Date)
            _CurrentTime = value

            If ((Not Initializing) AndAlso
                (Not CompletedProcessing)) Then
                ' as long as the current end time is not the default value, 
                ' we can calculate the elapsed seconds each time the current time is updated
                ElapsedSeconds =
                    CInt(Math.Truncate(CType((value - CurrentStartTime), TimeSpan).TotalSeconds))
            End If
        End Set
    End Property

    ''' <summary>
    ''' This is the saved row count from the last time (or zero if no previous runs)
    ''' </summary>
    Private _LastRowCount As Long = 0
    ''' <summary>
    ''' Saved Row Count from the last time (or zero if no previous runs)
    ''' </summary>
    ''' <returns></returns>
    Public Property LastRowCount As Long
        Get
            Return _LastRowCount
        End Get
        Set(value As Long)
            _LastRowCount = value

            If ((Not Initializing) AndAlso
                (value > 0)) Then
                ' once we have the last row count, we can estimate the number 
                ' of commits and the estimated time to complete
                Dim modCommits As Integer =
                    (value Mod Constants.DEFAULT_COMMIT_COUNT)

                Dim quotCommits As Integer =
                    (value \ Constants.DEFAULT_COMMIT_COUNT)

                EstimatedCommitCount = quotCommits

                If modCommits > 0 Then
                    EstimatedCommitCount += 1
                End If

                EstimatedNumberOfSeconds =
                    (EstimatedCommitCount * AmountOfSecondsPerCommit)
            End If
        End Set
    End Property

    Private _ToBeCounted As Boolean = False
    ''' <summary>
    ''' Indicates whether or not the rows need to be counted. If not, 
    ''' we use the last row count as the total expected number of rows.
    ''' </summary>
    ''' <returns></returns>
    Public Property ToBeCounted As Boolean
        Get
            Return _ToBeCounted
        End Get
        Set(value As Boolean)
            _ToBeCounted = value
        End Set
    End Property

    Private _IsBeingCounted As Boolean = False
    ''' <summary>
    ''' Indicates whether or not the rows are currently being counted. If not, 
    ''' we use the last row count as the total expected number of rows.
    ''' </summary>
    ''' <returns></returns>
    Public Property IsBeingCounted As Boolean
        Get
            Return _IsBeingCounted
        End Get
        Set(value As Boolean)
            _IsBeingCounted = value
        End Set
    End Property

    Private _HasBeenCounted As Boolean = False

    ''' <summary>
    ''' Indicates whether or not the rows have been counted. If not, 
    ''' we use the last row count as the total expected number of rows.
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBeenCounted As Boolean
        Get
            Return _HasBeenCounted
        End Get
        Set(value As Boolean)
            _HasBeenCounted = value
        End Set
    End Property

    ''' <summary>
    ''' This is the current number of counted rows for the current file
    ''' </summary>
    Private _CountedRows As Long = 0
    ''' <summary>
    ''' This is the current number of counted rows for the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property CountedRows As Long
        Get
            Return _CountedRows
        End Get
        Set(value As Long)
            _CountedRows = value

            HasBeenCounted = True

            If ((Not Initializing) AndAlso
                (Not CompletedProcessing) AndAlso
                (value > 0)) Then
                ' once we have the current row count, we can estimate the 
                ' number of commits and the estimated time remaining to complete
                ' this will be updated each time a commit is performed, 
                ' so that the estimated time remaining is updated as well

                Dim modCommits As Integer =
                    (value Mod Constants.DEFAULT_COMMIT_COUNT)

                Dim quotCommits As Integer =
                    (value \ Constants.DEFAULT_COMMIT_COUNT)

                EstimatedCommitCount = quotCommits

                If modCommits > 0 Then
                    EstimatedCommitCount += 1
                End If

                EstimatedNumberOfSeconds =
                    (EstimatedCommitCount * AmountOfSecondsPerCommit)
            End If
        End Set
    End Property

    Private _RemainingRowCount As Long = 0
    Public Property RemainingRowCount As Long
        Get
            Return _RemainingRowCount
        End Get
        Private Set(value As Long)
            _RemainingRowCount = value
        End Set
    End Property

    Public ReadOnly Property ProgressCompleted As Integer
        Get
            Return CInt(Math.Round((CurrentRowNumber / CountedRows) * 100))
        End Get
    End Property

    ''' <summary>
    ''' This is the current number of committed rows for the current file
    ''' </summary>
    Private _CurrentRowNumber As Long = 0

    ''' <summary>
    ''' This is the current number of committed rows for the current file
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentRowNumber As Long
        Get
            Return _CurrentRowNumber
        End Get
        Set(value As Long)
            _CurrentRowNumber = value

            ' once we have the current row count, we can estimate the 
            ' number of commits and the estimated time remaining to complete
            ' this will be updated each time a commit is performed, 
            ' so that the estimated time remaining is updated as well

            If ((Not Initializing) AndAlso
                (Not CompletedProcessing) AndAlso
                (value > 0)) Then

                Dim modCommits As Integer = 0
                Dim quotCommits As Integer = 0

                If HasBeenCounted Then
                    ' if the rows were actually counted, then go with that value
                    modCommits = ((CountedRows - value) Mod Constants.DEFAULT_COMMIT_COUNT)
                    quotCommits = ((CountedRows - value) \ Constants.DEFAULT_COMMIT_COUNT)

                Else
                    ' otherwise, use the previously saved RowCount for the given file (if there is one)
                    If LastRowCount > 0 Then
                        modCommits = ((LastRowCount - value) Mod Constants.DEFAULT_COMMIT_COUNT)
                        quotCommits = ((LastRowCount - value) \ Constants.DEFAULT_COMMIT_COUNT)
                    End If

                End If

                EstimatedRemainingCommitCount = quotCommits

                If modCommits > 0 Then
                    EstimatedRemainingCommitCount += 1
                End If

                EstimatedRemainingSeconds =
                    (EstimatedRemainingCommitCount * AmountOfSecondsPerCommit)

                RemainingRowCount = (CountedRows - CurrentRowNumber)
            End If
        End Set
    End Property

    ''' <summary>
    ''' This property indicates whether the object is currently being initialized. 
    ''' It is used to prevent certain calculations from being performed during initialization.
    ''' </summary>
    ''' <returns></returns>
    Private Property Initializing As Boolean = True

    ''' <summary>
    ''' This constructor initializes a new instance of the RawFileInfo class with the specified file type.
    ''' </summary>
    ''' <param name="fileType"></param>
    Public Sub New(fileType As FT)

        Me.FileType = fileType

        Initializing = False

    End Sub

    Public Shared Function GetTimeStringFromSeconds(totalSeconds As Integer) As String

        'Dim hours As Integer = totalSeconds \ 3600
        'Dim minutes As Integer = (totalSeconds Mod 3600) \ 60
        'Dim seconds As Integer = totalSeconds Mod 60
        Dim result As String = String.Empty

        Dim hours As Integer =
            (totalSeconds \ (60 * 60))

        Dim minutes As Integer =
            (
                (totalSeconds \ 60) -
                (hours * 60)
            )

        Dim seconds As Integer =
            (
                totalSeconds -
                (hours * 60 * 60) -
                (minutes * 60)
            )

        result = $"{hours:D2}:{minutes:D2}:{seconds:D2}"

        If result.StartsWith("00:") Then
            result = result.Substring(3)
        End If

        Return result

    End Function

    Public Shared Function GetTimeStringFromSeconds_General(totalSeconds As Integer) As String

        Dim result As String = String.Empty

        Dim hours As Integer =
            (totalSeconds \ (60 * 60))

        Dim minutes As Integer =
            (
                (totalSeconds \ 60) -
                (hours * 60)
            )

        Dim seconds As Integer =
            (
                totalSeconds -
                (hours * 60 * 60) -
                (minutes * 60)
            )

        If hours > 0 Then
            result = $"{hours:D2}:{minutes:D2}:{seconds:D2}"

        ElseIf minutes > 0 Then
            result = $"{minutes:D2}:{seconds:D2}"

        Else
            result = $"{seconds:D2}"

        End If

        Return result
    End Function

End Class