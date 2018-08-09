Imports OfficeOpenXml
Imports WarehouseReports.DAL

Namespace Reports
    Module ReportTasksByDay

        Public Sub ReportTasksByDayExecute(obj As Object)

            Dim TasksByDay As IEnumerable(Of TasksByDay)

            Using Context As New WarehouseDataEntities
                Dim SQL = From Task In Context.TaskDatas
                          Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick
                          Group Task By Task.ZoneShipper, Task.PreviousMonthNum, Task.PreviousDayNum, Task.PreviousWeekdayNum, Task.GangNum Into Count = Count
                          Select New TasksByDay With {.Склад = ZoneShipper, .MonthNum = PreviousMonthNum, .DayNum = PreviousDayNum,
                              .WeekdayNum = PreviousWeekdayNum, .GangNum = GangNum, .Задачи = Count}
                TasksByDay = (SQL).ToList
            End Using

            Dim newFile = Utils.GetFileInfo(Utils.GetDirectoryInfo("Test"), "TestPivot.xlsm")

            Using pck As New ExcelPackage(newFile)
                Dim wsData = pck.Workbook.Worksheets.Add("Данные")

                Dim dataRange = wsData.Cells("A1").LoadFromCollection(TasksByDay, True)

                Dim wsPivot2 = pck.Workbook.Worksheets.Add("Задачи по дням")
                Dim pivotTable2 = wsPivot2.PivotTables.Add(wsPivot2.Cells("A3"), dataRange, "Задачи по дням")

                pivotTable2.RowFields.Add(pivotTable2.Fields("Дата")).Sort = Table.PivotTable.eSortType.Ascending
                pivotTable2.RowFields.Add(pivotTable2.Fields("Смена")).Sort = Table.PivotTable.eSortType.Ascending
                pivotTable2.ColumnFields.Add(pivotTable2.Fields("Склад")).Sort = Table.PivotTable.eSortType.Ascending
                pivotTable2.DataFields.Add(pivotTable2.Fields("Задачи"))

                pck.Workbook.CreateVBAProject()

                wsPivot2.CodeModule.Code = Utils.GetCodeModule(Utils.GetDirectoryInfo("VBA-Code"), "TasksByDay.txt")

                pck.Save()
            End Using
        End Sub

    End Module
End Namespace