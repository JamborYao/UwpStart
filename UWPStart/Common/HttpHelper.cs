using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace UWPStart.Common
{
    public class HttpHelper
    {
        public static async Task<string> MyHttpGet()
        {
            string url = "http://10.168.172.243:8080/ThreadDetailManagerService.svc/getThreadsByFilter2?startTime=08/30/2015&endTime=09/29/2015&owner=none&status=-1";
           
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.UseDefaultCredentials = false;
            //request.Credentials = CredentialCache.DefaultNetworkCredentials;
            //request.Credentials = new NetworkCredential("v-jayao", "Change!13", "fareast.corp.microsoft.com");

            var response = request.GetResponseAsync();
            await response;
            Stream stream = response.Result.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            return content;
           // httpContent.Text = content;
            //outPutView.NavigateToString(content);
        }

        public  static async Task<string> HttpClientGetThreads()
        {
            string url = "http://10.168.172.243:8080/ThreadDetailManagerService.svc/getThreadsByFilter2?startTime=08/30/2015&endTime=09/29/2015&owner=none&status=-1";
            HttpClient client = new HttpClient();
            HttpResponseMessage httpResponse= await client.GetAsync(new Uri(url));
            httpResponse.EnsureSuccessStatusCode();
            string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            return httpResponseBody;

        }
    }
}
