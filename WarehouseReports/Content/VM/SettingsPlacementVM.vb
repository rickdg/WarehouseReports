Imports FirstFloor.ModernUI.Presentation
Imports GongSolutions.Wpf.DragDrop
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsPlacementVM
        Inherits NotifyPropertyChanged
        Implements IDropTarget

        Public Sub New()
            Dim MasterNode As New LogicNodeVM
            Dim RootNode As New LogicNodeVM(MasterNode)

            RootNode.Nodes.Add(New LogicNodeVM(RootNode))
            RootNode.Nodes.Add(New LogicNodeVM(RootNode))
            RootNode.Nodes.Add(New ExpressionNodeVM(RootNode))

            MasterNode.Nodes.Add(RootNode)
            MasterNode.Nodes.Add(New LogicNodeVM(MasterNode))
            MasterNode.Nodes.Add(New ExpressionNodeVM(MasterNode))
            ExpressionTree.Add(MasterNode)
        End Sub


        Public Property ExpressionTree As New ObservableCollection(Of BaseNodeVM)


        Public Sub DragOver(dropInfo As IDropInfo) Implements IDropTarget.DragOver
            Dim Source = TryCast(dropInfo.Data, LogicNodeVM)
            Dim Target = TryCast(dropInfo.TargetItem, LogicNodeVM)

            If IsNothing(Target) Then Return

            If Not Target.Equals(Source) AndAlso Not Target.Contains(Source) Then
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight
                dropInfo.Effects = DragDropEffects.Copy
            End If
        End Sub


        Public Sub Drop(dropInfo As IDropInfo) Implements IDropTarget.Drop
            Dim Source = TryCast(dropInfo.Data, BaseNodeVM)
            Dim Target = TryCast(dropInfo.TargetItem, LogicNodeVM)

            If Source.RemoveExecute(Nothing) Then
                Target.Nodes.Add(Source)
                Source.Parent = Target
            End If
        End Sub

    End Class
End Namespace