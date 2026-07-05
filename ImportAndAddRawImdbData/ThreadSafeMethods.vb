Public Class ThreadSafeMethods

    Public Shared Sub ClearControlText(ByVal [Control] As Control)

        With [Control]
            If .InvokeRequired Then
                .Invoke(New ClearControlText_Delegate(AddressOf ClearControlText),
                        New Object() {[Control]})
            Else
                .Text = String.Empty
            End If
        End With
    End Sub
    Delegate Sub ClearControlText_Delegate(ByVal [Control] As Control)

    Public Shared Sub ClearPictureBox(ByVal [PictureBox] As PictureBox)
        With [PictureBox]
            If .InvokeRequired Then
                .Invoke(New ClearPictureBox_Delegate(AddressOf ClearPictureBox),
                        New Object() {[PictureBox]})
            Else
                .Image = Nothing
            End If
        End With
    End Sub
    Delegate Sub ClearPictureBox_Delegate(ByVal [PictureBox] As PictureBox)

    Public Shared Sub SetPictureBoxImage(ByVal [PictureBox] As PictureBox, ByVal localImage As Image)
        With [PictureBox]
            If .InvokeRequired Then
                .Invoke(New SetPictureBoxImage_Delegate(AddressOf SetPictureBoxImage),
                        New Object() {[PictureBox], localImage})
            Else
                .Image = localImage
            End If
        End With
    End Sub
    Delegate Sub SetPictureBoxImage_Delegate(ByVal [PictureBox] As PictureBox, ByVal localImage As Image)

#Region "Set Methods"
    Public Shared Sub SetTextBoxReadOnly(ByVal [TextBox] As TextBox, ByVal [ReadOnly] As Boolean)
        With [TextBox]
            If .InvokeRequired Then
                .Invoke(New SetTextBoxReadOnly_Delegate(AddressOf SetTextBoxReadOnly),
                        New Object() {[TextBox], [ReadOnly]})
            Else
                .ReadOnly = [ReadOnly]
            End If
        End With
    End Sub
    Delegate Sub SetTextBoxReadOnly_Delegate(ByVal [TextBox] As TextBox, ByVal [ReadOnly] As Boolean)

    Public Shared Sub SetFormText(ByVal [Form] As Form, ByVal newText As String)
        With [Form]
            If .InvokeRequired Then
                .Invoke(New SetFormText_Delegate(AddressOf SetFormText),
                        New Object() {[Form], newText})
            Else
                .Text = newText
            End If
        End With
    End Sub
    Delegate Sub SetFormText_Delegate(ByVal [Form] As Form, ByVal newText As String)

    Public Shared Sub SetText(ByVal [Control] As Control, ByVal newText As String)
        With [Control]
            If .InvokeRequired Then
                .Invoke(New SetText_Delegate(AddressOf SetText),
                New Object() {[Control], newText})
            Else
                .Text = newText
            End If
        End With
    End Sub
    Delegate Sub SetText_Delegate(ByVal [Control] As Control, ByVal newText As String)

    Public Shared Sub AppendText(ByVal [Control] As Control, ByVal textToAppend As String)
        With [Control]
            If .InvokeRequired Then
                .Invoke(New AppendText_Delegate(AddressOf AppendText),
                New Object() {[Control], textToAppend})
            Else
                .Text &= textToAppend
            End If
        End With
    End Sub
    Delegate Sub AppendText_Delegate(ByVal [Control] As Control, ByVal textToAppend As String)

    Public Shared Sub SetEnabled(ByVal [Control] As Control, ByVal enabled As Boolean)
        With [Control]
            If .InvokeRequired Then
                .Invoke(New SetEnabled_Delegate(AddressOf SetEnabled),
                New Object() {[Control], enabled})
            Else
                .Enabled = enabled
            End If
        End With

    End Sub
    Delegate Sub SetEnabled_Delegate(ByVal [Control] As Control, ByVal enabled As Boolean)

    Public Shared Sub SetMaximum(ByVal [ProgressBar] As ProgressBar, ByVal maximum As Integer)
        With [ProgressBar]
            If .InvokeRequired Then
                .Invoke(New SetMaximum_Delegate(AddressOf SetMaximum),
                New Object() {[ProgressBar], maximum})
            Else
                .Maximum = maximum
            End If
        End With
    End Sub
    Delegate Sub SetMaximum_Delegate(ByVal [ProgressBar] As ProgressBar, ByVal maximum As Integer)

    Public Shared Sub SetMinimum(ByVal [ProgressBar] As ProgressBar, ByVal minimum As Integer)
        With [ProgressBar]
            If .InvokeRequired Then
                .Invoke(New SetMinimum_Delegate(AddressOf SetMinimum),
                New Object() {[ProgressBar], minimum})
            Else
                .Minimum = minimum
            End If
        End With
    End Sub
    Delegate Sub SetMinimum_Delegate(ByVal [ProgressBar] As ProgressBar, ByVal minimum As Integer)

    Public Shared Sub SetValue(ByVal [ProgressBar] As ProgressBar, ByVal value As Integer)
        With [ProgressBar]
            If .InvokeRequired Then
                .Invoke(New SetValue_Delegate(AddressOf SetValue),
                New Object() {[ProgressBar], value})
            Else
                .Value = value
            End If
        End With
    End Sub
    Delegate Sub SetValue_Delegate(ByVal [ProgressBar] As ProgressBar, ByVal value As Integer)


    Public Shared Sub SetVisible(ByVal [Control] As Control, ByVal visible As Boolean)
        With [Control]
            If .InvokeRequired Then
                .Invoke(New SetVisible_Delegate(AddressOf SetVisible),
                New Object() {[Control], visible})
            Else
                .Visible = visible
            End If
        End With
    End Sub
    Delegate Sub SetVisible_Delegate(ByVal [Control] As Control, ByVal visible As Boolean)

    Public Shared Sub SetSelectedIndex(ByVal [ComboBox] As ComboBox, ByVal selectedIndex As Integer)
        With [ComboBox]
            If .InvokeRequired Then
                .Invoke(New SetSelectedIndex_Delegate(AddressOf SetSelectedIndex),
                        New Object() {[ComboBox], selectedIndex})
            Else
                .SelectedIndex = selectedIndex
            End If
        End With
    End Sub
    Delegate Sub SetSelectedIndex_Delegate(ByVal [ComboBox] As ComboBox, ByVal selectedIndex As Integer)

    Public Shared Sub SetChecked(ByVal [CheckBox] As CheckBox, ByVal checked As Boolean)
        With [CheckBox]
            If .InvokeRequired Then
                .Invoke(New SetChecked_Delegate(AddressOf SetChecked),
                        New Object() {[CheckBox], checked})
            Else
                .Checked = checked
            End If
        End With
    End Sub
    Delegate Sub SetChecked_Delegate(ByVal [CheckBox] As CheckBox, ByVal checked As Boolean)

