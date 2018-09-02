Imports WarehouseReports.Content
Imports WarehouseReports.Pages

Public Class Linq
    Implements IDisposable

    Public ReadOnly StartDate As Date = PageReports.StartDate
    Public ReadOnly EndDate As Date = PageReports.EndDate
    Private ReadOnly Context As New WarehouseDataEntities


#Region "OnShifts"
#Region "Day"
    Public Function GetBy_Day_Gang_Group_Zone() As IEnumerable(Of Day_Gang_Group_Zone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New Day_Gang_Group_Zone With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .GangNum = GangNum, .Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Day_Main_Group(containsMainGroup As Integer()) As IEnumerable(Of Day_Group)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup
                  Select New Day_Group With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Group = MainGroup, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Day_Main_Group_UpDown(containsMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of Day_Group_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      containsMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.MainGroup, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, MainGroup, UpDown Descending
                  Select New Day_Group_UpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .Group = MainGroup, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Day_UpDown(filterUpDown As Boolean) As IEnumerable(Of Day_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By MonthNumOnShifts, DayNumOnShifts, WeekdayNumOnShifts, UpDown Descending
                  Select New Day_UpDown With {.MonthNum = MonthNumOnShifts, .DayNum = DayNumOnShifts,
                      .WeekdayNum = WeekdayNumOnShifts, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function
#End Region


#Region "Week"
    Public Function GetBy_Week_Gang_Group_Zone() As IEnumerable(Of Week_Gang_Group_Zone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                  Group Task By Task.WeekNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New Week_Gang_Group_Zone With {.WeekNum = WeekNumOnShifts,
                      .GangNum = GangNum, .Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Week_Main_Group(containsMainGroup As Integer()) As IEnumerable(Of Week_Group)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.WeekNumOnShifts, Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, MainGroup
                  Select New Week_Group With {.WeekNum = WeekNumOnShifts, .Group = MainGroup, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Week_MainGroup_UpDown(containsMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of Week_Group_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      containsMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.WeekNumOnShifts, Zone.MainGroup, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, MainGroup, UpDown Descending
                  Select New Week_Group_UpDown With {.WeekNum = WeekNumOnShifts, .Group = MainGroup, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Week_UpDown(filterUpDown As Boolean) As IEnumerable(Of Week_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.WeekNumOnShifts, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By WeekNumOnShifts, UpDown Descending
                  Select New Week_UpDown With {.WeekNum = WeekNumOnShifts, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function
#End Region


#Region "Month"
    Public Function GetBy_Month_Gang_Group_Zone() As IEnumerable(Of Month_Gang_Group_Zone)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Task.GangNum, Zone.MainGroup, Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, GangNum, MainGroup, ZoneShipper
                  Select New Month_Gang_Group_Zone With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts,
                      .GangNum = GangNum, .Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Month_Main_Group(containsMainGroup As Integer()) As IEnumerable(Of Month_Group)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, MainGroup
                  Select New Month_Group With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts, .Group = MainGroup, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Month_Main_Group_UpDown(containsMainGroup As Integer(), filterUpDown As Boolean) As IEnumerable(Of Month_Group_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      containsMainGroup.Contains(Zone.MainGroup) AndAlso Zone.UpDown = filterUpDown
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Zone.MainGroup, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, MainGroup, UpDown Descending
                  Select New Month_Group_UpDown With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts, .Group = MainGroup, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Month_UpDown(filterUpDown As Boolean) As IEnumerable(Of Month_UpDown)
        Dim SQL = From Task In Context.TaskDatas
                  Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso
                      Zone.UpDown = filterUpDown
                  Group Task By Task.YearNumOnShifts, Task.MonthNumOnShifts, Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                  Order By YearNumOnShifts, MonthNumOnShifts, UpDown Descending
                  Select New Month_UpDown With {.YearNum = YearNumOnShifts, .MonthNum = MonthNumOnShifts, .IsUp = UpDown, .Qty = Sum}
        Return SQL.ToList
    End Function
#End Region


#Region "Charts"
    Public Function GetAvgBy_Weekday() As IEnumerable(Of AvgByWeekday)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                                      Group Task By Task.MonthNumOnShifts, Task.DayNumOnShifts, Task.WeekdayNumOnShifts Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.WeekdayNumOnShifts Into Avg = Average(GroupTasks.Sum)
                  Order By WeekdayNumOnShifts
                  Select New AvgByWeekday With {.WeekdayNum = WeekdayNumOnShifts, .Avg = Avg}
        Return SQL.ToList
    End Function


    Public Function GetBy_MainGroup_Zone_PickingNorm() As IEnumerable(Of ByGroup_Zone_Norm)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                Group Task By Zone.MainGroup, Task.ZoneShipper, Zone.PickingNorm Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup, ZoneShipper
                Select New ByGroup_Zone_Norm With {.Group = MainGroup, .Zone = ZoneShipper, .Qty = Sum, .Norm = PickingNorm}).ToList
    End Function


    Public Function GetBy_MainGroup() As IEnumerable(Of ByGroup)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                Group Task By Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup
                Select New ByGroup With {.Group = MainGroup, .Qty = Sum}).ToList
    End Function


    Public Function GetBy_Zone(containsMainGroup As Integer()) As IEnumerable(Of ByZone)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New ByZone With {.Zone = ZoneShipper, .Qty = Sum}).ToList
    End Function


    Public Function GetBy_UpDown(containsMainGroup As Integer()) As IEnumerable(Of ByUpDown)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                Order By UpDown Descending
                Select New ByUpDown With {.IsUp = UpDown, .Qty = Sum}).ToList
    End Function


    Public Function GetBy_MainGroup(containsMainGroup As Integer()) As IEnumerable(Of ByGroup)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup)
                Group Task By Zone.MainGroup Into Sum = Sum(Task.QtyTasks)
                Order By MainGroup
                Select New ByGroup With {.Group = MainGroup, .Qty = Sum}).ToList
    End Function


    Public Function GetBy_Zone(containsZone As Integer?()) As IEnumerable(Of ByZone)
        Return (From Task In Context.TaskDatas
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New ByZone With {.Zone = ZoneShipper, .Qty = Sum}).ToList
    End Function


    Public Function GetMechanization() As IEnumerable(Of SingleIndicator)
        Dim TmpList = (From Task In Context.TaskDatas
                       Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                       Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate
                       Group Task By Zone.UpDown Into Sum = Sum(Task.QtyTasks)
                       Select New ByUpDown With {.IsUp = UpDown, .Qty = Sum}).ToList
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


    Public Function GetBy_Zone(containsMainGroup As Integer(), notContainsZone As Integer?()) As IEnumerable(Of ByZone)
        Return (From Task In Context.TaskDatas
                Join Zone In Context.Zones On Task.ZoneShipper Equals Zone.ZoneNum
                Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDateOnShifts >= StartDate AndAlso Task.XDateOnShifts <= EndDate AndAlso containsMainGroup.Contains(Zone.MainGroup) AndAlso
                      Not notContainsZone.Contains(Task.ZoneShipper)
                Group Task By Task.ZoneShipper Into Sum = Sum(Task.QtyTasks)
                Order By ZoneShipper
                Select New ByZone With {.Zone = ZoneShipper, .Qty = Sum}).ToList
    End Function
#End Region
#End Region



#Region "OnDate"
#Region "Charts"
    Public Function GetAvgBy_Hour() As IEnumerable(Of AvgByHour)
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


    Public Function GetBy_Date_Hour() As IEnumerable(Of Date_Hour)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                  Group Task By Task.XDate, Task.HourNum Into Sum = Sum(Task.QtyTasks)
                  Order By XDate
                  Select New Date_Hour With {.XDate = XDate, .HourNum = HourNum, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetAvgBy_Week_Hour() As IEnumerable(Of AvgByWeek_Hour)
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
    Public Function GetBy_Date(containsZone As Integer?()) As IEnumerable(Of ByDate)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                  Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks)
                  Select New ByDate With {.XDate = XDate, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetBy_Date(containsZone As Integer?(), containsRow As String()) As IEnumerable(Of ByDate)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso
                      containsZone.Contains(Task.ZoneShipper) AndAlso containsRow.Contains(Task.RowShipper)
                  Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks)
                  Select New ByDate With {.XDate = XDate, .Qty = Sum}
        Return SQL.ToList
    End Function


    Public Function GetExtraData(containsZone As Integer?()) As IEnumerable(Of ExData)
        Dim SQL = From ExtraData In Context.ExtraDatas
                  Where ExtraData.XDate >= StartDate AndAlso ExtraData.XDate <= EndDate AndAlso containsZone.Contains(ExtraData.ZoneShipper)
                  Select New ExData With {.XDate = ExtraData.XDate, .QtyUnloadedLPN = ExtraData.QtyUnloadedLPN,
                      .QtyOrders = ExtraData.QtyOrders, .AvgQtyPcs = ExtraData.AvgQtyPcs}
        Return SQL.ToList
    End Function


    Public Function GetPipelineData() As IEnumerable(Of PlData)
        Dim SQL = From PipelineData In Context.PipelineDatas
                  Where PipelineData.XDate >= StartDate AndAlso PipelineData.XDate <= EndDate
                  Select New PlData With {.XDate = PipelineData.XDate, .VolumeCargo = PipelineData.VolumeCargo,
                      .VolumeBox = PipelineData.VolumeBox, .QtyBoxesPassedWeightControl = PipelineData.QtyBoxesPassedWeightControl,
                      .QtyBoxesNotPassedWeightControl = PipelineData.QtyBoxesNotPassedWeightControl}
        Return SQL.ToList
    End Function


    Public Function GetBy_Hour(containsZone As Integer?()) As IEnumerable(Of GrData)
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
            Dim Result = Deserialize(Of SettingsPipelineDataVM)(My.Settings.FilePipeline).Gravitation
            If Result IsNot Nothing Then GravitationRow = Result
        End If

        Return (From Group520 In GetBy_Date(New Integer?() {520})
                From Group510 In GetBy_Date(New Integer?() {510}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From Group530 In GetBy_Date(New Integer?() {530}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From Gravitation In GetBy_Date(New Integer?() {520}, GravitationRow).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From ExtraData In GetExtraData(New Integer?() {520}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From PipelineData In GetPipelineData.Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
                From TasksByHour In GetBy_Hour(New Integer?() {520}).Where(Function(g) g.XDate = Group520.XDate).DefaultIfEmpty
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
    Public Function GetBy_Date_Employee_Hour(containsZone As Integer?()) As IEnumerable(Of Date_Employee_Hour)
        Dim SQL = From Task In Context.TaskDatas
                  Join Employee In Context.Employees On Task.Employee_id Equals Employee.Id
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                  Group Task By Task.XDate, Employee.Name, Task.HourNum Into Sum = Sum(Task.QtyTasks)
                  Order By XDate
                  Select New Date_Employee_Hour With {.XDate = XDate, .Employee = Name, .HourNum = HourNum, .Qty = Sum}
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