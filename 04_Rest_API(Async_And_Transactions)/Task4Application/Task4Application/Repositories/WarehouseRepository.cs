using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Task4Application.Exceptions;
using Task4Application.Models;

namespace Task4Application.Repositories;
public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;
    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> WarehouseExistsAsync(int warehouseId)
    {
        
            await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            await connection.OpenAsync();

            var query = "SELECT COUNT(1) FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdWarehouse", warehouseId);

            var exists = (int)await command.ExecuteScalarAsync() > 0;
            return exists;
        
    }

    


    public async Task<int?> RegisterProductInWarehouseAsync(int idWarehouse, int idProduct, int idOrder, int amount, DateTime createdAt)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();
        
        // Otwieramy transakcję
        await using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            // 1. Pobieranie ceny - MUSI mieć przekazaną transakcję (SqlTransaction)transaction
            var priceQuery = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
            await using var priceCommand = new SqlCommand(priceQuery, connection, (SqlTransaction)transaction);
            priceCommand.Parameters.AddWithValue("@IdProduct", idProduct);
            var pricePerUnit = (decimal)await priceCommand.ExecuteScalarAsync();

            var totalPrice = pricePerUnit * amount;

            // 2. Aktualizacja zamówienia - MUSI mieć przekazaną transakcję
            var updateQuery = "UPDATE \"Order\" SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
            await using var updateCommand = new SqlCommand(updateQuery, connection, (SqlTransaction)transaction);
            updateCommand.Parameters.AddWithValue("@IdOrder", idOrder);
            updateCommand.Parameters.AddWithValue("@FulfilledAt", DateTime.UtcNow);
            await updateCommand.ExecuteNonQueryAsync();
            
            // 3. Wstawianie do magazynu - MUSI mieć przekazaną transakcję
            var insertQuery = @"
                      INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, CreatedAt, Amount, Price)
                      OUTPUT Inserted.IdProductWarehouse
                      VALUES (@IdWarehouse, @IdProduct, @IdOrder, @CreatedAt, @Amount, @Price);";
            await using var insertCommand = new SqlCommand(insertQuery, connection, (SqlTransaction)transaction);
            insertCommand.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
            insertCommand.Parameters.AddWithValue("@IdProduct", idProduct);
            insertCommand.Parameters.AddWithValue("@IdOrder", idOrder);
            insertCommand.Parameters.AddWithValue("@CreatedAt", createdAt);
            insertCommand.Parameters.AddWithValue("@Amount", amount);
            insertCommand.Parameters.AddWithValue("@Price", totalPrice);
            
            var idProductWarehouse = (int)await insertCommand.ExecuteScalarAsync();

            // Zatwierdzamy transakcję, jeśli wszystko się udało
            await transaction.CommitAsync();
            return idProductWarehouse;
        }
        catch
        {
            // Wycofujemy zmiany, jeśli gdzieś po drodze wybuchł błąd
            await transaction.RollbackAsync();
            throw; // Rzucamy dalej, żeby zobaczyć błąd w razie czego
        }
    }

    public async Task<int> RegisterProductInWarehouseByProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime createdAt)
    {
        await using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await connection.OpenAsync();

            await using var command = new SqlCommand("AddProductToWarehouse", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
            command.Parameters.AddWithValue("@IdProduct", idProduct);
            command.Parameters.AddWithValue("@Amount", amount);
            command.Parameters.AddWithValue("@CreatedAt", createdAt);

            await command.ExecuteNonQueryAsync();
            await using var idCommand = new SqlCommand("SELECT @@IDENTITY", connection);
            return Convert.ToInt32(await idCommand.ExecuteScalarAsync());
            
            // var idProductWarehouse = (int)await command.ExecuteScalarAsync();
            // return idProductWarehouse;
    }

}