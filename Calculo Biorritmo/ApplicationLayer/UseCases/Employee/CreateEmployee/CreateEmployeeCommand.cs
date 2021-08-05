using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.UseCases.Employee.CreateEmployee
{
    class CreateEmployeeCommand : IRequest
    {
        public CreateEmployeeCommand(string curp,DateTime fecha_nacimiento,DateTime? fecha_accidente)
        {
            this.curp = curp;
            this.dias_vividos = dias_vividos;
            this.fecha_accidente = fecha_accidente;
            this.fecha_nacimiento = fecha_nacimiento;
        }

        public string curp { get; set; }
        public int dias_vividos { get; set; }
        public DateTime? fecha_accidente { get; set; }
        public DateTime fecha_nacimiento { get; set; }
    }
}
