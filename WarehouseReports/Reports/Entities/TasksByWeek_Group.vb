Public Class TasksByWeek_Group

    Public WeekNum As Integer


    Public ReadOnly Property Week As String
        Get
            Return $"{WeekNum} Неделя"
        End Get
    End Property
    Public Property Group As Integer
    Public Property Qty As Integer

End Class