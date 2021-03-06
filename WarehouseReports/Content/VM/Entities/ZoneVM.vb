﻿Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports FirstFloor.ModernUI.Presentation

Namespace Content
    Public Class ZoneVM

        Public Property ParentCollection As ObservableCollection(Of ZoneVM)
        Public Property Zone As Zone


        Public Property ZoneNum As Integer
            Get
                Return Zone.ZoneNum
            End Get
            Set
                Zone.ZoneNum = Math.Abs(Value)
                EntityModifed("ZoneNum")
            End Set
        End Property
        Public Property MainGroup As Integer
            Get
                Return Zone.MainGroup
            End Get
            Set
                Zone.MainGroup = Value
                EntityModifed("MainGroup")
            End Set
        End Property
        Public Property CustomGroup As Integer
            Get
                Return Zone.CustomGroup
            End Get
            Set
                Zone.CustomGroup = Math.Abs(Value)
                EntityModifed("CustomGroup")
            End Set
        End Property
        Public Property UpDown As Boolean
            Get
                Return Zone.UpDown
            End Get
            Set
                Zone.UpDown = Value
                EntityModifed("UpDown")
            End Set
        End Property
        Public Property PickingNorm As Double
            Get
                Return Zone.PickingNorm
            End Get
            Set
                Zone.PickingNorm = Value
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
                PickingNorm = Math.Abs(Result) / 100
            End Set
        End Property


#Region "Commands"
        Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
        Private Sub RemoveExecute(parameter As Object)
            Using Context = GetContext()
                Context.Entry(Zone).State = EntityState.Deleted
                Context.SaveChanges()
                ParentCollection.Remove(Me)
            End Using
        End Sub
#End Region


        Private Sub EntityModifed(propertyName As String)
            Using Context = GetContext()
                Context.Zones.Attach(Zone)
                Context.Entry(Zone).Property(propertyName).IsModified = True
                Context.SaveChanges()
            End Using
        End Sub

    End Class
End Namespace