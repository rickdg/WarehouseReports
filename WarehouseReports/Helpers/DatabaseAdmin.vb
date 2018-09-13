﻿Imports System.Collections.Specialized
Imports System.IO
Imports Microsoft.SqlServer.Management.Smo

Module DatabaseAdmin

    Const srvName = "(LocalDB)\MSSQLLocalDB"
    Const dbName = "WarehouseData"
    Const dbFile = "WarehouseData.mdf"
    Const dbFileLog = "WarehouseData_log.ldf"


    Private ReadOnly Property DbTarget As String
        Get
            Return Path.Combine(MyDocumentsDirectory.FullName, dbFile)
        End Get
    End Property


    Public Sub DbStartCheck()
        Dim Server As New Server(srvName)

        For Each db As Database In Server.Databases
            If db.Name = dbName Then
                If File.Exists(DbTarget) Then
                    Return
                Else
                    Try
                        db.Drop()
                    Catch ex As Exception
                        Windows.Application.Current.Shutdown()
                        Return
                    End Try
                End If
            End If
        Next

        Dim sc As New StringCollection From {
                MoveFile(dbFile),
                MoveFile(dbFileLog)}

        Server.AttachDatabase(dbName, sc, AttachOptions.None)
    End Sub


    Private Function MoveFile(fileName As String) As String
        Dim FromFile = Path.Combine(BaseDirectory.FullName, fileName)
        Dim ToFile = Path.Combine(MyDocumentsDirectory.FullName, fileName)

        If File.Exists(ToFile) Then File.Delete(ToFile)

        File.Move(FromFile, ToFile)

        Return ToFile
    End Function


    Public Function GetContext() As WarehouseDataEntities
        Dim Context As New WarehouseDataEntities
        Context.Database.Connection.ConnectionString = $"data source={srvName};attachdbfilename={DbTarget};integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"
        Return Context
    End Function

End Module