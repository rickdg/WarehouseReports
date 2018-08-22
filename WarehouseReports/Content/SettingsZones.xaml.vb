Namespace Content
    Partial Public Class SettingsZones
        Inherits UserControl

        Private Sub SettingsZones_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            DataContext = New SettingsZonesVM()
        End Sub

    End Class
End Namespace