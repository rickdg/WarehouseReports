Imports System.IO
Imports System.Reflection
Imports Newtonsoft.Json

Public Module JsonSerializer
    Private Function GetMyDocumentsPath(xpath As String) As String
        Dim ProductName = Assembly.GetExecutingAssembly().GetName.Name
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NetApps", ProductName, xpath)
    End Function


    Private Function CreateDirectory(xpath As String) As String
        Dim MyDocumentsPath = GetMyDocumentsPath(xpath)
        Directory.CreateDirectory(MyDocumentsPath)
        Return MyDocumentsPath
    End Function


    Public Sub Serialize(value As Object, xpath As String, name As String)
        Dim FullPath = Path.Combine(CreateDirectory(xpath), $"{name}.json")
        File.WriteAllText(FullPath, JsonConvert.SerializeObject(value))
    End Sub


    Public Function Deserialize(Of T)(xpath As String, name As String) As T
        Dim FullPath = Path.Combine(GetMyDocumentsPath(xpath), $"{name}.json")
        Return JsonConvert.DeserializeObject(Of T)(File.ReadAllText(FullPath))
    End Function


    Public Function FileExists(xpath As String, name As String) As Boolean
        Dim FullPath = Path.Combine(GetMyDocumentsPath(xpath), $"{name}.json")
        Return File.Exists(FullPath)
    End Function
End Module