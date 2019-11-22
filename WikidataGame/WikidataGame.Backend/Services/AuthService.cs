using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Services
{
    public class AuthService
    {
        public AuthService(string firebaseCredentials)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(firebaseCredentials)
            });
        }

        public async Task<string> VerifyTokenAsync(string token)
        {
            try
            {
                var tokenResponse = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                return tokenResponse.Uid;
            }
            catch(FirebaseAuthException)
            {
                return string.Empty;
            }
        }
    }
}
