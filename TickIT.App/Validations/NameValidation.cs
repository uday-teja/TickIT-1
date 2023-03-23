
using System.Globalization;
using System.Windows.Controls;

namespace TickIT.App.Validations
{
    public class NameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new(false, "Ticket name is mandatory");
            return new(true, null);
        }
    }
}
