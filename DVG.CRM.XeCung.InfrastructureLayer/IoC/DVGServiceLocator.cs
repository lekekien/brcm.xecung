using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.InfrastructureLayer.IoC
{
    public class DVGServiceLocator
    {
        private readonly ServiceProvider _currentServiceProvider;
        private static ServiceProvider _serviceProvider;

        public DVGServiceLocator(ServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = currentServiceProvider;
        }

        public static DVGServiceLocator Current => new DVGServiceLocator(_serviceProvider);

        public static void SetLocatorProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type serviceType)
        {
            return _currentServiceProvider.GetService(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return _currentServiceProvider.GetService<TService>();
        }
    }
}
