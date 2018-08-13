Public Class TasksByDayUpDown

    Public DayNum As Integer
    Public MonthNum As Integer
    Public WeekdayNum As Integer
    Public UpDown As Boolean


    Public ReadOnly Property Дата As String
        Get
            Return $"{Format(DayNum, "00")}.{Format(MonthNum, "00")} {WeekdayName(WeekdayNum, True)}"
        End Get
    End Property
    Public ReadOnly Property ВерхНиз As String
        Get
            Return If(UpDown, "Верх", "Низ")
        End Get
    End Property
    Public Property Задачи As Integer

End Class