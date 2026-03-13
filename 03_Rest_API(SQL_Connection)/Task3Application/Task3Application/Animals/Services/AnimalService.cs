namespace Task3Application.Animals.Services;

using Task3Application.Animals;
using Task3Application.Animals.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    
    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    public IEnumerable<Animal> GetAllAnimals(string orderBy)
    {
        return _animalRepository.FetchAllAnimals(orderBy);
    }
    public bool CreateAnimal(CreateAnimal animal)
    {
        var newAnimal = new Animal
        {
            Name = animal.Name,
            Description = animal.Description,
            Category = animal.Category,
            Area = animal.Area
        };

        return _animalRepository.CreateAnimal(newAnimal);
    }
    public Animal GetAnimalById(int id)
    {
        return _animalRepository.GetAnimalById(id);
    }
    public bool UpdateAnimal(Animal animal)
    {
        return _animalRepository.UpdateAnimal(animal);
    }
    public bool DeleteAnimal(int id)
    {
        return _animalRepository.DeleteAnimal(id);
    }
}