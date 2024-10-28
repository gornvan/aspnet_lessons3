using Microsoft.Extensions.DependencyInjection;
using SynopticumCore.Contract;
using System.Reflection;

namespace SynopticumCore
{
    public class SynopticumCoreModule
    {
        internal struct InterfaceToImplementation
        {
            public Type Interface;
            public Type Implementation;
        }

        public static void RegisterModule(IServiceCollection services)
        {
            var currentAssembly = Assembly.GetAssembly(typeof(SynopticumCoreModule));

            var allTypesInThisAssembly = currentAssembly.GetTypes();

            var serviceTypes = allTypesInThisAssembly
                .Where(type =>
                    type.IsAssignableTo(typeof(IService))
                    && !type.IsInterface
                );

            var interfaceToImplementationMap = serviceTypes.Select(serviceType => {
                var implementation = serviceType;
                var @interface = serviceType.GetInterfaces()
                    .First(serviceInterface => serviceInterface != typeof(IService));

                return new InterfaceToImplementation
                {
                    Interface = @interface,
                    Implementation = implementation,
                };
            });

            foreach (var serviceToInterface in interfaceToImplementationMap)
            {
                services.AddTransient(serviceToInterface.Interface, serviceToInterface.Implementation);
            }
        }
    }
}
