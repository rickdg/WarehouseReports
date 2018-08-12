Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsResupplyVM
        Inherits NotifyPropertyChanged

        Public Sub New()
            Dim MasterNode As New MasterNode
            Dim RootNode As New RootNode(MasterNode)

            RootNode.Nodes.Add(New RootNode(RootNode))
            RootNode.Nodes.Add(New LogicNode(RootNode))
            RootNode.Nodes.Add(New ExpressionNode(RootNode))

            MasterNode.Nodes.Add(RootNode)
            MasterNode.Nodes.Add(New LogicNode(MasterNode))
            MasterNode.Nodes.Add(New ExpressionNode(MasterNode))
            ExpressionTree.Add(MasterNode)
        End Sub


        Public Property ExpressionTree As New ObservableCollection(Of BaseNode)

    End Class
End Namespace