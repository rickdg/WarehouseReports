Imports System.Collections.ObjectModel

Public Class BaseNodeVM

    Public Property RootNode As BaseNodeVM
    Public Property Nodes As New ObservableCollection(Of BaseNodeVM)


    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(Sub()
                                                                          If IsNothing(RootNode) Then Return
                                                                          RootNode.Nodes.Remove(Me)
                                                                      End Sub)

End Class