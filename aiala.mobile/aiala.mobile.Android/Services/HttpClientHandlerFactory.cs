using aiala.mobile.Droid.Services;
using System.Net.Http;
using xappido.Mobile.Core.Services;

[assembly: Xamarin.Forms.Dependency(typeof(HttpClientHandlerFactory))]
namespace aiala.mobile.Droid.Services
{
    public class HttpClientHandlerFactory : IHttpClientHandlerFactory
    {
        public HttpMessageHandler CreateHandler()
        {
            return new HttpClientHandler();
        }
    }
}
