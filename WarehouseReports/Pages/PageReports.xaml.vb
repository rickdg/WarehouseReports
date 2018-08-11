Namespace Pages
    Partial Public Class PageReports
        Inherits UserControl

        Public Shared Property Model As New PageReportsVM


        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub

    End Class
End Namespace