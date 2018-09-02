Imports System.Collections.ObjectModel
Imports FirstFloor.ModernUI.Presentation

Namespace Content
    Partial Public Class SettingsCustomGroups
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Using Context As New WarehouseDataEntities
                For Each CustomGroup In Context.CustomGroups
                    CustomGroupCollection.Add(New CustomGroupVM() With {.CustomGroup = CustomGroup})
                Next
            End Using
            DataContext = Me
        End Sub


        Public Property CustomGroupCollection As New ObservableCollection(Of CustomGroupVM)


        Public ReadOnly Property CmdAddNewCustomGroup As ICommand = New RelayCommand(AddressOf AddNewCustomGroupExecute)
        Private Sub AddNewCustomGroupExecute(parameter As Object)
            Using Context As New WarehouseDataEntities
                Dim NewCustomGroup = Context.CustomGroups.Add(New CustomGroup)
                Context.SaveChanges()
                CustomGroupCollection.Add(New CustomGroupVM With {.ParentCollection = CustomGroupCollection, .CustomGroup = NewCustomGroup})
            End Using
        End Sub

    End Class
End Namespace