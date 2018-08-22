Imports FirstFloor.ModernUI.Presentation
Imports Newtonsoft.Json

Namespace Content
    Public Class SettingsKPIVM
        Inherits NotifyPropertyChanged

        Public Property SerializeFileName As String
        Public Property Mechanization As New Percentage
        Public Property Movements As New Percentage
        Public Property Productivity As Double

        <JsonIgnore>
        Public ReadOnly Property CmdSave As ICommand = New RelayCommand(Sub() Serialize(Of SettingsExpressionTree)(Me, "", SerializeFileName))


        Public Sub SetProperty(model As SettingsKPIVM)
            SerializeFileName = model.SerializeFileName
            Mechanization = model.Mechanization
            Movements = model.Movements
            Productivity = model.Productivity
        End Sub

    End Class
End Namespace