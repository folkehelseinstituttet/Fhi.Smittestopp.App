using AnonymousTokens.Core.Services;

using NDB.Covid19.Configuration;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NDB.Covid19.AnonymousTokens
{
    public class PublicKeyStore : IPublicKeyStore
    {
        public Task<ECPublicKeyParameters> GetAsync()
        {
            var certificate = new X509Certificate2(OAuthConf.OAUTH2_VERIFY_TOKEN_PUBLIC_KEY);

            var convertedCertificate = DotNetUtilities.FromX509Certificate(certificate);
            return Task.FromResult((ECPublicKeyParameters)convertedCertificate.GetPublicKey());
        }
    }
}
