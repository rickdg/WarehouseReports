Imports WarehouseReports.Enums

Public Class Data

    Public SystemTaskType_id As Integer


    Public ReadOnly Property Системный_Тип_Задачи As String
        Get
            Return CType([Enum].ToObject(GetType(SystemTaskTypeRu), SystemTaskType_id), SystemTaskTypeRu).ToString.Replace("_", " ")
        End Get
    End Property
    Public Property Зона_Отправитель As Integer?
    Public Property Ряд_Отправитель As String
    Public Property Зона_Получатель As Integer?
    Public Property Тип_Задачи_Пользователя As String
    Public Property Норматив As Double
    Public Property Работник As String
    Public Property Дата As Date
    Public Property Год As Integer
    Public Property Месяц As Integer
    Public Property Неделя As Integer
    Public Property День As Integer
    Public Property День_Недели As Integer
    Public Property Час As Integer
    Public Property Дата_По_Сменам As Date
    Public Property Год_По_Сменам As Integer
    Public Property Месяц_По_Сменам As Integer
    Public Property Неделя_По_Сменам As Integer
    Public Property День_По_Сменам As Integer
    Public Property День_Недели_По_Сменам As Integer
    Public Property Смена As Integer
    Public Property Задачи As Integer

End Class