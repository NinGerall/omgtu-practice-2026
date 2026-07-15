using System.Text.Json.Serialization;

namespace task13;

public class Student
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime BirthDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Subject>? Grades { get; set; } = new();
}