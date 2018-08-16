Namespace Content
    Partial Public Class SettingsCustomGroups
        Inherits UserControl

        Private Model As SettingsCustomGroupsVM = New SettingsCustomGroupsVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub

    End Class
End Namespace