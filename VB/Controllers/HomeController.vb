Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Script.Serialization
Imports TreeListOptimisticConcurrencyVBMvc.Models

Namespace TreeListOptimisticConcurrencyVBMvc.Controllers
    Public Class HomeController
        Inherits Controller
        Private db As New EmployeeDbContext()

        Public Function Index() As ActionResult
            Return View()
        End Function

        Public Function TreeListPartial() As ActionResult
            Return PartialView(db.Employees.ToList())
        End Function

        Public Function TreeListAddNewPartial(ByVal employee As Employee) As ActionResult
            Dim model = db.Employees

            If ModelState.IsValid Then
                Try
                    db.Entry(employee).State = System.Data.Entity.EntityState.Added
                    db.SaveChanges()
                Catch e As Exception
                    ViewData("EditError") = e.Message
                End Try
            Else
                ViewData("EditError") = "Please, correct all errors."
            End If

            Return PartialView("TreeListPartial", db.Employees.ToList())
        End Function

        Public Function TreeListUpdatePartial(ByVal employee As Employee) As ActionResult
            Dim model = db.Employees

            employee.RowVersion = CalculateOldRowVersion(employee.EmployeeID)

            If ModelState.IsValid Then
                Try
                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified
                    db.SaveChanges()
                Catch e As Exception
                    ViewData("EditError") = e.Message
                End Try
            Else
                ViewData("EditError") = "Please, correct all errors."
            End If

            Return PartialView("TreeListPartial", db.Employees.ToList())
        End Function

        Public Function TreeListMovePartial(ByVal employeeID As Integer, ByVal supervisorID As Nullable(Of Integer)) As ActionResult
            Dim employee As Employee = db.Employees.Find(employeeID)

            If Not employee.SupervisorID.Equals(supervisorID) Then
                Try
                    db.Entry(employee).OriginalValues("RowVersion") = CalculateOldRowVersion(employeeID)
                    employee.SupervisorID = supervisorID
                    db.SaveChanges()
                Catch e As Exception
                    ViewData("EditError") = e.Message
                End Try
            End If

            Return PartialView("TreeListPartial", db.Employees.ToList())
        End Function

        Public Function TreeListDeletePartial(ByVal employee As Employee) As ActionResult
            Dim model = db.Employees

            employee.RowVersion = CalculateOldRowVersion(employee.EmployeeID)

            If ModelState.IsValid Then
                Try
                    db.Entry(employee).State = System.Data.Entity.EntityState.Deleted
                    db.SaveChanges()
                Catch e As Exception
                    ViewData("EditError") = e.Message
                End Try
            Else
                ViewData("EditError") = "Please, correct all errors."
            End If

            Return PartialView("TreeListPartial", db.Employees.ToList())
        End Function

        Private Function CalculateOldRowVersion(ByVal id As Integer) As Byte()
            Dim serializer As New JavaScriptSerializer()
            Dim rowVersions As String = Request("RowVersions")
            Dim dictionary As Dictionary(Of Object, String) = CType(serializer.Deserialize(rowVersions, GetType(Dictionary(Of Object, String))), Dictionary(Of Object, String))
            Dim rowVersion() As Char = dictionary(id.ToString()).ToCharArray()

            Return Convert.FromBase64CharArray(rowVersion, 0, rowVersion.Length)
        End Function
    End Class
End Namespace
