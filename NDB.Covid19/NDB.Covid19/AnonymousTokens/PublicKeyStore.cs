using AnonymousTokens.Core.Services;

using NDB.Covid19.Configuration;

using Org.BouncyCastle.Crypto.Parameters;

using System.IO;
using System.Threading.Tasks;

namespace NDB.Covid19.AnonymousTokens
{
    public class PublicKeyStore : IPublicKeyStore
    {
        public Task<ECPublicKeyParameters> GetAsync()
        {
            return LoadAsECPublicKey();
        }

        private static Task<ECPublicKeyParameters> LoadAsECPublicKey()
        {
            using (var reader = new StringReader(OAuthConf.OAUTH2_VERIFY_TOKEN_PUBLIC_KEY))
            {
                var publicKey = (ECPublicKeyParameters)new Org.BouncyCastle.OpenSsl.PemReader(reader).ReadObject();

                return Task.FromResult(publicKey);
            }
        }
    }
}
