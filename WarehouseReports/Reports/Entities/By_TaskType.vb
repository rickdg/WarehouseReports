Imports WarehouseReports.Enums

Public Class By_TaskType

    Public SystemTaskType As Integer


    Public ReadOnly Property TaskTypeRu As String
        Get
            Return CType([Enum].ToObject(GetType(SystemTaskTypeRu), SystemTaskType), SystemTaskTypeRu).ToString.Replace("_", " ")
        End Get
    End Property
    Public Property Qty As Integer

End Class