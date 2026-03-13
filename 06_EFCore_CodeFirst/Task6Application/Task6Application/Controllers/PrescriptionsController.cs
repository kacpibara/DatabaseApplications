using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task6Application.Models;
using Task6Application.Models.DTOs;
using Task6Application.Context;

namespace Task6Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly DatabaseContext _context;

    public PrescriptionsController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionDTO prescriptionDto)
    {
        if (prescriptionDto.Medicaments.Count > 10)
            return BadRequest("Prescription can not contain more than 10 medicaments.");

        if (prescriptionDto.DueDate < prescriptionDto.Date)
            return BadRequest("DueDate must be greater than or equal to Date.");

        var patient = await _context.Patient.FirstOrDefaultAsync(p => 
            p.FirstName == prescriptionDto.PatientDto.FirstName && 
            p.LastName == prescriptionDto.PatientDto.LastName && 
            p.Birthdate == prescriptionDto.PatientDto.Birthdate);
            
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = prescriptionDto.PatientDto.FirstName,
                LastName = prescriptionDto.PatientDto.LastName,
                Birthdate = prescriptionDto.PatientDto.Birthdate
            };
            _context.Patient.Add(patient);
        }
        
        var doctor = await _context.Doctor.FindAsync(prescriptionDto.DoctorDto.IdDoctor);
        if (doctor == null)
            return BadRequest("Doctor not found.");

        var prescription = new Prescription
        {
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
            IdPatientNavigation = patient,
            IdDoctorNavigation = doctor,
            PrescriptionMedicament = new List<PrescriptionMedicament>()
        };

        foreach (var med in prescriptionDto.Medicaments)
        {
            var medicament = await _context.Medicament.FindAsync(med.IdMedicament);
            if (medicament == null)
                return BadRequest("Medicament not found.");

            prescription.PrescriptionMedicament.Add(new PrescriptionMedicament
            {
                IdMedicamentNavigation = medicament,
                Dose = med.Dose,
                Details = med.Description
            });
        }

        _context.Prescription.Add(prescription);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDetailsDTO>> GetPatientDetails(int id)
    {
        var patient = await _context.Patient
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.IdDoctorNavigation)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicament)
                .ThenInclude(pm => pm.IdMedicamentNavigation)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return NotFound("Patient not found.");
        }
        
        var patientDetails = new PatientDetailsDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions.Select(pr => new PrescriptionDetailsDTO
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.DueDate,
                Doctor = new DoctorDetailsDTO
                {
                    IdDoctor = pr.IdDoctorNavigation.IdDoctor,
                    FirstName = pr.IdDoctorNavigation.FirstName,
                    LastName = pr.IdDoctorNavigation.LastName,
                    Email = pr.IdDoctorNavigation.Email
                },
                Medicaments = pr.PrescriptionMedicament.Select(pm => new MedicamentDetailsDTO
                {
                    IdMedicament = pm.IdMedicamentNavigation.IdMedicament,
                    Name = pm.IdMedicamentNavigation.Name,
                    Description = pm.IdMedicamentNavigation.Description,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToList()
            }).OrderByDescending(pr => pr.DueDate).ToList()
        };

        return Ok(patientDetails);
    }
}