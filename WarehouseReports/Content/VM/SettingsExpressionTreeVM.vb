Imports FirstFloor.ModernUI.Presentation
Imports GongSolutions.Wpf.DragDrop
Imports ICSharpCode.AvalonEdit
Imports ICSharpCode.AvalonEdit.Highlighting
Imports Newtonsoft.Json
Imports System.Collections.ObjectModel
Imports WarehouseReports.Enums
Imports WarehouseReports.ExcelConnection

Namespace Content
    Public Class SettingsExpressionTree
        Inherits NotifyPropertyChanged
        Implements IDropTarget

        <JsonIgnore>
        Public Editor As TextEditor


        Public Sub New()
        End Sub


        Public Sub New(withLogicNode As Boolean)
            ExpressionTree.Add(New LogicNodeVM("AND"))
        End Sub


#Region "ViewModel"
        <JsonIgnore>
        Public ReadOnly Property SyntaxHighlighting As IHighlightingDefinition
            Get
                If MainWindow.Model.HighlightingDefinition Is Nothing Then
                    Return HighlightingManager.Instance.GetDefinition("SQL-LightTheme")
                End If
                Return HighlightingManager.Instance.GetDefinition(MainWindow.Model.HighlightingDefinition)
            End Get
        End Property
#End Region

        <JsonIgnore>
        Public Property LoadType As LoadType

#Region "Serialize"
        Public Property ExpressionTree As New ObservableCollection(Of BaseNodeVM)
        Public Property SerializeFileName As String
        Public Property CompiledExpression As String
#End Region


        <JsonIgnore>
        Public ReadOnly Property CmdSave As ICommand = New RelayCommand(AddressOf SaveExecute)
        Private Sub SaveExecute(parameter As Object)
            CompiledExpression = ExpressionTree.First.GetExpression
            Editor.Text = CompiledExpression
            Serialize(Me, SerializeFileName)
        End Sub
        <JsonIgnore>
        Public ReadOnly Property CmdViewData As ICommand = New RelayCommand(Sub() ViewData(LoadType))


        Public Sub DragOver(dropInfo As IDropInfo) Implements IDropTarget.DragOver
            Dim Source = TryCast(dropInfo.Data, LogicNodeVM)
            Dim Target = TryCast(dropInfo.TargetItem, LogicNodeVM)

            If Target Is Nothing Then Return

            If Not Target.Equals(Source) AndAlso Not Target.Contains(Source) Then
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight
                dropInfo.Effects = DragDropEffects.Copy
            End If
        End Sub


        Public Sub Drop(dropInfo As IDropInfo) Implements IDropTarget.Drop
            Dim Source = TryCast(dropInfo.Data, BaseNodeVM)
            Dim Target = TryCast(dropInfo.TargetItem, LogicNodeVM)

            Source.RemoveExecute(Nothing)
            Target.Nodes.Add(Source)
            Source.Parent = Target
        End Sub


        Public Sub SetProperty(model As SettingsExpressionTree)
            ExpressionTree = model.ExpressionTree
            SerializeFileName = model.SerializeFileName
            CompiledExpression = model.CompiledExpression
            Editor.Text = CompiledExpression
        End Sub


        Public Sub SyntaxHighlightingChanged()
            OnPropertyChanged("SyntaxHighlighting")
        End Sub

    End Class
End Namespace