Public Class ExpressionNode
    Inherits BaseNode

    Public Sub New(parent As BaseNode)
        MyBase.Parent = parent
    End Sub


    Public Property SelectedObject As String
    Public Property SelectedOperator As String
    Public Property Value As String

End Class