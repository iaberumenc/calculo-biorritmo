using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Utils
{
    class tools
    {
        internal static string generateId()
        {
            Thread.Sleep(1);

            var date = DateTime.Now.Ticks.ToString();
            var newId = string.Empty;

            newId = date.Substring(8,7);

            return newId;
        }
    }
}
