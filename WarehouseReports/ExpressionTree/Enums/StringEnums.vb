Public Class StringEnums

    Public Function GetExpressionObjects() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"Складское подразделение", "[Складское подразделение]"},
            {"Складское место", "[Складское место]"},
            {"Склад-получ.", "[Склад-получ#]"},
            {"СМ-получатель", "[СМ-получатель]"},
            {"Тип задачи пользователя", "[Тип задачи пользователя]"},
            {"Тип задачи системы", "[Тип задачи системы]"}}
    End Function


    Public Function GetExpressionOperators() As String()
        Return {"IN", "NOT IN", "IS NULL", "IS NOT NULL"}
    End Function


    Public Function GetLogicOperators() As String()
        Return {"AND", "OR"}
    End Function

End Class


Public Module Helper

    Private ReadOnly FieldDataTypeDict As New Dictionary(Of String, FieldDataType) From {
        {"[Складское подразделение]", FieldDataType.Int},
        {"[Складское место]", FieldDataType.Str},
        {"[Склад-получ#]", FieldDataType.Int},
        {"[СМ-получатель]", FieldDataType.Str},
        {"[Тип задачи пользователя]", FieldDataType.Str},
        {"[Тип задачи системы]", FieldDataType.Str}}

    Public Function GetFieldDataType(key As String) As FieldDataType
        Return FieldDataTypeDict(key)
    End Function

End Module