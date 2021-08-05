using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.Queries.Accidents.Data
{
    public class GetAccidentDataGridResponse
    {
        public GetAccidentDataGridResponse()
        {
            data = new List<accidentGridItem>();
        }
        public List<accidentGridItem> data { get; set; }
    }

    public class accidentGridItem
    {
        public string curp { get; set; }
        public DateTime? fecha_accidente { get; set; }
        public double residuo_fisico { get; set; }
        public double residuo_emocional { get; set; }
        public double residuo_intelectual { get; set; }
        public double residuo_intuicional { get; set; }
    }
    
}
