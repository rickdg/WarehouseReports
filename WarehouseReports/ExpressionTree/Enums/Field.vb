Public Class Field

    Public Property Name As String
    Public Property DataType As FieldDataType


    Public Overrides Function ToString() As String
        Return Name
    End Function

End Class