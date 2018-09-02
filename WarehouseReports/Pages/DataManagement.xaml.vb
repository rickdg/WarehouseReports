Namespace Pages
    Partial Public Class DataManagement
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Dim Model As New DataManagementVM With {.AxisX = AxisX}
            AddHandler AxisX.PreviewRangeChanged, AddressOf Model.Axis_PreviewRangeChanged
            DataContext = Model
        End Sub

    End Class
End Namespace