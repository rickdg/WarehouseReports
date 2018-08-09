Namespace Pages
    Partial Public Class PageReports
        Inherits UserControl

        Public Shared Property ReportsVM As New ReportsVM


        Public Sub New()
            InitializeComponent()
            DataContext = ReportsVM
        End Sub

    End Class
End Namespace