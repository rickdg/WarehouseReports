Namespace Pages
    Partial Public Class DataManagement
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            'Model.AxisX = AxisX
            'AddHandler AxisX.PreviewRangeChanged, AddressOf Model.Axis_PreviewRangeChanged
            DataContext = Model
        End Sub


        Public Shared Property Model As New DataManagementVM

    End Class
End Namespace