using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.Queries.Employees.Data
{
    class GetEmployeeDataGridResponse
    {
        public GetEmployeeDataGridResponse()
        {
            data = new List<employeeGridItem>();
        }
        public List<employeeGridItem> data { get; set; }
    }

    public class employeeGridItem
    {
        public string curp { get; set; }
        public int dias_vividos { get; set; }
        public DateTime fecha_nacimiento { get; set; }
    }
}
