using System;
using System.Linq;
using Autofac;
using aiala.mobile.BackgroundServices;
using xappido.Mobile;

namespace aiala.mobile.Extensions
{
    public static class AppContainerExtensions
    {
        public static AppContainer RegisterActivityHandler<TActivityHandler>(this AppContainer appContainer)
            where TActivityHandler : IActivityHandler
        {
            var builder = appContainer.Builder.RegisterType<TActivityHandler>()
                .As<IActivityHandler>();

            var handlerType = typeof(TActivityHandler);
            var handlerBaseType = typeof(IActivityHandler<>);
            var interfaceTypes = handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerBaseType);

            foreach (var interfaceType in interfaceTypes)
            {
                builder.As(interfaceType);
            }

            return appContainer;
        }
    }
}
