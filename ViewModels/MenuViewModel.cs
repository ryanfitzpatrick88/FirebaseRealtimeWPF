using Caliburn.Micro;

namespace FirebaseRealtimeWPF.ViewModels
{
    public class MenuViewModel : Screen
    {
        private readonly INavigationService navigationService;

        public MenuViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            Features = new BindableCollection<FeatureViewModel>
            {
                new FeatureViewModel("Product", "", typeof(ProductViewModel)),
                new FeatureViewModel("Web Browser", "", typeof(WebBrowserViewModel))
            };
        }

        public BindableCollection<FeatureViewModel> Features { get; }

        public void ShowFeature(FeatureViewModel feature)
        {
            navigationService.NavigateToViewModel(feature.ViewModel);
        }
    }
}