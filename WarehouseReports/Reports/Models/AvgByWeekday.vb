Public Class AvgByWeekday
    Inherits AvgBasik

    Public WeekdayNum As Integer


    Public ReadOnly Property WeekdayName As String
        Get
            Return DateAndTime.WeekdayName(WeekdayNum, True)
        End Get
    End Property

End Class