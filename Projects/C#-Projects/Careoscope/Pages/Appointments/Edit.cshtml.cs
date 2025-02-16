using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Careoscope.Models;

namespace Careoscope.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly Careoscope.Models.CareoscopeDbContext _context;

        public EditModel(Careoscope.Models.CareoscopeDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment =  await _context.Appointments.FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }
            Appointment = appointment;
           ViewData["DoctorID"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID");
           ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "PatientID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(Appointment.AppointmentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }
    }
}
