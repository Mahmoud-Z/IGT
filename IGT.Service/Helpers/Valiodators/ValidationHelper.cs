using IGT.Core.Resources;
using IGT.Service.Helpers.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IGT.Service.Helpers.Valiodators
{
    public class ValidationHelper
    {
        public static void ValidateRequiredProperties(object obj)
        {
            if (obj == null)
            {
                throw new BussinessException("No body was found");
            }

            var properties = obj.GetType().GetProperties();
            string keys = "";
            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value == null || (value is string strValue && string.IsNullOrWhiteSpace(strValue)))
                {
                    keys += property.Name + " ,";
                }
            }
            if (!keys.IsNullOrEmpty())
            {
                throw new BussinessException(keys.Substring(0, keys.Length - 1) + "is required");
            }
        }
        public static void IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(email))
            {
                throw new BussinessException(GeneralResource.ValidEmail);
            }
        }
    }
}
