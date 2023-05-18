using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reactive.Subjects;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;

namespace FirebaseRealtimeWPF
{
    public class FirebaseService
    {
        private Settings settings;
        private Firebase.Auth.UserCredential userCredential;
        private string token;
        
        
        public FirebaseService()
        {
            this.settings = new Settings();
            this.settings.LoadSecrets();
        }
        
        
        public FirebaseClient GetFirebaseClient(FirebaseAuthClient firebaseClient, Subject<string> tokenRefreshes)
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

        public FirebaseAuthClient GetFirebaseAuthClient()
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
    }
}