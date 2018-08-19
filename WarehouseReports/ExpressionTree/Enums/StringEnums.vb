Public Class StringEnums

    Public Function GetExpressionObjects() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"Склад отправитель", "[Складское подразделение]"},
            {"Адрес отправитель", "[Складское место]"},
            {"Склад получатель", "[Склад-получ#]"},
            {"Адрес получатель", "[СМ-получатель]"}}
    End Function


    Public Function GetExpressionOperators() As String()
        Return {"IN", "NOT IN", "IS NULL", "IS NOT NULL"}
    End Function


    Public Function GetLogicOperators() As String()
        Return {"AND", "OR"}
    End Function

End Class