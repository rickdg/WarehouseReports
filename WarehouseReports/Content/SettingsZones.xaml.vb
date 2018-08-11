Namespace Content
    Partial Public Class SettingsZones
        Inherits UserControl

        Private Model As SettingsZonesVM = New SettingsZonesVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub
    End Class
End Namespace