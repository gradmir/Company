using System;

namespace Company.Api.Models
{
    public class Employee
    {
        /// <summary>
        /// Employee's id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Employee's start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Employee's base salary
        /// </summary>
        public decimal BaseSalary { get; set; }

        /// <summary>
        /// Employee's chief
        /// </summary>
        /// <value></value>
        public ChiefBase Cheif { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Employee's id</param>
        /// <param name="name">Employee's name</param>
        /// <param name="startDate">Employee's start date</param>
        /// <param name="baseSalary">Employee's base salary</param>
        public Employee(int id, string name, DateTime startDate, decimal baseSalary = 100)
        {
            this.Id = id;
            this.Name = name;
            this.StartDate = startDate;
            this.BaseSalary = baseSalary;
        }

    }
}