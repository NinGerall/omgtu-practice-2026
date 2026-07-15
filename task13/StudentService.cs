using System.Text.Json;

namespace task13;

public static class StudentService
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public static string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student, Options);
    }

    public static Student? Deserialize(string json)
    {
        var student = JsonSerializer.Deserialize<Student>(json, Options);
        if (student != null)
        {
            Validate(student);
        }
        return student;
    }

    public static void SaveToFile(string filePath, Student student)
    {
        string json = Serialize(student);
        File.WriteAllText(filePath, json);
    }

    public static Student LoadFromFile(string filePath)
    {
        string json = File.ReadAllText(filePath);
        var student = Deserialize(json);
        if (student == null)
        {
            throw new InvalidOperationException();
        }
        return student;
    }

    public static void Validate(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            throw new ArgumentException();
        }

        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            throw new ArgumentException();
        }

        if (student.BirthDate > DateTime.Now)
        {
            throw new ArgumentException();
        }

        if (student.Grades != null)
        {
            foreach (var subject in student.Grades)
            {
                if (string.IsNullOrWhiteSpace(subject.Name))
                {
                    throw new ArgumentException();
                }
                if (subject.Grade < 2 || subject.Grade > 5)
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}