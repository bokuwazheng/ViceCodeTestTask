using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ValidationResult = System.Windows.Controls.ValidationResult;

namespace ViceCodeTestTask
{
    class ProperEmailAddressRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = "";

            if (((string)value).Length > 0)
            {
                input = (string)value;
            }

            if (new EmailAddressAttribute().IsValid(input))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "That doesn't look like a real email address. Try again?");
            }
        }
    }
}
