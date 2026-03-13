namespace Task3Application.Students;

public interface IStudentService
{
    public IEnumerable<Student> GetAllStudents(string orderBy);
    public bool AddStudent(CreateStudentDTO dto);
}

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    public IEnumerable<Student> GetAllStudents(string orderBy)
    {
        return _studentRepository.FetchAllStudents(orderBy);
    }

    public bool AddStudent(CreateStudentDTO dto)
    {
        return _studentRepository.CreateStudent(dto.Email);
    }
}