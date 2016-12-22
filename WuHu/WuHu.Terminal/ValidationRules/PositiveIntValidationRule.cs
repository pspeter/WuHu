﻿using System;
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
            int num;
            try
            {
                num = int.Parse((string) value ?? "");
            }
            catch
            {
                return new ValidationResult(false, "Keine Nummer");
            }

            if (num < 0)
            {
                return new ValidationResult(false, "Kleiner Null");
            }

            return ValidationResult.ValidResult;
        }
    }
}