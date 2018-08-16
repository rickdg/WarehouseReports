Namespace Content
    Partial Public Class SettingsGangs
        Inherits UserControl

        Private Model As SettingsGangsVM = New SettingsGangsVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub

    End Class
End Namespace