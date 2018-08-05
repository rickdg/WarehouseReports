Namespace Pages
    Partial Public Class DataBaseManagement
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New DataBaseManagementVM
        End Sub
    End Class
End Namespace