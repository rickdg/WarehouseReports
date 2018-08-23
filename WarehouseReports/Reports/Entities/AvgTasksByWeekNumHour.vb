Public Class AvgTasksByWeekNumHour

    Public WeekNum As Integer
    Public AvgTasks As Double


    Public Property Час As Integer
    Public ReadOnly Property СреднееКолвоЗадач As Integer
        Get
            Return CInt(AvgTasks)
        End Get
    End Property

End Class