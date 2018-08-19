Namespace Content
    Partial Public Class SettingsPlacement
        Inherits UserControl

        Public Shared Property Model As New SettingsExpressionTree(True) With {.SerializeFileName = "Placement"}
        Public Shared SerializeFileName As String = "Placement"


        Public Sub New()
            InitializeComponent()
            Model.Editor = TextEditor
            If FileExists("", SerializeFileName) Then
                Model.SetProperty(Deserialize(Of SettingsExpressionTree)("", SerializeFileName))
            End If
            DataContext = Model
        End Sub

    End Class
End Namespace