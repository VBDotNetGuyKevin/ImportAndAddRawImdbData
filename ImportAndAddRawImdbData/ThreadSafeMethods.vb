''' <summary>
''' This class contains methods that can be used to safely 
''' update controls on a form from a different thread than 
''' the one that created the control. This is necessary 
''' because Windows Forms controls are not thread-safe, 
''' and attempting to update them from a different thread 
''' can lead to unpredictable behavior or exceptions. The 
''' methods in this class use the InvokeRequired property 
''' and the Invoke method to ensure that updates to controls 
''' are performed on the correct thread.
''' </summary>
Public Class ThreadSafeMethods

    ''' <summary>
    ''' This method clears the text of a control in a thread-safe manner.
    ''' </summary>
    ''' <param name="Control">The control whose text is to be cleared.</param>
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

    ''' <summary>
    ''' This method clears the image of a PictureBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[PictureBox]"></param>
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

    ''' <summary>
    ''' This method sets the image of a PictureBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[PictureBox]"></param>
    ''' <param name="localImage"></param>
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
    ''' <summary>
    ''' This method sets the ReadOnly property of a TextBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[TextBox]">The TextBox whose ReadOnly property is to be set.</param>
    ''' <param name="[ReadOnly]">The value to set the ReadOnly property to.</param>
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

    ''' <summary>
    ''' This method sets the text of a form in a thread-safe manner.
    ''' </summary>
    ''' <param name="[Form]">The form whose text is to be set.</param>
    ''' <param name="newText">The new text to set.</param>
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

    ''' <summary>
    ''' This method sets the text of a control in a thread-safe manner.
    ''' </summary>
    ''' <param name="[Control]">The control whose text is to be set.</param>
    ''' <param name="newText">The new text to set.</param>
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

    ''' <summary>
    ''' This method appends text to a control's existing text in a thread-safe manner.
    ''' </summary>
    ''' <param name="[Control]">The control whose text is to be appended.</param>
    ''' <param name="textToAppend">The text to append to the control's existing text.</param>
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

    ''' <summary>
    ''' This method sets the Enabled property of a control in a thread-safe manner.
    ''' </summary>
    ''' <param name="[Control]">The control whose Enabled property is to be set.</param>
    ''' <param name="enabled">The value to set the Enabled property to.</param>
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

    ''' <summary>
    ''' This method sets the maximum value of a ProgressBar in a thread-safe manner.
    ''' </summary>
    ''' <param name="[ProgressBar]">The ProgressBar whose maximum value is to be set.</param>
    ''' <param name="maximum">The maximum value to set.</param>
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

    ''' <summary>
    ''' This method sets the minimum value of a ProgressBar in a thread-safe manner.
    ''' </summary>
    ''' <param name="[ProgressBar]">The ProgressBar whose minimum value is to be set.</param>
    ''' <param name="minimum">The minimum value to set.</param>
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

    ''' <summary>
    ''' This method sets the value of a ProgressBar in a thread-safe manner.
    ''' </summary>
    ''' <param name="[ProgressBar]">The ProgressBar whose value is to be set.</param>
    ''' <param name="value">The value to set.</param>
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

    ''' <summary>
    ''' This method sets the visibility of a control in a thread-safe manner.
    ''' </summary>
    ''' <param name="[Control]">The control whose visibility is to be set.</param>
    ''' <param name="visible">The value to set the Visible property to.</param>
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

    ''' <summary>
    ''' This method sets the selected index of a ComboBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[ComboBox]">The ComboBox whose selected index is to be set.</param>
    ''' <param name="selectedIndex">The value to set the SelectedIndex property to.</param> 
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

    ''' <summary>
    ''' This method sets the Checked property of a CheckBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[CheckBox]">The CheckBox whose Checked property is to be set.</param>
    ''' <param name="checked">The value to set the Checked property to.</param>
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
    ''' <summary>
    ''' This method gets the ReadOnly property of a TextBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[TextBox]">The TextBox whose ReadOnly property is to be retrieved.</param>
    ''' <returns>The value of the ReadOnly property.</returns>
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

    ''' <summary>
    ''' This method gets the Text property of a Control in a thread-safe manner.
    ''' </summary>
    ''' <param name="[Control]">The Control whose Text property is to be retrieved.</param>
    ''' <returns>The value of the Text property.</returns>
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

    ''' <summary>
    ''' This method gets the Checked property of a CheckBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[CheckBox]">The CheckBox whose Checked property is to be retrieved.</param>
    ''' <returns>The value of the Checked property.</returns>
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

    ''' <summary>
    ''' This method gets the SelectedIndex property of a ComboBox in a thread-safe manner.
    ''' </summary>
    ''' <param name="[ComboBox]">The ComboBox whose SelectedIndex property is to be retrieved.</param>
    ''' <returns>The value of the SelectedIndex property.</returns>
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

    ''' <summary>
    ''' This method gets the Enabled property of a Control in a thread-safe manner.
    ''' </summary>
    ''' <param name="[Control]">The Control whose Enabled property is to be retrieved.</param>
    ''' <returns>The value of the Enabled property.</returns>
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