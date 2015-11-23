<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebNotifyBackend._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">





    <h2>Push Notification</h2>
    <div>
        <asp:Label runat="server" Text="Channel URI:" Font-Bold="true"></asp:Label><br />
        <asp:TextBox runat="server" ID="tbChannel" Width="800"></asp:TextBox><br />
        <asp:Label runat="server" Text="Package Security Identifier (SID):" Font-Bold="true"></asp:Label><br />
        <asp:TextBox runat="server" ID="tbSID" Width="800"></asp:TextBox><br />
        <asp:Label runat="server" Text="Client Secret:" Font-Bold="true"></asp:Label><br />
        <asp:TextBox runat="server" ID="tbSecret" Width="800"></asp:TextBox><br />
        <asp:Label runat="server" Text="DIY Push:" Font-Bold="true"></asp:Label><br />
        <asp:TextBox runat="server" ID="tbPush" Width="800" Height="300" TextMode="MultiLine" ValidateRequestMode="Disabled"></asp:TextBox><br />
        <asp:DropDownList runat="server" ID="dpType" OnSelectedIndexChanged="dpType_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Value="0">toast</asp:ListItem>
            <asp:ListItem Value="1">tile</asp:ListItem>
            <asp:ListItem Value="2">raw</asp:ListItem>
            <asp:ListItem Value="4">badage</asp:ListItem>
        </asp:DropDownList>
        <br />
        <asp:Button runat="server" Text="Send Push" OnClick="Unnamed6_Click" />
    </div>
    <div style="background: azure; width: 800px; height: 400px">
        <div style="background: #4cff00; width: 200px; height: 200px; margin: 10px; float: left; text-align: center;">
            <span style="color: white; line-height: 200px;">Cloud Service</span></div>

        <div style="background: #00c1ff; width: 200px; height: 200px; margin: 10px; float: right; text-align: center;">
            <span style="color: white; line-height: 200px;">WNS</span></div>
        <img src="arrow01.PNG" id="arrowRight" width="300" /><img src="arrow02.PNG" id="arrowLeft" width="300" />
        <table>
            <tr>
                <td style="word-wrap: break-word; word-break: break-all;">
                    <asp:Label runat="server" ID="lbSteps" ValidateRequestMode="Disabled" Width="300" Text="ysa9GiBQaU/zNQWdFHyFd6hmmf4J3+NqiB499eyfKhWGzrdtS2VuLe/dhehtsxTjWnShA6fSJ1N9ERLTaHhHKBA3GN51wujKYT5VsfUWKP6dNkWNWDKKEOf5ay97VwGHJ/Q7+9/kdpTBMcdr/qVSPgFVmMAFoAjAAAAAAAXwIXSPbKTlb2yk5W60gEABAAMTY3LjIyMC4yMzIuMTY5AAAAAABcAG1zLWFwcDovL3MtMS0xNS0yLTQwMjk2MjIwNi0xODE4ODkxMjUtMjc4NDMyNzAyMy0zMDE3NDY3NjExLTEyNDY2NTA1NS0yNjA4NTg5NDQ5LTE1ODgwODQ1ODkA"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <input id="btStep" onclick="JsStep()" value="Next Step" type="button" />
        <input id="btBack" onclick="JsBack()" value="Back" type="button" />
        <%--<asp:Button runat="server" OnClick="Step_click" Text="Steps" />--%></div>
    <asp:HiddenField ID="stepsValue" runat="server" ValidateRequestMode="Disabled" Value="test" />
    <script type="text/javascript">
        var flag = 0;
        $(function () {
            $('#arrowRight').hide();
            $('#arrowLeft').hide();
            $('span[id*=lbSteps]').html('');
        });
        function JsStep() {

            var result = $('input[name*=stepsValue]').val();
            var stepLabel = $('span[id*=lbSteps]');
          
            var steps = $.parseJSON(result);
            ShowUI(steps);
          
        }
        function JsBack()
        {
            var result = $('input[name*=stepsValue]').val();
            var stepLabel = $('span[id*=lbSteps]');

            var steps = $.parseJSON(result);
            flag = flag - 2;
            if (flag < 0) { flag = 0; return; }
            ShowUI(steps);
        }
        function ShowArrow(arrow)
        {
            $('#arrowRight').hide();
            $('#arrowLeft').hide();
            $('#'+arrow).show();
        }
        function ShowUI(steps)
        {
            var stepLabel = $('span[id*=lbSteps]');
            switch (flag) {
                case 0:
                    stepLabel.html('');
                    stepLabel.html('WNS get token service:' + steps.TokenUrl);
                    ShowArrow('arrowRight');
                    flag++;
                    break;
                case 1:
                    stepLabel.html('');
                    stepLabel.html('SID:' + steps.SID);
                    ShowArrow('arrowRight');
                    flag++;
                    break;
                case 2:
                    stepLabel.html('');
                    stepLabel.html('Client Secret:' + steps.ClientSecret);
                    ShowArrow('arrowRight');
                    flag++;
                    break;
                case 3:
                    stepLabel.html('');
                    stepLabel.html('token:' + steps.Token);
                    ShowArrow('arrowLeft');
                    flag++;
                    break;
                case 4:
                    stepLabel.html('');
                    stepLabel.html('channel url:' + steps.RequestURL);
                    ShowArrow('arrowRight');
                    flag++;
                    break;
                case 5:
                    stepLabel.html('');
                    stepLabel.text('content:' + steps.RequestContent);
                    ShowArrow('arrowRight');
                    flag++;
                    break;
                case 6:
                    stepLabel.html('');
                    stepLabel.html('state code' + steps.StateCode);
                    ShowArrow('arrowLeft');
                    flag = 0;
                    break;

            }
        }
    </script>

</asp:Content>
