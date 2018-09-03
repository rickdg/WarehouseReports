Imports FirstFloor.ModernUI.Presentation
Imports WarehouseReports.Enums
Imports WarehouseReports.ExcelConnection

Namespace Pages
    Partial Public Class DataValidation
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub


        Public ReadOnly Property CmdViewData As ICommand = New RelayCommand(AddressOf ViewDataExecute)
        Private Sub ViewDataExecute(parameter As Object)
            ViewData(CType(parameter, LoadType))
        End Sub

    End Class
End Namespace