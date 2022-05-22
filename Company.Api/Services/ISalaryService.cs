using System;
using Company.Api.Models;

public interface ISalaryService {
    /// <summary>
    /// Calculates employee's salary at an arbitrary date 
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <param name="calculationDate">Date for which the salary is calculated</param>
    /// <returns>Calculated salary</returns>
    public decimal CalculateSalary(Employee employee, DateTime calculationDate);

    /// <summary>
    /// Calculates the total salary in the company at an arbitrary date
    /// </summary>
    /// <param name="calculationDate">Date for which the salary is calculated</param>
    /// <returns>Calculated total salary in the company</returns>
    public decimal CalculateTotalSalary(DateTime calculationDate);
}