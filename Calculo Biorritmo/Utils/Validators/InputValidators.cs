using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Utils.Validators
{
    class InputValidators
    {
        public static bool validateCURP(string RFC)
        {
            bool valid = true;
            try
            {
                var datos = RFC.Substring(4, 6);

                var year = datos.Substring(0, 2);
                year = (Convert.ToInt32(year) > 30) ? $"19{year}" : $"20{year}";
                var month = datos.Substring(2, 2);
                var day = datos.Substring(4, 2);

                Convert.ToInt32(day);
                Convert.ToInt32(month);

                string birthDateString = $"{year}/{month}/{day}";

                DateTime.ParseExact(birthDateString, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                valid = false;
            }
            return valid;
        }
    }
}
