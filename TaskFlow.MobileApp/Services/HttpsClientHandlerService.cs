// TaskFlow.MobileApp/Services/HttpsClientHandlerService.cs

namespace TaskFlow.MobileApp.Services
{
    // This class provides a platform-specific HttpMessageHandler.
    // For Windows, in DEBUG mode, it will bypass certificate validation.
    public class HttpsClientHandlerService
    {
        public HttpMessageHandler GetPlatformMessageHandler()
        {
#if DEBUG && WINDOWS
            // Create a handler that ignores custom certificate errors
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    // If the certificate is for "localhost", trust it.
                    if (cert != null && cert.Issuer.Equals("CN=localhost"))
                        return true;
                    
                    // Otherwise, use the default validation.
                    return errors == System.Net.Security.SslPolicyErrors.None;
                }
            };
            return handler;
#else
            // For all other cases (Release mode, Android, iOS), use the default handler.
            return new HttpClientHandler();
#endif
        }
    }
}
