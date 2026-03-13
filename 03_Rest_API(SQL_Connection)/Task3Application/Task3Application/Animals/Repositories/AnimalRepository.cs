using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Task3Application.Animals;


public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;
    
    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IEnumerable<Animal> FetchAllAnimals(string orderBy)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();

        var safeOrderBy = new string[] { "Name", "Description", "Category", "Area" }.Contains(orderBy) ? orderBy : "Name";
        using var command = new SqlCommand($"SELECT * FROM Animal ORDER BY {safeOrderBy}", connection);
        
        using var reader = command.ExecuteReader();

        var animals = new List<Animal>();
        while (reader.Read())
        {
            var animal = new Animal
            {
                IdAnimal = (int)reader["IdAnimal"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Category = reader["Category"].ToString(),
                Area = reader["Area"].ToString()
            };
            animals.Add(animal);
        }

        return animals;
    }

    public bool CreateAnimal(Animal animal)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand(@"INSERT INTO Animal (Name, Description, Category, Area) 
                                       VALUES (@Name, @Description, @Category, @Area)", connection);

        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);

        var affectedRows = command.ExecuteNonQuery();
        return affectedRows == 1;
    }
    
    public Animal GetAnimalById(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();

        using var command = new SqlCommand("SELECT * FROM Animal WHERE IdAnimal = @IdAnimal", connection);
        command.Parameters.AddWithValue("@IdAnimal", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Animal
            {
                IdAnimal = (int)reader["IdAnimal"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Category = reader["Category"].ToString(),
                Area = reader["Area"].ToString()
            };
        }

        return null;
    }
    
    public bool UpdateAnimal(Animal animal)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand(@"UPDATE Animal 
                                       SET Name = @Name, Description = @Description, 
                                           Category = @Category, Area = @Area
                                       WHERE IdAnimal = @IdAnimal", connection);

        command.Parameters.AddWithValue("@IdAnimal", animal.IdAnimal);
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Description", animal.Description);
        command.Parameters.AddWithValue("@Category", animal.Category);
        command.Parameters.AddWithValue("@Area", animal.Area);

        var affectedRows = command.ExecuteNonQuery();
        return affectedRows == 1;
    }
    
    public bool DeleteAnimal(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @IdAnimal", connection);
        command.Parameters.AddWithValue("@IdAnimal", id);

        var affectedRows = command.ExecuteNonQuery();
        return affectedRows == 1;
    }
}
