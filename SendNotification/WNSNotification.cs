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
        public const string packageSID = "ms-app://s-1-15-2-4222701300-3819617421-3546372303-955901872-4262490533-1071502919-2139900694";
                                         
        public const string clientSecret = "3FQOGWyWqDPuH3mVnp73RoWgiL5Ds6yQ";
                                            
        public const string scope = "notify.windows.com";
        public const string grant_type = "client_credentials";

        public void GetTokenFromWNS(out Dictionary<FlowSteps,string> steps)
        {
            steps = new Dictionary<FlowSteps, string>();
            string url = "https://login.live.com/accesstoken.srf";
            steps[FlowSteps.TokenUrl] = url;
            string data = string.Format("grant_type={0}&client_id={1}&client_secret={2}&scope={3}",
                grant_type,
               HttpUtility.UrlEncode(packageSID),
               clientSecret,
               scope);
            steps[FlowSteps.SID] = packageSID;
            steps[FlowSteps.ClientSecret] = clientSecret;
            //steps.Add("");
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

            steps[FlowSteps.Token]= GetOAuthTokenFromJson(result).AccessToken;
            //return GetOAuthTokenFromJson(result).AccessToken;
            // steps["token"]= GetOAuthTokenFromJson(result).AccessToken;

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
        
        public Dictionary<FlowSteps, string> SendNotifytoWNS(string content, string NotififyType, string url)
        {
            Dictionary<FlowSteps, string> steps;
           
            //try
            //{
            var accessToken = "EgAdAQMAAAAEgAAAC4AAE8FBsxWhRUmFugYgVuldsgabQja9FbsUQ7uJRZouuZN1aXWMFif+/md58HTGlzcrKzdTHsn8rTyguNT+r4cU3hS7dTOmBrTZycvOlNoeOF28+NLSnwzQ2JBk6NoXAS0wsomYqETHrhf1e7S5j9w+RYnKWsBf48vNdkwkfdg0qpKMAFoAjAAAAAAAXwIXSHkcTFZ5HExW60gEABAAMTY3LjIyMC4yMzIuMTY5AAAAAABcAG1zLWFwcDovL3MtMS0xNS0yLTQwMjk2MjIwNi0xODE4ODkxMjUtMjc4NDMyNzAyMy0zMDE3NDY3NjExLTEyNDY2NTA1NS0yNjA4NTg5NDQ5LTE1ODgwODQ1ODkA";
            GetTokenFromWNS(out steps);
            var toast = string.Format(@"<toast><visual><binding template=""ToastText01""><text id=""1"">{0}</text></binding></visual></toast>", "hello world!!!");
            steps[FlowSteps.RequestContent] = content;
            accessToken = steps[FlowSteps.Token];

            byte[] contentInBytes = Encoding.UTF8.GetBytes(content);


            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            steps[FlowSteps.RequestURL] = url;
            request.Method = "POST";
            request.Headers.Add("X-WNS-RequestForStatus","true");

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
                steps[FlowSteps.StateCode] = state+webResponse.Headers["X-WNS-Status"]+ "message ID"+webResponse.Headers["X-WNS-Msg-ID"]+"DeviceConnectionStatus"+staus;
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
            return steps;

        } }
    [DataContract]
    public class OAuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }
}
