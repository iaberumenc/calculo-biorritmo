﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Api.Contracts
{
    class AccidentContract
    {
        public AccidentContract(int id, string curp, string fecha_accidente, double residuo_fisico, double residuo_emocional, double residuo_intelectual, double residuo_intuicional)
        {
            this.id = id;
            this.curp = curp;
            this.fecha_accidente = fecha_accidente;
            this.residuo_fisico = residuo_fisico;
            this.residuo_emocional = residuo_emocional;
            this.residuo_intelectual = residuo_intelectual;
            this.residuo_intuicional = residuo_intuicional;
        }

        public AccidentContract()
        {

        }

        public int id { get; set; }
        public string curp { get; set; }
        public string fecha_accidente { get; set; }
        public double residuo_fisico { get; set; }
        public double residuo_emocional { get; set; }
        public double residuo_intelectual { get; set; }
        public double residuo_intuicional { get; set; }
    }

}
