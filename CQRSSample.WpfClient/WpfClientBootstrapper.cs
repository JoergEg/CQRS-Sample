using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Castle.Core;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CQRSSample.WpfClient.UI.Shell;

namespace CQRSSample.WpfClient
{
    public class WpfClientBootstrapper : Bootstrapper<ShellViewModel>
    {
        private IWindsorContainer _container;

        protected override void Configure()
        {
            _container = new WindsorContainer();
            // adds and configures all components using WindsorInstallers from executing assembly  
            _container.Install(FromAssembly.This());
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key) ? _container.Resolve(service) : _container.Resolve(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return (IEnumerable<object>) _container.ResolveAll(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }


    public static class WindsorExtensions
    {

        public static void BuildUp(this IWindsorContainer container, object instance)
        {
            instance.GetType().GetProperties()
                 .Where(property => property.CanWrite && property.PropertyType.IsPublic)
                 .Where(property => container.Kernel.HasComponent(property.PropertyType))
                 .ForEach(property => property.SetValue(instance, container.Resolve(property.PropertyType), null));
        }
    }
}