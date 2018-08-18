Namespace Content
    Partial Public Class SettingsResupply
        Inherits UserControl

        Private Model As SettingsResupplyVM = New SettingsResupplyVM()


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub

    End Class
End Namespace