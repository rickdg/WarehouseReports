Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsZonesVM
        Inherits NotifyPropertyChanged

        Private _ZoneNum As Integer
        Private _CustomGroup As Integer


        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each ZoneGroup In Context.Zones
                    ZoneCollection.Add(New ZoneVM With {.ParentCollection = ZoneCollection, .Zone = ZoneGroup})
                Next
            End Using
            MainGroup = 100
        End Sub


        Public Property ZoneNum As Integer
            Get
                Return _ZoneNum
            End Get
            Set
                _ZoneNum = Math.Abs(Value)
            End Set
        End Property
        Public Property MainGroup As Integer
        Public Property CustomGroup As Integer
            Get
                Return _CustomGroup
            End Get
            Set
                _CustomGroup = Math.Abs(Value)
            End Set
        End Property
        Public Property UpDown As Boolean
        Public Property PickingNorm As Double
        Public Property PickingNormText As String
            Get
                Return PickingNorm.ToString("P0")
            End Get
            Set
                If Double.TryParse(Value, PickingNorm) Then
                    PickingNorm = Math.Abs(PickingNorm) / 100
                Else
                    PickingNorm = 0
                End If
            End Set
        End Property


        Public Property ZoneCollection As New ObservableCollection(Of ZoneVM)


        Public ReadOnly Property CmdAddNewZone As ICommand = New RelayCommand(AddressOf AddNewZoneExecute)
        Private Sub AddNewZoneExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim NewZone = Context.Zones.Add(New Zone With {
                                                  .ZoneNum = ZoneNum,
                                                  .MainGroup = MainGroup,
                                                  .CustomGroup = CustomGroup,
                                                  .UpDown = UpDown,
                                                  .PickingNorm = PickingNorm})
                Context.SaveChanges()
                ZoneCollection.Add(New ZoneVM With {.ParentCollection = ZoneCollection, .Zone = NewZone})
            End Using
        End Sub

    End Class
End Namespace