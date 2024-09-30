using WorkingHoursCounterSystemCore.DataWrapper;
using WorkingHoursCounterSystemCore.Models;

namespace WorkingHoursCounterSystemCore.Services
{
    public class AuthorizeLogic
    {
        private AuthorizeDbContext _authorizeDbContext;

        public AuthorizeLogic(AuthorizeDbContext authorizeDbContext)
        {
            _authorizeDbContext = authorizeDbContext;
        }

        public User GetUserByAuthtoken(string authToken)
        {
            return _authorizeDbContext.GetUserByToken(authToken);
        }
    }
}
