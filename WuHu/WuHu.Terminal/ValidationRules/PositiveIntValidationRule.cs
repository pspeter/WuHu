using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WuHu.Terminal.ValidationRules
{
    public class PositiveIntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "");
            }
            int num;
            try
            {
                num = int.Parse((string) value ?? "");
            }
            catch
            {
                return new ValidationResult(false, "");
            }

            if (num < 0)
            {
                return new ValidationResult(false, "Kleiner Null");
            }

            return ValidationResult.ValidResult;
        }
    }
}
