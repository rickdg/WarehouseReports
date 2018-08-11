Namespace Content
    Partial Public Class SettingsWorkSchedule
        Inherits UserControl

        Private Model As SettingsWorkScheduleVM = New SettingsWorkScheduleVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub
    End Class
End Namespace