Imports FirstFloor.ModernUI.Presentation
Imports Newtonsoft.Json
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsPipelineDataVM

        Public Sub New()
            Using Context = GetContext()
                For Each PipelineData In Context.PipelineDatas
                    PipelineDataCollection.Add(New PipelineDataVM With {.ParentCollection = PipelineDataCollection, .PipelineData = PipelineData})
                Next
            End Using
        End Sub


        Public Property SerializeFileName As String
        Public Property Gravitation As String()
        <JsonIgnore>
        Public Property GravitationText As String
            Get
                Return Join(Gravitation, ";")
            End Get
            Set
                Gravitation = Split(Value, ";")
            End Set
        End Property
        <JsonIgnore>
        Public Property PipelineDataCollection As New ObservableCollection(Of PipelineDataVM)


        <JsonIgnore>
        Public ReadOnly Property CmdSave As ICommand = New RelayCommand(Sub() Serialize(Me, SerializeFileName))
        <JsonIgnore>
        Public ReadOnly Property CmdAddNewPipelineData As ICommand = New RelayCommand(AddressOf AddNewPipelineDataExecute)
        Private Sub AddNewPipelineDataExecute(parameter As Object)
            Using Context = GetContext()
                Dim NewPipelineData = Context.PipelineDatas.Add(New PipelineData With {.XDate = Today.AddDays(-1)})
                Context.SaveChanges()
                PipelineDataCollection.Add(New PipelineDataVM With {.ParentCollection = PipelineDataCollection, .PipelineData = NewPipelineData})
            End Using
        End Sub


    End Class
End Namespace