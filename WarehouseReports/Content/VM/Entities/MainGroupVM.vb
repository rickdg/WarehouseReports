Namespace Content
    Public Class MainGroupVM

        Public Property MainGroup As MainGroup
        Public Property PickingNorm As Double
            Get
                Return MainGroup.PickingNorm
            End Get
            Set
                MainGroup.PickingNorm = Value
                EntityModifed("PickingNorm")
            End Set
        End Property
        Public Property PickingNormText As String
            Get
                Return PickingNorm.ToString("P0")
            End Get
            Set
                If Double.TryParse(Value, PickingNorm) Then
                    PickingNorm = Math.Abs(PickingNorm) / 100
                Else
                    PickingNorm = 0
                End If
            End Set
        End Property


        Private Sub EntityModifed(propertyName As String)
            Using Context As New WarehouseDataEntities
                Context.MainGroups.Attach(MainGroup)
                Context.Entry(MainGroup).Property(propertyName).IsModified = True
                Context.SaveChanges()
            End Using
        End Sub

    End Class
End Namespace