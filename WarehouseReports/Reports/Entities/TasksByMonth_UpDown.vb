Public Class TasksByMonth_UpDown

    Public YearNum As Integer
    Public MonthNum As Integer
    Public IsUp As Boolean


    Public ReadOnly Property Month As String
        Get
            Return $"{YearNum}.{Format(MonthNum, "00")}"
        End Get
    End Property
    Public ReadOnly Property UpDown As String
        Get
            Return If(IsUp, "Верх", "Низ")
        End Get
    End Property
    Public Property Qty As Integer

End Class