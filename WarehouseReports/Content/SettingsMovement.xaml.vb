Namespace Content
    Partial Public Class SettingsMovement
        Inherits UserControl

        Private Model As SettingsMovementVM = New SettingsMovementVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub
    End Class
End Namespace