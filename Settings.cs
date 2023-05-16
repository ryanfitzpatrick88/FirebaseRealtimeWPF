using System;
using System.Xml.Linq;

namespace FirebaseRealtimeWPF
{
    public class Settings
    {
        private string apikey;
        private string authDomain;
        private string email;
        private string password;
        private string baseUrl;
        public Settings()
        {
            
        }

        public string ApiKey
        {
            get { return apikey;  }
            set { apikey = value; }
        }
        
        public string AuthDomain
        {
            get { return authDomain;  }
            set { authDomain = value; }
        }
        
        public string Email
        {
            get { return email;  }
            set { email = value; }
        }
        
        public string Password
        {
            get { return password;  }
            set { password = value; }
        }
        
        public string BaseUrl
        {
            get { return baseUrl;  }
            set { baseUrl = value; }
        }

        public void LoadSecrets()
        {
            // Load the XML file into an XDocument object
            XDocument doc = XDocument.Load("secrets.xml");

            // Query the data as needed. For example, to get all elements with a certain tag:
            foreach (XElement element in doc.Descendants("configuration"))
            {
                foreach (XElement element2 in element.Descendants("secrets"))
                {
                    foreach (XElement element3 in element2.Descendants("secret"))
                    {
                        //Console.WriteLine(element3.FirstAttribute.Value + ' ' + element3.LastAttribute.Value);
                        switch (element3.FirstAttribute.Value)
                        {
                            case "apikey":
                                ApiKey = element3.LastAttribute.Value;
                                break;
                            case "authDomain":
                                AuthDomain = element3.LastAttribute.Value;
                                break;
                            case "email":
                                Email = element3.LastAttribute.Value;
                                break;
                            case "password":
                                Password = element3.LastAttribute.Value;
                                break;
                            case "baseUrl":
                                BaseUrl = element3.LastAttribute.Value;
                                break;
                        }
                    }
                }
            }
        }
    }
}