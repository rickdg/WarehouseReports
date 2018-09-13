Imports FirstFloor.ModernUI.Presentation
Imports System.Collections.ObjectModel

Namespace Content
    Partial Public Class SettingsZones
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Using Context = GetContext()
                For Each ZoneGroup In Context.Zones
                    ZoneCollection.Add(New ZoneVM With {.ParentCollection = ZoneCollection, .Zone = ZoneGroup})
                Next
            End Using
            DataContext = Me
        End Sub


        Public Property ZoneCollection As New ObservableCollection(Of ZoneVM)


        Public ReadOnly Property CmdAddNewZone As ICommand = New RelayCommand(AddressOf AddNewZoneExecute)
        Private Sub AddNewZoneExecute(parameter As Object)
            Using Context = GetContext()
                Dim NewZone = Context.Zones.Add(New Zone With {.MainGroup = 100})
                Context.SaveChanges()
                ZoneCollection.Add(New ZoneVM With {.ParentCollection = ZoneCollection, .Zone = NewZone})
            End Using
        End Sub

    End Class
End Namespace