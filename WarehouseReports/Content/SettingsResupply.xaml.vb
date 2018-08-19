Namespace Content
    Partial Public Class SettingsResupply
        Inherits UserControl

        Public Shared Property Model As New SettingsExpressionTree(True) With {.SerializeFileName = SerializeFileName}
        Private Const SerializeFileName As String = "Resupply"


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