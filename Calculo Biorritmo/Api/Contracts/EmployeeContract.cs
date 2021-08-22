using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Api.Contracts
{
    class EmployeeContract
    {
        public EmployeeContract(int id, string curp, string fecha_nacimiento)
        {
            this.id = id;
            this.curp = curp;
            this.fecha_nacimiento = fecha_nacimiento;
        }

        public int id { get; set; }
        public string curp { get; set; }
        public string fecha_nacimiento { get; set; }
    }
}
