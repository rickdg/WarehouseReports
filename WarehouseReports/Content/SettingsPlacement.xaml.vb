Namespace Content
    Partial Public Class SettingsPlacement
        Inherits UserControl

        Private Model As SettingsPlacementVM = New SettingsPlacementVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub
    End Class
End Namespace