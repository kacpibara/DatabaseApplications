using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Task3Application.Students;

public interface IStudentRepository
{
    public IEnumerable<Student> FetchAllStudents(string orderBy);
    public bool CreateStudent(string email);
}

public class StudentRepository : IStudentRepository
{
    private readonly IConfiguration _configuration;
    public StudentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IEnumerable<Student> FetchAllStudents(string orderBy)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();

        var safeOrderBy = new string[] { "Id", "Email" }.Contains(orderBy) ? orderBy : "Id";
        var command = new SqlCommand($"SELECT * FROM Students ORDER BY {safeOrderBy}", connection);
        using var reader = command.ExecuteReader();

        var students = new List<Student>();
        while (reader.Read())
        {
            var student = new Student()
            {
                Id = (int)reader["Id"],
                Email = reader["Email"].ToString()!
            };
            students.Add(student);
        }

        return students;
    }

    public bool CreateStudent(string email)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("INSERT INTO Students (Email) VALUES (@email)", connection);
        command.Parameters.AddWithValue("@email", email);
        var affectedRows = command.ExecuteNonQuery();
        return affectedRows == 1;
    }
}