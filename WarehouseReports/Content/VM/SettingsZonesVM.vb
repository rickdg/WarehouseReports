Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsZonesVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each ZoneGroup In Context.Zones
                    ZoneGroups.Add(New ZoneGroupVM() With {.Parent = ZoneGroups, .ZoneGroup = ZoneGroup})
                Next
            End Using
        End Sub


        Public Property ZoneNum As UInteger
        Public Property MainGroup As UInteger
        Public Property CustomGroup As UInteger
        Public Property UpDown As Boolean
        Public Property PickingNorm As Double
        Public Property PickingNormText As String
            Get
                Return PickingNorm.ToString("P0")
            End Get
            Set(value As String)
                If Double.TryParse(value, PickingNorm) Then
                    PickingNorm /= 100
                Else
                    PickingNorm = 0
                End If
            End Set
        End Property
        Public Property ZoneGroups As New ObservableCollection(Of ZoneGroupVM)


        Public ReadOnly Property CmdAddZoneGroup As ICommand = New RelayCommand(AddressOf AddZoneGroupExecute)
        Private Sub AddZoneGroupExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim ZoneGroup = Context.Zones.Add(New Zone With {
                                                       .ZoneNum = CInt(ZoneNum),
                                                       .MainGroup = CInt(MainGroup),
                                                       .CustomGroup = CInt(CustomGroup),
                                                       .UpDown = UpDown,
                                                       .PickingNorm = PickingNorm})
                Context.SaveChanges()
                ZoneGroups.Add(New ZoneGroupVM() With {.Parent = ZoneGroups, .ZoneGroup = ZoneGroup})
            End Using
        End Sub

    End Class
End Namespace