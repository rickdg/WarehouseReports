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
                Dim Result As Double
                Double.TryParse(Value.TrimEnd("%"c).Replace(".", ","), Result)
                PickingNorm = Math.Abs(Result / 100)
            End Set
        End Property


        Private Sub EntityModifed(propertyName As String)
            Using Context = GetContext()
                Context.MainGroups.Attach(MainGroup)
                Context.Entry(MainGroup).Property(propertyName).IsModified = True
                Context.SaveChanges()
            End Using
        End Sub

    End Class
End Namespace