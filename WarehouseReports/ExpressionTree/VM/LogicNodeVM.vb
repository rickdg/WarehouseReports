Imports System.Collections.ObjectModel
Imports Newtonsoft.Json

Public Class LogicNodeVM
    Inherits BaseNodeVM

    Public Sub New()
    End Sub


    Public Sub New(logicOperator As String)
        Me.LogicOperator = logicOperator
    End Sub


    Public Sub New(parent As LogicNodeVM)
        MyBase.Parent = parent
        LogicOperator = "AND"
    End Sub


    Public Property LogicOperator As String


    Public Property Nodes As New ObservableCollection(Of BaseNodeVM)


    <JsonIgnore>
    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNodeVM(Me)))
    <JsonIgnore>
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ConditionNodeVM(Me)))


    Public Function Contains(logicNode As LogicNodeVM) As Boolean
        If IsNothing(logicNode) OrElse IsNothing(Parent) Then Return False
        If Parent.Equals(logicNode) Then Return True
        Return Parent.Contains(logicNode)
    End Function


    Public Overrides Function GetExpression() As String
        If Nodes.Where(Function(n) n.GetType.Equals(GetType(ConditionNodeVM))).Count = 0 Then Return Nothing
        Dim Result = $"{Join(Nodes.Where(Function(n) Not IsNothing(n.GetExpression)).
                             Select(Function(n) n.GetExpression).ToArray, $" {LogicOperator} ")}"
        If Not IsNothing(Parent) Then Return $"({Result})"
        Return Result
    End Function

End Class