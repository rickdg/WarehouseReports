﻿Public Class Week_UpDown

    Public IsUp As Boolean
    Public WeekNum As Integer


    Public ReadOnly Property Week As String
        Get
            Return $"{WeekNum} Неделя"
        End Get
    End Property
    Public ReadOnly Property UpDown As String
        Get
            Return If(IsUp, "Верх", "Низ")
        End Get
    End Property
    Public Property Qty As Integer

End Class