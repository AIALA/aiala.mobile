using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using aiala.mobile.BackgroundServices;

namespace aiala.mobile.Activities
{
    public class ActivityHandlerService
    {
        private readonly IComponentContext _context;

        public ActivityHandlerService(IComponentContext context)
        {
            _context = context;
        }

        public async Task<bool> HandleActivity(ActivityBase activity)
        {
            var handlerType = typeof(IActivityHandler<>).MakeGenericType(activity.GetType());

            // handle
            var result = false;
            if (_context.Resolve(handlerType) is IActivityHandler handler)
            {
                result = await handler.HandleActivity(activity);
            }

            return result;
        }
    }
}
