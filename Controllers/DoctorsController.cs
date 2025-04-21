using AppointmentsApp.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using schedule_api.Models;
using System.Text.RegularExpressions;

namespace schedule_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ScheduleDBContext _context;

        public DoctorsController(ScheduleDBContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctor()
        {
            return await _context.Doctor.ToListAsync();
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(Guid id)
        {
            var doctor = await _context.Doctor.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        //api/Doctors/Search/name
        [HttpGet("Search/{name}")]
        public async Task<ActionResult<IEnumerable<Doctor>>> SearchByName(string name)
        {
            //RAW SQL
            var parameter = new SqlParameter("comparison", $"%{name}%");
            return await _context.Doctor.FromSqlRaw("SELECT * FROM Doctor WHERE name LIKE @comparison", parameter).ToListAsync();

            //var doctors = await _context.Doctor.ToListAsync();
            //return doctors.Where(doctor => doctor.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList();

            //var query = from doctor in _context.Doctor where doctor.Name.ToLower().Contains(name.ToLower()) select doctor;
            //return await query.ToListAsync();
        }


        // POST: api/Doctors/Create
        [HttpPost]
        public async Task<ActionResult<Doctor>> CreateDoctor(Doctor doctor)
        {
            if (!string.IsNullOrEmpty(doctor.Email))
            {
                if (!IsValidEmail(doctor.Email))
                {
                    ModelState.AddModelError(nameof(doctor.Email), "The Email is invalid.");
                }
            }

            if (!ModelState.IsValid)
            {
                return Problem("Invalid information");
            }

            _context.Doctor.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
        }

        //Regex
        private bool IsValidEmail(string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }



        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(Guid id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest();
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        //{
        //    _context.Doctor.Add(doctor);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetDoctor", new { id = doctor.Id }, doctor);
        //}

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(Guid id)
        {
            return _context.Doctor.Any(e => e.Id == id);
        }
    }
}
