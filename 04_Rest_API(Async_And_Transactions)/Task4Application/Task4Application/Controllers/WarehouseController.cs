using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task4Application.Dto;
using Task4Application.Exceptions;
using Task4Application.Services;

namespace Task4Application.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;
    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    
    // POST /api/warehouse
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterProductInWarehouseAsync([FromBody] RegisterProductInWarehouseRequestDTO dto)
    {
        try
        {
            var idProductWarehouse = await _warehouseService.RegisterProductInWarehouseAsync(dto);
            return Ok(new { idProductWarehouse = idProductWarehouse });
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
    
    // POST /api/warehouse/procedure
    [HttpPost("procedure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterProductWithProcedureAsync([FromBody] RegisterProductInWarehouseRequestDTO dto)
    {
        try
        {
            var idProductWarehouse = await _warehouseService.RegisterProductInWarehouseUsingProcedureAsync(dto);
            return Ok(new { idProductWarehouse });
        }
        catch (Exception e) // Łapiemy ogólny wyjątek z bazy danych
        {
            // can add SqlException
            return Conflict(e.Message); 
        }
    }
}