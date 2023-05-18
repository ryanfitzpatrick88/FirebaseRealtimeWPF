using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;

namespace FirebaseRealtimeWPF.ViewModels
{
    public class ShellViewModel: Screen
    {
        private readonly SimpleContainer container;
        private INavigationService navigationService;

        public ShellViewModel(SimpleContainer container)
        {
            this.container = container;
        }
        
        public void RegisterFrame(Frame frame)
        {
            navigationService = new FrameAdapter(frame);

            container.Instance(navigationService);

            navigationService.NavigateToViewModel(typeof(MenuViewModel));
        }
    }
}