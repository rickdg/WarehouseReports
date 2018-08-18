Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Public Class SettingsResupplyVM
        Inherits NotifyPropertyChanged

        Public Property ExpressionTree As New ObservableCollection(Of BaseNodeVM)

    End Class
End Namespace