Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsMainGroupsVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each MainGroup In Context.MainGroups
                    MainGroupCollection.Add(New MainGroupVM() With {.MainGroup = MainGroup})
                Next
            End Using
        End Sub


        Public Property MainGroupCollection As New ObservableCollection(Of MainGroupVM)

    End Class
End Namespace