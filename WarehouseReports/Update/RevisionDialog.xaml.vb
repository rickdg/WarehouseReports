Partial Public Class RevisionDialog
    Inherits UserControl

    Public Sub New(element As UIElement)
        InitializeComponent()
        Grid.Children.Add(element)
    End Sub

End Class