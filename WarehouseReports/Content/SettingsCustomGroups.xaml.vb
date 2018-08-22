Namespace Content
    Partial Public Class SettingsCustomGroups
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New SettingsCustomGroupsVM()
        End Sub

    End Class
End Namespace