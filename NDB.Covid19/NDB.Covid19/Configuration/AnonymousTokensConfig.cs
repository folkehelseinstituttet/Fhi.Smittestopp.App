namespace NDB.Covid19.Configuration
{
    public class AnonymousTokensConfig
    {
        /// <summary>
        /// The public key used by the Anonymous Tokens protocol in PEM-format.
        /// 
        /// Example:
        /// -----BEGIN PUBLIC KEY-----
        /// MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAESwcNFtWPA+ea0dKLy8qu94az50x9
        /// 3FO39ogmOgWLhpjKc3wvaMXOpHzJq6BR3hIniaqCJ8UKdw0Kd42RBnYghg==
        /// -----END PUBLIC KEY-----
        /// </summary>
        public static string ANONYMOUS_TOKENS_PUBLIC_KEY = "INJECTED_IN_APP_CENTER_DURING_BUILD";
    }
}
