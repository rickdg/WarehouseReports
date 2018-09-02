Imports FirstFloor.ModernUI.Presentation
Imports FirstFloor.ModernUI.Windows.Controls
Imports WarehouseReports.Content
Imports LiveCharts
Imports LiveCharts.Wpf
Imports WarehouseReports.Enums
Imports LiveCharts.Configurations

Namespace Pages

    Public Enum Scale
        Day
        Month
    End Enum


    Public Class DataManagementVM
        Inherits NotifyPropertyChanged

        Private ReadOnly DayFormatter As Func(Of Double, String) = Function(v) New DateTime(CLng(v * TimeSpan.FromDays(1).Ticks)).ToString("d")
        Private ReadOnly MonthFormatter As Func(Of Double, String) = Function(v) New DateTime(CLng(v * TimeSpan.FromDays(1).Ticks * 30.44)).ToString("MMM yyyy")
        Private ReadOnly NFormatter As Func(Of Double, String) = Function(v) v.ToString("N0")
        Private ReadOnly PFormatter As Func(Of Double, String) = Function(v) v.ToString("P0")


        Private _YFormatter As Func(Of Double, String)
        Private _XFormatter As Func(Of Double, String)


        Public Sub New()
            Scale = Scale.Day
            DayMapper.X(Function(m) m.XDate.Ticks / TimeSpan.FromDays(1).Ticks)
            DayMapper.Y(Function(m) m.Value)
            MonthMapper.X(Function(m) m.XDate.Ticks / (TimeSpan.FromDays(1).Ticks * 30.44))
            MonthMapper.Y(Function(m) m.Value)
            XFormatter = DayFormatter
            YFormatter = NFormatter
            StackMode = StackMode.Values
            SeriesCollection.AddRange(GetStackedColumnSeriesDay())
        End Sub


        Public Property SeriesCollection As New SeriesCollection
        Public Property XFormatter As Func(Of Double, String)
            Get
                Return _XFormatter
            End Get
            Set
                _XFormatter = Value
                OnPropertyChanged("XFormatter")
            End Set
        End Property
        Public Property YFormatter As Func(Of Double, String)
            Get
                Return _YFormatter
            End Get
            Set(ByVal value As Func(Of Double, String))
                _YFormatter = value
                OnPropertyChanged("YFormatter")
            End Set
        End Property
        Public Property StackMode As StackMode
        Public Property DayMapper As New CartesianMapper(Of MeasureModel)
        Public Property MonthMapper As New CartesianMapper(Of MeasureModel)
        Public Property Scale As Scale
        Public Property AxisX As Axis


        Public ReadOnly Property CmdLoadTasks As ICommand = New RelayCommand(AddressOf LoadTasksExecute)
        Private Sub LoadTasksExecute(parameter As Object)
            Dim Dlg As New ModernDialog
            Dlg.Buttons = {Dlg.OkButton}
            Dlg.Content = New DataLoader(Dlg)
            Dlg.ShowDialog()
            RefreshSeriesCollection()
        End Sub
        Public ReadOnly Property CmdDeleteTasks As ICommand = New RelayCommand(AddressOf DeleteTasksExecute)
        Private Sub DeleteTasksExecute(parameter As Object)
            Dim Dlg As New ModernDialog With {.Title = "Удаление данных"}
            Dlg.Buttons = {Dlg.YesButton, Dlg.CancelButton}
            Dlg.Content = New DeleteTasks(Dlg)
            If Dlg.ShowDialog() Then RefreshSeriesCollection()
        End Sub
        Public ReadOnly Property CmdChangeStackMode As ICommand = New RelayCommand(AddressOf ChangeStackModeExecute)
        Private Sub ChangeStackModeExecute(ByVal parameter As Object)
            If StackMode = StackMode.Values Then
                YFormatter = PFormatter
                StackMode = StackMode.Percentage
            Else
                YFormatter = NFormatter
                StackMode = StackMode.Values
            End If
            For Each Series In SeriesCollection.Cast(Of StackedColumnSeries)
                Series.StackMode = StackMode
            Next
        End Sub


        Private Sub RefreshSeriesCollection()
            SeriesCollection.Clear()
            Select Case Scale
                Case Scale.Day
                    SeriesCollection.AddRange(GetStackedColumnSeriesDay())
                Case Scale.Month
                    SeriesCollection.AddRange(GetStackedColumnSeriesMonth())
            End Select
        End Sub


        Private Function GetDataByDay() As IEnumerable(Of Date_TaskType)
            Using Context As New WarehouseDataEntities
                Return (From Task In Context.TaskDatas
                        Group Task By Task.SystemTaskType_id, Task.XDate Into Sum = Sum(Task.QtyTasks)
                        Select New Date_TaskType With {.TaskType = SystemTaskType_id, .XDate = XDate, .Qty = Sum}).ToList
            End Using
        End Function


        Private Function GetDataByMonth() As IEnumerable(Of Month_TaskType)
            Using Context As New WarehouseDataEntities
                Return (From Task In Context.TaskDatas
                        Group Task By Task.SystemTaskType_id, Task.YearNum, Task.MonthNum Into Sum = Sum(Task.QtyTasks)
                        Select New Month_TaskType With {.TaskType = SystemTaskType_id, .YearNum = YearNum, .MonthNum = MonthNum, .Qty = Sum}).ToList
            End Using
        End Function


        Private Function GetStackedColumnSeriesDay() As IEnumerable(Of StackedColumnSeries)
            Return (From s In GetDataByDay()
                    Group New MeasureModel(s.XDate, s.Qty) By s.TaskType Into List = ToList
                    Order By TaskType
                    Select New StackedColumnSeries With {
                        .StackMode = StackMode,
                        .Configuration = DayMapper,
                        .Title = CType([Enum].ToObject(GetType(SystemTaskTypeRu), TaskType), SystemTaskTypeRu).ToString,
                        .Values = New ChartValues(Of MeasureModel)(List.OrderBy(Function(m) m.XDate))}).ToList
        End Function


        Private Function GetStackedColumnSeriesMonth() As IEnumerable(Of StackedColumnSeries)
            Return (From s In GetDataByMonth()
                    Group New MeasureModel(New DateTime(s.YearNum, s.MonthNum, 1), s.Qty) By s.TaskType Into List = ToList
                    Order By TaskType
                    Select New StackedColumnSeries With {
                        .StackMode = StackMode,
                        .Configuration = MonthMapper,
                        .Title = CType([Enum].ToObject(GetType(SystemTaskTypeRu), TaskType), SystemTaskTypeRu).ToString,
                        .Values = New ChartValues(Of MeasureModel)(List.OrderBy(Function(m) m.XDate))}).ToList
        End Function


        Public Sub Axis_PreviewRangeChanged(e As Events.PreviewRangeChangedEventArgs)
            If e.PreviewMaxValue < 0 OrElse e.PreviewMinValue < 0 Then
                Stop
            End If
            Dim Range = e.PreviewMaxValue - e.PreviewMinValue
            If Range = e.Range Then Return
            Select Case Scale
                Case Scale.Day
                    Select Case Range
                        Case Is < 7
                            e.Cancel = True
                        Case Is > 160
                            'Scale = Scale.Month
                            'AxisX.MinValue = Double.NaN
                            'AxisX.MaxValue = Double.NaN
                            'SeriesCollection.Clear()
                            'XFormatter = MonthFormatter
                            'SeriesCollection.AddRange(GetStackedColumnSeriesMonth())
                            'AxisX.MinValue = e.PreviewMinValue / 30.44
                            'AxisX.MaxValue = e.PreviewMaxValue / 30.44
                            e.Cancel = True
                    End Select
                Case Scale.Month
                    Select Case Range
                        Case Is < 6
                            Scale = Scale.Day
                            AxisX.MinValue = Double.NaN
                            AxisX.MaxValue = Double.NaN
                            SeriesCollection.Clear()
                            XFormatter = DayFormatter
                            SeriesCollection.AddRange(GetStackedColumnSeriesDay())
                            AxisX.MinValue = e.PreviewMinValue * 30.44
                            AxisX.MaxValue = e.PreviewMaxValue * 30.44
                            e.Cancel = True
                        Case Is > 120
                            e.Cancel = True
                    End Select
            End Select
        End Sub

    End Class
End Namespace