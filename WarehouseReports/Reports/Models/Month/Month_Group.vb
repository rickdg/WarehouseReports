Public Class Month_Group

    Public GangNum As Integer
    Public YearNum As Integer
    Public MonthNum As Integer


    Public ReadOnly Property Month As String
        Get
            Return $"{YearNum}.{Format(MonthNum, "00")}"
        End Get
    End Property
    Public Property Group As Integer
    Public Property Qty As Integer

End Class