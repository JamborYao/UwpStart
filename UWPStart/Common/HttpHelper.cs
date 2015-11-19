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

        public  static async Task<string> HttpClientGetThreads(string startDate,string endDate)
        {
            startDate = startDate??"08/30/2015";
            endDate = endDate??"01/01/2016";
            string url =string.Format( "http://10.168.172.243:8080/ThreadDetailManagerService.svc/getThreadsByFilter2?startTime={0}&endTime={1}&owner=none&status=-1",
                startDate,
                endDate);
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage httpResponse = await client.GetAsync(new Uri(url));
                httpResponse.EnsureSuccessStatusCode();
                string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                return httpResponseBody;
            }
            catch (Exception e)
            {
                return @"[{'Alias':'v-jayao','CSSAction':5,'CSSActionName':'Answered','CreateBy':null,'CreateTime':'\/Date(1442278654137+0800)\/','CustomerLookingFor':'','Description':'今天突然不论是部署在Azure的网站，还是本地的管理工具都无法访问SQL Azure了，有朋友碰到同样情况的么？\u000d\u000a\u000d\u000a更新：刚看到世纪互联的通告：\u000d\u000aNetwork Infrastructure功能在华北地区出现问题，已经确认受影响的服务...','Diffcult':'','EngineerTeamID':1,'EngnieerTeamName':'MSDN','FirstReplyTime':null,'ForumID':null,'ForumName':null,'IRT':'1545','Id':'6e475471-d3b0-4c0f-97f8-f120f011a176','IsChanged':null,'IsReplied':true,'IssueTypeID':5,'IssueTypeName':'Mooncake feature - troubleshooting','Labors':35,'LastReplyUser':'JamborYaoMSFT','OwnerID':2,'ReplyNum':2,'Status':'1','TeamID':1,'TeamName':'MSDN','TechCategoryID':3,'TechCategoryName':'SQL Azure','ThreadCreateTime':'\/Date(1442207595000+0800)\/','ThreadLink':'http:\/\/ask.csdn.net\/questions\/206346','ThreadTitle':'SQL Azure突然无法访问？','UT':null},{'Alias':'v-jayao','CSSAction':1,'CSSActionName':'Solution Delivered','CreateBy':null,'CreateTime':'\/Date(1441955026390+0800)\/','CustomerLookingFor':null,'Description':'第一次使用Azure，\u000a1. 创建了一个虚拟机然后在上面安装了nginx\u000a2. 配置nginx状态检查\u000a   server {\u000a        listen       80; \u000a        charset utf-8;\u000a\u000a        access_l...','Diffcult':null,'EngineerTeamID':1,'EngnieerTeamName':'MSDN','FirstReplyTime':null,'ForumID':null,'ForumName':null,'IRT':'622','Id':'04c95217-2456-401c-b570-7d62a3b2fdc4','IsChanged':null,'IsReplied':null,'IssueTypeID':null,'IssueTypeName':null,'Labors':65,'LastReplyUser':'JamborYaoMSFT','OwnerID':2,'ReplyNum':2,'Status':'1','TeamID':1,'TeamName':'MSDN','TechCategoryID':null,'TechCategoryName':null,'ThreadCreateTime':'\/Date(1441953338000+0800)\/','ThreadLink':'http:\/\/ask.csdn.net\/questions\/205803','ThreadTitle':'在虚拟机上安装nginx，无法通过http访问nginx的状态','UT':null},{'Alias':'v-jayao','CSSAction':9,'CSSActionName':'Off Topic','CreateBy':null,'CreateTime':'\/Date(1441767203917+0800)\/','CustomerLookingFor':null,'Description':'该如何是好，完全写不完整\u000aServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);\u000a            ExchangeService servic...','Diffcult':null,'EngineerTeamID':1,'EngnieerTeamName':'MSDN','FirstReplyTime':null,'ForumID':null,'ForumName':null,'IRT':'8240','Id':'cfe0efef-8f4d-47b4-9a80-88bf8012a798','IsChanged':null,'IsReplied':null,'IssueTypeID':null,'IssueTypeName':null,'Labors':null,'LastReplyUser':'devmiao','OwnerID':2,'ReplyNum':1,'Status':'1','TeamID':1,'TeamName':'MSDN','TechCategoryID':null,'TechCategoryName':null,'ThreadCreateTime':'\/Date(1441704700000+0800)\/','ThreadLink':'http:\/\/ask.csdn.net\/questions\/205101','ThreadTitle':'exchange manager API 写邮件','UT':null}]";
                return e.Message;
            }
        }
    }
}
