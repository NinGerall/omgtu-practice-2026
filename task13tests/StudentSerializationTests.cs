using Xunit;
using task13;

public class StudentSerializationTests
{
    [Fact]
    public void ShouldSerializeCorrectly()
    {
        var student = new Student
        {
            FirstName = "Герман",
            LastName = "Фищев",
            BirthDate = new DateTime(2005, 10, 15),
            Grades = new List<Subject>
            {
                new() { Name = "Математика", Grade = 5 }
            }
        };

        string json = StudentService.Serialize(student);

        Assert.Contains("\"BirthDate\": \"15-10-2005\"", json);
        Assert.Contains("\"Grade\": 5", json);
    }

    [Fact]
    public void ShouldIgnoreNullGradesOnSerialization()
    {
        var student = new Student
        {
            FirstName = "Иван",
            LastName = "Иванов",
            BirthDate = new DateTime(2000, 1, 1),
            Grades = null
        };

        string json = StudentService.Serialize(student);

        Assert.DoesNotContain("Grades", json);
    }

    [Fact]
    public void ShouldThrowExceptionOnInvalidGrade()
    {
        var student = new Student
        {
            FirstName = "Петр",
            LastName = "Петров",
            BirthDate = new DateTime(2000, 1, 1),
            Grades = new List<Subject>
            {
                new() { Name = "Физика", Grade = 6 }
            }
        };

        Assert.Throws<ArgumentException>(() => StudentService.Validate(student));
    }

    [Fact]
    public void ShouldSaveAndLoadFromFileCorrectly()
    {
        string tempFile = Path.GetTempFileName();
        var student = new Student
        {
            FirstName = "Герман",
            LastName = "Фищев",
            BirthDate = new DateTime(2005, 10, 15),
            Grades = new List<Subject>
            {
                new() { Name = "Алгебра", Grade = 5 }
            }
        };

        try
        {
            StudentService.SaveToFile(tempFile, student);
            var loaded = StudentService.LoadFromFile(tempFile);

            Assert.NotNull(loaded.Grades);
            Assert.Single(loaded.Grades);
            Assert.Equal("Алгебра", loaded.Grades![0].Name);
            Assert.Equal(5, loaded.Grades![0].Grade);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}