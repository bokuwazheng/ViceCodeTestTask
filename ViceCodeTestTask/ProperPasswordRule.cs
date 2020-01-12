using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ViceCodeTestTask
{
    class ProperPasswordRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (((string)value).Length < 6)
            {
                return new ValidationResult(false, "Password must be at least 6 characters long.");
            }

            return new ValidationResult(true, null);
        }
    }
}
