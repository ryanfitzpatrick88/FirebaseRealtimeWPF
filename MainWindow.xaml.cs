using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IdentityModel.Tokens.Jwt;

namespace FirebaseRealtimeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private FirebaseAuthClient authClient;
        private FirebaseClient firebaseClient;
        private Settings settings;
        
        
        public MainWindow()
        {
            this.settings = new Settings();
            this.settings.LoadSecrets();
            
            InitializeComponent();
            DataContext = this;
            
            authClient = GetFirebaseAuthClient();
            var tokenRefreshes = new Subject<string>();
            var tokenSubscription = tokenRefreshes.Subscribe(token =>
            {
                Console.WriteLine("Token refreshed: " + token);
            });
            firebaseClient = GetFirebaseClient(authClient, tokenRefreshes);

            /*
            var factory = new ProductFactory();
            var newProducts = factory.GenerateProducts(1000);
            foreach (var product in newProducts)
                products.Add(product);
                */
            RunTask().Wait();
        }

        public ObservableCollection<Product> Products { get { return products; } }

        private async Task RunTask()
        {
            try {
                var child = firebaseClient.Child("products");

                var observable = child.AsObservable<Product>();

                var subscription = observable
                    .Where(f => !string.IsNullOrEmpty(f.Key)) // you get empty Key when there are no data on the server for specified node
                    .Subscribe((f) =>
                    {
                        var existing = Products.Where(i => i.Id == f.Object.Id).FirstOrDefault();
                        if(existing != null)
                        {
                            Dispatcher.Invoke(() =>
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
                            Dispatcher.Invoke(() =>
                            {
                                Products.Add(product);
                            });
                        }
                    });


                //foreach (var product in child)
                //{
                //    Console.WriteLine($"{product.Key} is {product.Object.Name}.");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private Firebase.Auth.UserCredential userCredential;
        private string token;
        private FirebaseClient GetFirebaseClient(FirebaseAuthClient firebaseClient, Subject<string> tokenRefreshes)
        {
            var client = new FirebaseClient(settings.BaseUrl, new FirebaseOptions()
            {
                AuthTokenAsyncFactory = async () =>
                {
                    if (userCredential == null)
                    {
                        userCredential = await firebaseClient.SignInWithEmailAndPasswordAsync(settings.Email, settings.Password);
                    }

                    if (token == null)
                    {
                        token = await userCredential.User.GetIdTokenAsync(true);
                        tokenRefreshes.OnNext(token); // emit event
                    }
                    else
                    {
                        // Decode the token without validating it
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(token);

                        // Check if the token is expired
                        var expiryTimeUnix = jwtToken.Payload.Exp.Value;
                        var expiryDateTime = DateTimeOffset.FromUnixTimeSeconds(expiryTimeUnix).UtcDateTime;

                        if (expiryDateTime > DateTime.UtcNow)
                        {
                            // Token is still valid
                            return token;
                        }
                        else
                        {
                            // Token is expired, refresh it
                            token = await userCredential.User.GetIdTokenAsync(true);
                            tokenRefreshes.OnNext(token); // emit event
                        }
                    }

                    return token;
                }
            });
            return client;
        }

        private FirebaseAuthClient GetFirebaseAuthClient()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = settings.ApiKey, // your firebase API Key
                AuthDomain = settings.AuthDomain, // your firebase domain
                Providers = new FirebaseAuthProvider[]
                {
                    // Add and configure individual providers
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
                UserRepository = new FileUserRepository("FirebaseSample") // persist data into %AppData%\FirebaseSample
            };

            // ...and create your FirebaseAuthClient
            var firebaseClient = new FirebaseAuthClient(config);
            
            return firebaseClient;
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
    }
}
