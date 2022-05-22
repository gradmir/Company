using System;
using Xunit;
using Moq;
using Company.Api.Services;
using System.Collections.Generic;
using Company.Api.Models;
using Company.Api.Repositories;

namespace Company.UnitTests
{
    public class SalaryServiceTests
    {
        [Fact]
        public void CalculateSalary_IfEmployeeIsNull_ShouldThrowExcepion()
        {
            //Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            var salaryService = new SalaryService(mockRepository.Object);

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => salaryService.CalculateSalary(null, DateTime.Today));
        }

        [Theory]
        [MemberData(nameof(SimpleEmpoyesData))]
        [MemberData(nameof(SimpleManagersData))]
        [MemberData(nameof(SimpleSalesData))]
        public void CalculateSalary_EmployeeWithoutSubordinates_ShouldReturnSalary(Employee employee, decimal expected)
        {
            //Arrange
            var mockRepository = new Mock<ICompanyRepository>();
            var salaryService = new SalaryService(mockRepository.Object);

            //Act
            var result = salaryService.CalculateSalary(employee, DateTime.Today);

            //Assert
            Assert.Equal(expected, result, 2);
        }

        [Theory]
        [MemberData(nameof(DataWithSubordinates))]
        public void CalculateSalary_EmployeeWithSubordinates_ShouldReturnSalary(Employee baseEmployee, decimal expected)
        {
            //Arrange 
            var mockRepository = new Mock<ICompanyRepository>();
            var salaryService = new SalaryService(mockRepository.Object);

            //Act
            var result = salaryService.CalculateSalary(baseEmployee, DateTime.Today);

            //Assert
            Assert.Equal(expected, result, 2);
        }

        [Theory]
        [MemberData(nameof(DataWithTwoTrees))]
        public void CalculateTotalSalary_TwoTrees_ShouldReturnSalary(List<Employee> data, decimal expected)
        {
            //Arrange 
            var mockRepository = new Mock<ICompanyRepository>();
            mockRepository.Setup(repo => repo.GetAllEmployees()).Returns(data);
            var salaryService = new SalaryService(mockRepository.Object);

            //Act
            var result = salaryService.CalculateTotalSalary(DateTime.Today);

            //Assert
            Assert.Equal(expected, result, 2);
        }

        public static IEnumerable<object[]> SimpleEmpoyesData => new List<object[]>
        {
            new object[] { new Employee(1, "Jhon", DateTime.Today), 100 },
            new object[] { new Employee(2, "Jhon", DateTime.Today.AddYears(-1)), 103 },
            new object[] { new Employee(3, "Jhon", DateTime.Today.AddYears(-1).AddDays(-1)), 103 },
            new object[] { new Employee(4, "Jhon", DateTime.Today.AddYears(-5)), 115 },
            new object[] { new Employee(5, "Jhon", DateTime.Today.AddYears(-10)), 130 },
            new object[] { new Employee(6, "Jhon", DateTime.Today.AddYears(-20)), 130 }
        };

        public static IEnumerable<object[]> SimpleManagersData => new List<object[]>
        {
            new object[] { new Manager(1, "Jhon", DateTime.Today), 100 },
            new object[] { new Manager(2, "Jhon", DateTime.Today.AddYears(-1)), 105 },
            new object[] { new Manager(3, "Jhon", DateTime.Today.AddYears(-1).AddDays(-1)), 105 },
            new object[] { new Manager(4, "Jhon", DateTime.Today.AddYears(-5)), 125 },
            new object[] { new Manager(5, "Jhon", DateTime.Today.AddYears(-8)), 140 },
            new object[] { new Manager(6, "Jhon", DateTime.Today.AddYears(-20)), 140 }
        };

        public static IEnumerable<object[]> SimpleSalesData => new List<object[]>
        {
            new object[] { new Sales(1, "Jhon", DateTime.Today), 100 },
            new object[] { new Sales(2, "Jhon", DateTime.Today.AddYears(-1)), 101 },
            new object[] { new Sales(3, "Jhon", DateTime.Today.AddYears(-1).AddDays(-1)), 101 },
            new object[] { new Sales(4, "Jhon", DateTime.Today.AddYears(-10)), 110 },
            new object[] { new Sales(5, "Jhon", DateTime.Today.AddYears(-35)), 135 },
            new object[] { new Sales(6, "Jhon", DateTime.Today.AddYears(-50)), 135 }
        };

        public static IEnumerable<object[]> DataWithSubordinates()
        {
            var sales6 = new Sales(1, "Jhon", DateTime.Today);
            var sales5 = new Sales(2, "Jhon", DateTime.Today);
            var manager2 = new Manager(3, "Jhon", DateTime.Today);
            manager2.Subordinates = new List<Employee> { sales5, sales6 };

            var sales4 = new Sales(4, "Jhon", DateTime.Today);
            var sales3 = new Sales(5, "Jhon", DateTime.Today);
            var sales2 = new Sales(6, "Jhon", DateTime.Today);
            sales2.Subordinates = new List<Employee> { sales5, sales6 };
            var sales1 = new Sales(7, "Jhon", DateTime.Today);
            sales1.Subordinates = new List<Employee> { sales2, manager2 };
            var manager1 = new Manager(8, "Jhon", DateTime.Today);
            manager1.Subordinates = new List<Employee> { sales2, manager2 };

            return new List<object[]> { new object[] { sales1, 101.8M},
             new object[] { manager1, 101.01M}};
        }

        public static IEnumerable<object[]> DataWithTwoTrees()
        {
            var manager1 = new Manager(3, "Jhon", DateTime.Today);
            var sales5 = new Sales(1, "Jhon", DateTime.Today);
            var sales6 = new Sales(2, "Jhon", DateTime.Today);
            sales5.Cheif = manager1;
            sales6.Cheif = manager1;
            manager1.Subordinates = new List<Employee> { sales5, sales6 };

            var sales2 = new Sales(6, "Jhon", DateTime.Today);
            var sales3 = new Sales(4, "Jhon", DateTime.Today);
            var sales4 = new Sales(5, "Jhon", DateTime.Today);
            sales3.Cheif = sales2;
            sales4.Cheif = sales2;
            sales2.Subordinates = new List<Employee> { sales5, sales6 };

            var sales1 = new Sales(7, "Jhon", DateTime.Today);
            sales2.Cheif = sales1;
            manager1.Cheif = sales1;
            sales1.Subordinates = new List<Employee> { sales2, manager1 };

            var employee1 = new Employee(1, "Jhon", DateTime.Today);
            var employee2 = new Employee(1, "Jhon", DateTime.Today);
            var employee3 = new Employee(1, "Jhon", DateTime.Today);
            var employee4 = new Employee(1, "Jhon", DateTime.Today);
            var sales7 = new Sales(1, "Jhon", DateTime.Today);
            var sales8 = new Sales(1, "Jhon", DateTime.Today);
            sales7.Subordinates = new List<Employee> { employee1, employee2, employee3, employee4 };
            employee1.Cheif = sales7;
            employee2.Cheif = sales7;
            employee3.Cheif = sales7;
            employee4.Cheif = sales7;
            sales8.Subordinates = new List<Employee> { sales7 };
            sales7.Cheif = sales8;

            var data = new List<Employee> { sales1, sales2, sales3, sales4, sales5, sales6, sales7, sales8, manager1, employee1, employee2, employee3, employee4 };

            return new List<object[]> { new object[] { data, 1306.11M } };
        }
    }
}
