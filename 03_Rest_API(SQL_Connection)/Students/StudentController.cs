using Microsoft.AspNetCore.Mvc;

namespace Task3Application.Students;

[ApiController]
[Route("/api/student")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllStudents([FromQuery] string orderBy)
    {
        var students = _studentService.GetAllStudents(orderBy);
        return Ok(students);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetStudent([FromRoute] int id)
    {
        return Ok(id);
    }
    
    [HttpPost]
    public IActionResult CreateStudent([FromBody] CreateStudentDTO dto)
    {
        var success = _studentService.AddStudent(dto);
        return success ? StatusCode(StatusCodes.Status201Created) : Conflict();
    }
}