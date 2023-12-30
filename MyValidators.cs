using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace WpfLibrary
{
    public class MyValidators
    {
    }

    public class MinimumLengthRule : ValidationRule
    {
        public int MinimumLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if(str.Length < MinimumLength)
            {
                return new ValidationResult(false, "קצר מידי");
            }
            return new ValidationResult(true, null);
        }
    }

    public class MaxLengthRule: ValidationRule
    {
        public int MaxLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if(str.Length > MaxLength)
            {
                return new ValidationResult(false, $" תווים {MaxLength} ארוך מידי, ניתן להקליד עד");
            }
            return new ValidationResult(true, null);
        }
    }


}
