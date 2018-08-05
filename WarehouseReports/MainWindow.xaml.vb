Imports FirstFloor.ModernUI.Windows.Controls
Imports FirstFloor.ModernUI.Presentation

Partial Public Class MainWindow
    Inherits ModernWindow

    Private Model As MainWindowVM
    Private Const SerializeFileName As String = "Settings"


    Public Sub New()
        InitializeComponent()
        If JsonSerializer.FileExists("", SerializeFileName) Then
            Model = JsonSerializer.Deserialize(Of MainWindowVM)("", SerializeFileName)
        Else
            Model = New MainWindowVM With {.Height = 500, .Width = 700, .Top = 100, .Left = 300}
        End If
        DataContext = Model
    End Sub


    Private Sub ModernWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Model.ThemeSource = AppearanceManager.Current.ThemeSource
        Model.AccentColor = AppearanceManager.Current.AccentColor

        JsonSerializer.Serialize(Model, "", SerializeFileName)
    End Sub
End Class