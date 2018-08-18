Imports System.IO
Imports System.Reflection
Imports Newtonsoft.Json

Public Module SerializerObjects

    Public Function GetMyDocumentsPath(xpath As String) As String
        Dim ProductName = Assembly.GetExecutingAssembly().GetName.Name
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NetApps", ProductName, xpath)
    End Function


    Private Function CreateDirectory(xpath As String) As String
        Dim MyDocumentsPath = GetMyDocumentsPath(xpath)
        Directory.CreateDirectory(MyDocumentsPath)
        Return MyDocumentsPath
    End Function


    Public Sub Serialize(Of T)(value As Object, xpath As String, name As String)
        Dim FullPath = Path.Combine(CreateDirectory(xpath), $"{name}.json")
        Dim Serializer As New JsonSerializer With {
            .TypeNameHandling = TypeNameHandling.Auto,
            .NullValueHandling = NullValueHandling.Ignore,
            .Formatting = Formatting.Indented,
            .PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            .ReferenceLoopHandling = ReferenceLoopHandling.Serialize}
        Serializer.Converters.Add(New Converters.JavaScriptDateTimeConverter())
        Using StreamWriter As New StreamWriter(FullPath)
            Using Writer As New JsonTextWriter(StreamWriter)
                Serializer.Serialize(Writer, value, GetType(T))
            End Using
        End Using
    End Sub


    Public Function Deserialize(Of T)(xpath As String, name As String) As T
        Dim FullPath = Path.Combine(GetMyDocumentsPath(xpath), $"{name}.json")
        Return JsonConvert.DeserializeObject(Of T)(File.ReadAllText(FullPath), New JsonSerializerSettings With {
                                                   .TypeNameHandling = TypeNameHandling.Auto,
                                                   .NullValueHandling = NullValueHandling.Ignore})
    End Function


    Public Function FileExists(xpath As String, name As String) As Boolean
        Dim FullPath = Path.Combine(GetMyDocumentsPath(xpath), $"{name}.json")
        Return File.Exists(FullPath)
    End Function

End Module