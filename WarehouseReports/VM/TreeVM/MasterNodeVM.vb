Public Class MasterNodeVM
    Inherits BaseNodeVM

    Public ReadOnly Property CmdAddRootNode As ICommand = New RelayCommand(Sub() Nodes.Add(New RootNodeVM(Me)))
    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNodeVM(Me)))
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ExpressionNodeVM(Me)))

End Class