using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task4Application.Exceptions;

namespace Task4Application.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> ProductExistsAsync(int productId)
        {
                await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
                await connection.OpenAsync();

                var query = "SELECT COUNT(1) FROM Product WHERE IdProduct = @IdProduct";
                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProduct", productId);

                var exists = (int)await command.ExecuteScalarAsync() > 0;
                return exists;
            
            
        }
    }
}
