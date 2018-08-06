Public Class RootNodeVM
    Inherits BaseNodeVM

    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNodeVM))
    Public ReadOnly Property CmdAddRootNode As ICommand = New RelayCommand(Sub() Nodes.Add(New RootNodeVM))
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ExpressionNodeVM))

End Class