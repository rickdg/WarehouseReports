Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsPipelineDataVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Using Context As New WarehouseDataEntities
                For Each PipelineData In Context.PipelineDatas
                    PipelineDataCollection.Add(New PipelineDataVM With {.ParentCollection = PipelineDataCollection, .PipelineData = PipelineData})
                Next
            End Using
        End Sub


        Public Property PipelineDataCollection As New ObservableCollection(Of PipelineDataVM)


        Public ReadOnly Property CmdAddNewPipelineData As ICommand = New RelayCommand(AddressOf AddNewPipelineDataExecute)
        Private Sub AddNewPipelineDataExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                Dim NewPipelineData = Context.PipelineDatas.Add(New PipelineData With {.xDate = Today.AddDays(-1)})
                Context.SaveChanges()
                PipelineDataCollection.Add(New PipelineDataVM With {.ParentCollection = PipelineDataCollection, .PipelineData = NewPipelineData})
            End Using
        End Sub

    End Class
End Namespace