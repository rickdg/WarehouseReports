Imports FirstFloor.ModernUI.Presentation
Imports GongSolutions.Wpf.DragDrop
Imports Newtonsoft.Json
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsExpressionTree
        Inherits NotifyPropertyChanged
        Implements IDropTarget

        Public Sub New()
        End Sub


        Public Sub New(withLogicNode As Boolean)
            ExpressionTree.Add(New LogicNodeVM("AND"))
        End Sub


        Public Property SerializeFileName As String
        Public Property ExpressionTree As New ObservableCollection(Of BaseNodeVM)

        <JsonIgnore>
        Public ReadOnly Property CmdSave As ICommand = New RelayCommand(Sub() Serialize(Of SettingsExpressionTree)(Me, "", SerializeFileName))


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