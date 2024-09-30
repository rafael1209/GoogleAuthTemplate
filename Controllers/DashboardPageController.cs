using Microsoft.AspNetCore.Mvc;
using WorkingHoursCounterSystemCore.Middlewere;
using WorkingHoursCounterSystemCore.Services;

namespace WorkingHoursCounterSystemCore.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DashboardPageController : Controller
    {
        private DashboardLogic _dashboardLogic;
        private AuthorizeLogic _authLogic;

        public DashboardPageController(AuthorizeLogic authLogic, DashboardLogic dashboardLogic)
        {
            _authLogic = authLogic;

            _dashboardLogic = dashboardLogic;
        }

        [HttpGet]
        [AuthMiddleware]
        public IActionResult GetDashboardPage()
        {
            Request.Headers.TryGetValue("Authorization", out var token);

            var user = _authLogic.GetUserByAuthtoken(token!);

            return Ok(user);
        }
    }
}
