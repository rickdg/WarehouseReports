Namespace Content
    Partial Public Class SettingsKPI
        Inherits UserControl

        Public Shared Property Model As New SettingsKPIVM With {.SerializeFileName = "KPI"}
        Public Shared SerializeFileName As String = "KPI"


        Private Sub SettingsKPI_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            If FileExists("", SerializeFileName) Then
                Model.SetProperty(Deserialize(Of SettingsKPIVM)("", SerializeFileName))
            End If
            DataContext = Model
        End Sub

    End Class
End Namespace