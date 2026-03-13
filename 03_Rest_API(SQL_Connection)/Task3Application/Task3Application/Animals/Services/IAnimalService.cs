namespace Task3Application.Animals.Services;

public interface IAnimalService
{
    IEnumerable<Animal> GetAllAnimals(string orderBy);
    public bool CreateAnimal(CreateAnimal animal);
    Animal GetAnimalById(int id);
    bool UpdateAnimal(Animal animal);
    bool DeleteAnimal(int id);
}