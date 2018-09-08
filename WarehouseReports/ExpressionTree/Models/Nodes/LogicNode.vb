Imports System.Collections.ObjectModel
Imports FirstFloor.ModernUI.Presentation
Imports Newtonsoft.Json

Public Class LogicNode
    Inherits BaseNode

    Public Sub New()
    End Sub


    Public Sub New(logicOperator As String)
        Me.LogicOperator = logicOperator
    End Sub


    Public Sub New(parent As LogicNode)
        MyBase.Parent = parent
        LogicOperator = "AND"
    End Sub


    Public Property LogicOperator As String
    <JsonIgnore>
    Public ReadOnly Property HasParent As Boolean
        Get
            Return Parent IsNot Nothing
        End Get
    End Property


    Public Property Nodes As New ObservableCollection(Of BaseNode)

    <JsonIgnore>
    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNode(Me)))
    <JsonIgnore>
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ConditionNode(Me)))


    Public Function Contains(logicNode As LogicNode) As Boolean
        If logicNode Is Nothing OrElse Parent Is Nothing Then Return False
        If Parent.Equals(logicNode) Then Return True
        Return Parent.Contains(logicNode)
    End Function


    Public Overrides Function GetExpression() As String
        Dim Result = $"({Join(Nodes.Where(Function(n) n.GetExpression IsNot Nothing).
                              Select(Function(n) n.GetExpression).ToArray, $" {LogicOperator} ")})"
        Return If(Result = "()", Nothing, Result)
    End Function

End Class