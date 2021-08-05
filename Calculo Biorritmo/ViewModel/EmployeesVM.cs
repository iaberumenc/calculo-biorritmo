using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ViewModel
{
    class EmployeesVM
    {
        public int id { get; set; }
        public string curp { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public DateTime fecha_accidente { get; set; }
        public int dias_vividos { get; set; }
        
    }
}
