''' <summary>
''' Provides a delegate-based implementation of <see cref="ICommand"/>.
''' </summary>
Public Class RelayCommand
    Implements ICommand


    Private ReadOnly cgExecute As Action
    Private ReadOnly cgCanExecute As Func(Of Boolean)


    ''' <summary>
    ''' Initializes a new instance of the <see cref="RelayCommand"/> class.
    ''' </summary>
    ''' <param name="execute">The delegate to call when the command is executed.</param>
    Public Sub New(execute As Action)
        Me.New(execute, Nothing)
    End Sub


    ''' <summary>
    ''' Initializes a new instance of the <see cref="RelayCommand"/> class.
    ''' </summary>
    ''' <param name="execute">The delegate to call when the command is executed.</param>
    ''' <param name="canExecute">The delegate to call to check if the command can be executed.</param>
    Public Sub New(
            execute As Action,
            canExecute As Func(Of Boolean)
        )

        cgExecute = execute
        cgCanExecute = canExecute
    End Sub


    ''' <summary>
    ''' Determines whether the command can be executed.
    ''' </summary>
    ''' <param name="parameter">The parameter to use.</param>
    ''' <returns>True if the command can be executed; otherwise, False.</returns>
    <DebuggerStepThrough()>
    Public Function CanExecute(parameter As Object) As Boolean _
        Implements ICommand.CanExecute

        Return (cgCanExecute Is Nothing) OrElse cgCanExecute()
    End Function


    ''' <summary>
    ''' Raises the <see cref="CanExecuteChanged"/> event.
    ''' </summary>
    Public Sub InvalidateCanExecute()
        RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub


    ''' <summary>
    ''' Raised when command's CanExecute value is changed.
    ''' </summary>
    Public Custom Event CanExecuteChanged As EventHandler _
        Implements ICommand.CanExecuteChanged

        AddHandler(value As EventHandler)
            If cgCanExecute IsNot Nothing Then
                AddHandler CommandManager.RequerySuggested, value
            End If
        End AddHandler

        RemoveHandler(value As EventHandler)
            If cgCanExecute IsNot Nothing Then
                RemoveHandler CommandManager.RequerySuggested, value
            End If
        End RemoveHandler

        RaiseEvent(sender As Object, e As EventArgs)
            ' Block intentionally left empty.
        End RaiseEvent
    End Event


    ''' <summary>
    ''' Executes the command.
    ''' </summary>
    ''' <param name="parameter">The parameter to use.</param>
    <DebuggerStepThrough()>
    Public Sub Execute(parameter As Object) _
        Implements ICommand.Execute

        cgExecute()
    End Sub

End Class
