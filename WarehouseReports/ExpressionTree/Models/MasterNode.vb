Public Class MasterNode
    Inherits BaseNode

    Public ReadOnly Property CmdAddRootNode As ICommand = New RelayCommand(Sub() Nodes.Add(New RootNode(Me)))
    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNode(Me)))
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ExpressionNode(Me)))

End Class