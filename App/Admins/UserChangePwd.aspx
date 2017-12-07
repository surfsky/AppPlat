<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserChangePwd.aspx.cs"  Inherits="App.Admins.UserChangePwd" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  AutoScroll="true" runat="server">

            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server" Text="关闭" />
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose"
                            OnClick="btnSaveClose_Click" runat="server" Text="保存后关闭" />
                    </Items>
                </f:Toolbar>
            </Toolbars>

            <Items>
                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"  Title="SimpleForm">
                    <Items>
                        <f:Label ID="labUserName" Label="用户名" runat="server" />
                        <f:Label ID="labUserRealName" Label="姓名" runat="server" />
                        <f:TextBox ID="tbxPassword" runat="server" Label="新密码" Required="true" ShowRedStar="true" TextMode="Password" />
                        <f:TextBox ID="tbxConfirmPassword" runat="server" Label="确认密码" Required="true"
                            ShowRedStar="true" TextMode="Password" CompareControl="tbxPassword" CompareOperator="Equal" />
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
