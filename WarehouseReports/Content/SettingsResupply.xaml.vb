Namespace Content
    Partial Public Class SettingsResupply
        Inherits UserControl

        Public Shared Property Model As New SettingsExpressionTree(True) With {.SerializeFileName = "Resupply"}
        Public Shared SerializeFileName As String = "Resupply"


        Private Sub SettingsResupply_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            Model.Editor = TextEditor
            If FileExists("", SerializeFileName) Then
                Model.SetProperty(Deserialize(Of SettingsExpressionTree)("", SerializeFileName))
            End If
            DataContext = Model
        End Sub

    End Class
End Namespace