Imports WarehouseReports.Enums

Public Class Date_TypeRu

    Public TaskType As Integer

    Public Property XDate As Date
    Public ReadOnly Property TypeRu As String
        Get
            Return CType([Enum].ToObject(GetType(SystemTaskTypeRu), TaskType), SystemTaskTypeRu).ToString
        End Get
    End Property
    Public Property Qty As Integer

End Class