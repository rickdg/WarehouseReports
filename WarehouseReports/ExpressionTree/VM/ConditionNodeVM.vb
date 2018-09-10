Imports Newtonsoft.Json

Public Class ConditionNodeVM
    Inherits BaseNodeVM

    Private _SelectedOperator As String
    Private _SelectedObject As String


    Public Sub New()
    End Sub


    Public Sub New(parent As LogicNodeVM)
        MyBase.Parent = parent
    End Sub


    Public Property DataType As FieldDataType
    Public Property SelectedObject As String
        Get
            Return _SelectedObject
        End Get
        Set
            _SelectedObject = Value
            DataType = GetFieldDataType(Value)
        End Set
    End Property
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
        If SelectedObject Is Nothing Then Return Result
        If HasExpression Then
            Dim ResultExpression As String
            If DataType = FieldDataType.Int Then
                ResultExpression = $"({Replace(Expression, ";", ", ")})"
            Else
                ResultExpression = $"('{Replace(Expression, ";", "', '")}')"
            End If
            Return $"{Result} {ResultExpression}"
        End If
        Return Result
    End Function

End Class