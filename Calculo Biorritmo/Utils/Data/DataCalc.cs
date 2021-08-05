using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Utils.Data
{
    public static class  DataCalc
    {
        
        public static DateTime getBirthDate(string RFC)
        {
            var datos = RFC.Substring(4, 6);

            var year = datos.Substring(0,2);
            var maxYear = DateTime.Now.Year.ToString().Substring(2, 2);
            year = (Convert.ToInt32(year) > Convert.ToInt32(maxYear)) ? $"19{year}" : $"20{year}";
            var month = datos.Substring(2,2);
            var day = datos.Substring(4,2);

            string birthDateString = $"{year}/{month}/{day}";

            DateTime dt = DateTime.ParseExact(birthDateString, "yyyy/MM/dd", CultureInfo.InvariantCulture);

            return dt;
        }

        public static int daysLived(DateTime birthDate)
        {
            DateTime today = DateTime.Now;
            int days = (today - birthDate).Days;
            return days;
        }

        public static int daysLived(DateTime birthDate,DateTime accidentDate)
        {
            int days = (accidentDate - birthDate).Days;
            return days;
        }

        public static DateTime getFirstDayMonth()
        {
            DateTime today = DateTime.Now;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            return firstDayOfMonth;
        }

        public static List<Double> CalculateBiorritm(int diasVividos, int teoria)
        {
            List<Double> values = new List<Double>();

            for (int i = -1; i < 2; i++)
            {
                var dayValue = (2 * Math.PI * (diasVividos + i)) / teoria;
                var sinValue = Math.Sin(dayValue);
                var roundedValue = Math.Round(sinValue, 9, MidpointRounding.ToEven);
                values.Add(roundedValue);
            }

            return values;
        }
    }
}
