Namespace Content
    Partial Public Class SettingsMainGroups
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New SettingsMainGroupsVM()
        End Sub

    End Class
End Namespace