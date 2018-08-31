﻿Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsZonesVM

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each ZoneGroup In Context.Zones
                    ZoneCollection.Add(New ZoneVM With {.ParentCollection = ZoneCollection, .Zone = ZoneGroup})
                Next
            End Using
        End Sub


        Public Property ZoneCollection As New ObservableCollection(Of ZoneVM)


        Public ReadOnly Property CmdAddNewZone As ICommand = New RelayCommand(AddressOf AddNewZoneExecute)
        Private Sub AddNewZoneExecute(parameter As Object)
            Using Context As New WarehouseDataEntities
                Dim NewZone = Context.Zones.Add(New Zone With {.MainGroup = 100})
                Context.SaveChanges()
                ZoneCollection.Add(New ZoneVM With {.ParentCollection = ZoneCollection, .Zone = NewZone})
            End Using
        End Sub

    End Class
End Namespace