Public Class AvgTasksByWeekday

    Public WeekdayNum As Integer
    Public AvgTasks As Double


    Public ReadOnly Property ДеньНедели As String
        Get
            Return WeekdayName(WeekdayNum, True)
        End Get
    End Property
    Public ReadOnly Property СреднееКолвоЗадач As Integer
        Get
            Return CInt(AvgTasks)
        End Get
    End Property

End Class