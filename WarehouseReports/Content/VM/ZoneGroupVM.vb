Imports System.Collections.ObjectModel
Imports System.Data.Entity

Namespace Content
    Public Class ZoneGroupVM

        Public Property Parent As ObservableCollection(Of ZoneGroupVM)
        Public Property ZoneGroup As ZoneGroup


#Region "Commands"
        Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
        Private Sub RemoveExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Context.Entry(ZoneGroup).State = EntityState.Deleted
                Context.SaveChanges()
                Parent.Remove(Me)
            End Using
        End Sub
#End Region

    End Class
End Namespace