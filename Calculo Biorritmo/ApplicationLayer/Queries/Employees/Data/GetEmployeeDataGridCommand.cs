using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.Queries.Employees.Data
{
    class GetEmployeeDataGridCommand : IRequest<GetEmployeeDataGridResponse>
    {
        public string curp { get; set; }
    }
}
