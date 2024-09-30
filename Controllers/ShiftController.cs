using Microsoft.AspNetCore.Mvc;

namespace WorkingHoursCounterSystemCore.Controllers
{
    public class ShiftController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
