Public Class RootNodeVM
    Inherits BaseNodeVM

    Public Sub New(root As RootNodeVM)
        RootNode = root
    End Sub


    Public ReadOnly Property CmdAddRootNode As ICommand = New RelayCommand(Sub() Nodes.Add(New RootNodeVM(Me)))
    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNodeVM(Me)))
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ExpressionNodeVM(Me)))

End Class