#End Region

#Region "Get Methods"
    Public Shared Function GetTextBoxReadOnly(ByVal [TextBox] As TextBox) As Boolean
        Dim returnValue As Boolean = False
        With [TextBox]
            If .InvokeRequired Then
                returnValue = CBool(.Invoke(New GetTextBoxReadOnly_Delegate(AddressOf GetTextBoxReadOnly),
                New Object() {[TextBox]}))
            Else
                returnValue = .ReadOnly
            End If
        End With
        Return returnValue
    End Function
    Delegate Function GetTextBoxReadOnly_Delegate(ByVal [TextBox] As TextBox) As Boolean

    Public Shared Function GetText(ByVal [Control] As Control) As String
        Dim returnValue As String = String.Empty
        With [Control]
            If .InvokeRequired Then
                returnValue = CStr(.Invoke(New GetText_Delegate(AddressOf GetText),
                New Object() {[Control]}))
            Else
                returnValue = .Text
            End If
        End With
        Return returnValue
    End Function
    Delegate Function GetText_Delegate(ByVal [Control] As Control) As String

    Public Shared Function GetChecked(ByVal [CheckBox] As CheckBox) As Boolean
        Dim returnValue As Boolean = False
        With [CheckBox]
            If .InvokeRequired Then
                returnValue = CBool(.Invoke(New GetChecked_Delegate(AddressOf GetChecked),
                New Object() {[CheckBox]}))
            Else
                returnValue = .Checked
            End If
        End With
        Return returnValue
    End Function
    Delegate Function GetChecked_Delegate(ByVal [CheckBox] As CheckBox) As Boolean

    Public Shared Function GetSelectedIndex(ByVal [ComboBox] As ComboBox) As Integer
        Dim returnValue As Integer = -1
        With [ComboBox]
            If .InvokeRequired Then
                returnValue = CInt(.Invoke(New GetSelectedIndex_Delegate(AddressOf GetSelectedIndex),
                                           New Object() {[ComboBox]}))
            Else
                returnValue = .SelectedIndex
            End If
        End With
        Return returnValue
    End Function
    Delegate Function GetSelectedIndex_Delegate(ByVal [ComboBox] As ComboBox) As Integer

    Public Shared Function GetEnabled(ByVal [Control] As Control) As Boolean
        Dim returnValue As Boolean = False
        With [Control]
            If .InvokeRequired Then
                returnValue = CBool(.Invoke(New GetEnabled_Delegate(AddressOf GetEnabled),
                                            New Object() {[Control]}))
            Else
                returnValue = .Enabled
            End If
        End With
        Return returnValue
    End Function
    Delegate Function GetEnabled_Delegate(ByVal [Control] As Control) As Boolean
#End Region

End Class