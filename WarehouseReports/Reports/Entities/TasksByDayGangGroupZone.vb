Public Class TasksByDayGangGroupZone

    Public DayNum As Integer
    Public MonthNum As Integer
    Public WeekdayNum As Integer
    Public GangNum As Integer


    Public ReadOnly Property Дата As String
        Get
            Return $"{Format(DayNum, "00")}.{Format(MonthNum, "00")} {WeekdayName(WeekdayNum, True)}"
        End Get
    End Property
    Public ReadOnly Property Смена As String
        Get
            Return $"Смена {GangNum}"
        End Get
    End Property
    Public Property Группа As Integer
    Public Property Склад As Integer?
    Public Property Задачи As Integer

End Class