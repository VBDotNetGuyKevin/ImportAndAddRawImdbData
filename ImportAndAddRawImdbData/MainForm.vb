Imports System.IO
Imports System.Net.Http

Public Class MainForm
    Private Property FolderLocation As String = String.Empty
    Private Property LocationExists As Boolean = False

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

    Private Sub MainForm_Load(sender As Object, e As EventArgs) _
        Handles Me.Load

        With My.Settings
            .Reload()
            Me.FolderLocation = .FolderLocation
            FolderLocationTextBox.Text = Me.FolderLocation
        End With

    End Sub

    Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs) _
        Handles Me.FormClosed

        With My.Settings
            .FolderLocation = Me.FolderLocation
            .Save()
        End With

    End Sub

    Private Sub FolderLocationTextBox_TextChanged(sender As Object, e As EventArgs) _
        Handles FolderLocationTextBox.TextChanged

        LocationExists = Directory.Exists(FolderLocationTextBox.Text)

        DownloadUpdatedArchivesButton.Enabled = LocationExists
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

    ' 7 files to download, each around 1.5GB, so we need to do this asynchronously and with progress reporting
    ' https://datasets.imdbws.com/name.basics.tsv.gz
    ' https://datasets.imdbws.com/title.akas.tsv.gz
    ' https://datasets.imdbws.com/title.basics.tsv.gz
    ' https://datasets.imdbws.com/title.crew.tsv.gz
    ' https://datasets.imdbws.com/title.principals.tsv.gz
    ' https://datasets.imdbws.com/title.ratings.tsv.gz
    ' https://datasets.imdbws.com/title.episodes.tsv.gz


    Public Async Function DownloadFileWithProgress(url As String,
                                                   destinationPath As String) As Task

        Using client As New HttpClient()
            ' Get headers first without downloading the whole body
            Using response = Await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)
                response.EnsureSuccessStatusCode()

                Dim totalBytes = response.Content.Headers.ContentLength

                Using contentStream = Await response.Content.ReadAsStreamAsync(),
                  fileStream = New FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, True)

                    Dim buffer(8191) As Byte
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
                            Console.WriteLine($"Progress: {progress:F2}%")
                        End If
                    Loop While True
                End Using
            End Using
        End Using

    End Function

End Class