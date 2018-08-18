Imports System.Collections.ObjectModel

Public Class LogicNodeVM
    Inherits BaseNodeVM

    Public Sub New()
        LogicOperator = "AND"
    End Sub


    Public Sub New(parent As LogicNodeVM)
        MyBase.Parent = parent
        LogicOperator = "AND"
    End Sub


    Public Property LogicOperator As String


    Public Property Nodes As New ObservableCollection(Of BaseNodeVM)


    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNodeVM(Me)))
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ExpressionNodeVM(Me)))


    Public Function Contains(logicNode As LogicNodeVM) As Boolean
        If IsNothing(logicNode) OrElse IsNothing(Parent) Then Return False
        If Parent.Equals(logicNode) Then Return True
        Return Parent.Contains(logicNode)
    End Function

End Class