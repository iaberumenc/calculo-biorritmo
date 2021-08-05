using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.Queries.Accidents.Data
{
    public class GetAccidentDataGridCommand : IRequest<GetAccidentDataGridResponse>
    {
        public string curp { get; set; }
    }
}
