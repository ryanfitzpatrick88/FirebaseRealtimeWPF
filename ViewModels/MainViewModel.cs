using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Caliburn.Micro;

namespace FirebaseRealtimeWPF.ViewModels
{
    public class MainViewModel : Screen
    {
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyOfPropertyChange("Title");
            }
        }


        private IEventAggregator _eventAggregator;

        public MainViewModel(IEventAggregator eventAgg)
        {
            Title = "Welcome to Caliburn Micro in WPF";
            _eventAggregator = eventAgg;
        }

 
        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            await _eventAggregator.PublishOnUIThreadAsync("Closing");
        }

        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await _eventAggregator.PublishOnUIThreadAsync("Loading");
            await Task.Delay(2000);
            await _eventAggregator.PublishOnUIThreadAsync(string.Empty);

        }
    }
}