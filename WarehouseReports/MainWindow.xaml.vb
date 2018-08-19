Imports FirstFloor.ModernUI.Windows.Controls
Imports FirstFloor.ModernUI.Presentation
Imports System.IO
Imports System.Data.SqlClient
Imports ICSharpCode.AvalonEdit.Highlighting
Imports System.Xml

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

        If FileExists("", SerializeFileName) Then
            Model = Deserialize(Of MainWindowVM)("", SerializeFileName)
        Else
            Model = New MainWindowVM With {.Height = 480, .Width = 945, .Top = 100, .Left = 300}
        End If
        DataContext = Model

        'ReadOrderData()
    End Sub


    Public Sub ReadOrderData()

        Dim conn As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;;Integrated Security=True")
        Dim cmd As New SqlCommand("", conn)
        Dim rdr As SqlDataReader

        cmd.CommandText = "SELECT DISTINCT    CATALOG_NAME    FROM    INFORMATION_SCHEMA.SCHEMATA"

        conn.Open()

        rdr = cmd.ExecuteReader()
        While (rdr.Read())
            MsgBox(rdr.GetString(0))
        End While

        rdr.Dispose()
        cmd.Dispose()
        conn.Dispose()



        'Dim DataBaseFile As FileInfo
        'Using connection As New SqlConnection(My.Settings.WarehouseDataConnectionString)
        '    connection.Open()
        '    DataBaseFile = New FileInfo(connection.Database)
        'End Using

        'DataBaseFile.CopyTo(Path.Combine(GetMyDocumentsPath(""), DataBaseFile.Name))

        'Using c As New WarehouseDataEntities
        '    c.Database.Connection.ConnectionString = c.Database.Connection.ConnectionString.Replace("localhost", "Live")
        'End Using
    End Sub


    Private Sub ModernWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Model.ThemeSource = AppearanceManager.Current.ThemeSource
        Model.AccentColor = AppearanceManager.Current.AccentColor

        Serialize(Of MainWindowVM)(Model, "", SerializeFileName)
    End Sub

End Class