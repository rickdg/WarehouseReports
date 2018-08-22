Imports System.IO
Imports System.Reflection
Imports Newtonsoft.Json

Public Module SerializerObjects

    Public Function GetMyDocumentsPath() As String
        Dim ProductName = Assembly.GetExecutingAssembly().GetName.Name
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NetApps", ProductName)
    End Function


    Private Function CreateDirectory() As String
        Dim MyDocumentsPath = GetMyDocumentsPath()
        Directory.CreateDirectory(MyDocumentsPath)
        Return MyDocumentsPath
    End Function


    Public Sub Serialize(value As Object, name As String)
        Dim FullPath = Path.Combine(CreateDirectory, $"{name}.json")
        Dim Serializer As New JsonSerializer With {
            .TypeNameHandling = TypeNameHandling.Auto,
            .NullValueHandling = NullValueHandling.Ignore,
            .Formatting = Formatting.Indented,
            .PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            .ReferenceLoopHandling = ReferenceLoopHandling.Serialize}
        Serializer.Converters.Add(New Converters.JavaScriptDateTimeConverter())
        Using StreamWriter As New StreamWriter(FullPath)
            Using Writer As New JsonTextWriter(StreamWriter)
                Serializer.Serialize(Writer, value)
            End Using
        End Using
    End Sub


    Public Function Deserialize(Of T)(name As String) As T
        Dim FullPath = Path.Combine(GetMyDocumentsPath, $"{name}.json")
        Return JsonConvert.DeserializeObject(Of T)(File.ReadAllText(FullPath), New JsonSerializerSettings With {
                                                   .TypeNameHandling = TypeNameHandling.Auto,
                                                   .NullValueHandling = NullValueHandling.Ignore})
    End Function


    Public Function FileExists(name As String) As Boolean
        Dim FullPath = Path.Combine(GetMyDocumentsPath, $"{name}.json")
        Return File.Exists(FullPath)
    End Function

End Module