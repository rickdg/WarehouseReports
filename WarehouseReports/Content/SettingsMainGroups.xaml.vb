Imports System.Collections.ObjectModel

Namespace Content
    Partial Public Class SettingsMainGroups
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Using Context As New WarehouseDataEntities
                For Each MainGroup In Context.MainGroups
                    MainGroupCollection.Add(New MainGroupVM() With {.MainGroup = MainGroup})
                Next
            End Using
            DataContext = Me
        End Sub


        Public Property MainGroupCollection As New ObservableCollection(Of MainGroupVM)

    End Class
End Namespace