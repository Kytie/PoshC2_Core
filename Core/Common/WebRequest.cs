using System;
using System.Net;
using System.Collections.Generic;

namespace Core.Common
{
    internal static class WebRequest
    {
        internal static WebClient Curl(string url = null, string df = null, string purl = null, string proxyUser = null, string proxyPassword = null,
            string username = null, string password = null, bool digestAuth = false, 
            string useragent = null, string cookie = null, Dictionary<string,string> headers = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType) 192 | (SecurityProtocolType) 768 | (SecurityProtocolType) 3072;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var webClientObject = new WebClient();
            webClientObject.UseDefaultCredentials = true;
            webClientObject.Proxy.Credentials = CredentialCache.DefaultCredentials;

            if (!string.IsNullOrEmpty(purl))
            {
                var proxy = new WebProxy {Address = new Uri(purl), Credentials = new NetworkCredential(proxyUser, proxyPassword)};
                if (string.IsNullOrEmpty(proxyUser))
                {
                    proxy.UseDefaultCredentials = true;
                }

                proxy.BypassProxyOnLocal = false;
                webClientObject.Proxy = proxy;
            }

            if (!string.IsNullOrEmpty(username))
            {
                webClientObject.UseDefaultCredentials = false;
                var credentialCache = new CredentialCache();
                var authMethod = "Basic";
                if (digestAuth)
                {
                    authMethod = "Digest";
                }
                var uri = new Uri(url);
                credentialCache.Add(
                  new Uri(uri.GetLeftPart(UriPartial.Path)),
                  authMethod,
                  new NetworkCredential(username, password)
                );
                webClientObject.Credentials = credentialCache;
            }

            if (!string.IsNullOrEmpty(df))
            {
                webClientObject.Headers.Add("Host", df);
            }
            if (string.IsNullOrEmpty(useragent))
            {
                useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.122 Safari/537.36";
            }
            webClientObject.Headers.Add("User-Agent", useragent);
            webClientObject.Headers.Add("Referer", "");
            webClientObject.Headers.Add("Cookie", cookie);
            if (headers != null )
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    webClientObject.Headers.Add(header.Key, header.Value);
                }
            }

            return webClientObject;
        }
    }
}