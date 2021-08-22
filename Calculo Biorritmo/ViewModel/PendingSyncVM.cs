using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ViewModel
{
    class PendingSyncVM
    {
        public PendingSyncVM(int id, string model, string id_object)
        {
            this.id = id;
            this.model = model;
            this.id_object = id_object;
        }

        public int id { get; set; }
        public string model { get; set; }
        public string id_object { get; set; }
    }
}
