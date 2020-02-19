using aiala.mobile.Actions;
using aiala.mobile.Activities;
using System.Threading.Tasks;

namespace aiala.mobile.BackgroundServices
{
    public abstract class ActivityHandler<TActivity> : IActivityHandler, IActivityHandler<TActivity>
        where TActivity : ActivityBase
    {
        abstract public Task<bool> HandleInternal(TActivity activity);

        public async Task<bool> HandleActivity(ActivityBase activity)
        {
            return await this.HandleInternal(activity as TActivity);
        }
    }
}
