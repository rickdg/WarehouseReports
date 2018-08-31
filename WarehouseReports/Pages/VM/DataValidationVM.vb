Imports FirstFloor.ModernUI.Presentation
Imports WarehouseReports.Enums
Imports WarehouseReports.ExcelConnection

Namespace Pages
    Public Class DataValidationVM

        Public ReadOnly Property CmdViewData As ICommand = New RelayCommand(AddressOf ViewDataExecute)
        Private Sub ViewDataExecute(parameter As Object)
            ViewData(CType(parameter, SystemTaskType))
        End Sub

    End Class
End Namespace