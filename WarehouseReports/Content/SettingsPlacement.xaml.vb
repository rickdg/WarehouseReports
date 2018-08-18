Namespace Content
    Partial Public Class SettingsPlacement
        Inherits UserControl

        Private Model As SettingsExpressionTree
        Private Const SerializeFileName As String = "Placement"


        Public Sub New()
            InitializeComponent()
            If FileExists("", SerializeFileName) Then
                Model = Deserialize(Of SettingsExpressionTree)("", SerializeFileName)
            Else
                Model = New SettingsExpressionTree(True) With {.SerializeFileName = SerializeFileName}
            End If
            DataContext = Model
        End Sub

    End Class
End Namespace