using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SendNotification
{
    public class WNSNotification
    {
        public const string packageSID = "ms-app://s-1-15-2-402962206-181889125-2784327023-3017467611-124665055-2608589449-1588084589";
        public const string clientSecret = "lw6IVYDiA7R3ZJuJCyw8AmljgStCFSMi";
        public const string scope = "notify.windows.com";
        public const string grant_type = "client_credentials";

        public string GetTokenFromWNS()
        {
            string url = "https://login.live.com/accesstoken.srf";
            string data = string.Format("grant_type={0}&client_id={1}&client_secret={2}&scope={3}",
                grant_type,
               HttpUtility.UrlEncode(packageSID),
               clientSecret,
               scope);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
              
               
                stream.Write(bytes, 0, bytes.Length);
            }
            string result = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream);
                    result = reader.ReadToEnd();

                }
            }
            

            return GetOAuthTokenFromJson(result).AccessToken;

        }
        private OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        public void SendNotifytoWNS(string content,string NotififyType,string url)
        {
           
            //try
            //{
                var accessToken = "EgAdAQMAAAAEgAAAC4AAE8FBsxWhRUmFugYgVuldsgabQja9FbsUQ7uJRZouuZN1aXWMFif+/md58HTGlzcrKzdTHsn8rTyguNT+r4cU3hS7dTOmBrTZycvOlNoeOF28+NLSnwzQ2JBk6NoXAS0wsomYqETHrhf1e7S5j9w+RYnKWsBf48vNdkwkfdg0qpKMAFoAjAAAAAAAXwIXSHkcTFZ5HExW60gEABAAMTY3LjIyMC4yMzIuMTY5AAAAAABcAG1zLWFwcDovL3MtMS0xNS0yLTQwMjk2MjIwNi0xODE4ODkxMjUtMjc4NDMyNzAyMy0zMDE3NDY3NjExLTEyNDY2NTA1NS0yNjA4NTg5NDQ5LTE1ODgwODQ1ODkA";
                accessToken = GetTokenFromWNS();
                var toast = string.Format(@"<toast><visual><binding template=""ToastText01""><text id=""1"">{0}</text></binding></visual></toast>", "hello world!!!");
              
          
           
        byte[] contentInBytes = Encoding.UTF8.GetBytes(content);
         

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentLength = contentInBytes.Length;
            switch (NotififyType)
            {
                case "toast":
                    request.Headers.Add("X-WNS-Type", "wns/toast");
                    request.ContentType = "text/xml";
                    break;
                case "tile":
                    request.Headers.Add("X-WNS-Type", "wns/tile");
                    request.ContentType = "text/xml";
                    break;
                case "raw":
                    request.Headers.Add("X-WNS-Type", "wns/raw");
                    request.ContentType = "application/octet-stream";
                    //application/octet-stream
                    //payload smaller than 5 KB in size.
                    break;
                case "badge":
                    request.Headers.Add("X-WNS-Type", "wns/badge");
                    break;
                    
            }
               
                //request.Headers.Add("X-WNS-Tag","msdn");
             
                request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));

                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(contentInBytes, 0, contentInBytes.Length);

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    string state = webResponse.StatusCode.ToString();
                    StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                    string result = reader.ReadToEnd();
                    string staus = webResponse.Headers["X-WNS-DeviceConnectionStatus"];
                }
            //}
            //catch (WebException webException)
            //{
            //    string[] debugOutput = {                                      
            //                           webException.Response.Headers["X-WNS-Debug-Trace"],
            //                           webException.Response.Headers["X-WNS-Error-Description"],
            //                           webException.Response.Headers["X-WNS-Msg-ID"],
            //                           webException.Response.Headers["X-WNS-Status"]
            //                       };

            //}
        }
        }
    [DataContract]
    public class OAuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }
}
