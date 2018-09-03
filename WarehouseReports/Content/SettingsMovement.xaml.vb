Namespace Content
    Partial Public Class SettingsMovement
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Model.Editor = TextEditor
            If FileExists(Model.SerializeFileName) Then
                Model.SetProperty(Deserialize(Of SettingsExpressionTree)(Model.SerializeFileName))
            End If
            DataContext = Model
        End Sub


        Public Shared Property Model As New SettingsExpressionTree(True) With {
            .SerializeFileName = My.Settings.FileMovement,
            .LoadType = Enums.LoadType.Movement}

    End Class
End Namespace