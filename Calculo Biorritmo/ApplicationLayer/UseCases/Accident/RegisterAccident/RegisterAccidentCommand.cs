using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.UseCases.Accident
{
    class RegisterAccidentCommand : IRequest
    {
        public RegisterAccidentCommand(string curp, DateTime fecha_accidente, double fisico, double emocional, double intelectual, double intuicional)
        {
            this.curp = curp;
            this.fecha_accidente = fecha_accidente;
            this.residuo_fisico = fisico;
            this.residuo_emocional = emocional;
            this.residuo_intelectual = intelectual;
            this.residuo_intuicional = intuicional;

        }

        public string curp { get; set; }
        public DateTime fecha_accidente { get; set; }
        public double residuo_fisico { get; set; }
        public double residuo_emocional { get; set; }
        public double residuo_intelectual { get; set; }
        public double residuo_intuicional { get; set; }
    }
}
