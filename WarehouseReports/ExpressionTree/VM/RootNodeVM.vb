Public Class RootNodeVM
    Inherits BaseNodeVM

    Public Sub New(parent As BaseNodeVM)
        MyBase.Parent = parent
    End Sub

End Class