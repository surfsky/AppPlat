<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profiles.aspx.cs" Inherits="App.Admins.Profiles" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowHeader="false"  ShowBorder="false" Layout="Fit" BodyPadding="0px" runat="server" AutoScroll="true">
        <Items>
            <f:SimpleForm ID="SimpleForm1" runat="server" LabelWidth="100px" BodyPadding="5px"
                LabelAlign="Left"  ShowBorder="false" ShowHeader="false"
                >
                <Toolbars>
                    <f:Toolbar runat="server">
                        <Items>
                            <f:Button ID="btnSave" runat="server" Icon="SystemSave" OnClick="btnSave_OnClick"
                                ValidateForms="SimpleForm1" ValidateTarget="Top" Text="修改"
                                />
                        </Items>
                    </f:Toolbar>
                </Toolbars>

                <Items>
                    <f:TextBox ID="tbxOldPassword" TextMode="Password" runat="server" Label="当前密码" Required="true" ShowRedStar="true" />
                    <f:TextBox ID="tbxNewPassword" TextMode="Password" runat="server" Label="新密码" Required="true" MinLength="6" ShowRedStar="true" />
                    <f:TextBox ID="tbxConfirmNewPassword" TextMode="Password" runat="server" Label="确认新密码" Required="true" ShowRedStar="true" />
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
