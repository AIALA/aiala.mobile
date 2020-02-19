using aiala.mobile.Actions;
using aiala.mobile.Activities;
using System.Threading.Tasks;

namespace aiala.mobile.BackgroundServices
{
    public interface IActivityHandler
    {
        Task<bool> HandleActivity(ActivityBase activity);
    }

    public interface IActivityHandler<TActivity> : IActivityHandler
        where TActivity : ActivityBase
    {
    }
}
