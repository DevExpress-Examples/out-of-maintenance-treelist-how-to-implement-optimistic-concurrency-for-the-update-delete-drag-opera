using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Collections.Generic;

namespace TreeListOptimisticConcurrencyMvc.Models {
    public class Employee {
        public Employee() {
        }
        public Employee(int employeeID, int? supervisorID, string firstName, string middleName, string lastName, string title) {
            EmployeeID = employeeID;
            SupervisorID = supervisorID;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Title = title;
        }
        
        [Key]
        public int EmployeeID { get; set; }
        public int? SupervisorID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        [Timestamp]
        public Byte[] RowVersion { get; set; }
    }

    public class EmployeeDbContext : DbContext {
        public EmployeeDbContext()
            : base("EmployeeDbContext") {
            Database.SetInitializer(new EmployeeDbContextInitializer());
        }

        public DbSet<Employee> Employees { get; set; }
    }

    public class EmployeeDbContextInitializer : DropCreateDatabaseIfModelChanges<EmployeeDbContext> {
        protected override void Seed(EmployeeDbContext context) {
            IList<Employee> defaultEmployees = new List<Employee>();

            defaultEmployees.Add(new Employee(1, null, "David", "Jordan", "Adler", "Vice President"));
            defaultEmployees.Add(new Employee(2, 1, "Michael", "Christopher", "Alcamo", "Associate Vice President"));
            defaultEmployees.Add(new Employee(3, 1, "Eric", "Zachary", "Berkowitz", "Associate Vice President"));
            defaultEmployees.Add(new Employee(4, 2, "Amy", "Gabrielle", "Altmann", "Business Manager"));
            defaultEmployees.Add(new Employee(5, 3, "Kyle", "", "Bernardo", "Acting Director"));
            defaultEmployees.Add(new Employee(6, 2, "Mark", "Sydney", "Atlas", "Executive Director"));
            defaultEmployees.Add(new Employee(7, 3, "Meredith", "", "Berman", "Manager"));
            defaultEmployees.Add(new Employee(8, 3, "Liz", "", "Bice", "Controller"));

            foreach (Employee std in defaultEmployees)
                context.Employees.Add(std);

            base.Seed(context);
        }
    }
}