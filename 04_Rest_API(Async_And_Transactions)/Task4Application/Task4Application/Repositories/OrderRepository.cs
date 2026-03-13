using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task4Application.Exceptions;

namespace Task4Application.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int?> OrderExistsAsync(int productId, int amount, DateTime createdAt)
        {
            
                await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
                await connection.OpenAsync();

                var query = "SELECT IdOrder FROM \"Order\" WHERE IdProduct = @IdProduct AND Amount = @Amount AND CreatedAt < @CreatedAt AND FulfilledAt IS NULL";                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdProduct", productId);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@CreatedAt", createdAt);

                var result = await command.ExecuteScalarAsync();
                
                if (result == null || result == DBNull.Value)
                {
                    return null;
                }

                return (int)result;
            
            
        }
        public async Task<bool> IsOrderFulfilledAsync(int orderId)
        {
            await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            await connection.OpenAsync();

            var query = "SELECT COUNT(1) FROM Product_Warehouse WHERE IdOrder = @IdOrder";
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdOrder", orderId);

            var isFulfilled = (int)await command.ExecuteScalarAsync() > 0;
            return isFulfilled;
        }
    }
}
