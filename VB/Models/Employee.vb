Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Entity
Imports System.Collections.Generic

Namespace TreeListOptimisticConcurrencyVBMvc.Models
    Public Class Employee
        Public Sub New()
        End Sub
        Public Sub New(ByVal employeeID As Integer, ByVal supervisorID As Nullable(Of Integer), ByVal firstName As String, ByVal middleName As String, ByVal lastName As String, ByVal title As String)
            privateEmployeeID = employeeID
            privateSupervisorID = supervisorID
            privateFirstName = firstName
            privateMiddleName = middleName
            privateLastName = lastName
            privateTitle = title
        End Sub

        Private privateEmployeeID As Integer
        <Key()> _
        Public Property EmployeeID() As Integer
            Get
                Return privateEmployeeID
            End Get
            Set(ByVal value As Integer)
                privateEmployeeID = value
            End Set
        End Property
        Private privateSupervisorID As Nullable(Of Integer)
        Public Property SupervisorID() As Nullable(Of Integer)
            Get
                Return privateSupervisorID
            End Get
            Set(ByVal value As Nullable(Of Integer))
                privateSupervisorID = value
            End Set
        End Property
        Private privateFirstName As String
        Public Property FirstName() As String
            Get
                Return privateFirstName
            End Get
            Set(ByVal value As String)
                privateFirstName = value
            End Set
        End Property
        Private privateMiddleName As String
        Public Property MiddleName() As String
            Get
                Return privateMiddleName
            End Get
            Set(ByVal value As String)
                privateMiddleName = value
            End Set
        End Property
        Private privateLastName As String
        Public Property LastName() As String
            Get
                Return privateLastName
            End Get
            Set(ByVal value As String)
                privateLastName = value
            End Set
        End Property
        Private privateTitle As String
        Public Property Title() As String
            Get
                Return privateTitle
            End Get
            Set(ByVal value As String)
                privateTitle = value
            End Set
        End Property
        Private privateRowVersion As Byte()
        <Timestamp()> _
        Public Property RowVersion() As Byte()
            Get
                Return privateRowVersion
            End Get
            Set(ByVal value As Byte())
                privateRowVersion = value
            End Set
        End Property
    End Class

    Public Class EmployeeDbContext
        Inherits DbContext
        Public Sub New()
            MyBase.New("EmployeeDbContext")
            Database.SetInitializer(New EmployeeDbContextInitializer())
        End Sub

        Private privateEmployees As DbSet(Of Employee)
        Public Property Employees() As DbSet(Of Employee)
            Get
                Return privateEmployees
            End Get
            Set(ByVal value As DbSet(Of Employee))
                privateEmployees = value
            End Set
        End Property
    End Class

    Public Class EmployeeDbContextInitializer
        Inherits DropCreateDatabaseIfModelChanges(Of EmployeeDbContext)
        Protected Overrides Sub Seed(ByVal context As EmployeeDbContext)
            Dim defaultEmployees As IList(Of Employee) = New List(Of Employee)()

            defaultEmployees.Add(New Employee(1, Nothing, "David", "Jordan", "Adler", "Vice President"))
            defaultEmployees.Add(New Employee(2, 1, "Michael", "Christopher", "Alcamo", "Associate Vice President"))
            defaultEmployees.Add(New Employee(3, 1, "Eric", "Zachary", "Berkowitz", "Associate Vice President"))
            defaultEmployees.Add(New Employee(4, 2, "Amy", "Gabrielle", "Altmann", "Business Manager"))
            defaultEmployees.Add(New Employee(5, 3, "Kyle", "", "Bernardo", "Acting Director"))
            defaultEmployees.Add(New Employee(6, 2, "Mark", "Sydney", "Atlas", "Executive Director"))
            defaultEmployees.Add(New Employee(7, 3, "Meredith", "", "Berman", "Manager"))
            defaultEmployees.Add(New Employee(8, 3, "Liz", "", "Bice", "Controller"))

            For Each std As Employee In defaultEmployees
                context.Employees.Add(std)
            Next std

            MyBase.Seed(context)
        End Sub
    End Class
End Namespace