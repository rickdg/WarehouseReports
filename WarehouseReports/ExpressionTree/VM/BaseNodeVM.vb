Public MustInherit Class BaseNodeVM

    Public Property Parent As LogicNodeVM


    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
    Public Function RemoveExecute(value As Object) As Boolean
        If IsNothing(Parent) Then Return False
        Parent.Nodes.Remove(Me)
        Return True
    End Function

End Class