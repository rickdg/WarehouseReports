Imports System.Collections.ObjectModel

Namespace Pages
    Public Class PageReportsVM

        Private _StartDate As Date = New DateTime(Now.Year, Now.Month, 1)
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