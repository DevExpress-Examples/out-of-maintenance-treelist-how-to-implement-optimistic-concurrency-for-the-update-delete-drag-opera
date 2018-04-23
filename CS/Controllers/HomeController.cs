using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TreeListOptimisticConcurrencyMvc.Models;

namespace TreeListOptimisticConcurrencyMvc.Controllers {
    public class HomeController : Controller {
        private EmployeeDbContext db = new EmployeeDbContext();
        
        public ActionResult Index() {
            return View();
        }

        public ActionResult TreeListPartial() {
            return PartialView(db.Employees.ToList());
        }

        public ActionResult TreeListAddNewPartial(Employee employee) {
            var model = db.Employees;

            if (ModelState.IsValid) {
                try {
                    db.Entry(employee).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }
                catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("TreeListPartial", db.Employees.ToList());
        }

        public ActionResult TreeListUpdatePartial(Employee employee) {
            var model = db.Employees;

            employee.RowVersion = CalculateOldRowVersion(employee.EmployeeID);

            if (ModelState.IsValid) {
                try {
                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("TreeListPartial", db.Employees.ToList());
        }

        public ActionResult TreeListMovePartial(int employeeID, int? supervisorID) {
            Employee employee = db.Employees.Find(employeeID);

            if (employee.SupervisorID != supervisorID) {
                try {
                    db.Entry(employee).OriginalValues["RowVersion"] = CalculateOldRowVersion(employeeID);
                    employee.SupervisorID = supervisorID;
                    db.SaveChanges();
                }
                catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("TreeListPartial", db.Employees.ToList());
        }

        public ActionResult TreeListDeletePartial(Employee employee) {
            var model = db.Employees;

            employee.RowVersion = CalculateOldRowVersion(employee.EmployeeID);

            if (ModelState.IsValid) {
                try {
                    db.Entry(employee).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("TreeListPartial", db.Employees.ToList());
        }

        private byte[] CalculateOldRowVersion(int id) {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string rowVersions = Request["RowVersions"];
            Dictionary<object, string> dictionary = (Dictionary<object, string>)serializer.Deserialize(rowVersions, typeof(Dictionary<object, string>));
            char[] rowVersion = dictionary[id.ToString()].ToCharArray();

            return Convert.FromBase64CharArray(rowVersion, 0, rowVersion.Length);
        }
    }
}