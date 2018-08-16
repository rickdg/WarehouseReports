Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsGangsVM
        Inherits NotifyPropertyChanged

        Private _StartTime As TimeSpan
        Private _EndTime As TimeSpan
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
        Public Property StartTime As TimeSpan
            Get
                Return _StartTime
            End Get
            Set
                If Value.Days = 0 Then
                    _StartTime = Value
                End If
            End Set
        End Property
        Public Property EndTime As TimeSpan
            Get
                Return _EndTime
            End Get
            Set
                If Value.Days = 0 Then
                    _EndTime = Value
                End If
            End Set
        End Property
        Public Property PreviousDay As Boolean


        Public Property GangCollection As New ObservableCollection(Of GangVM)


        Public ReadOnly Property CmdAddNewGang As ICommand = New RelayCommand(AddressOf AddNewGangExecute)
        Private Sub AddNewGangExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim NewGang = Context.Gangs.Add(New Gang With {
                                                .Number = GangNumber,
                                                .StartTime = StartTime,
                                                .EndTime = EndTime,
                                                .PreviousDay = PreviousDay})
                Context.SaveChanges()
                GangCollection.Add(New GangVM With {.ParentCollection = GangCollection, .Gang = NewGang})
            End Using
        End Sub

    End Class
End Namespace