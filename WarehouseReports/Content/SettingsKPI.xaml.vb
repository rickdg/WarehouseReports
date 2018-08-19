Namespace Content
    Partial Public Class SettingsKPI
        Inherits UserControl

        Public Shared Property Model As New SettingsKPIVM With {.SerializeFileName = "KPI"}
        Public Shared SerializeFileName As String = "KPI"


        Public Sub New()
            InitializeComponent()
            If FileExists("", SerializeFileName) Then
                Model.SetProperty(Deserialize(Of SettingsKPIVM)("", SerializeFileName))
            End If
            DataContext = Model
        End Sub

    End Class
End Namespace