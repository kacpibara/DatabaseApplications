namespace Task6Application.Models.DTOs;

public class MedicamentDetailsDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }
}