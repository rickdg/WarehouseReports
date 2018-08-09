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

    Public Overridable Property Employees() As DbSet(Of Employee)
    Public Overridable Property Gangs() As DbSet(Of Gang)
    Public Overridable Property SystemTaskTypes() As DbSet(Of SystemTaskType)
    Public Overridable Property TaskDatas() As DbSet(Of TaskData)
    Public Overridable Property UserTaskTypes() As DbSet(Of UserTaskType)
    Public Overridable Property ZoneConsignees() As DbSet(Of ZoneConsignee)
    Public Overridable Property ZoneShippers() As DbSet(Of ZoneShipper)

    Public Overridable Function LoadTasks() As Integer
        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("LoadTasks")
    End Function

End Class
