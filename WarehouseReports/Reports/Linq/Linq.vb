Imports WarehouseReports.Pages

Public Class Linq

    Private ReadOnly StartDate As Date = PageReports.Model.StartDate.Date
    Private ReadOnly EndDate As Date = PageReports.Model.EndDate.Date
    Private ReadOnly Context As New WarehouseDataEntities

    Public Function GetTasksByDayGangZone() As IEnumerable(Of TasksByDayGangZone)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Task.GangNum, Task.ZoneShipper Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, GangNum, ZoneShipper
                  Select New TasksByDayGangZone With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .GangNum = GangNum, .Склад = ZoneShipper, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByDayGroupA(filterGroupA As Integer()) As IEnumerable(Of TasksByDayGroupA)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterGroupA.Contains(Group.GroupA)
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Group.GroupA Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, GroupA
                  Select New TasksByDayGroupA With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Группа = GroupA, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByDayGroupAUpDown(filterGroupA As Integer(), filterUpDown As Boolean) As IEnumerable(Of TasksByDayGroupAUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso
                      filterGroupA.Contains(Group.GroupA) AndAlso Group.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Group.GroupA, Group.UpDown Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, GroupA, UpDown Descending
                  Select New TasksByDayGroupAUpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Группа = GroupA, .UpDown = UpDown, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByDayUpDown(filterUpDown As Boolean) As IEnumerable(Of TasksByDayUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso
                      Group.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Group.UpDown Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, UpDown Descending
                  Select New TasksByDayUpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .UpDown = UpDown, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByGroupAZone() As IEnumerable(Of TasksByGroupAZone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Group.GroupA, Task.ZoneShipper Into Count = Count
                  Order By GroupA, ZoneShipper
                  Select New TasksByGroupAZone With {.ГруппаА = GroupA, .Склад = ZoneShipper, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetAvgTasksByWeekday() As IEnumerable(Of AvgTasksByWeekday)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                                      Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts Into Count = Count)
                  Group GroupTasks By GroupTasks.WeekdayNumOnShifts Into Avg = Average(GroupTasks.Count)
                  Order By WeekdayNumOnShifts
                  Select New AvgTasksByWeekday With {.WeekdayNum = WeekdayNumOnShifts, .AvgTasks = Avg}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByGroupA() As IEnumerable(Of TasksByGroupA)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Group.GroupA Into Count = Count
                  Order By GroupA
                  Select New TasksByGroupA With {.ГруппаА = GroupA, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByGroupA(filterGroupA As Integer()) As IEnumerable(Of TasksByGroupA)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterGroupA.Contains(Group.GroupA)
                  Group Task By Group.GroupA Into Count = Count
                  Order By GroupA
                  Select New TasksByGroupA With {.ГруппаА = GroupA, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByZoneGroupA(filterGroupA As Integer()) As IEnumerable(Of TasksByZone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterGroupA.Contains(Group.GroupA)
                  Group Task By Task.ZoneShipper Into Count = Count
                  Order By ZoneShipper
                  Select New TasksByZone With {.Склад = ZoneShipper, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByZone(filterZone As Integer?()) As IEnumerable(Of TasksByZone)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterZone.Contains(Task.ZoneShipper)
                  Group Task By Task.ZoneShipper Into Count = Count
                  Order By ZoneShipper
                  Select New TasksByZone With {.Склад = ZoneShipper, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetTasksByUpDownGroupA(filterGroupA As Integer()) As IEnumerable(Of TasksByUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterGroupA.Contains(Group.GroupA)
                  Group Task By Group.UpDown Into Count = Count
                  Order By UpDown Descending
                  Select New TasksByUpDown With {.UpDown = UpDown, .Задачи = Count}
        Return (SQL).ToList
    End Function


    Public Function GetAvgTasksByHour() As IEnumerable(Of AvgTasksByHour)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                                      Group Task By Task.WeekNum, Task.DayNum, Task.HourNum Into Count = Count)
                  Group GroupTasks By GroupTasks.HourNum Into Avg = Average(GroupTasks.Count)
                  Order By HourNum
                  Select New AvgTasksByHour With {.Час = HourNum, .AvgTasks = Avg}
        Return (SQL).ToList
    End Function


    Public Function GetMechanization() As IEnumerable(Of SingleIndicator)
        Dim SQL = From Task In Context.TaskDatas
                  Join Group In Context.ZoneGroups On Task.ZoneShipper Equals Group.Zone
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Group.UpDown Into Count = Count
                  Select New TasksByUpDown With {.UpDown = UpDown, .Задачи = Count}
        Dim TmpList = (SQL).ToList
        Return New List(Of SingleIndicator) From {
            New SingleIndicator With {.Value = TmpList.Where(Function(i) i.UpDown = True).FirstOrDefault.Задачи / TmpList.Sum(Function(i) i.Задачи)},
            New SingleIndicator With {.Value = 0.07}}
    End Function


    Public Sub Dispose()
        Context.Dispose()
    End Sub

End Class