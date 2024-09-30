using WorkingHoursCounterSystemCore.Enums;

namespace WorkingHoursCounterSystemCore.Models
{
    public class UserSettings
    {
        public EnumLanguages Languages { get; set; }

        public EnumCurrencies Currencies { get; set; }
    }
}