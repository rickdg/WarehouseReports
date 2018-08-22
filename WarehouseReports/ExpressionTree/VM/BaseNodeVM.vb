Imports FirstFloor.ModernUI.Presentation
Imports Newtonsoft.Json

Public MustInherit Class BaseNodeVM
    Inherits NotifyPropertyChanged

    Public Property Parent As LogicNodeVM

    <JsonIgnore>
    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
    Public Sub RemoveExecute(value As Object)
        Parent.Nodes.Remove(Me)
    End Sub


    Public MustOverride Function GetExpression() As String

End Class