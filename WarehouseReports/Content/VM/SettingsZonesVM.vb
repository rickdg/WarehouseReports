Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsZonesVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each ZoneGroup In Context.ZoneGroups
                    ZoneGroups.Add(New ZoneGroupVM() With {.Parent = ZoneGroups, .ZoneGroup = ZoneGroup})
                Next
            End Using
        End Sub


        Public Property Zone As UInteger
        Public Property GroupA As UInteger
        Public Property GroupB As UInteger
        Public Property UpDown As Boolean
        Public Property PickingNorm As Double
        Public Property ZoneGroups As New ObservableCollection(Of ZoneGroupVM)


        Public ReadOnly Property CmdAddZoneGroup As ICommand = New RelayCommand(AddressOf AddZoneGroupExecute)
        Private Sub AddZoneGroupExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim ZoneGroup = Context.ZoneGroups.Add(New ZoneGroup With {
                                                       .Zone = CInt(Zone),
                                                       .GroupA = CInt(GroupA),
                                                       .GroupB = CInt(GroupB),
                                                       .UpDown = UpDown,
                                                       .PickingNorm = PickingNorm})
                Context.SaveChanges()
                ZoneGroups.Add(New ZoneGroupVM() With {.Parent = ZoneGroups, .ZoneGroup = ZoneGroup})
            End Using
        End Sub

    End Class
End Namespace