Imports WarehouseReports.Content
Imports WarehouseReports.Pages

Public Class Linq
    Implements IDisposable

    Public ReadOnly StartDate As Date = PageReports.Model.StartDate.Date
    Public ReadOnly EndDate As Date = PageReports.Model.EndDate.Date
    Private ReadOnly Context As New WarehouseDataEntities

#Region "MainReport"
#Region "Data"
    Public Function GetTasksByDayGangGroupZone() As IEnumerable(Of TasksByDayGangGroupZone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New TasksByDayGangGroupZone With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .GangNum = GangNum, .Группа = MainGroup, .Склад = ZoneShipper, .Задачи = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayMainGroup(containsMainGroup As Integer()) As IEnumerable(Of TasksByDayGroup)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup
                  Select New TasksByDayGroup With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Группа = MainGroup, .Задачи = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayMainGroupUpDown(containsMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of TasksByDayGroupUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      containsMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup, UpDown Descending
                  Select New TasksByDayGroupUpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Группа = MainGroup, .UpDown = UpDown, .Задачи = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayUpDown(filterUpDown As Boolean) As IEnumerable(Of TasksByDayUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, UpDown Descending
                  Select New TasksByDayUpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .UpDown = UpDown, .Задачи = Sum}
        Return SQL.ToList
    End Function
#End Region


#Region "Charts"
    Public Function GetTasksByMainGroupZonePickingNorm() As IEnumerable(Of TasksByGroupZoneNorm)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                Group Task By Zone.MainGroup, Task.ZoneShipper, Zone.PickingNorm Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup, ZoneShipper
                Select New TasksByGroupZoneNorm With {.Группа = MainGroup, .Склад = ZoneShipper, .Задачи = Sum, .Норматив = PickingNorm}).ToList
    End Function


    Public Function GetAvgTasksByHour() As IEnumerable(Of AvgTasksByHour)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                                      Group Task By Task.WeekNum, Task.DayNum, Task.HourNum Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.HourNum Into Avg = Average(GroupTasks.Sum)
                  Select New AvgTasksByHour With {.Час = HourNum, .AvgTasks = Avg}
        Dim TmpList = SQL.ToList
        For h = 0 To 23
            Dim HourNum = h
            If TmpList.Where(Function(i) i.Час = HourNum).Count = 0 Then
                TmpList.Add(New AvgTasksByHour With {.Час = HourNum})
            End If
        Next
        Return TmpList.OrderBy(Function(i) i.Час).ToList
    End Function


    Public Function GetAvgTasksByWeekday() As IEnumerable(Of AvgTasksByWeekday)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                                      Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.WeekdayNumOnShifts Into Avg = Average(GroupTasks.Sum)
                  Order By WeekdayNumOnShifts
                  Select New AvgTasksByWeekday With {.WeekdayNum = WeekdayNumOnShifts, .AvgTasks = Avg}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMainGroup() As IEnumerable(Of TasksByGroup)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                Group Task By Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup
                Select New TasksByGroup With {.Группа = MainGroup, .Задачи = Sum}).ToList
    End Function


    Public Function GetTasksByZone(containsMainGroup As Integer()) As IEnumerable(Of TasksByZone)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New TasksByZone With {.Склад = ZoneShipper, .Задачи = Sum}).ToList
    End Function


    Public Function GetTasksByUpDown(containsMainGroup As Integer()) As IEnumerable(Of TasksByUpDown)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                Order By UpDown Descending
                Select New TasksByUpDown With {.UpDown = UpDown, .Задачи = Sum}).ToList
    End Function


    Public Function GetTasksByMainGroup(containsMainGroup As Integer()) As IEnumerable(Of TasksByGroup)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup
                Select New TasksByGroup With {.Группа = MainGroup, .Задачи = Sum}).ToList
    End Function


    Public Function GetTasksByZone(containsZone As Integer?()) As IEnumerable(Of TasksByZone)
        Return (From Task In Context.TaskDatas
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New TasksByZone With {.Склад = ZoneShipper, .Задачи = Sum}).ToList
    End Function


    Public Function GetMechanization() As IEnumerable(Of SingleIndicator)
        Dim TmpList = (From Task In Context.TaskDatas
                       Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                       Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                       Group Task By Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                       Select New TasksByUpDown With {.UpDown = UpDown, .Задачи = Sum}).ToList
        Dim Value = TmpList.Where(Function(i) i.UpDown = True).FirstOrDefault

        If Value Is Nothing Then Return New List(Of SingleIndicator) From {New SingleIndicator}

        Dim Mechanization As Double = 0
        If FileExists(My.Settings.FileKPI) Then
            Mechanization = Deserialize(Of SettingsKPIVM)(My.Settings.FileKPI).Mechanization.Value2
        End If
        Return New List(Of SingleIndicator) From {New SingleIndicator With {
            .Норматив = Mechanization,
            .Показатель = Value.Задачи / TmpList.Sum(Function(i) i.Задачи)}}
    End Function


    Public Function GetTasksByZone(containsMainGroup As Integer(), notContainsZone As Integer?()) As IEnumerable(Of TasksByZone)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup) AndAlso
                      Not notContainsZone.Contains(Task.ZoneShipper)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New TasksByZone With {.Склад = ZoneShipper, .Задачи = Sum}).ToList
    End Function


    Public Function GetTasksByDateHour() As IEnumerable(Of TasksByDateHour)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                  Group Task By Task.XDate, Task.HourNum Into Sum = Sum(Task.QtyTasks)
                  Order By XDate
                  Select New TasksByDateHour With {.Дата = XDate, .Час = HourNum, .Задачи = Sum}
        Return SQL.ToList
    End Function


    Public Function GetAvgTasksByWeekNumHour() As IEnumerable(Of AvgTasksByWeekNumHour)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                                      Group Task By Task.WeekNum, Task.DayNum, Task.HourNum Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.WeekNum, GroupTasks.HourNum Into Avg = Average(GroupTasks.Sum)
                  Order By WeekNum
                  Select New AvgTasksByWeekNumHour With {.WeekNum = WeekNum, .Час = HourNum, .AvgTasks = Avg}
        Return SQL.ToList
    End Function
