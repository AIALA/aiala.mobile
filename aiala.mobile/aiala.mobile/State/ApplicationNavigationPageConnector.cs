using aiala.mobile.ViewModels;
using aiala.mobile.Views;
using Xamarin.Forms;
using xappido.Mobile.Core.Navigation;

namespace aiala.mobile
{
    public class ApplicationNavigationPageConnector : MasterDetailNavigationPageConnector<LoginView, MasterViewModel, MasterDetailViewModel>
    {
        public void ResetMaster()
        {
            var mainPage = Application.Current.MainPage as MasterDetailPage;
            mainPage.Master = CreatePageInternal(typeof(MasterViewModel));
        }
    }
}
