Namespace Content
    Partial Public Class SettingsAppearance
        Inherits UserControl

        Private Model As SettingsAppearanceVM = New SettingsAppearanceVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub
    End Class
End Namespace