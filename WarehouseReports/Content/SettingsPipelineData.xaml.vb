Namespace Content
    Partial Public Class SettingsPipelineData
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New SettingsPipelineDataVM()
        End Sub

    End Class
End Namespace