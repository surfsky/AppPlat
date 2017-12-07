<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CoachForm.aspx.cs"  Inherits="App.Fits.CoachForm" ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>教练（未完成）</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  AutoScroll="true" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button runat="server" ID="btnSave" Text="保存" OnClick="btnSave_Click" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"  Title="SimpleForm">
                    <Items>
                        <f:TextBox ID="tbClasses" runat="server" Label="提供的课程"/>
                        <f:NumberBox ID="tbPositiveCnt" runat="server" Label="好评数"/>
                        <f:NumberBox ID="tbMiddleCnt" runat="server" Label="中评数"/>
                        <f:NumberBox ID="tbNegativeCnt" runat="server" Label="差评数"/>
                        <f:NumberBox ID="tbPositiveRate" runat="server" Label="好评率"/>
                        <f:NumberBox ID="tbTeachPrice" runat="server" Label="私教费用（每节）"/>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
