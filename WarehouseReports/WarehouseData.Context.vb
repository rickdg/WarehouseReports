﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Этот код создан по шаблону.
'
'     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
'     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Data.Entity.Core.Objects
Imports System.Linq

Partial Public Class WarehouseDataEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=WarehouseDataEntities")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property CustomGroups() As DbSet(Of CustomGroup)
    Public Overridable Property Employees() As DbSet(Of Employee)
    Public Overridable Property ExtraDatas() As DbSet(Of ExtraData)
    Public Overridable Property Gangs() As DbSet(Of Gang)
    Public Overridable Property MainGroups() As DbSet(Of MainGroup)
    Public Overridable Property TaskDatas() As DbSet(Of TaskData)
    Public Overridable Property UserTaskTypes() As DbSet(Of UserTaskType)
    Public Overridable Property WorkHours() As DbSet(Of WorkHour)
    Public Overridable Property Zones() As DbSet(Of Zone)

    Public Overridable Function LoadTasks() As Integer
        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("LoadTasks")
    End Function

    Public Overridable Function LoadWorkHour() As Integer
        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("LoadWorkHour")
    End Function

End Class
