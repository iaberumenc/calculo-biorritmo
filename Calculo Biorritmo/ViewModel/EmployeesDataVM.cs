using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ViewModel
{
    class EmployeesDataVM
    {
        public string curp { get; set; }
        public double residuo_fisico { get; set; }
        public bool isCriticFisic { get; set; }
        public double residuo_emocional { get; set; }
        public bool isCriticEmotional { get; set; }

        public double residuo_intelectual { get; set; }
        public bool isCriticIntelectual { get; set; }

        public double residuo_intuicional { get; set; }
        public bool isCriticIntuitional { get; set; }


        public EmployeesDataVM(string curp, double residuo_fisico, bool isCriticFisic, double residuo_emocional, 
            bool isCriticEmotional, double residuo_intelectual, bool isCriticIntelectual, double residuo_intuicional, bool isCriticIntuitional)
        {
            this.curp = curp;
            this.residuo_fisico = residuo_fisico;
            this.isCriticFisic = isCriticFisic;
            this.residuo_emocional = residuo_emocional;
            this.isCriticEmotional = isCriticEmotional;
            this.residuo_intelectual = residuo_intelectual;
            this.isCriticIntelectual = isCriticIntelectual;
            this.residuo_intuicional = residuo_intuicional;
            this.isCriticIntuitional = isCriticIntuitional;
        }
    }
}
