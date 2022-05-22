using System;
using System.Collections.Generic;
using System.Linq;
using Company.Api.Models;
using Company.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Company.Api.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly ICompanyRepository companyRepository;

        public SalaryService(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        readonly Dictionary<string, decimal> PERCENTAGE_RATE = 
            new Dictionary<string, decimal>
            {
                { "Company.Api.Models.Employee", 0.03M },
                { "Company.Api.Models.Manager", 0.05M },
                { "Company.Api.Models.Sales", 0.01M }
            };

        readonly Dictionary<string, decimal> MAX_PERCENTAGE_RATE = 
            new Dictionary<string, decimal>
            {
                { "Company.Api.Models.Employee", 0.3M },
                { "Company.Api.Models.Manager", 0.4M },
                { "Company.Api.Models.Sales", 0.35M }
            };

        readonly Dictionary<string, decimal> PERCENTAGE_RATE_FROM_SUBORDINATES = 
            new Dictionary<string, decimal>
            {
                { "Company.Api.Models.Manager", 0.005M },
                { "Company.Api.Models.Sales", 0.003M }
            };

        /// <summary>
        /// Calculates employee's salary at an arbitrary date 
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <param name="calculationDate">Date for which the salary is calculated</param>
        /// <returns>Calculated salary</returns>
        public decimal CalculateSalary(Employee employee, DateTime calculationDate)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            return CalculateWithSubordinates(employee, calculationDate).EmployeeSalary;
        }

        /// <summary>
        /// Calculates the total salary in the company at an arbitrary date
        /// </summary>
        /// <param name="calculationDate">Date for which the salary is calculated</param>
        /// <returns>Calculated total salary in the company</returns>
        public decimal CalculateTotalSalary(DateTime calculationDate)
        {
            var topChiefs = this.companyRepository
                .GetAllEmployees()
                .Where(emp => emp.Cheif == null);
            var totalSalary = 0M;

            foreach (var chief in topChiefs)
            {
                (var сhiefSalary, var subordinatesSalary) = CalculateWithSubordinates(chief, calculationDate);
                totalSalary += сhiefSalary + subordinatesSalary;
            }

            return totalSalary;
        }

        /// <summary>
        /// Calculates the salary of employee taking into account the work years, 
        /// but without taking into account his subordinates
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <param name="calculationDate">Date for which the salary is calculated</param>
        /// <returns>Calculated own salary of employee</returns>
        private decimal CalculateOwnRaisedSalary(Employee employee, DateTime calculationDate)
        {
            int workYears = calculationDate.Year - employee.StartDate.Year;
            if (employee.StartDate > calculationDate.AddYears(-workYears))
                workYears--;

            var percentageRate = PERCENTAGE_RATE[employee.ToString()];
            var maxPercentageRate = MAX_PERCENTAGE_RATE[employee.ToString()];

            var raisePercentage = workYears * percentageRate;
            if (raisePercentage > maxPercentageRate)
            {
                raisePercentage = maxPercentageRate;
            }

            return employee.BaseSalary * (1 + raisePercentage);
        }

        /// <summary>
        /// which recursively calculates the salary of an employee taking into account
        /// the work years and subordinates, as well as the total salary of all 
        /// his subordinates
        /// </summary>
        /// <param name="currentEmployee">Employee</param>
        /// <param name="calculationDate">Date for which the salary is calculated</param>
        /// <returns>Employees salary and salary of all it's subordinates</returns>
        private (decimal EmployeeSalary, decimal SubordinatesSalary) CalculateWithSubordinates(Employee currentEmployee, DateTime calculationDate)
        {
            var directSubsTotalSalary = 0M;
            var allSubsTotalSalary = 0M;

            if (currentEmployee is ChiefBase chief)
            {
                if (chief.Subordinates is not null)
                {
                    foreach (var directSub in chief.Subordinates)
                    {
                        var salaries = CalculateWithSubordinates(directSub, calculationDate);
                        directSubsTotalSalary += salaries.EmployeeSalary;
                        allSubsTotalSalary += salaries.EmployeeSalary + salaries.SubordinatesSalary;
                    }
                }

                if (chief is Manager)
                    return (
                        CalculateOwnRaisedSalary(chief, calculationDate)
                            + PERCENTAGE_RATE_FROM_SUBORDINATES[chief.ToString()] * directSubsTotalSalary,
                        allSubsTotalSalary
                    );
                if (chief is Sales)
                    return (
                        CalculateOwnRaisedSalary(chief, calculationDate)
                            + PERCENTAGE_RATE_FROM_SUBORDINATES[chief.ToString()] * allSubsTotalSalary,
                        allSubsTotalSalary
                    );
            }

            return (CalculateOwnRaisedSalary(currentEmployee, calculationDate), 0);
        }
    }
}
