Public Class ExpressionNodeVM
    Inherits BaseNodeVM

    Public Sub New(root As BaseNodeVM)
        RootNode = root
    End Sub


    Public Property SelectedObject As String
    Public Property SelectedOperator As String
    Public Property Value As String

End Class