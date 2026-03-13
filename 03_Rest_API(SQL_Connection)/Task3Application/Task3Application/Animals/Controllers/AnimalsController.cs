using Microsoft.AspNetCore.Mvc;
using Task3Application.Animals.Services;

namespace Task3Application.Animals;

[ApiController]
[Route("/api/animals")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;
    
    public AnimalsController(IAnimalService animalService)
    {
        _animalService = animalService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllAnimals([FromQuery] string orderBy = "Name")
    {
        if (!IsValidOrderBy(orderBy))
        {
            return BadRequest("Invalid orderBy parameter.");
        }
        
        var animals = _animalService.GetAllAnimals(orderBy);
        return Ok(animals);
    }
    
    private bool IsValidOrderBy(string orderBy)
    {
        return orderBy == "Name" || orderBy == "Description" || orderBy == "Category" || orderBy == "Area";
    }

    [HttpPost]
    public IActionResult CreateAnimal([FromBody] CreateAnimal animal)
    {
        var success = _animalService.CreateAnimal(animal);
        return success ? StatusCode(StatusCodes.Status201Created) : Conflict();
    }

    [HttpPut("{idAnimal}")]
    public IActionResult UpdateAnimal(int idAnimal, [FromBody] UpdateAnimal updateRequest)
    {
        var animalToUpdate = new Animal
        {
            IdAnimal = idAnimal,
            Name = updateRequest.Name,
            Description = updateRequest.Description,
            Category = updateRequest.Category,
            Area = updateRequest.Area
        };

        var success = _animalService.UpdateAnimal(animalToUpdate);
    
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        var success = _animalService.DeleteAnimal(idAnimal);
    
        return success ? NoContent() : NotFound(); 
    }
}
