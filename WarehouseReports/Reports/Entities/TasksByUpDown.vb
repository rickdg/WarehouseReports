Public Class TasksByUpDown

    Public IsUp As Boolean


    Public ReadOnly Property UpDown As String
        Get
            Return If(IsUp, "Верх", "Низ")
        End Get
    End Property
    Public Property Qty As Integer

End Class