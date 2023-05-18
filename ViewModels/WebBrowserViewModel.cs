using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using FirebaseRealtimeWPF.Services;
using Microsoft.Web.WebView2.Wpf;

namespace FirebaseRealtimeWPF.ViewModels
{
    public class WebBrowserViewModel : Screen
    {
        private readonly IWebViewService _webViewService;
        
        public WebBrowserViewModel(IWebViewService webViewService)
        {
            _webViewService = webViewService;
        }
    }
}