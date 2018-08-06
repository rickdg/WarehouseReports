Public Class LogicNodeVM
    Inherits BaseNodeVM

    Public Sub New()
        SelectedOperator = "AND"
    End Sub


    Public Property LogicOperators As String() = {"OR", "AND"}
    Public Property SelectedOperator As String

End Class