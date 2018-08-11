Imports System.Collections.ObjectModel

Public MustInherit Class BaseNode

    Public Property Parent As BaseNode
    Public Property Nodes As New ObservableCollection(Of BaseNode)


    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(Sub()
                                                                          If IsNothing(Parent) Then Return
                                                                          Parent.Nodes.Remove(Me)
                                                                      End Sub)

End Class