namespace Task4Application.Repositories
{
    public interface IProductRepository
    {
        Task<bool> ProductExistsAsync(int productId);
    }
}
