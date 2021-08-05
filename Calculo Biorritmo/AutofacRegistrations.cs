using Autofac;
using Calculo_Biorritmo.Data;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo
{
    class AutofacRegistrations
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(AutofacRegistrations).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionActionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestExceptionProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder
            .RegisterAssemblyTypes(typeof(IRequest<>).Assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IRequest<>)))
            .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(typeof(IRequestHandler<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<EmployeeEntity>(ctx =>
            {
                return new EmployeeEntity();
            });

            return builder.Build();
        }
    }
}
