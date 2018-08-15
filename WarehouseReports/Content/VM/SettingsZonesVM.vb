Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsZonesVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each ZoneGroup In Context.Zones
                    Zones.Add(New ZoneVM() With {.Parent = Zones, .Zone = ZoneGroup})
                Next
            End Using
        End Sub


        Public Property ZoneNum As UInteger
        Public Property MainGroup As Integer
        Public Property CustomGroup As UInteger
        Public Property UpDown As Boolean
        Public Property PickingNorm As Double
        Public Property PickingNormText As String
            Get
                Return PickingNorm.ToString("P0")
            End Get
            Set
                If Double.TryParse(Value, PickingNorm) Then
                    PickingNorm /= 100
                Else
                    PickingNorm = 0
                End If
            End Set
        End Property
        Public Property Zones As New ObservableCollection(Of ZoneVM)


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
                Zones.Add(New ZoneVM() With {.Parent = Zones, .Zone = ZoneGroup})
            End Using
        End Sub

    End Class
End Namespace