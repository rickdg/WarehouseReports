Imports System.Collections.ObjectModel
Imports FirstFloor.ModernUI.Presentation
Imports Microsoft.Win32
Imports WarehouseReports.DAL

Namespace Pages
    Public Class DataBaseManagementVM
        Inherits NotifyPropertyChanged

        Private ReadOnly CreateGangVM As Func(Of ObservableCollection(Of GangVM), Gang, GangVM) = Function(parent, gang) New GangVM() With {.Parent = parent, .Gang = gang}


        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each Gang In Context.Gangs
                    Gangs.Add(CreateGangVM(Gangs, Gang))
                Next
            End Using

            Dim MasterNode As New MasterNodeVM
            Dim RootNode As New RootNodeVM(MasterNode)

            RootNode.Nodes.Add(New RootNodeVM(RootNode))
            RootNode.Nodes.Add(New LogicNodeVM(RootNode))
            RootNode.Nodes.Add(New ExpressionNodeVM(RootNode))

            MasterNode.Nodes.Add(RootNode)
            MasterNode.Nodes.Add(New LogicNodeVM(MasterNode))
            MasterNode.Nodes.Add(New ExpressionNodeVM(MasterNode))
            TreeRoot.Add(MasterNode)
        End Sub


        Public Property GangNumber As UInteger
        Public Property StartTime As TimeSpan
        Public Property EndTime As TimeSpan
        Public Property PreviousDay As Boolean
        Public Property Gangs As New ObservableCollection(Of GangVM)
        Public Property TreeRoot As New ObservableCollection(Of BaseNodeVM)


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
#End Region

    End Class
End Namespace