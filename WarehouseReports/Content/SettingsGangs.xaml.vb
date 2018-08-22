Namespace Content
    Partial Public Class SettingsGangs
        Inherits UserControl

        Private Sub SettingsGangs_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            DataContext = New SettingsGangsVM()
        End Sub

    End Class
End Namespace