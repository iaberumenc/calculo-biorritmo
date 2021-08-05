using Calculo_Biorritmo.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.Queries.Accidents.Data
{
    class GetAccidentDataGridHandler : IRequestHandler<GetAccidentDataGridCommand, GetAccidentDataGridResponse>
    {
        private readonly EmployeeEntity _ctx;

        public GetAccidentDataGridHandler(EmployeeEntity ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task<GetAccidentDataGridResponse> Handle(GetAccidentDataGridCommand request, CancellationToken cancellationToken)
        {
            var response = new GetAccidentDataGridResponse();

            request.curp = request.curp?.Trim();

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@search_term", $"%{request.curp}%"));

            string addtitionalFilters = $" where curp LIKE @search_term";


            var query = $@"
                             SELECT * 
                             from accident";


            /*query += $@"    {additionalFilters}
           	                ORDER BY {request.orderBy} {request.orderType} OFFSET {request.offset} ROWS  
							FETCH NEXT {request.rowsPerPage} ROWS ONLY
                           ";*/

            if (!string.IsNullOrEmpty(request.curp))
                query += $@"{addtitionalFilters}";

            response.data = await _ctx.Database.SqlQuery<accidentGridItem>(query, parameters.ToArray()).ToListAsync();
            return response;
        }
    }
}
