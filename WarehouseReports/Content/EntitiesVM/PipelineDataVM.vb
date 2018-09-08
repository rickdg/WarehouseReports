Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports FirstFloor.ModernUI.Presentation

Namespace Content
    Public Class PipelineDataVM

        Public Property ParentCollection As ObservableCollection(Of PipelineDataVM)
        Public Property PipelineData As PipelineData


        Public Property XDate As Date
            Get
                Return PipelineData.xDate
            End Get
            Set
                PipelineData.xDate = Value.Date
                EntityModifed("xDate")
            End Set
        End Property
        Public Property VolumeCargo As Double
            Get
                Return PipelineData.VolumeCargo
            End Get
            Set
                PipelineData.VolumeCargo = Value
                EntityModifed("VolumeCargo")
            End Set
        End Property
        Public Property VolumeBox As Double
            Get
                Return PipelineData.VolumeBox
            End Get
            Set
                PipelineData.VolumeBox = Value
                EntityModifed("VolumeBox")
            End Set
        End Property
        Public Property QtyBoxesNotPassedWeightControl As Integer
            Get
                Return PipelineData.QtyBoxesNotPassedWeightControl
            End Get
            Set
                PipelineData.QtyBoxesNotPassedWeightControl = Value
                EntityModifed("QtyBoxesNotPassedWeightControl")
            End Set
        End Property
        Public Property QtyBoxesPassedWeightControl As Integer
            Get
                Return PipelineData.QtyBoxesPassedWeightControl
            End Get
            Set
                PipelineData.QtyBoxesPassedWeightControl = Value
                EntityModifed("QtyBoxesPassedWeightControl")
            End Set
        End Property


#Region "Commands"
        Public ReadOnly Property CmdRemove As ICommand = New RelayCommand(AddressOf RemoveExecute)
        Private Sub RemoveExecute(parameter As Object)
            Using Context As New WarehouseDataEntities
                Context.Entry(PipelineData).State = EntityState.Deleted
                Context.SaveChanges()
                ParentCollection.Remove(Me)
            End Using
        End Sub
#End Region


        Private Sub EntityModifed(propertyName As String)
            Using Context As New WarehouseDataEntities
                Context.PipelineDatas.Attach(PipelineData)
                Context.Entry(PipelineData).Property(propertyName).IsModified = True
                Context.SaveChanges()
            End Using
        End Sub

    End Class
End Namespace