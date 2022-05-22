using System;
using System.Collections.Generic;
using Company.Api.Models;

namespace Company.Api.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        List<Employee> data = new List<Employee>();

        public CompanyRepository()
        {
            //first tree
            var sales1 = this.CreateSales(1, "Jhon", DateTime.Today);
            var sales2 = this.CreateSales(2, "Jhon", DateTime.Today, chief: sales1);
            var manager1 = this.CreateManager(3, "Jhon", DateTime.Today, chief: sales1);
            var sales4 = this.CreateSales(4, "Jhon", DateTime.Today, chief: sales2);
            var sales3 = this.CreateSales(5, "Jhon", DateTime.Today, chief: sales2);
            var sales6 = this.CreateSales(6, "Jhon", DateTime.Today, chief: manager1);
            var sales5 = this.CreateSales(7, "Jhon", DateTime.Today, chief: manager1);

            //second tree
            var sales7 = this.CreateSales(8, "Jhon", DateTime.Today);
            var sales8 = this.CreateSales(9, "Jhon", DateTime.Today, chief: sales7);
            var employee1 = this.CreateEmployee(10, "Jhon", DateTime.Today, chief: sales8);
            var employee2 = this.CreateEmployee(11, "Jhon", DateTime.Today, chief: sales8);
            var employee3 = this.CreateEmployee(12, "Jhon", DateTime.Today, chief: sales8);
            var employee4 = this.CreateEmployee(13, "Jhon", DateTime.Today, chief: sales8);
        }

        /// <summary>
        /// Returns employee by it's id from repository
        /// </summary>
        /// <param name="id">Employee's id</param>
        /// <returns>Employee</returns>
        public Employee GetEmployeeById(int id)
        {
            var result = data.Find(emp => emp.Id == id);
            return result;
        }

        /// <summary>
        /// Returns all employees from repository
        /// </summary>
        /// <returns>List of employees</returns>
        public List<Employee> GetAllEmployees()
        {
            return data;
        }

        /// <summary>
        /// Creates employee and adds it to repository
        /// </summary>
        /// <param name="id">Employee's id</param>
        /// <param name="name">Employee's name</param>
        /// <param name="startDate">Employee's start date</param>
        /// <param name="baseSalary">Employee's base salary</param>
        /// <param name="chief">Employee's chief</param>
        /// <returns>Created employee</returns>
        public Employee CreateEmployee(int id, string name, DateTime startDate, decimal baseSalary = 100, ChiefBase chief = null)
        {
            var newEmployee = new Employee(id, name, startDate, baseSalary);
            if (chief is not null)
            {
                AddSubordinate(chief, newEmployee);
            }
            data.Add(newEmployee);
            return newEmployee;
        }

        /// <summary>
        /// Creates sales and adds it to repository
        /// </summary>
        /// <param name="id">Sales' id</param>
        /// <param name="name">Sales' name</param>
        /// <param name="startDate">Sales' start date</param>
        /// <param name="baseSalary">Sales' base salary</param>
        /// <param name="chief">Sales' chief</param>
        /// <param name="subordinates">List of sales' subordinates</param>
        /// <returns>Created sales</returns>
        public Sales CreateSales(int id, string name, DateTime startDate, decimal baseSalary = 100, ChiefBase chief = null, List<Employee> subordinates = null)
        {
            var newSales = new Sales(id, name, startDate, baseSalary);
            if (chief is not null)
            {
                AddSubordinate(chief, newSales);
            }
            if (subordinates is not null)
            {
                foreach (var subordinate in subordinates)
                {
                    AddSubordinate(newSales, subordinate);
                }
            }

            data.Add(newSales);
            return newSales;
        }

        /// <summary>
        /// Creates manager and adds it to repository
        /// </summary>
        /// <param name="id">Manager's id</param>
        /// <param name="name">Manager's name</param>
        /// <param name="startDate">Manager's start date</param>
        /// <param name="baseSalary">Manager's base salary</param>
        /// <param name="chief">Manager's chief</param>
        /// <param name="subordinates">List of manager's subordinates</param>
        /// <returns>Created manager</returns>
        public Manager CreateManager(int id, string name, DateTime startDate, decimal baseSalary = 100, ChiefBase chief = null, List<Employee> subordinates = null)
        {
            var newManager = new Manager(id, name, startDate, baseSalary);
            if (chief is not null)
            {
                AddSubordinate(chief, newManager);
            }
            if (subordinates is not null)
            {
                foreach (var subordinate in subordinates)
                {
                    AddSubordinate(newManager, subordinate);
                }
            }

            data.Add(newManager);
            return newManager;
        }

        /// <summary>
        /// Deletes employee from repository
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <exception cref="ArgumentNullException">Throws if employee is null</exception>
        public void DeleteEmployee(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (employee.Cheif is not null)
            {
                this.RemoveSubordinate(employee.Cheif, employee);
            }
            if (employee is ChiefBase chief && chief.Subordinates is not null)
            {
                foreach (var subordinate in chief.Subordinates)
                {
                    this.RemoveSubordinate(chief, subordinate);
                }
            }

            data.Remove(employee);
        }

        /// <summary>
        /// Creates relationship between chief and employee
        /// </summary>
        /// <param name="chief">Chief</param>
        /// <param name="employee">Employee</param>
        /// <exception cref="ArgumentNullException">Throws if chief or employee are null</exception>
        /// <exception cref="ArgumentException">Throws when employee already has a chief</exception>
        /// <exception cref="ArgumentException">Throws when trying assign chief to the employee himself</exception>
        public void AddSubordinate(ChiefBase chief, Employee employee)
        {
            if (chief is null)
                throw new ArgumentNullException(nameof(chief));
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));
            if (employee.Cheif is not null)
                throw new ArgumentException("Employee is already has chief", nameof(employee));
            if (employee == chief)
                throw new ArgumentException("Employee cannot be his own chief");

            if (chief.Subordinates is null)
            {
                chief.Subordinates = new List<Employee> { employee };
            }
            else
            {
                chief.Subordinates.Add(employee);
            }
        }

        /// <summary>
        /// Removes relationship between chief and employee
        /// </summary>
        /// <param name="chief">Chief</param>
        /// <param name="employee">Employee</param>
        /// <exception cref="ArgumentNullException">Throws if chief or employee are null</exception>
        /// <exception cref="ArgumentException">Throws when employee is not subordinate of chief</exception>
        public void RemoveSubordinate(ChiefBase chief, Employee employee)
        {
            if (chief is null)
                throw new ArgumentNullException(nameof(chief));
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));
            if (employee.Cheif != chief)
                throw new ArgumentException("Employee is not subordinate of cheief");

            employee.Cheif.Subordinates?.Remove(employee);
            employee.Cheif = null;
        }
    }
}
