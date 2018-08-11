Imports System.Collections.ObjectModel
Imports FirstFloor.ModernUI.Presentation
Imports Microsoft.Win32
Imports WarehouseReports.DAL

Namespace Pages
    Public Class DataBaseManagementVM
        Inherits NotifyPropertyChanged

        Private ReadOnly CreateGangVM As Func(Of ObservableCollection(Of GangVM), Gang, GangVM) = Function(parent, gang) New GangVM() With {.Parent = parent, .Gang = gang}
        Private ReadOnly ZoneGroupVM As Func(Of ObservableCollection(Of ZoneGroupVM), ZoneGroup, ZoneGroupVM) = Function(parent, zoneGroup) New ZoneGroupVM() With {.Parent = parent, .ZoneGroup = zoneGroup}

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each Gang In Context.Gangs
                    Gangs.Add(CreateGangVM(Gangs, Gang))
                Next
                For Each ZoneGroup In Context.ZoneGroups
                    ZoneGroups.Add(ZoneGroupVM(ZoneGroups, ZoneGroup))
                Next
            End Using

            Dim MasterNode As New MasterNode
            Dim RootNode As New RootNode(MasterNode)

            RootNode.Nodes.Add(New RootNode(RootNode))
            RootNode.Nodes.Add(New LogicNode(RootNode))
            RootNode.Nodes.Add(New ExpressionNode(RootNode))

            MasterNode.Nodes.Add(RootNode)
            MasterNode.Nodes.Add(New LogicNode(MasterNode))
            MasterNode.Nodes.Add(New ExpressionNode(MasterNode))
            TreeRoot.Add(MasterNode)
        End Sub


        Public Property GangNumber As UInteger
        Public Property StartTime As TimeSpan
        Public Property EndTime As TimeSpan
        Public Property PreviousDay As Boolean
        Public Property Gangs As New ObservableCollection(Of GangVM)

        Public Property Zone As UInteger
        Public Property GroupA As UInteger
        Public Property GroupB As UInteger
        Public Property UpDown As Boolean
        Public Property ZoneGroups As New ObservableCollection(Of ZoneGroupVM)

        Public Property TreeRoot As New ObservableCollection(Of BaseNode)


#Region "Commands"
        Public ReadOnly Property CmdLoadTasks As ICommand = New RelayCommand(AddressOf LoadTasksExecute)
        Private Sub LoadTasksExecute(obj As Object)
            Dim DialogWindow As New OpenFileDialog With {.Title = "Выбрать файл"}
            If Not DialogWindow.ShowDialog Then Return
            DataLoader.LoadTasks(DialogWindow.FileName)
        End Sub


        Public ReadOnly Property CmdAddGang As ICommand = New RelayCommand(AddressOf AddGangExecute)
        Private Sub AddGangExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim Gang = Context.Gangs.Add(New Gang With {
                                             .Number = CInt(GangNumber),
                                             .StartTime = StartTime,
                                             .EndTime = EndTime,
                                             .PreviousDay = PreviousDay})
                Context.SaveChanges()
                Gangs.Add(CreateGangVM(Gangs, Gang))
            End Using
        End Sub


        Public ReadOnly Property CmdAddZoneGroup As ICommand = New RelayCommand(AddressOf AddZoneGroupExecute)
        Private Sub AddZoneGroupExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim ZoneGroup = Context.ZoneGroups.Add(New ZoneGroup With {
                                                       .Zone = CInt(Zone),
                                                       .GroupA = CInt(GroupA),
                                                       .GroupB = CInt(GroupB),
                                                       .UpDown = UpDown})
                Context.SaveChanges()
                ZoneGroups.Add(ZoneGroupVM(ZoneGroups, ZoneGroup))
            End Using
        End Sub
#End Region

    End Class
End Namespace