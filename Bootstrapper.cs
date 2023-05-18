using System;
using System.Collections.Generic;
using Caliburn.Micro;
using System.Windows;
using FirebaseRealtimeWPF.Services;
using FirebaseRealtimeWPF.ViewModels;
using FirebaseRealtimeWPF.Views;

namespace FirebaseRealtimeWPF
{
    public class Bootstrapper : BootstrapperBase
    {
        SimpleContainer container;

        public Bootstrapper()
        {
            Initialize();
            LogManager.GetLog = type => new DebugLog(type);
        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            container.Instance(container);
            container.Singleton<IWebViewService, WebBrowserView>();
            container.PerRequest<MenuViewModel>();
            container.PerRequest<ShellViewModel>();
            container.PerRequest<ProductViewModel>();
            container.PerRequest<WebBrowserViewModel>();
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.PerRequest<MainViewModel>();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            await DisplayRootViewForAsync(typeof(ShellViewModel));
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}