Public Class Day_Gang_Group_Zone

    Public GangNum As Integer
    Public DayNum As Integer
    Public MonthNum As Integer
    Public WeekdayNum As Integer


    Public ReadOnly Property XDate As String
        Get
            Return $"{Format(MonthNum, "00")}.{Format(DayNum, "00")} {WeekdayName(WeekdayNum, True)}"
        End Get
    End Property
    Public ReadOnly Property Gang As String
        Get
            Return $"Смена {GangNum}"
        End Get
    End Property
    Public Property Group As Integer
    Public Property Zone As Integer?
    Public Property Qty As Integer

End Class