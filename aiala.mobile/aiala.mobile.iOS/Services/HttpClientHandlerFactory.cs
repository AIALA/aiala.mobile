using aiala.mobile.iOS.Services;
using System.Net.Http;
using xappido.Mobile.Core.Services;

[assembly: Xamarin.Forms.Dependency(typeof(HttpClientHandlerFactory))]
namespace aiala.mobile.iOS.Services
{
    public class HttpClientHandlerFactory : IHttpClientHandlerFactory
    {
        public HttpMessageHandler CreateHandler()
        {
            return new NSUrlSessionHandler();
        }
    }
}
