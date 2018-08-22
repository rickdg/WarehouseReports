Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports FirstFloor.ModernUI.Presentation

Namespace Content
    Public Class GangVM

        Public Property ParentCollection As ObservableCollection(Of GangVM)
        Public Property Gang As Gang


        Public ReadOnly Property Name As String
            Get
                Return $"Смена {Gang.Number}"
            End Get
        End Property
        Public Property StartTime As TimeSpan
            Get
                Return Gang.StartTime
            End Get
            Set
                If Value.Days = 0 Then
                    Gang.StartTime = Value
                    EntityModifed("StartTime")
                End If
            End Set
        End Property
        Public Property EndTime As TimeSpan
            Get
                Return Gang.EndTime
            End Get
            Set
                If Value.Days = 0 Then
                    Gang.EndTime = Value
                    EntityModifed("EndTime")
                End If
            End Set
        End Property
        Public Property PreviousDay As Boolean
            Get
                Return Gang.PreviousDay
            End Get
            Set
                Gang.PreviousDay = Value
                EntityModifed("PreviousDay")
            End Set
        End Property


#Region "Commands"
        Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
        Private Sub RemoveExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Context.Entry(Gang).State = EntityState.Deleted
                Context.SaveChanges()
                ParentCollection.Remove(Me)
            End Using
        End Sub
#End Region


        Private Sub EntityModifed(propertyName As String)
            Using Context As New WarehouseDataEntities
                Context.Gangs.Attach(Gang)
                Context.Entry(Gang).Property(propertyName).IsModified = True
                Context.SaveChanges()
            End Using
        End Sub

    End Class
End Namespace