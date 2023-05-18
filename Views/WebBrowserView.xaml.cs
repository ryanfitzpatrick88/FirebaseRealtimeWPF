using System;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Fiddler;
using FirebaseRealtimeWPF.Services;
using Microsoft.Web.WebView2.Wpf;
using System.Windows.Controls;

namespace FirebaseRealtimeWPF.Views
{
    public partial class WebBrowserView : Page, IWebViewService
    {
        private Settings settings;
        
        public WebBrowserView()
        {
            InitializeComponent();
            
            settings = new Settings();
            settings.LoadSecrets();

            this.Loaded += (sender, args) =>
            {
                Login();
            };
        }

        public async void Login()
        {
            try
            {
                await webBrowser.EnsureCoreWebView2Async();
                
                webBrowser.CoreWebView2.Settings.UserAgent = "Microsoft Edge WebView2";
                
                string script = $@"
                                document.getElementById('username').value = '{settings.Email}';
                                document.getElementById('password').value = '{settings.Password}';
                                document.getElementById('setCredentialsForm').querySelector('button').click();
                            ";
                await Task.Delay(TimeSpan.FromSeconds(0.25)); // Wait for the page to load
                await webBrowser.ExecuteScriptAsync(script);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}