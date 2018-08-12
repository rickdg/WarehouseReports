Imports System.Collections.ObjectModel

Public MustInherit Class BaseNodeVM

    Public Property Parent As BaseNodeVM
    Public Property Nodes As New ObservableCollection(Of BaseNodeVM)


    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(Sub()
                                                                          If IsNothing(Parent) Then Return
                                                                          Parent.Nodes.Remove(Me)
                                                                      End Sub)

End Class