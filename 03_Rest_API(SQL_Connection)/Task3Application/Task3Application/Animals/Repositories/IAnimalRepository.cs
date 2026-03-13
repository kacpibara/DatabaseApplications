namespace Task3Application.Animals;

public interface IAnimalRepository
{
    IEnumerable<Animal> FetchAllAnimals(string orderBy);
    public bool CreateAnimal(Animal animal);
    Animal GetAnimalById(int id);
    bool UpdateAnimal(Animal animal);
    bool DeleteAnimal(int id);
}
