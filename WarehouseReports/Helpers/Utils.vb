Imports System.IO

Public Class Utils

    Shared _BaseDirectory As DirectoryInfo = Nothing


    Public Shared Property BaseDirectory As DirectoryInfo
        Get
            Return _BaseDirectory
        End Get
        Set(ByVal value As DirectoryInfo)
            _BaseDirectory = value
            If Not _BaseDirectory.Exists Then
                _BaseDirectory.Create()
            End If
        End Set
    End Property


    Public Shared Function GetFileInfo(file As String, Optional deleteIfExists As Boolean = True) As FileInfo
        Dim fi = New FileInfo(Path.Combine(BaseDirectory.FullName, file))
        If deleteIfExists AndAlso fi.Exists Then
            fi.Delete()
        End If
        Return fi
    End Function


    Public Shared Function GetFileInfo(altOutputDir As DirectoryInfo, file As String, Optional deleteIfExists As Boolean = True) As FileInfo
        Dim fi = New FileInfo(Path.Combine(altOutputDir.FullName, file))
        If deleteIfExists AndAlso fi.Exists Then
            fi.Delete()
        End If
        Return fi
    End Function


    Friend Shared Function GetDirectoryInfo(directory As String) As DirectoryInfo
        Dim di = New DirectoryInfo(Path.Combine(_BaseDirectory.FullName, directory))
        If Not di.Exists Then
            di.Create()
        End If
        Return di
    End Function


    Public Shared Function GetCodeModule(codeDir As DirectoryInfo, fileName As String) As String
        Return File.ReadAllText(GetFileInfo(codeDir, fileName, False).FullName)
    End Function

End Class