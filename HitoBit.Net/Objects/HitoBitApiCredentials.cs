using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using System.Security;

namespace HitoBit.Net.Objects
{
    /// <summary>
    /// HitoBit Api credentials
    /// </summary>
    public class HitoBitApiCredentials : ApiCredentials
    {
        /// <summary>
        /// Type of the credentials
        /// </summary>
        public ApiCredentialsType Type { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="key">The API key</param>
        /// <param name="secret">The API secret (Hmac type) or the RSA private key in either xml or pem format (Rsa type)</param>
        /// <param name="type">The type of authentication</param>
        public HitoBitApiCredentials(SecureString key, SecureString secret, ApiCredentialsType type = ApiCredentialsType.Hmac) : base(key, secret)
        {
            Type = type;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="key">The API key</param>
        /// <param name="secret">The API secret (Hmac type) or the RSA private key in either xml or pem format (Rsa type)</param>
        /// <param name="type">The type of authentication</param>
        public HitoBitApiCredentials(string key, string secret, ApiCredentialsType type = ApiCredentialsType.Hmac) : base(key, secret)
        {
            Type = type;
        }

        /// <inheritdoc />
        public override ApiCredentials Copy()
        { 
            return new HitoBitApiCredentials(Key!.GetString(), Secret!.GetString(), Type);
        }
    }

    /// <summary>
    /// Credentials type
    /// </summary>
    public enum ApiCredentialsType
    {
        /// <summary>
        /// Hmac keys credentials
        /// </summary>
        Hmac,
        /// <summary>
        /// Rsa keys credentials in xml format
        /// </summary>
        RsaXml,
        /// <summary>
        /// Rsa keys credentials in pem/base64 format. Only available for .NetStandard 2.1 and up, use xml format for lower.
        /// </summary>
        RsaPem
    }
}
