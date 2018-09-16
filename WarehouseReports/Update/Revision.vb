Imports System.Data.SqlClient
Imports FirstFloor.ModernUI.Windows.Controls

Public Class Revision

    Public Property Version As String
    Public Property Number As Integer
    Public Property IsUpdate As Boolean


    Public Sub UpdateDateBase()
        If Not IsUpdate Then Return

        Using Connection As New SqlConnection(GetSqlConnectionString)
            Connection.Open()
            Using Command = Connection.CreateCommand()
                Command.CommandTimeout = 1800
                Command.CommandText = GetSqlCommand()
                Command.ExecuteNonQuery()
            End Using
        End Using
    End Sub


    Public Sub Show()
        Dim Dlg As New ModernDialog With {
            .WindowStartupLocation = WindowStartupLocation.CenterScreen,
            .Title = $"{Version}.{Number}",
            .Content = New RevisionDialog(GetContent)}
        Dlg.ShowDialog()
    End Sub


    Private Function GetContent() As UIElement
        Return CType(Application.Current.FindResource($"Content-{Number}"), UIElement)
    End Function


    Private Function GetSqlCommand() As String
        Return CStr(Application.Current.FindResource($"SqlCommand-{Number}"))
    End Function

End Class