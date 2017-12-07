<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserNew.aspx.cs"  Inherits="App.Fits.UserNew" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  AutoScroll="true" runat="server">
            <Items>
                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"  Title="SimpleForm">
                    <Items>
                        <f:DropDownList ID="ddlUser" runat="server"  Label="用户" />
                        <f:DropDownList ID="ddlCard" runat="server" Label="卡" />
                        <f:DropDownList ID="ddlCoach" runat="server" Label="绑定教练" />
                        <f:RadioButtonList ID="rblSts" runat="server" Label="状态" />
                        <f:DatePicker ID="dpBuyDt" runat="server" Label="购买日期" />
                        <f:DatePicker ID="dpStartDt" runat="server" Label="启用日期" />
                        <f:DatePicker ID="dpExpireDt" runat="server" Label="过期日期" />
                        <f:NumberBox ID="tbTotalCnt" runat="server" Label="次卡总次数"/>
                        <f:NumberBox ID="tbUsedCnt" runat="server" Label="次卡使用次数"/>
                        <f:Label ID="tbLeftCnt" runat="server" Label="次卡剩余次数"/>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
