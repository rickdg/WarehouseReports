Namespace Content
    Partial Public Class SettingsMainGroups
        Inherits UserControl

        Private Model As SettingsMainGroupsVM = New SettingsMainGroupsVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub
    End Class
End Namespace