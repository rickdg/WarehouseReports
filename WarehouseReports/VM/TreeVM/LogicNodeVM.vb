Public Class LogicNodeVM
    Inherits BaseNodeVM

    Public Sub New(root As BaseNodeVM)
        RootNode = root
        SelectedOperator = "AND"
    End Sub


    Public Property SelectedOperator As String

End Class