using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Models
{
    public class employee
    {
        public int id { get; set; }
        [MaxLength(50)]
        public string curp { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        
    }
}
