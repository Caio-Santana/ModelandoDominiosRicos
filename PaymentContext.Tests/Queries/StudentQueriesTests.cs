using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentQueriesTests
    {
        private IList<Student> _students = new List<Student>();

        public StudentQueriesTests()
        {
            for (int i = 0; i < 10; i++)
            {
                _students.Add(
                    new Student(
                        new Name("Aluno", i.ToString()),
                        new Document($"1234567891{i}", EDocumentType.CPF),
                        new Email($"aluno{i}@batman.com")
                    )
                );
            }
        }

        // Red, Green, Refactor

        [TestMethod]
        public void ShouldReturnNullWhenDocumentNotExists()
        {
            var exp = StudentQueries.GetStudentInfo("99999999999");
            var student = _students.AsQueryable<Student>().Where(exp).FirstOrDefault();

            Assert.IsNull(student);
        }
        
        [TestMethod]
        public void ShouldReturnStudentWhenDocumentNotExists()
        {
            var exp = StudentQueries.GetStudentInfo("12345678911");
            var student = _students.AsQueryable<Student>().Where(exp).FirstOrDefault();

            Assert.IsNotNull(student);
        }

    }
}
