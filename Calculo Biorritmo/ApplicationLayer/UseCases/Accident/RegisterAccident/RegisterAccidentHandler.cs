using Calculo_Biorritmo.Data;
using Calculo_Biorritmo.Models;
using Calculo_Biorritmo.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.ApplicationLayer.UseCases.Accident.RegisterAccident
{
    class RegisterAccidentHandler : IRequestHandler<RegisterAccidentCommand>
    {
        private readonly EmployeeEntity _ctx;

        public RegisterAccidentHandler(EmployeeEntity ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task<Unit> Handle(RegisterAccidentCommand request, CancellationToken cancellationToken)
        {
            var accident = new accident();
            accident.id = int.Parse(tools.generateId());
            accident.curp = request.curp;
            accident.fecha_accidente = request.fecha_accidente;
            accident.residuo_fisico = request.residuo_fisico;
            accident.residuo_emocional = request.residuo_emocional;
            accident.residuo_intelectual = request.residuo_intelectual;
            accident.residuo_intuicional = request.residuo_intuicional;

            using (var dbContextTransaction = _ctx.Database.BeginTransaction())
            {

                _ctx.accidents.Add(accident);
                await _ctx.SaveChangesAsync();
                dbContextTransaction.Commit();
            }

            return Unit.Value;
        }
    }
}
