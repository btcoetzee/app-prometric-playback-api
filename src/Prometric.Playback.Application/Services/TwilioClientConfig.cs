using Microsoft.Extensions.Configuration;
using Twilio;

namespace ProProctor.Conferences.Infrastructure.Services
{
    public class TwilioClientConfig
    {
        public static string AccountSID { get; set; }

        public static string AuthToken { get; set; }

        public static void InitTwilioClient(IConfiguration configuration)
        {
            var twilio = configuration.GetSection("twilio");
            AccountSID = twilio["AccountSID"];
            AuthToken = twilio["AuthToken"];

            if (hasValue(AccountSID) && hasValue(AuthToken))
                TwilioClient.Init(AccountSID, AuthToken);
        }

        private static bool hasValue(string s)
            => !string.IsNullOrEmpty(s);
    }
}
