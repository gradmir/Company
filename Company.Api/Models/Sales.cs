using System;
using System.Collections.Generic;

namespace Company.Api.Models
{
    public class Sales : ChiefBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Sales's id</param>
        /// <param name="name">Sales's name</param>
        /// <param name="startDate">Sales's start date</param>
        /// <param name="baseSalary">Sales's base salary</param> 
        public Sales(int id, string name, DateTime startDate, decimal baseSalary = 100) : base(id, name, startDate, baseSalary)
        {
        }
    }
}