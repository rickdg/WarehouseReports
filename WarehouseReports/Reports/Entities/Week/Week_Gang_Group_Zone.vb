Public Class Week_Gang_Group_Zone

    Public GangNum As Integer
    Public WeekNum As Integer


    Public ReadOnly Property Week As String
        Get
            Return $"{WeekNum} Неделя"
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