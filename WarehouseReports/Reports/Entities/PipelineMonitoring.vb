Public Class PipelineMonitoring

    Public XDate As Date
    Public ReadOnly Property Дата As String
        Get
            Return XDate.ToShortDateString
        End Get
    End Property
    Public Property Задачи520 As Integer
    Public Property Задачи510 As Integer
    Public Property Задачи530 As Integer
    Public ReadOnly Property Процент520 As Double
        Get
            Dim Sum500 = Задачи520 + Задачи510 + Задачи530
            If Sum500 = 0 Then Return 0
            Return Задачи520 / Sum500
        End Get
    End Property
    Public Property Гравитация As Integer
    Public ReadOnly Property ПроцентГравитации As Double
        Get
            If Задачи520 = 0 Then Return 0
            Return Гравитация / Задачи520
        End Get
    End Property
    Public Property Короба As Integer?
    Public Property Заказы As Integer?
    Public ReadOnly Property КоробовВЗаказе As Double?
        Get
            If Заказы = 0 Then Return 0
            Return Короба / Заказы
        End Get
    End Property
    Public Property ОбъемТовара As Double?
    Public Property ОбъемТары As Double?
    Public ReadOnly Property Заполнение As Double?
        Get
            If ОбъемТары = 0 Then Return 0
            Return ОбъемТовара / ОбъемТары
        End Get
    End Property
    Public Property СреднееКолВоЗадачВЧас As Integer
    Public Property МаксимальноеКолВоЗадачВЧас As Integer
    Public Property СреднееКолВоШтукПоСтрокеЗнП As Integer?
    Public Property КоробаПрошедшиеВесовойКонтроль As Integer?
    Public Property КоробаНеПрошедшиеВесовойКонтроль As Integer?
    Public ReadOnly Property ПроцентНеПрошедших As Double?
        Get
            Dim SumBox = КоробаПрошедшиеВесовойКонтроль + КоробаНеПрошедшиеВесовойКонтроль
            If SumBox = 0 Then Return 0
            Return КоробаНеПрошедшиеВесовойКонтроль / SumBox
        End Get
    End Property

End Class