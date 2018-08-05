Namespace Pages
    Partial Public Class Reports
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New ReportsVM
        End Sub
    End Class
End Namespace