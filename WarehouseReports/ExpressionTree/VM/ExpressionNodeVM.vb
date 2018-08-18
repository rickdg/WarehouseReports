Public Class ExpressionNodeVM
    Inherits BaseNodeVM

    Public Sub New(parent As LogicNodeVM)
        MyBase.Parent = parent
    End Sub


    Public Property SelectedObject As String
    Public Property SelectedOperator As String
    Public Property Value As String

End Class