using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ViceCodeTestTask
{
    class ProperUsernameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (((string)value).Length < 2)
            {
                return new ValidationResult(false, "Username must be at least 2 characters long.");
            }

            return new ValidationResult(true, null);
        }
    }
}
