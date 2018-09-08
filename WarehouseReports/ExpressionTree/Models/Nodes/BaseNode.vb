Imports FirstFloor.ModernUI.Presentation
Imports Newtonsoft.Json

Public MustInherit Class BaseNode
    Inherits NotifyPropertyChanged

    Public Property Parent As LogicNode

    <JsonIgnore>
    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
    Public Sub RemoveExecute(parameter As Object)
        Parent.Nodes.Remove(Me)
    End Sub


    Public MustOverride Function GetExpression() As String

End Class