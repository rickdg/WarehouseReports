Imports WarehouseReports.Pages

Public Class Linq
    Implements IDisposable

    Private ReadOnly StartDate As Date = PageReports.Model.StartDate.Date
    Private ReadOnly EndDate As Date = PageReports.Model.EndDate.Date
    Private ReadOnly Context As New WarehouseDataEntities


    Public Function GetAvgTasksByHour() As IEnumerable(Of AvgTasksByHour)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                                      Group Task By Task.WeekNum, Task.DayNum, Task.HourNum Into Count = Count)
                  Group GroupTasks By GroupTasks.HourNum Into Avg = Average(GroupTasks.Count)
                  Order By HourNum
                  Select New AvgTasksByHour With {.Час = HourNum, .AvgTasks = Avg}
        Return SQL.ToList
    End Function


    Public Function GetAvgTasksByWeekday() As IEnumerable(Of AvgTasksByWeekday)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                                      Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts Into Count = Count)
                  Group GroupTasks By GroupTasks.WeekdayNumOnShifts Into Avg = Average(GroupTasks.Count)
                  Order By WeekdayNumOnShifts
                  Select New AvgTasksByWeekday With {.WeekdayNum = WeekdayNumOnShifts, .AvgTasks = Avg}
        Return SQL.ToList
    End Function


    Public Function GetMechanization() As IEnumerable(Of SingleIndicator)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Zone.UpDown Into Count = Count
                  Select New TasksByUpDown With {.UpDown = UpDown, .Задачи = Count}
        Dim TmpList = SQL.ToList
        Dim Value = TmpList.Where(Function(i) i.UpDown = True).FirstOrDefault

        If Value Is Nothing Then Return New List(Of SingleIndicator) From {New SingleIndicator}

        Return New List(Of SingleIndicator) From {
            New SingleIndicator With {.Value = Value.Задачи / TmpList.Sum(Function(i) i.Задачи)},
            New SingleIndicator With {.Value = 0.07}}
    End Function


    Public Function GetTasksByDayGangGroupZone() As IEnumerable(Of TasksByDayGangGroupZone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New TasksByDayGangGroupZone With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .GangNum = GangNum, .Группа = MainGroup, .Склад = ZoneShipper, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayMainGroup(filterMainGroup As Integer()) As IEnumerable(Of TasksByDayGroup)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup
                  Select New TasksByDayGroup With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Группа = MainGroup, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayMainGroupUpDown(filterMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of TasksByDayGroupUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso
                      filterMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup, Zone.UpDown Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup, UpDown Descending
                  Select New TasksByDayGroupUpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Группа = MainGroup, .UpDown = UpDown, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayUpDown(filterUpDown As Boolean) As IEnumerable(Of TasksByDayUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.UpDown Into Count = Count
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, UpDown Descending
                  Select New TasksByDayUpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .UpDown = UpDown, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByGroupAZonePickingNorm() As IEnumerable(Of TasksByGroupZoneNorm)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Zone.MainGroup, Task.ZoneShipper, Zone.PickingNorm Into Count = Count
                  Order By MainGroup, ZoneShipper
                  Select New TasksByGroupZoneNorm With {.Группа = MainGroup, .Склад = ZoneShipper, .Задачи = Count, .Норматив = PickingNorm}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMainGroup() As IEnumerable(Of TasksByGroup)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate
                  Group Task By Zone.MainGroup Into Count = Count
                  Order By MainGroup
                  Select New TasksByGroup With {.Группа = MainGroup, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMainGroup(filterMainGroup As Integer()) As IEnumerable(Of TasksByGroup)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterMainGroup.Contains(Zone.MainGroup)
                  Group Task By Zone.MainGroup Into Count = Count
                  Order By MainGroup
                  Select New TasksByGroup With {.Группа = MainGroup, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByUpDown(filterMainGroup As Integer()) As IEnumerable(Of TasksByUpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterMainGroup.Contains(Zone.MainGroup)
                  Group Task By Zone.UpDown Into Count = Count
                  Order By UpDown Descending
                  Select New TasksByUpDown With {.UpDown = UpDown, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByZone(filterMainGroup As Integer()) As IEnumerable(Of TasksByZone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.ZoneShipper Into Count = Count
                  Order By ZoneShipper
                  Select New TasksByZone With {.Склад = ZoneShipper, .Задачи = Count}
        Return SQL.ToList
    End Function


    Public Function GetTasksByZone(filterZone As Integer?()) As IEnumerable(Of TasksByZone)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.TaskDateOnShifts >= StartDate AndAlso Task.TaskDateOnShifts <= EndDate AndAlso filterZone.Contains(Task.ZoneShipper)
                  Group Task By Task.ZoneShipper Into Count = Count
                  Order By ZoneShipper
                  Select New TasksByZone With {.Склад = ZoneShipper, .Задачи = Count}
        Return SQL.ToList
    End Function



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