Namespace Content
    Partial Public Class SettingsPipelineData
        Inherits UserControl

        Private Model As SettingsPipelineDataVM


        Public Sub New()
            InitializeComponent()
            If FileExists(My.Settings.FilePipeline) Then
                Model = Deserialize(Of SettingsPipelineDataVM)(My.Settings.FilePipeline)
            Else
                Model = New SettingsPipelineDataVM With {.SerializeFileName = My.Settings.FilePipeline}
            End If
            DataContext = Model
        End Sub

    End Class
End Namespace