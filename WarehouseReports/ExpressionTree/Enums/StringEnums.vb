Public Class StringEnums

    Public Function GetExpressionObjects() As Dictionary(Of String, Field)
        Return New Dictionary(Of String, Field) From {
            {"Склад отправитель", New Field With {.Name = "[Складское подразделение]", .DataType = FieldDataType.Int}},
            {"Адрес отправитель", New Field With {.Name = "[Складское место]", .DataType = FieldDataType.Str}},
            {"Склад получатель", New Field With {.Name = "[Склад-получ#]", .DataType = FieldDataType.Int}},
            {"Адрес получатель", New Field With {.Name = "[СМ-получатель]", .DataType = FieldDataType.Str}},
            {"Тип задачи", New Field With {.Name = "[Тип задачи пользователя]", .DataType = FieldDataType.Str}}}
    End Function


    Public Function GetExpressionOperators() As String()
        Return {"IN", "NOT IN", "IS NULL", "IS NOT NULL"}
    End Function


    Public Function GetLogicOperators() As String()
        Return {"AND", "OR"}
    End Function

End Class