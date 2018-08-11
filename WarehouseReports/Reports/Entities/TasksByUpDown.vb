Public Class TasksByUpDown

    Public UpDown As Boolean


    Public ReadOnly Property ВерхНиз As String
        Get
            Return If(UpDown, "Верх", "Низ")
        End Get
    End Property
    Public Property Задачи As Integer

End Class