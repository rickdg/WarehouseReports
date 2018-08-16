Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsCustomGroupsVM
        Inherits NotifyPropertyChanged

        Private _Group As Integer


        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each CustomGroup In Context.CustomGroups
                    CustomGroupCollection.Add(New CustomGroupVM() With {.CustomGroup = CustomGroup})
                Next
            End Using
        End Sub


        Public Property Group As Integer
            Get
                Return _Group
            End Get
            Set
                _Group = Math.Abs(Value)
            End Set
        End Property
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


        Public Property CustomGroupCollection As New ObservableCollection(Of CustomGroupVM)


        Public ReadOnly Property CmdAddNewCustomGroup As ICommand = New RelayCommand(AddressOf AddNewCustomGroupExecute)
        Private Sub AddNewCustomGroupExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim NewCustomGroup = Context.CustomGroups.Add(New CustomGroup With {.Group = Group, .PickingNorm = PickingNorm})
                Context.SaveChanges()
                CustomGroupCollection.Add(New CustomGroupVM With {.ParentCollection = CustomGroupCollection, .CustomGroup = NewCustomGroup})
            End Using
        End Sub

    End Class
End Namespace