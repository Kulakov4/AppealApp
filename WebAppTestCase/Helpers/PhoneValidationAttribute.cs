using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTestCase.Helpers
{
    public class MyPhoneAttribute: ValidationAttribute
    {
        private readonly string region;
        public MyPhoneAttribute(string region)
        {
            this.region = region;
        }

        public override bool IsValid(object value)
        {
            if (!(value is string))
                return false;
            try
            {
                string phone = (string)value;
                var phoneService = PhoneNumberUtil.GetInstance();
                return phoneService.IsValidNumberForRegion(phoneService.Parse(phone, region), region);
            }
            catch
            {
                return false;
            }
        }
    }
}
