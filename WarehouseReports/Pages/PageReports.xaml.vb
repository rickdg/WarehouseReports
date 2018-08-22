Namespace Pages
    Partial Public Class PageReports
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = Model
        End Sub


        Public Shared Property Model As New PageReportsVM

    End Class
End Namespace