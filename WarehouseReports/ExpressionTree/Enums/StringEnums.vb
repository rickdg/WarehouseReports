Public Class StringEnums

    Public Function GetExpressionObjects() As Dictionary(Of String, Field)
        Return New Dictionary(Of String, Field) From {
            {"Складское подразделение", New Field With {.Name = "[Складское подразделение]", .DataType = FieldDataType.Int}},
            {"Складское место", New Field With {.Name = "[Складское место]", .DataType = FieldDataType.Str}},
            {"Склад-получ.", New Field With {.Name = "[Склад-получ#]", .DataType = FieldDataType.Int}},
            {"СМ-получатель", New Field With {.Name = "[СМ-получатель]", .DataType = FieldDataType.Str}},
            {"Тип задачи пользователя", New Field With {.Name = "[Тип задачи пользователя]", .DataType = FieldDataType.Str}},
            {"Тип задачи системы", New Field With {.Name = "[Тип задачи системы]", .DataType = FieldDataType.Str}}}
    End Function


    Public Function GetExpressionOperators() As String()
        Return {"IN", "NOT IN", "IS NULL", "IS NOT NULL"}
    End Function


    Public Function GetLogicOperators() As String()
        Return {"AND", "OR"}
    End Function

End Class