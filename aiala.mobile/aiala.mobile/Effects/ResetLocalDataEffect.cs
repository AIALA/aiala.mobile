using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Auth.Actions;
using xappido.Mobile.Core.Redux;
using xappido.Mobile.Core.Services;
using xappido.Mobile.State.Actions;

namespace aiala.mobile.Effects
{
    public class ResetLocalDataEffect : Effect<LogoutSuccessAction>
    {
        private readonly IStore<ApplicationState> _store;
        private readonly ILoggerService _loggerService;

        public ResetLocalDataEffect(IStore<ApplicationState> store, ILoggerService loggerService, LogoutSuccessAction action) : base(action)
        {
            _store = store;
            _loggerService = loggerService;
        }

        public override Task OnExecute(LogoutSuccessAction action)
        {
            _loggerService.LogInfo("Resetting local data on logout");
            _store.Dispatch(new ResetLocalAppDataAction());

            return Task.CompletedTask;
        }
    }
}
