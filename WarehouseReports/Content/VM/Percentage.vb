Imports Newtonsoft.Json

Namespace Content
    Public Class Percentage

        Public Property Value2 As Double
        <JsonIgnore>
        Public Property Text As String
            Get
                Return Value2.ToString("P1")
            End Get
            Set
                If Double.TryParse(Value, Value2) Then
                    Value2 = Math.Abs(Value2) / 100
                Else
                    Value2 = 0
                End If
            End Set
        End Property

    End Class
End Namespace