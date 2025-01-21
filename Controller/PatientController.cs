using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private static List<Patient> Patients = new List<Patient>
        {
            new Patient { Id = 1, firstName = "Mostafa", lastName = "KHAZNADAR" },
            new Patient { Id = 2, firstName = "Khaireddine", lastName = "PACHA" }
        };

    	[Authorize(Roles = "Admin,User")]
        [HttpGet]
        public IActionResult GetPatients()
        {
            return Ok(Patients);
        }

    	[Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public IActionResult GetPatient(int id)
        {
            var patient = Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }

    	[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreatePatient(Patient patient)
	{
	    // Check if a patient with the same ID already exists
	    if (Patients.Any(p => p.Id == patient.Id))
	    {
		// Return a conflict response if the ID already exists
		return Conflict(new { message = $"A patient with ID {patient.Id} already exists." });
	    }

	    // If no ID is provided, generate a new one
	    if (patient.Id == 0)
	    {
		patient.Id = Patients.Any() ? Patients.Max(p => p.Id) + 1 : 1; 
	    }

	    Patients.Add(patient);
	    return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
	}


    	[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, Patient updatedPatient)
        {
            var patient = Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();

            patient.firstName = updatedPatient.firstName;
            patient.lastName = updatedPatient.lastName;
            patient.email = updatedPatient.email;
            patient.phoneNumber = updatedPatient.phoneNumber;
	    patient.dob = updatedPatient.dob;        
    

            return NoContent();
        }

    	[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var patient = Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();

            Patients.Remove(patient);
            return NoContent();
        }
    }
}

