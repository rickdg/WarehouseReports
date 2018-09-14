Imports FirstFloor.ModernUI.Windows.Controls
Imports FirstFloor.ModernUI.Presentation
Imports System.IO
Imports ICSharpCode.AvalonEdit.Highlighting
Imports System.Xml
Imports System.Reflection

Partial Public Class MainWindow
    Inherits ModernWindow

    Public Shared Property Model As MainWindowVM
    Private SerializeFileName As String = "Settings"


    Public Sub New()
        Dim CustomHighlighting As IHighlightingDefinition
        Using Stream = GetType(MainWindow).Assembly.GetManifestResourceStream("WarehouseReports.SQL-DarkTheme.xshd")
            Using Reader As New XmlTextReader(Stream)
                CustomHighlighting = Xshd.HighlightingLoader.Load(Reader, HighlightingManager.Instance)
            End Using
        End Using
        HighlightingManager.Instance.RegisterHighlighting("SQL-DarkTheme", New String() {".sql"}, CustomHighlighting)
        Using Stream = GetType(MainWindow).Assembly.GetManifestResourceStream("WarehouseReports.SQL-LightTheme.xshd")
            Using Reader As New XmlTextReader(Stream)
                CustomHighlighting = Xshd.HighlightingLoader.Load(Reader, HighlightingManager.Instance)
            End Using
        End Using
        HighlightingManager.Instance.RegisterHighlighting("SQL-LightTheme", New String() {".sql"}, CustomHighlighting)

        InitializeComponent()

        SetValue(TextOptions.TextFormattingModeProperty, TextFormattingMode.Display)

        BaseDirectory = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
        MyDocumentsDirectory = New DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                                      "NetApps",
                                                      Assembly.GetExecutingAssembly().GetName.Name))
        DbStartCheck()

        If FileExists(SerializeFileName) Then
            Model = Deserialize(Of MainWindowVM)(SerializeFileName)
        Else
            Model = New MainWindowVM With {
                .Height = 480, .Width = 854, .Top = 100, .Left = 300,
                .AppVersion = Assembly.GetExecutingAssembly.GetName.Version}
        End If
        DataContext = Model
    End Sub


    Private Sub ModernWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Model.ThemeSource = AppearanceManager.Current.ThemeSource
        Model.AccentColor = AppearanceManager.Current.AccentColor
        Serialize(Model, SerializeFileName)
    End Sub

End Class