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

        /// <summary>
        /// Loads an Elliptical Curve public key (in PEM-format) and returns the public key parameters.
        /// </summary>
        /// <returns>EC public key parameters</returns>
        private static Task<ECPublicKeyParameters> LoadAsECPublicKey()
        {
            using (var reader = new StringReader(AnonymousTokensConfig.ANONYMOUS_TOKENS_PUBLIC_KEY))
            {
                var publicKey = (ECPublicKeyParameters)new Org.BouncyCastle.OpenSsl.PemReader(reader).ReadObject();

                return Task.FromResult(publicKey);
            }
        }
    }
}
