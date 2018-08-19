Imports Newtonsoft.Json

Public Class ConditionNodeVM
    Inherits BaseNodeVM

    Private _SelectedOperator As String


    Public Sub New()
    End Sub


    Public Sub New(parent As LogicNodeVM)
        MyBase.Parent = parent
    End Sub


    Public Property SelectedObject As String
    Public Property SelectedOperator As String
        Get
            Return _SelectedOperator
        End Get
        Set
            _SelectedOperator = Value
            OnPropertyChanged("HasExpression")
            If Not HasExpression Then Expression = Nothing
            OnPropertyChanged("Expression")
        End Set
    End Property
    Public Property Expression As String
    <JsonIgnore>
    Public ReadOnly Property HasExpression As Boolean
        Get
            Return Not {"IS NULL", "IS NOT NULL"}.Contains(_SelectedOperator)
        End Get
    End Property


    Public Overrides Function GetExpression() As String
        Dim Result = $"{SelectedObject} {SelectedOperator}"
        If HasExpression Then
            Dim ResultExpression As String
            If {"[Складское подразделение]", "[Склад-получ#]"}.Contains(SelectedObject) Then
                ResultExpression = $"({Replace(Expression, ";", ", ")})"
            Else
                ResultExpression = $"('{Replace(Expression, ";", "', '")}')"
            End If
            Return $"{Result} {ResultExpression}"
        End If
        Return Result
    End Function

End Class