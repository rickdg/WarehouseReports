Public Class LogicNode
    Inherits BaseNode

    Public Sub New(parent As BaseNode)
        MyBase.Parent = parent
        SelectedOperator = "AND"
    End Sub


    Public Property SelectedOperator As String

End Class