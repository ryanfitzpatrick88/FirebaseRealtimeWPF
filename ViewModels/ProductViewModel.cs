using Caliburn.Micro;
using System.Windows;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;

namespace FirebaseRealtimeWPF.ViewModels
{
    public class ProductViewModel : Screen, INotifyPropertyChanged
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private FirebaseAuthClient authClient;
        private FirebaseClient firebaseClient;
        private Settings settings;
        private string status;
        private FirebaseService firebaseService;
        
        private IEventAggregator _eventAggregator;
        public ProductViewModel(IEventAggregator eventAgg)
        {
            _eventAggregator = eventAgg;
            
            Status = "Initializing...";
            this.settings = new Settings();
            this.settings.LoadSecrets();

            firebaseService = new FirebaseService();
            
            //InitializeComponent();
            //DataContext = this;
            
            authClient = firebaseService.GetFirebaseAuthClient();
            var tokenRefreshes = new Subject<string>();
            var tokenSubscription = tokenRefreshes.Subscribe(token =>
            {
                Console.WriteLine("Token refreshed: " + token);
            });
            firebaseClient = firebaseService.GetFirebaseClient(authClient, tokenRefreshes);

            /*
            var factory = new ProductFactory();
            var newProducts = factory.GenerateProducts(1000);
            foreach (var product in newProducts)
                products.Add(product);
                */
            
            ConfigureProducts("products");
        }

        public ObservableCollection<Product> Products { get { return products; } }
        public IDisposable Subscription { get; set; }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        private void ConfigureProducts(string childName)
        {
            try {
                Status = "Configuring products...";
                var child = firebaseClient.Child(childName);

                Status = "creating observable...";
                var observable = child.AsObservable<Product>();

                System.Timers.Timer timer = new System.Timers.Timer(1000);
                timer.AutoReset = false; // So it only fires once
                timer.Elapsed += (sender, e) =>
                {
                    Status = "Done";
                    timer.Stop();
                };
                
                Status = "Subscribing to products... please wait...";
                this.Subscription = observable
                    .Where(f => !string.IsNullOrEmpty(f.Key)) // you get empty Key when there are no data on the server for specified node
                    .Subscribe((f) =>
                    {
                        timer.Stop(); // stop the timer
                        timer.Start(); // and start it again. This will reset the timer.
                        
                        Status = $"Updating products... {f.Key}";
                        var existing = Products.Where(i => i.Id == f.Object.Id).FirstOrDefault();
                        if(existing != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                            existing.Id = f.Object.Id;
                                existing.Name = f.Object.Name;
                                existing.Price = f.Object.Price;
                                existing.ManufactureDate = f.Object.ManufactureDate;
                                existing.ExpiryDate = f.Object.ExpiryDate;
                                existing.Manufacturer = f.Object.Manufacturer;
                                existing.Model = f.Object.Model;
                                existing.Category = f.Object.Category;
                                existing.Weight = f.Object.Weight;
                                existing.WeightUnit = f.Object.WeightUnit;
                                existing.Description = f.Object.Description;
                                existing.StockQuantity = f.Object.StockQuantity;
                            });
                        }
                        else
                        {
                            var product = new Product()
                            {
                                Id = f.Object.Id,
                                Name = f.Object.Name,
                                Price = f.Object.Price,
                                ManufactureDate = f.Object.ManufactureDate,
                                ExpiryDate = f.Object.ExpiryDate,
                                Manufacturer = f.Object.Manufacturer,
                                Model = f.Object.Model,
                                Category = f.Object.Category,
                                Weight = f.Object.Weight,
                                WeightUnit = f.Object.WeightUnit,
                                Description = f.Object.Description,
                                StockQuantity = f.Object.StockQuantity
                            };
                            
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Products.Add(product);
                            });
                        }
                    });
            }
            catch (Exception ex)
            {
                Status = $"Error configuring products. {ex.ToString()}";
                Console.WriteLine(ex.ToString());
            }
        }

        

        private async void UploadClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var child = firebaseClient.Child("products");
                
                foreach (var product in Products)
                {
                    var result= await child.PostAsync(product.ToJson());
                    Console.WriteLine($"Added new product and got key {result}");
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void ReconnectClicked()
        {
            products.Clear();
            Subscription.Dispose();
            ConfigureProducts("products");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
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