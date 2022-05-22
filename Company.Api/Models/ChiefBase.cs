using System;
using System.Collections.Generic;

namespace Company.Api.Models
{
    public abstract class ChiefBase : Employee
    {
        /// <summary>
        /// List of subordinates
        /// </summary>
        public List<Employee> Subordinates { get; set; }

        public ChiefBase(int id, string name, DateTime startDate, decimal baseSalary = 100) : base(id, name, startDate, baseSalary)
        {
        }
    }
}