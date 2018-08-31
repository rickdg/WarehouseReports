Imports System.Text
Imports WarehouseReports.Enums

Namespace ExcelConnection
    Module SourceFields

        Public Function GetTasksFields() As IEnumerable(Of Column)
            Return {New Column("План/задача", AdoEnums.adWChar),
                New Column("Тип задачи системы", AdoEnums.adWChar),
                New Column("Заголовок источника", AdoEnums.adDouble),
                New Column("Номер строки", AdoEnums.adWChar),
                New Column("Позиция", AdoEnums.adDouble),
                New Column("Складское подразделение", AdoEnums.adDouble),
                New Column("Складское место", AdoEnums.adWChar),
                New Column("Склад-получ#", AdoEnums.adDouble),
                New Column("СМ-получатель", AdoEnums.adWChar),
                New Column("Количество", AdoEnums.adDouble),
                New Column("Тип задачи пользователя", AdoEnums.adWChar),
                New Column("Работник", AdoEnums.adWChar),
                New Column("Назначенное время", AdoEnums.adDate),
                New Column("Время загрузки", AdoEnums.adDate),
                New Column("НЗ содержимого", AdoEnums.adWChar),
                New Column("Номерной знак отправителя", AdoEnums.adWChar),
                New Column("Загруженный НЗ", AdoEnums.adWChar),
                New Column("Выгруженный НЗ", AdoEnums.adWChar)}
        End Function


        Public Function GetReceiptFields() As IEnumerable(Of Column)
            Return {New Column("Тип транзакции", AdoEnums.adWChar),
                New Column("Получатель", AdoEnums.adWChar),
                New Column("Номерной знак переноса", AdoEnums.adWChar),
                New Column("Дата", AdoEnums.adDate)}
        End Function


        Public Function GetLoadFields() As IEnumerable(Of Column)
            Return {New Column("Дата", AdoEnums.adDate),
                New Column("Наименование сотрудника", AdoEnums.adWChar),
                New Column("LPN", AdoEnums.adWChar)}
        End Function


        Public Function CheckColumns(taskType As SystemTaskType, verifiable As IEnumerable(Of Column)) As String
            Dim Result As New StringBuilder
            Dim Original As IEnumerable(Of Column)

            Select Case taskType
                Case SystemTaskType.Receipt
                    Original = GetReceiptFields()
                Case SystemTaskType.Placement, SystemTaskType.Resupply,
                     SystemTaskType.ManualResupply, SystemTaskType.Movement,
                     SystemTaskType.Pick, SystemTaskType.Control,
                     SystemTaskType.ExtraData, SystemTaskType.Union
                    Original = GetTasksFields()
                Case SystemTaskType.Load
                    Original = GetLoadFields()
                Case Else
                    Return "Тип задачи не определен"
            End Select

            For Each Source In Original
                Dim Target = verifiable.SingleOrDefault(Function(c) c.Name = Source.Name)
                If Target Is Nothing Then
                    Result.AppendLine($"Отсутствует столбец - [b]{Source}[/b]")
                Else
                    If Target.DataType <> Source.DataType Then
                        Result.AppendLine($"Столбец - [b]{Target}[/b] ([url=https://docs.microsoft.com/ru-ru/sql/ado/reference/ado-api/datatypeenum?view=sql-server-2017][b]{Target.DataType}[/b][/url]) несоответствует типу данных [url=https://docs.microsoft.com/ru-ru/sql/ado/reference/ado-api/datatypeenum?view=sql-server-2017][b]{Source.DataType}[/b][/url]")
                    End If
                End If
            Next
            Return Result.ToString
        End Function

    End Module
End Namespace