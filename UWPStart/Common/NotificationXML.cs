using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPStart.Common
{
    public static class NotificationXML
    {
        public static string  test =
               "<toast>"
               + "  <visual>"
               + "<binding template='ToastGeneric'>"
               + "  <text>Photo Share</text>"
               + "      <text>Andrew sent you a picture</text>"
               + "      <text>See it in full size!</text>"
               + "      <image placement='appLogoOverride' src='A.png' />"
               + "  <image placement='inline' src='StoreLogo.png' />"
               + "    </binding>"
               + "  </visual>"
               + "</toast>";

        public static string ToastWithActionXML =
                "<toast>"
                + "  <visual>"
                + "    <binding template='ToastGeneric'>"
                + "      <text>Sample</text>"
                + "      <text>This is a simple toast notification example</text>"
                + "      <image placement='AppLogoOverride' src='StoreLogo.png' />"
                + "    </binding>"
                + "  </visual>"
                + "  <actions>"
                + "<input id = 'message' type = 'text' placeholderContent = 'reply here' />"
                + "    <action content='check' arguments='check' imageUri='StoreLogo.png' activationType='background'/>"
                + "    <action content='cancel' arguments='cancel' />"
                + "  </actions>"
                       //  + "  <audio src='ms - winsoundevent:Notification.Reminder'/>"
                + "</toast>";
        public static string offtopic_ToastActionXML =
           "<toast>"
           + "  <visual>"
           + "    <binding template='ToastGeneric'>"
           + "      <text>{0}</text>"
           + "      <text>{1}</text>"
           + "      <image placement='AppLogoOverride' src='StoreLogo.png' />"
           + "    </binding>"
           + "  </visual>"
           + "  <actions>"
           + "<input id = 'message' type = 'text' placeholderContent = 'remark:' />"
           + "    <action content='Mark as Offtopic' arguments='offtopic'  activationType='background'/>" //background  foreground
           + "    <action content='Ignore' arguments='ignore' />"
           + "  </actions>"         
           + "</toast>";
        public static string ToastInternetXML =
               "<toast>"
               + "  <visual>"
               + "    <binding template='ToastGeneric'>"
               + "      <text>{0}</text>"
               + "      <text>{1}</text>"              
               + "    </binding>"
               + "  </visual>"   
               + "</toast>";

        public static string TileXml =
              "<tile>"
              + "<visual version='3'>"
              + "<binding template='TileSquare150x150Text04' fallback='TileSquareText04'>"
              + "<text id='1'>{0}</text>"
              + "</binding>"
              + "</visual>"
              + "</tile>";
    }
}
