Imports System.Collections.ObjectModel
Imports FirstFloor.ModernUI.Presentation

Namespace Pages
    Public Class PageReportsVM
        Inherits NotifyPropertyChanged

        Private _StartDate As Date = Now
        Private _EndDate As Date = Now


        Public Sub New()
            Reports.Add(New MainReportVM)
        End Sub


        Public Property StartDate As Date
            Get
                Return _StartDate.Date
            End Get
            Set
                _StartDate = Value
            End Set
        End Property
        Public Property EndDate As Date
            Get
                Return _EndDate.Date
            End Get
            Set
                _EndDate = Value
            End Set
        End Property
        Public Property Reports As New ObservableCollection(Of BaseReportVM)

    End Class
End Namespace