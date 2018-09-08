Public Class Day_UpDown

    Public IsUp As Boolean
    Public DayNum As Integer
    Public MonthNum As Integer
    Public WeekdayNum As Integer


    Public ReadOnly Property XDate As String
        Get
            Return $"{Format(MonthNum, "00")}.{Format(DayNum, "00")} {WeekdayName(WeekdayNum, True)}"
        End Get
    End Property
    Public ReadOnly Property UpDown As String
        Get
            Return If(IsUp, "Верх", "Низ")
        End Get
    End Property
    Public Property Qty As Integer

End Class