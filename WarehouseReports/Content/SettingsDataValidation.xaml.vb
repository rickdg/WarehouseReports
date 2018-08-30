Namespace Content
    Partial Public Class SettingsDataValidation
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New SettingsDataValidationVM()
        End Sub

    End Class
End Namespace