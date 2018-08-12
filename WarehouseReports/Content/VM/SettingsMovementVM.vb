Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsMovementVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Dim MasterNode As New MasterNodeVM
            Dim RootNode As New RootNodeVM(MasterNode)

            RootNode.Nodes.Add(New RootNodeVM(RootNode))
            RootNode.Nodes.Add(New LogicNodeVM(RootNode))
            RootNode.Nodes.Add(New ExpressionNodeVM(RootNode))

            MasterNode.Nodes.Add(RootNode)
            MasterNode.Nodes.Add(New LogicNodeVM(MasterNode))
            MasterNode.Nodes.Add(New ExpressionNodeVM(MasterNode))
            ExpressionTree.Add(MasterNode)
        End Sub


        Public Property ExpressionTree As New ObservableCollection(Of BaseNodeVM)

    End Class
End Namespace