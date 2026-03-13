namespace Task6Application.Models.DTOs;

public class PrescriptionDTO
{
    public PatientDTO PatientDto { get; set; }
    public DoctorDTO DoctorDto { get; set; }
    public List<MedicamentDTO> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public PrescriptionDTO()
    {
        Medicaments = new List<MedicamentDTO>();
    }
}