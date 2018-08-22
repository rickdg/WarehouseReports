Namespace Content
    Partial Public Class SettingsCustomGroups
        Inherits UserControl

        Private Sub SettingsCustomGroups_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            DataContext = New SettingsCustomGroupsVM()
        End Sub

    End Class
End Namespace