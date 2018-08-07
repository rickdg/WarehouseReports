Public Class StringEnums

    Public Function GetExpressionObjects() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"Склад отправитель", "F6"},
            {"Адрес отправитель", "F7"},
            {"Склад получатель", "F8"},
            {"Адрес получатель", "F9"}}
    End Function


    Public Function GetExpressionOperators() As String()
        Return {"IN", "NOT IN", "IS NULL", "IS NOT NULL"}
    End Function


    Public Function GetLogicOperators() As String()
        Return {"AND", "OR"}
    End Function

End Class