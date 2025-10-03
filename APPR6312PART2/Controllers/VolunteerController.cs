using Microsoft.AspNetCore.Mvc;
using APPR6312PART2.Models;
using System.Collections.Generic;
using System.Linq;

namespace APPR6312PART2.Controllers
{
    public class VolunteerController : Controller
    {
        // Static list to store volunteers
        private static List<Volunteer> _volunteers = new List<Volunteer>();
        private static int _nextVolunteerId = 1;

        // Volunteer Registration Page
        public IActionResult Register()
        {
            return View();
        }

        // Handle Volunteer Registration Submission
        [HttpPost]
        public IActionResult Register(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                // Assign ID and add to list
                volunteer.VolunteerId = _nextVolunteerId++;
                _volunteers.Add(volunteer);

                TempData["SuccessMessage"] = "Thank you for registering as a volunteer!";
                return RedirectToAction("Index", "Home");
            }
            return View(volunteer);
        }

        // View All Volunteers (Admin view)
        public IActionResult ViewVolunteers()
        {
            return View(_volunteers);
        }

        // Volunteer Details
        public IActionResult Details(int id)
        {
            var volunteer = _volunteers.FirstOrDefault(v => v.VolunteerId == id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // Available Tasks for Volunteers
        public IActionResult AvailableTasks()
        {
            var tasks = new List<object>
            {
                new {
                    Title = "Community Food Bank Support",
                    Description = "Assist with sorting and distributing food packages",
                    Location = "Downtown Community Center",
                    Duration = "3-5 hours"
                },
                new {
                    Title = "Educational Program Assistant",
                    Description = "Support after-school programs for children",
                    Location = "Local Schools",
                    Duration = "2-4 hours"
                },
                new {
                    Title = "Environmental Cleanup Crew",
                    Description = "Participate in park cleanup initiatives",
                    Location = "City Parks",
                    Duration = "4 hours"
                },
                new {
                    Title = "Elderly Companion Volunteer",
                    Description = "Provide companionship to seniors",
                    Location = "Senior Living Communities",
                    Duration = "2-3 hours"
                }
            };

            return View(tasks);
        }

        // Volunteer Impact Statistics
        public IActionResult VolunteerImpact()
        {
            var stats = new
            {
                TotalVolunteers = _volunteers.Count,
                ActiveVolunteers = _volunteers.Count,
                NewVolunteers = _volunteers.Count(v => v.RegistrationDate.Date == System.DateTime.Today)
            };

            return View(stats);
        }
    }
}