using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Models;
using Calculo_Biorritmo.Utils;
using Calculo_Biorritmo.Utils.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.UseCases.Employee.CreateEmployee
{
    class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand>
    {
        private readonly EmployeeEntity _ctx;

        public CreateEmployeeHandler(EmployeeEntity ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new employee();
            employee.id = int.Parse(tools.generateId());
            employee.curp = request.curp;
            //int days = DataCalc.daysLived(request.fecha_nacimiento);
            //employee.dias_vividos = days;
            employee.fecha_nacimiento = request.fecha_nacimiento;

            using (var dbContextTransaction = _ctx.Database.BeginTransaction())
            {
                
                _ctx.employees.Add(employee);
                await _ctx.SaveChangesAsync();
                dbContextTransaction.Commit();
            }

            return Unit.Value;
        }
    }
}
