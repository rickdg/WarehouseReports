Namespace Content
    Partial Public Class SettingsKPI
        Inherits UserControl

        Private Model As SettingsKPIVM


        Public Sub New()
            InitializeComponent()
            If FileExists(My.Settings.FileKPI) Then
                Model = Deserialize(Of SettingsKPIVM)(My.Settings.FileKPI)
            Else
                Model = New SettingsKPIVM With {.SerializeFileName = My.Settings.FileKPI}
            End If
            DataContext = Model
        End Sub

    End Class
End Namespace