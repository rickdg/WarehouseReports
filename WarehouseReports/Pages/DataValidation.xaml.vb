Namespace Pages
    Partial Public Class DataValidation
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New DataValidationVM()
        End Sub

    End Class
End Namespace