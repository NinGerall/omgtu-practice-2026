using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using task02;

namespace task02tests
{
    [TestFixture]
    public class StudentServiceTests
    {
        private StudentService _service = null!;

        [SetUp]
        public void Setup()
        {
            var students = new List<Student>
            {
                new() { Name = "Иван", Faculty = "ФИТ", Grades = new() { 5, 4, 5 } },
                new() { Name = "Анна", Faculty = "ФИТ", Grades = new() { 3, 4, 3 } },
                new() { Name = "Петр", Faculty = "Экономика", Grades = new() { 5, 5, 5 } }
            };
            _service = new StudentService(students);
        }

        [Test]
        public void GetStudentsByFaculty_ReturnsCorrectStudents()
        {
            var result = _service.GetStudentsByFaculty("ФИТ").ToList();
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetStudentsWithMinAverageGrade_ReturnsCorrect()
        {
            var result = _service.GetStudentsWithMinAverageGrade(4.5).ToList();
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetStudentsOrderedByName_ReturnsCorrect()
        {
            var result = _service.GetStudentsOrderedByName().ToList();
            Assert.That(result[0].Name, Is.EqualTo("Анна"));
        }

        [Test]
        public void GroupStudentsByFaculty_ReturnsCorrect()
        {
            var result = _service.GroupStudentsByFaculty();
            Assert.That(result["ФИТ"].Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
        {
            var result = _service.GetFacultyWithHighestAverageGrade();
            Assert.That(result, Is.EqualTo("Экономика"));
        }
    }
}
