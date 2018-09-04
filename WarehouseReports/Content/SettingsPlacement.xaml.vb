Namespace Content
    Partial Public Class SettingsPlacement
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Model.TextEditor = TextEditor
            If FileExists(Model.SerializeFileName) Then
                Model.SetProperty(Deserialize(Of SettingsExpressionTree)(Model.SerializeFileName))
            Else
                Model.RefreshTextEditor()
            End If
            DataContext = Model
        End Sub


        Public Shared Property Model As New SettingsExpressionTree(True) With {
            .SerializeFileName = My.Settings.FilePlacement,
            .LoadType = Enums.LoadType.Placement}

    End Class
End Namespace