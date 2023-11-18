using Microsoft.Identity.Client;
using Microsoft.Rest;

namespace empower_assessment_app
{
    internal class AzureConnector
    {
        private readonly string _applicationId;
        private readonly string _authenticationKey;
        private readonly string _tenantId;

        public AzureConnector(string applicationId, string authenticationKey, string tenantId)
        {
            _applicationId = applicationId;
            _authenticationKey = authenticationKey;
            _tenantId = tenantId;
        }
        public ServiceClientCredentials GetCredentials()
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
           .Create(_applicationId)
           .WithAuthority($"https://login.microsoftonline.com/{_tenantId}")
           .WithClientSecret(_authenticationKey)
           .WithLegacyCacheCompatibility(false)
           .WithCacheOptions(CacheOptions.EnableSharedCacheOptions)
           .Build();

                AuthenticationResult result = app.AcquireTokenForClient(new string[] { "https://management.azure.com//.default" })
                                              .ExecuteAsync()
                                              .Result;
                return new TokenCredentials(result.AccessToken);
        }
    }
}
