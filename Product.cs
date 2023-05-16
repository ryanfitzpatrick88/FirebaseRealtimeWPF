using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace FirebaseRealtimeWPF
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        HomeGoods,
        Food,
        Books
    }

    public class Product : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private decimal price;
        private DateTime manufactureDate;
        private DateTime expiryDate;
        private string manufacturer;
        private string model;
        private ProductCategory category;
        private decimal weight;
        private string weightUnit;
        private string description;
        private int stockQuantity;

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public decimal Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        public DateTime ManufactureDate
        {
            get { return manufactureDate; }
            set
            {
                if (manufactureDate != value)
                {
                    manufactureDate = value;
                    OnPropertyChanged("ManufactureDate");
                }
            }
        }

        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set
            {
                if (expiryDate != value)
                {
                    expiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }

        public string Manufacturer
        {
            get { return manufacturer; }
            set
            {
                if (manufacturer != value)
                {
                    manufacturer = value;
                    OnPropertyChanged("Manufacturer");
                }
            }
        }

        public string Model
        {
            get { return model; }
            set
            {
                if (model != value)
                {
                    model = value;
                    OnPropertyChanged("Model");
                }
            }
        }

        public ProductCategory Category
        {
            get { return category; }
            set
            {
                if (category != value)
                {
                    category = value;
                    OnPropertyChanged("Category");
                }
            }
        }

        public decimal Weight
        {
            get { return weight; }
            set
            {
                if (weight != value)
                {
                    weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }

        public string WeightUnit
        {
            get { return weightUnit; }
            set
            {
                if (weightUnit != value)
                {
                    weightUnit = value;
                    OnPropertyChanged("WeightUnit");
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public int StockQuantity
        {
            get { return stockQuantity; }
            set
            {
                if (stockQuantity != value)
                {
                    stockQuantity = value;
                    OnPropertyChanged("StockQuantity");
                }
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
