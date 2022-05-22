using System;
using System.Linq;
using System.Threading.Tasks;
using Company.Api.Models;
using Company.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;
        private readonly ISalaryService salaryService;

        public SalaryController(ICompanyRepository companyRepository, ISalaryService salaryService)
        {
            this.companyRepository = companyRepository;
            this.salaryService = salaryService;
        }

        // GET: api/Salary/5
        /// <summary>
        /// Returns salary of employee by it's id
        /// </summary>
        /// <param name="id">Employee's id</param>
        /// <returns>Salary</returns>
        [HttpGet("{id}")]
        public ActionResult<decimal> GetSalary(int id)
        {
            var employee = companyRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            return salaryService.CalculateSalary(employee, DateTime.Today);
        }

        // GET: api/Salary
        /// <summary>
        /// Return total salary in company
        /// </summary>
        /// <returns>Total salary</returns>
        [HttpGet]
        public ActionResult<decimal> GetTotalSalary()
        {
            return salaryService.CalculateTotalSalary(DateTime.Today);
        }
    }
}
