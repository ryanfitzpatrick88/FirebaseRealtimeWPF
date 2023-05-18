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
using System.Windows.Controls;

namespace FirebaseRealtimeWPF.Views
{
    public partial class ProductView: Page
    {
        public ProductView()
        {
            InitializeComponent();
        }
    }
}