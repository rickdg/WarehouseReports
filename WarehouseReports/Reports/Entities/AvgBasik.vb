Public Class AvgBasik

    Public Avg As Double


    Public ReadOnly Property RoundAvg As Integer
        Get
            Return CInt(Avg)
        End Get
    End Property

End Class