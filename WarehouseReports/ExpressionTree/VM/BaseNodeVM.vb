Imports FirstFloor.ModernUI.Presentation
Imports Newtonsoft.Json

Public MustInherit Class BaseNodeVM
    Inherits NotifyPropertyChanged

    Public Property Parent As LogicNodeVM


    <JsonIgnore>
    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
    Public Function RemoveExecute(value As Object) As Boolean
        If IsNothing(Parent) Then Return False
        Parent.Nodes.Remove(Me)
        Return True
    End Function


    Public MustOverride Function GetExpression() As String

End Class