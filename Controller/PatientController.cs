using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private static List<Patient> Patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "John Doe", Age = 30, Diagnosis = "Flu" },
            new Patient { Id = 2, Name = "Jane Smith", Age = 25, Diagnosis = "Cold" }
        };

        [HttpGet]
        public IActionResult GetPatients()
        {
            return Ok(Patients);
        }

        [HttpGet("{id}")]
        public IActionResult GetPatient(int id)
        {
            var patient = Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public IActionResult CreatePatient(Patient patient)
        {
            patient.Id = Patients.Max(p => p.Id) + 1;
            Patients.Add(patient);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, Patient updatedPatient)
        {
            var patient = Patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();

            patient.Name = updatedPatient.Name;
            patient.Age = updatedPatient.Age;
            patient.Diagnosis = updatedPatient.Diagnosis;
            return NoContent();
        }

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

