Imports System.Collections.ObjectModel

Namespace Pages
    Partial Public Class PageReports
        Inherits UserControl

        Private Shared _StartDate As Date = New DateTime(Now.Year, Now.Month, 1)
        Private Shared _EndDate As Date = Now


        Public Sub New()
            InitializeComponent()
            Reports.Add(New MainReportVM)
            DataContext = Me
        End Sub


        Public Shared Property StartDate As Date
            Get
                Return _StartDate.Date
            End Get
            Set
                _StartDate = Value
            End Set
        End Property
        Public Shared Property EndDate As Date
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