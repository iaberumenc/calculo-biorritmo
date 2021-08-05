using Calculo_Biorritmo.ApplicationLayer.Queries.Employees.Data;
using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Utils.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.Queries.Employees
{
    class GetEmployeeDataGridHandler : IRequestHandler<GetEmployeeDataGridCommand, GetEmployeeDataGridResponse>
    {
        private readonly EmployeeEntity _ctx;

        public GetEmployeeDataGridHandler(EmployeeEntity ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task<GetEmployeeDataGridResponse> Handle(GetEmployeeDataGridCommand request, CancellationToken cancellationToken)
        {
            var response = new GetEmployeeDataGridResponse();

            request.curp = request.curp?.Trim();

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@search_term", $"%{request.curp}%"));

            string addtitionalFilters = $" WHERE curp LIKE @search_term";

            var query = $@"
                             SELECT
                             curp,
                             fecha_nacimiento
                             from Employee";

            
            /*query += $@"    {additionalFilters}
           	                ORDER BY {request.orderBy} {request.orderType} OFFSET {request.offset} ROWS  
							FETCH NEXT {request.rowsPerPage} ROWS ONLY
                           ";*/

            if(!string.IsNullOrEmpty(request.curp))
                query += $@"{addtitionalFilters}";

            response.data = await _ctx.Database.SqlQuery<employeeGridItem>(query, parameters.ToArray()).ToListAsync();

            foreach (employeeGridItem item in response.data){
                int days = DataCalc.daysLived(item.fecha_nacimiento);
                item.dias_vividos = days;
            }

            return response;
        }
    }
}