#End Region


#Region "Pipeline"
    Public Function GetPipelineMonitoring() As IEnumerable(Of PipelineMonitoring)
        Dim SQL = From Group520 In (From Task In Context.TaskDatas
                                    Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                        Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso Task.ZoneShipper = 520
                                    Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks))
                  Join Group510 In (From Task In Context.TaskDatas
                                    Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                        Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso Task.ZoneShipper = 510
                                    Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks))
                      On Group520.XDate Equals Group510.XDate
                  Join Group530 In (From Task In Context.TaskDatas
                                    Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                        Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso Task.ZoneShipper = 530
                                    Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks))
                      On Group520.XDate Equals Group530.XDate
                  Join Gravitation In (From Task In Context.TaskDatas
                                       Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                        Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso Task.ZoneShipper = 520 AndAlso Task.RowShipper = "1"
                                       Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks))
                      On Group520.XDate Equals Gravitation.XDate
                  From ExtraData In Context.ExtraDatas.Where(Function(e) e.xDate = Group520.XDate AndAlso e.ZoneShipper = 520).DefaultIfEmpty
                  From PipelineData In Context.PipelineDatas.Where(Function(p) p.xDate = Group520.XDate).DefaultIfEmpty
                  Order By Group520.XDate
                  Select New PipelineMonitoring With {
                      .XDate = Group520.XDate,
                      .Задачи520 = Group520.Sum,
                      .Задачи510 = Group510.Sum,
                      .Задачи530 = Group530.Sum,
                      .Гравитация = Gravitation.Sum,
                      .Короба = ExtraData.QtyUnloadedLPN,
                      .Заказы = ExtraData.QtyOrders,
                      .СреднееКолВоШтукПоСтрокеЗнП = ExtraData.AvgQtyPcs,
                      .ОбъемТовара = PipelineData.VolumeCargo,
                      .ОбъемТары = PipelineData.VolumeBox,
                      .КоробаПрошедшиеВесовойКонтроль = PipelineData.QtyBoxesPassedWeightControl,
                      .КоробаНеПрошедшиеВесовойКонтроль = PipelineData.QtyBoxesNotPassedWeightControl}
        Return SQL.ToList
    End Function
#End Region


#Region "Pick520"
    Public Function GetTasksByDateEmployeeHour(containsZone As Integer?()) As IEnumerable(Of TasksByDateEmployeeHour)
        Dim SQL = From Task In Context.TaskDatas
                  Join Employee In Context.Employees On Task.Employee_id Equals Employee.Id
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                  Group Task By Task.XDate, Employee.Name, Task.HourNum Into Sum = Sum(Task.QtyTasks)
                  Order By XDate
                  Select New TasksByDateEmployeeHour With {.Дата = XDate, .Работник = Name, .Час = HourNum, .Задачи = Sum}
        Return SQL.ToList
    End Function
#End Region
#End Region


#Region "IDisposable Support"
    Private disposedValue As Boolean


    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                Context.Dispose()
            End If
        End If
        disposedValue = True
    End Sub


    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
    End Sub
#End Region

End Class