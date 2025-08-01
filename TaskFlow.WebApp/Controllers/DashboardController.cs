// TaskFlow.WebApp/Controllers/DashboardController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.WebApp.Controllers
{
    [Authorize] // This ensures only logged-in users can access this controller
    public class DashboardController : Controller
    {
        // GET: /Dashboard/Index or /
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                // If the user is an Admin, redirect them to the Jobs list.
                return RedirectToAction("Index", "Jobs");
            }

            if (User.IsInRole("Technician"))
            {
                // If the user is a Technician, redirect them to their jobs list.
                return RedirectToAction("MyJobs", "Technician");
            }

            // If the user has an unknown role or something went wrong, log them out.
            return RedirectToAction("Logout", "Account");
        }
    }
}
