Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports FirstFloor.ModernUI.Presentation

Namespace Content
    Public Class CustomGroupVM

        Public Property ParentCollection As ObservableCollection(Of CustomGroupVM)
        Public Property CustomGroup As CustomGroup


        Public Property Group As Integer
            Get
                Return CustomGroup.Group
            End Get
            Set
                CustomGroup.Group = Math.Abs(Value)
                EntityModifed("Group")
            End Set
        End Property
        Public Property PickingNorm As Double
            Get
                Return CustomGroup.PickingNorm
            End Get
            Set
                CustomGroup.PickingNorm = Value
                EntityModifed("PickingNorm")
            End Set
        End Property
        Public Property PickingNormText As String
            Get
                Return PickingNorm.ToString("P0")
            End Get
            Set
                Dim Result As Double
                Double.TryParse(Value.TrimEnd("%"c).Replace(".", ","), Result)
                PickingNorm = Math.Abs(Result / 100)
            End Set
        End Property


#Region "Commands"
        Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
        Private Sub RemoveExecute(parameter As Object)
            Using Context As New WarehouseDataEntities
                Context.Entry(CustomGroup).State = EntityState.Deleted
                Context.SaveChanges()
                ParentCollection.Remove(Me)
            End Using
        End Sub
#End Region


        Private Sub EntityModifed(propertyName As String)
            Using Context As New WarehouseDataEntities
                Context.CustomGroups.Attach(CustomGroup)
                Context.Entry(CustomGroup).Property(propertyName).IsModified = True
                Context.SaveChanges()
            End Using
        End Sub

    End Class
End Namespace