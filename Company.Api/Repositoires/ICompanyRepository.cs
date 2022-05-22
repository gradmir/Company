using System;
using System.Collections.Generic;
using Company.Api.Models;

namespace Company.Api.Repositories {
    public interface ICompanyRepository {
        /// <summary>
        /// Returns employee by it's id from repository
        /// </summary>
        /// <param name="id">Employee's id</param>
        /// <returns>Employee</returns>
        public Employee GetEmployeeById(int id);

        /// <summary>
        /// Returns all employees from repository
        /// </summary>
        /// <returns>List of employees</returns>
        public List<Employee> GetAllEmployees();

        /// <summary>
        /// Creates employee and adds it to repository
        /// </summary>
        /// <param name="id">Employee's id</param>
        /// <param name="name">Employee's name</param>
        /// <param name="startDate">Employee's start date</param>
        /// <param name="baseSalary">Employee's base salary</param>
        /// <param name="chief">Employee's chief</param>
        /// <returns>Created employee</returns>
        public Employee CreateEmployee(int id, string name, DateTime startDate, decimal baseSalary, ChiefBase chief);

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
        public Manager CreateManager(int id, string name, DateTime startDate, decimal baseSalary, ChiefBase chief, List<Employee> subordinates);
        
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
        public Sales CreateSales(int id, string name, DateTime startDate, decimal baseSalary, ChiefBase chief, List<Employee> subordinates);

        /// <summary>
        /// Deletes employee from repository
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <exception cref="ArgumentNullException">Throws if employee is null</exception>
        public void DeleteEmployee(Employee employee);

        /// <summary>
        /// Creates relationship between chief and employee
        /// </summary>
        /// <param name="chief">Chief</param>
        /// <param name="employee">Employee</param>
        /// <exception cref="ArgumentNullException">Throws if chief or employee are null</exception>
        /// <exception cref="ArgumentException">Throws when employee already has a chief</exception>
        /// <exception cref="ArgumentException">Throws when trying assign chief to the employee himself</exception>
        public void AddSubordinate(ChiefBase chief, Employee employee);

        /// <summary>
        /// Removes relationship between chief and employee
        /// </summary>
        /// <param name="chief">Chief</param>
        /// <param name="employee">Employee</param>
        /// <exception cref="ArgumentNullException">Throws if chief or employee are null</exception>
        /// <exception cref="ArgumentException">Throws when employee is not subordinate of chief</exception>
        public void RemoveSubordinate(ChiefBase chief, Employee employee);
    }
}