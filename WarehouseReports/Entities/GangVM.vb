Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports WarehouseReports.DAL

Public Class GangVM

    Public Property Parent As ObservableCollection(Of GangVM)
    Public Property Gang As Gang
    Public ReadOnly Property Name As String
        Get
            Return $"Смена {Gang.Number}"
        End Get
    End Property


#Region "Commands"
    Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
    Private Sub RemoveExecute(obj As Object)
        Using Context As New WarehouseDataEntities
            Context.Entry(Gang).State = EntityState.Deleted
            Context.SaveChanges()
            Parent.Remove(Me)
        End Using
    End Sub
#End Region

End Class