using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebAppTestCase.Extensions
{
    public static class StringExtensionsClass
    {
        public static string AsPhone(this string s, PhoneNumberFormat format)
        {
            try
            {
                var phoneService = PhoneNumberUtil.GetInstance();
                return phoneService.Format(phoneService.Parse(s, "RU"), format);
            }
            catch {
                return s;
            }
        }
    }
}
