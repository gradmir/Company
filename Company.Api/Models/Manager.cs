using System;
using System.Collections.Generic;

namespace Company.Api.Models
{
    public class Manager : ChiefBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Manager's id</param>
        /// <param name="name">Manager's name</param>
        /// <param name="startDate">Manager's start date</param>
        /// <param name="baseSalary">Manager's base salary</param>
        public Manager(int id, string name, DateTime startDate, decimal baseSalary = 100) : base(id, name, startDate, baseSalary)
        {
        }
    }
}