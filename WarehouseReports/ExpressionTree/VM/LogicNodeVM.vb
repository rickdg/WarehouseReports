Imports System.Collections.ObjectModel
Imports FirstFloor.ModernUI.Presentation
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
    <JsonIgnore>
    Public ReadOnly Property HasParent As Boolean
        Get
            Return Parent IsNot Nothing
        End Get
    End Property


    Public Property Nodes As New ObservableCollection(Of BaseNodeVM)

    <JsonIgnore>
    Public ReadOnly Property CmdAddLogicNode As ICommand = New RelayCommand(Sub() Nodes.Add(New LogicNodeVM(Me)))
    <JsonIgnore>
    Public ReadOnly Property CmdAddExpressionNode As ICommand = New RelayCommand(Sub() Nodes.Add(New ConditionNodeVM(Me)))


    Public Function Contains(logicNode As LogicNodeVM) As Boolean
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