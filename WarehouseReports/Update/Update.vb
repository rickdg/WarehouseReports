Module Update

    Public ReadOnly Property Revisions As New Dictionary(Of Integer, Revision) From {
        {90, New Revision With {.VersionPart = "1.0.7", .Number = 90, .XDate = New DateTime(2018, 11, 20)}},
        {89, New Revision With {.VersionPart = "1.0.6", .Number = 89, .XDate = New DateTime(2018, 11, 16)}},
        {88, New Revision With {.VersionPart = "1.0.5", .Number = 88, .XDate = New DateTime(2018, 9, 16)}},
        {87, New Revision With {.VersionPart = "1.0.5", .Number = 87, .XDate = New DateTime(2018, 9, 15)}}}


    Public Sub ExecuteUpdate(oldRevision As Integer, newRevision As Integer)
        For r = oldRevision To newRevision
            Revisions(r).UpdateDateBase()
        Next
    End Sub


    Public Sub ShowUpdate()
        'Revisions(88).UpdateDateBase()
        'Revisions(88).Show()
        If MainWindow.Model.IsNewVersion Then
            For r = MainWindow.Model.OldRevision To MainWindow.Model.NewRevision
                Revisions(r).Show()
            Next
        End If
    End Sub

End Module