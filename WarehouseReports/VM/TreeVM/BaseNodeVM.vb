Imports System.Collections.ObjectModel

Public Class BaseNodeVM

    Public Property RootNode As BaseNodeVM
    Public Property Nodes As New ObservableCollection(Of BaseNodeVM)


    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(Sub() RootNode.Nodes.Remove(Me))

End Class