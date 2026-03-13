using Task4Application.Dto;


    namespace Task4Application.Services
    {
        public interface IWarehouseService
        {
            Task<int> RegisterProductInWarehouseAsync(RegisterProductInWarehouseRequestDTO dto);
            Task<int> RegisterProductInWarehouseUsingProcedureAsync(RegisterProductInWarehouseRequestDTO dto);
        }
    }


