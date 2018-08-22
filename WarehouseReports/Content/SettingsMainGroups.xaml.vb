Namespace Content
    Partial Public Class SettingsMainGroups
        Inherits UserControl

        Private Sub SettingsMainGroups_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            DataContext = New SettingsMainGroupsVM()
        End Sub

    End Class
End Namespace