using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FirebaseRealtimeWPF
{
    public class ProductFactory
    {
        public ProductFactory()
        {

        }

        public ObservableCollection<Product> GenerateProducts(int count)
        {
            var random = new Random();
            var products = new ObservableCollection<Product>();

            var manufacturers = this.manufacturers;
            var categories = Enum.GetValues(typeof(ProductCategory));
            var weightUnits = new[] { "kg", "lb" };
            var models = this.models;
            var descriptions = GenerateDescriptions(count);
            var names = GenerateNames(count);

            for (int i = 0; i < count; i++)
            {
                var product = new Product
                {
                    Id = i,
                    Name = names[i],
                    Price = (decimal)(random.NextDouble() * 1000),
                    ManufactureDate = DateTime.Now.AddDays(random.Next(-1000, 1000)),
                    ExpiryDate = DateTime.Now.AddDays(random.Next(1, 1000)),
                    Manufacturer = manufacturers[random.Next(manufacturers.Length)],
                    Model = models[random.Next(models.Length)],
                    Category = (ProductCategory)categories.GetValue(random.Next(categories.Length)),
                    Weight = (decimal)(random.NextDouble() * 10),
                    WeightUnit = weightUnits[random.Next(weightUnits.Length)],
                    Description = descriptions[i],
                    StockQuantity = random.Next(0, 100)
                };

                products.Add(product);
            }

            return products;
        }

        private string[] GenerateDescriptions(int count)
        {
            var adjectives = new string[] { "Incredible", "Fantastic", "Excellent", "Amazing", "Marvelous", "Stupendous", "Outstanding", "Superior", "Wonderful", "Impressive", "Exceptional", "Astonishing", "Remarkable", "Magnificent", "Splendid" };
            var nouns = new string[] { "Quality", "Design", "Craftsmanship", "Performance", "Efficiency", "Durability", "Versatility", "Reliability", "Style", "Comfort", "Elegance", "Innovation", "Convenience", "Value", "Functionality" };
            var phrases = new string[] { "guaranteed to satisfy", "sure to impress", "top-of-the-line", "highly recommended", "a popular choice", "worth every penny", "designed to please", "not to be missed", "a must-have", "second to none", "top-notch", "first-rate", "unmatched", "unsurpassed", "superb" };

            var random = new Random();
            var descriptions = new List<string>();

            for (int i = 0; i < count; i++)
            {
                var adjective = adjectives[random.Next(adjectives.Length)];
                var noun = nouns[random.Next(nouns.Length)];
                var phrase = phrases[random.Next(phrases.Length)];

                descriptions.Add($"{adjective} {noun}, {phrase}.");
            }

            return descriptions.ToArray();
        }

        private string[] GenerateNames(int count)
        {
            var adjectives = new string[] { "Super", "Mega", "Ultra", "Hyper", "Mighty", "Turbo", "Extreme", "Supreme", "Deluxe", "Power", "Max", "Pro", "Premier", "Elite", "Prime" };
            var nouns = new string[] { "T-Shirt", "Movie", "Book", "Laptop", "Smartphone", "Sweater","Jeans","Sneakers","Smartwatch","Headphones","Bag","Jacket","Tablet","Camera","Speaker","Sunglasses","Hat","Belt","Scarf","Necklace","Pants","Blouse","Perfume","Handbag","Boots","Game","Album","TV","Console","Bicycle" };
            var suffixes = new string[] { "X", "XL", "XXL", "S", "SE", "Pro", "Max", "Ultra", "Plus", "Advance", "Premium", "Elite", "Deluxe", "Master", "Extreme" };

            var random = new Random();
            var productNames = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                var adjective = adjectives[random.Next(adjectives.Length)];
                var noun = nouns[random.Next(nouns.Length)];
                var suffix = suffixes[random.Next(suffixes.Length)];

                productNames.Add($"{adjective} {noun} {suffix}");
            }

            return productNames.ToArray();
        }

        private string[] manufacturers = new string[]{
            "Beaver Tail Widgets Co.",
            "Maple Syrup Gadgets Inc.",
            "Moose Muffins Ltd.",
            "Poutine Paradise Corp.",
            "Nanaimo Bar Novelties LLC.",
            "Igloo Innovations Inc.",
            "Loonie Toonie Tech Ltd.",
            "Blame Canada Creations Corp.",
            "Yukon Yeti Yarns Co.",
            "Prairie Puck Products Inc.",
            "Timbit Trinkets Ltd.",
            "Eh-sentials Corp.",
            "Mountie Mounts LLC.",
            "Canuck Can Openers Co.",
            "Double-Double Devices Inc.",
            "Rocky Mountain Rockers Ltd.",
            "Totem Tech LLC.",
            "Klondike Klocks Corp.",
            "Polar Bear Plushies Co.",
            "Great White North Gadgets Inc."
        };

        private string[] models = new[] { "",
            "Model X", 
            "Model Y", 
            "Model Z", 
            "Niagara Nifty", 
            "Quebec Quirky", 
            "Toronto Trendy", 
            "Vancouver Versatile", 
            "Montreal Modern", 
            "Banff Brilliant", 
            "Ottawa Outstanding", 
            "Winnipeg Wonder", 
            "Calgary Classy", 
            "Saskatoon Stylish", 
            "Edmonton Elegant", 
            "Yukon Youthful", 
            "Manitoba Majestic", 
            "Halifax Hip", 
            "Victoria Vibrant" 
        };

    }
}
