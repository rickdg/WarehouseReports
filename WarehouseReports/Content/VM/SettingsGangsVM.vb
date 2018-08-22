Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsGangsVM
        Inherits NotifyPropertyChanged

        Private _GangNumber As Integer


        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each Gang In Context.Gangs
                    GangCollection.Add(New GangVM With {.ParentCollection = GangCollection, .Gang = Gang})
                Next
            End Using
        End Sub


        Public Property GangNumber As Integer
            Get
                Return _GangNumber
            End Get
            Set
                _GangNumber = Math.Abs(Value)
            End Set
        End Property


        Public Property GangCollection As New ObservableCollection(Of GangVM)


        Public ReadOnly Property CmdAddNewGang As ICommand = New RelayCommand(AddressOf AddNewGangExecute)
        Private Sub AddNewGangExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim NewGang = Context.Gangs.Add(New Gang With {.Number = GangNumber})
                Context.SaveChanges()
                GangCollection.Add(New GangVM With {.ParentCollection = GangCollection, .Gang = NewGang})
            End Using
        End Sub

    End Class
End Namespace