Public Class LogicNodeVM
    Inherits BaseNodeVM

    Public Sub New(parent As BaseNodeVM)
        MyBase.Parent = parent
        SelectedOperator = "AND"
    End Sub


    Public Property SelectedOperator As String

End Class