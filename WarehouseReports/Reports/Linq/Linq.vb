Imports WarehouseReports.Content
Imports WarehouseReports.Pages

Public Class Linq
    Implements IDisposable

    Public ReadOnly StartDate As Date = PageReports.Model.StartDate.Date
    Public ReadOnly EndDate As Date = PageReports.Model.EndDate.Date
    Private ReadOnly Context As New WarehouseDataEntities

#Region "MainReport"
#Region "Data"
    Public Function GetTasksByDayGangGroupZone() As IEnumerable(Of TasksByDay_Gang_Group_Zone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New TasksByDay_Gang_Group_Zone With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .GangNum = GangNum, .Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByWeekGangGroupZone() As IEnumerable(Of TasksByWeek_Gang_Group_Zone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                  Group Task By Task.WeekNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New TasksByWeek_Gang_Group_Zone With {.WeekNum = WeekNumOnShifts,
                      .GangNum = GangNum, .Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMonthGangGroupZone() As IEnumerable(Of TasksByMonth_Gang_Group_Zone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New TasksByMonth_Gang_Group_Zone With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts,
                      .GangNum = GangNum, .Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayMainGroup(containsMainGroup As Integer()) As IEnumerable(Of TasksByDay_Group)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup
                  Select New TasksByDay_Group With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Group = MainGroup, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByWeekMainGroup(containsMainGroup As Integer()) As IEnumerable(Of TasksByWeek_Group)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.WeekNumOnShifts, Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, MainGroup
                  Select New TasksByWeek_Group With {.WeekNum = WeekNumOnShifts, .Group = MainGroup, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMonthMainGroup(containsMainGroup As Integer()) As IEnumerable(Of TasksByMonth_Group)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, MainGroup
                  Select New TasksByMonth_Group With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts, .Group = MainGroup, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayMainGroupUpDown(containsMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of TasksByDay_Group_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      containsMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup, UpDown Descending
                  Select New TasksByDay_Group_UpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Group = MainGroup, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByWeekMainGroupUpDown(containsMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of TasksByWeek_Group_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      containsMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.WeekNumOnShifts, Zone.MainGroup, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, MainGroup, UpDown Descending
                  Select New TasksByWeek_Group_UpDown With {.WeekNum = WeekNumOnShifts, .Group = MainGroup, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMonthMainGroupUpDown(containsMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of TasksByMonth_Group_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      containsMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Zone.MainGroup, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, MainGroup, UpDown Descending
                  Select New TasksByMonth_Group_UpDown With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts, .Group = MainGroup, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDayUpDown(filterUpDown As Boolean) As IEnumerable(Of TasksByDay_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, UpDown Descending
                  Select New TasksByDay_UpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByWeekUpDown(filterUpDown As Boolean) As IEnumerable(Of TasksByWeek_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.WeekNumOnShifts, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, UpDown Descending
                  Select New TasksByWeek_UpDown With {.WeekNum = WeekNumOnShifts, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMonthUpDown(filterUpDown As Boolean) As IEnumerable(Of TasksByMonth_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, UpDown Descending
                  Select New TasksByMonth_UpDown With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function
#End Region


#Region "Charts"
    Public Function GetTasksByMainGroupZonePickingNorm() As IEnumerable(Of TasksByGroup_Zone_Norm)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                Group Task By Zone.MainGroup, Task.ZoneShipper, Zone.PickingNorm Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup, ZoneShipper
                Select New TasksByGroup_Zone_Norm With {.Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum, .Norm = PickingNorm}).ToList
    End Function


    Public Function GetAvgTasksByHour() As IEnumerable(Of AvgByHour)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                                      Group Task By Task.WeekNum, Task.DayNum, Task.HourNum Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.HourNum Into Avg = Average(GroupTasks.Sum)
                  Select New AvgByHour With {.HourNum = HourNum, .Avg = Avg}
        Dim TmpList = SQL.ToList
        For h = 0 To 23
            Dim HourNum = h
            If TmpList.Where(Function(i) i.HourNum = HourNum).Count = 0 Then
                TmpList.Add(New AvgByHour With {.HourNum = HourNum})
            End If
        Next
        Return TmpList.OrderBy(Function(i) i.HourNum).ToList
    End Function


    Public Function GetAvgTasksByWeekday() As IEnumerable(Of AvgByWeekday)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                                      Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.WeekdayNumOnShifts Into Avg = Average(GroupTasks.Sum)
                  Order By WeekdayNumOnShifts
                  Select New AvgByWeekday With {.WeekdayNum = WeekdayNumOnShifts, .Avg = Avg}
        Return SQL.ToList
    End Function


    Public Function GetTasksByMainGroup() As IEnumerable(Of TasksByGroup)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                Group Task By Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup
                Select New TasksByGroup With {.Group = MainGroup, .Qty = Sum}).ToList
    End Function


    Public Function GetTasksByZone(containsMainGroup As Integer()) As IEnumerable(Of TasksByZone)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New TasksByZone With {.Zone = ZoneShipper, .Qty = Sum}).ToList
    End Function


    Public Function GetTasksByUpDown(containsMainGroup As Integer()) As IEnumerable(Of TasksByUpDown)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                Order By UpDown Descending
                Select New TasksByUpDown With {.IsUp = UpDown, .Qty = Sum}).ToList
    End Function


    Public Function GetTasksByMainGroup(containsMainGroup As Integer()) As IEnumerable(Of TasksByGroup)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup
                Select New TasksByGroup With {.Group = MainGroup, .Qty = Sum}).ToList
    End Function


    Public Function GetTasksByZone(containsZone As Integer?()) As IEnumerable(Of TasksByZone)
        Return (From Task In Context.TaskDatas
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New TasksByZone With {.Zone = ZoneShipper, .Qty = Sum}).ToList
    End Function


    Public Function GetMechanization() As IEnumerable(Of SingleIndicator)
        Dim TmpList = (From Task In Context.TaskDatas
                       Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                       Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                       Group Task By Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                       Select New TasksByUpDown With {.IsUp = UpDown, .Qty = Sum}).ToList
        Dim Value = TmpList.Where(Function(i) i.IsUp = True).FirstOrDefault

        If Value Is Nothing Then Return New List(Of SingleIndicator) From {New SingleIndicator}

        Dim Mechanization As Double = 0
        If FileExists(My.Settings.FileKPI) Then
            Mechanization = Deserialize(Of SettingsKPIVM)(My.Settings.FileKPI).Mechanization.Value2
        End If
        Return New List(Of SingleIndicator) From {New SingleIndicator With {
            .Norm = Mechanization,
            .Indicator = Value.Qty / TmpList.Sum(Function(i) i.Qty)}}
    End Function


    Public Function GetTasksByZone(containsMainGroup As Integer(), notContainsZone As Integer?()) As IEnumerable(Of TasksByZone)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup) AndAlso
                      Not notContainsZone.Contains(Task.ZoneShipper)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New TasksByZone With {.Zone = ZoneShipper, .Qty = Sum}).ToList
    End Function


    Public Function GetTasksByDateHour() As IEnumerable(Of TasksByDate_Hour)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                  Group Task By Task.XDate, Task.HourNum Into Sum = Sum(Task.QtyTasks)
                  Order By XDate
                  Select New TasksByDate_Hour With {.XDate = XDate, .HourNum = HourNum, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetAvgTasksByWeekHour() As IEnumerable(Of AvgByWeek_Hour)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                                      Group Task By Task.WeekNum, Task.DayNum, Task.HourNum Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.WeekNum, GroupTasks.HourNum Into Avg = Average(GroupTasks.Sum)
                  Order By WeekNum
                  Select New AvgByWeek_Hour With {.WeekNum = WeekNum, .HourNum = HourNum, .Avg = Avg}
        Return SQL.ToList
    End Function
#End Region


#Region "Pipeline"
    Public Function GetTasksByDate(containsZone As Integer?()) As IEnumerable(Of TasksByDate)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                  Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks)
                  Select New TasksByDate With {.XDate = XDate, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDate(containsZone As Integer?(), containsRow As String()) As IEnumerable(Of TasksByDate)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso
                      containsZone.Contains(Task.ZoneShipper) AndAlso containsRow.Contains(Task.RowShipper)
                  Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks)
                  Select New TasksByDate With {.XDate = XDate, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetExtraData(containsZone As Integer?()) As IEnumerable(Of ExData)
        Dim SQL = From ExtraData In Context.ExtraDatas
                  Where ExtraData.xDate >= StartDate AndAlso ExtraData.xDate <= EndDate AndAlso containsZone.Contains(ExtraData.ZoneShipper)
                  Select New ExData With {.XDate = ExtraData.xDate, .QtyUnloadedLPN = ExtraData.QtyUnloadedLPN,
                      .QtyOrders = ExtraData.QtyOrders, .AvgQtyPcs = ExtraData.AvgQtyPcs}
        Return SQL.ToList
    End Function


    Public Function GetPipelineData() As IEnumerable(Of PlData)
        Dim SQL = From PipelineData In Context.PipelineDatas
                  Where PipelineData.xDate >= StartDate AndAlso PipelineData.xDate <= EndDate
                  Select New PlData With {.XDate = PipelineData.xDate, .VolumeCargo = PipelineData.VolumeCargo,
                      .VolumeBox = PipelineData.VolumeBox, .QtyBoxesPassedWeightControl = PipelineData.QtyBoxesPassedWeightControl,
                      .QtyBoxesNotPassedWeightControl = PipelineData.QtyBoxesNotPassedWeightControl}
        Return SQL.ToList
    End Function


    Public Function GetTasksByHour(containsZone As Integer?()) As IEnumerable(Of GrData)
        Dim SQL = From Gr In (From Task In Context.TaskDatas
                              Join Employee In Context.Employees On Task.Employee_id Equals Employee.Id
                              Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                  Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                              Group Task By Task.XDate, Employee.Name, Task.HourNum Into Sum = Sum(Task.QtyTasks))
                  Group Gr By Gr.XDate Into Max = Max(Gr.Sum), Avg = Average(Gr.Sum)
                  Select New GrData With {.XDate = XDate, .Max = Max, .Avg = Avg}
        Return SQL.ToList
    End Function


    Public Function GetPipelineMonitoring() As IEnumerable(Of PipelineMonitoring)
        Dim GravitationRow As String() = {""}
        If FileExists(My.Settings.FilePipeline) Then
            GravitationRow = Deserialize(Of SettingsPipelineDataVM)(My.Settings.FilePipeline).Gravitation
        End If

        Return (From Group520 In GetTasksByDate(New Integer?() {520})
                From Group510 In GetTasksByDate(New Integer?() {510}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From Group530 In GetTasksByDate(New Integer?() {530}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From Gravitation In GetTasksByDate(New Integer?() {520}, GravitationRow).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From ExtraData In GetExtraData(New Integer?() {520}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From PipelineData In GetPipelineData.Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From TasksByHour In GetTasksByHour(New Integer?() {520}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                Order By Group520.XDate
                Select New PipelineMonitoring With {
                    .XDate = If(Group520 Is Nothing, Nothing, Group520.XDate),
                    .Задачи520 = If(Group520 Is Nothing, Nothing, Group520.Qty),
                    .Задачи510 = If(Group510 Is Nothing, Nothing, Group510.Qty),
                    .Задачи530 = If(Group530 Is Nothing, Nothing, Group530.Qty),
                    .Гравитация = If(Gravitation Is Nothing, Nothing, Gravitation.Qty),
                    .Короба = If(ExtraData Is Nothing, Nothing, ExtraData.QtyUnloadedLPN),
                    .Заказы = If(ExtraData Is Nothing, Nothing, ExtraData.QtyOrders),
                    .ОбъемТовара = If(PipelineData Is Nothing, Nothing, PipelineData.VolumeCargo),
                    .ОбъемТары = If(PipelineData Is Nothing, Nothing, PipelineData.VolumeBox),
                    .СреднееКолВоЗадачВЧас = If(TasksByHour Is Nothing, Nothing, CInt(TasksByHour.Avg)),
                    .МаксимальноеКолВоЗадачВЧас = If(TasksByHour Is Nothing, Nothing, TasksByHour.Max),
                    .СреднееКолВоШтукПоСтрокеЗнП = If(ExtraData Is Nothing, Nothing, ExtraData.AvgQtyPcs),
                    .КоробаПрошедшиеВесовойКонтроль = If(PipelineData Is Nothing, Nothing, PipelineData.QtyBoxesPassedWeightControl),
                    .КоробаНеПрошедшиеВесовойКонтроль = If(PipelineData Is Nothing, Nothing, PipelineData.QtyBoxesNotPassedWeightControl)}).ToList
    End Function
#End Region


#Region "Pick520"
    Public Function GetTasksByDateEmployeeHour(containsZone As Integer?()) As IEnumerable(Of TasksByDate_Employee_Hour)
        Dim SQL = From Task In Context.TaskDatas
                  Join Employee In Context.Employees On Task.Employee_id Equals Employee.Id
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                  Group Task By Task.XDate, Employee.Name, Task.HourNum Into Sum = Sum(Task.QtyTasks)
                  Order By XDate
                  Select New TasksByDate_Employee_Hour With {.XDate = XDate, .Employee = Name, .HourNum = HourNum, .Qty = Sum}
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