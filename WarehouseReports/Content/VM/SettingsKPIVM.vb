Imports FirstFloor.ModernUI.Presentation
Imports Newtonsoft.Json

Namespace Content
    Public Class SettingsKPIVM
        Inherits NotifyPropertyChanged

        Private _Mechanization As Double


        Public Property SerializeFileName As String
        Public Property Mechanization As Double
            Get
                Return _Mechanization
            End Get
            Set
                _Mechanization = Math.Abs(Value)
            End Set
        End Property
        Public Property PercentageMovements As Double
        <JsonIgnore>
        Public Property PercentageMovementsText As String
            Get
                Return PercentageMovements.ToString("P1")
            End Get
            Set
                If Double.TryParse(Value, PercentageMovements) Then
                    PercentageMovements = Math.Abs(PercentageMovements) / 100
                Else
                    PercentageMovements = 0
                End If
            End Set
        End Property


        <JsonIgnore>
        Public ReadOnly Property CmdSave As ICommand = New RelayCommand(Sub() Serialize(Of SettingsExpressionTree)(Me, "", SerializeFileName))


        Public Sub SetProperty(model As SettingsKPIVM)
            SerializeFileName = model.SerializeFileName
        End Sub

    End Class
End Namespace