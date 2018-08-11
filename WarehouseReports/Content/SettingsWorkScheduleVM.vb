Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel
Imports WarehouseReports.DAL

Namespace Content
    Public Class SettingsWorkScheduleVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each Gang In Context.Gangs
                    Gangs.Add(New GangVM() With {.Parent = Gangs, .Gang = Gang})
                Next
            End Using
        End Sub


        Public Property GangNumber As UInteger
        Public Property StartTime As TimeSpan
        Public Property EndTime As TimeSpan
        Public Property PreviousDay As Boolean
        Public Property Gangs As New ObservableCollection(Of GangVM)


        Public ReadOnly Property CmdAddGang As ICommand = New RelayCommand(AddressOf AddGangExecute)
        Private Sub AddGangExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim Gang = Context.Gangs.Add(New Gang With {
                                             .Number = CInt(GangNumber),
                                             .StartTime = StartTime,
                                             .EndTime = EndTime,
                                             .PreviousDay = PreviousDay})
                Context.SaveChanges()
                Gangs.Add(New GangVM() With {.Parent = Gangs, .Gang = Gang})
            End Using
        End Sub

    End Class
End Namespace