using Microsoft.VisualStudio.TestTools.UnitTesting;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Data.OracleClient;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Reflection;
using BluePrint.EF;
using BluePrint.Shared.Models;
using Microsoft.Extensions.Configuration;
using BluePrint.Shared.DTO;
 

namespace BluePrint.UT.UT
{ 
    [TestClass]
    public class UT_Student
    {
        static IConfiguration Configuration = InitConfiguration();
        static DbContextOptionsBuilder<BluePrintOracleContext> _optionsBuilder = new DbContextOptionsBuilder<BluePrintOracleContext>();
        static BluePrintOracleContext _context;

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }
        [ClassInitialize()]
        public static void InitTestSuite(TestContext testContext)
        {
            _optionsBuilder.UseOracle(Configuration.GetConnectionString("BluePrintOracleConnection"));
            _context = new BluePrintOracleContext(_optionsBuilder.Options);
        }

        [TestMethod]
        public void GetStudents()
        {
            ICollection<Student> students = _context.Students
                .Include(b => b.Enrollments)
                .ThenInclude(b => b.Grades)
                .Where(b => b.StudentId == 147)
                .ToList();

            var Grade = students.FirstOrDefault();

            Grade.FirstName = "Joe";
            
            _context.SaveChanges();

            Assert.IsTrue(1 == 1);
        }


        public void AddStudent()
        {
            Student stu = new Student();
            stu.Employer = "none";
            stu.FirstName = "Bert";
            stu.LastName = "Gibbons";
            stu.Phone = "555-1212";
            stu.Salutation = "Mr.";
            stu.StreetAddress = "123 Easy way";
            stu.Zip = "07024";
            _context.Add(stu);
            _context.SaveChanges();
            Assert.IsTrue(stu.StudentId > 0);
        }
    }

}
