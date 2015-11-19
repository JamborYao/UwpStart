<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebNotifyBackend._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

 

    
       
            <h2>Push Notification</h2>
          <div>
              <asp:Label runat="server" Text="Channel URI:" Font-Bold="true"></asp:Label><br />
              <asp:TextBox  runat="server" ID="tbChannel" Width="800" ></asp:TextBox><br />
              <asp:Label runat="server" Text="Package Security Identifier (SID):" Font-Bold="true"></asp:Label><br />
              <asp:TextBox  runat="server" ID="tbSID" Width="800"></asp:TextBox><br />
              <asp:Label runat="server" Text="Client Secret:" Font-Bold="true"></asp:Label><br />
              <asp:TextBox  runat="server" ID="tbSecret" Width="800"></asp:TextBox><br />
                <asp:Label runat="server" Text="DIY Push:" Font-Bold="true"></asp:Label><br /> 
                <asp:TextBox  runat="server" ID="tbPush" Width="800" Height="300" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox><br />
                <asp:DropDownList runat="server" ID="dpType" OnSelectedIndexChanged="dpType_SelectedIndexChanged" AutoPostBack="true">
                  <asp:ListItem Value="0">toast</asp:ListItem>
                   <asp:ListItem Value="1">tile</asp:ListItem>
                   <asp:ListItem Value="2">raw</asp:ListItem>                
                   <asp:ListItem Value="4">badage</asp:ListItem>
              </asp:DropDownList> <br />
              <asp:Button runat="server" Text="Send Push" OnClick="Unnamed6_Click"/>
          </div>
      
       

</asp:Content>
