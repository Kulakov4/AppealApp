using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTestCase.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString DisplayForPhone(this IHtmlHelper helper, string phone, PhoneNumberFormat format)
        {
            if (phone == null)
            {
                return new HtmlString(string.Empty);
            }

            string formatted = phone.AsPhone(format);
            string s = $"<a href='tel:{phone}'>{formatted}</a>";
            return new HtmlString(s);
        }
    }
}
