Imports FirstFloor.ModernUI.Presentation
Imports Microsoft.Win32

Namespace Pages
    Public Class DataBaseManagementVM
        Inherits NotifyPropertyChanged

        Public ReadOnly Property CmdLoadTasks As ICommand = New RelayCommand(AddressOf LoadTasksExecute)
        Private Sub LoadTasksExecute(obj As Object)
            Dim DialogWindow As New OpenFileDialog With {.Title = "Выбрать файл"}
            If Not DialogWindow.ShowDialog Then Return
            DataLoader.LoadTasks(DialogWindow.FileName)
        End Sub

    End Class
End Namespace