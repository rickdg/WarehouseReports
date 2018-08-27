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


    Public Function GetAvgTasksByWeekHour() As IEnumerable(Of AvgTasksByWeekHour)
        Dim SQL = From GroupTasks In (From Task In Context.TaskDatas
                                      Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                                          Task.XDate >= StartDate AndAlso Task.XDate <= EndDate
                                      Group Task By Task.WeekNum, Task.DayNum, Task.HourNum Into Sum = Sum(Task.QtyTasks))
                  Group GroupTasks By GroupTasks.WeekNum, GroupTasks.HourNum Into Avg = Average(GroupTasks.Sum)
                  Order By WeekNum
                  Select New AvgTasksByWeekHour With {.WeekNum = WeekNum, .Час = HourNum, .AvgTasks = Avg}
        Return SQL.ToList
    End Function
#End Region


#Region "Pipeline"
    Public Function GetTasksByDate(containsZone As Integer?()) As IEnumerable(Of TasksByDate)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso containsZone.Contains(Task.ZoneShipper)
                  Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks)
                  Select New TasksByDate With {.XDate = XDate, .Tasks = Sum}
        Return SQL.ToList
    End Function


    Public Function GetTasksByDate(containsZone As Integer?(), containsRow As String()) As IEnumerable(Of TasksByDate)
        Dim SQL = From Task In Context.TaskDatas
                  Where Task.SystemTaskType_id = Enums.SystemTaskType.Pick AndAlso
                      Task.XDate >= StartDate AndAlso Task.XDate <= EndDate AndAlso
                      containsZone.Contains(Task.ZoneShipper) AndAlso containsRow.Contains(Task.RowShipper)
                  Group Task By Task.XDate Into Sum = Sum(Task.QtyTasks)
                  Select New TasksByDate With {.XDate = XDate, .Tasks = Sum}
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
                    .Задачи520 = If(Group520 Is Nothing, Nothing, Group520.Tasks),
                    .Задачи510 = If(Group510 Is Nothing, Nothing, Group510.Tasks),
                    .Задачи530 = If(Group530 Is Nothing, Nothing, Group530.Tasks),
                    .Гравитация = If(Gravitation Is Nothing, Nothing, Gravitation.Tasks),
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