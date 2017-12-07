<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TitleForm.aspx.cs"  Inherits="App.Admins.TitleForm" %>

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
                        <f:TextBox ID="tbxName" runat="server" Label="名称" Required="true" ShowRedStar="true" />
                        <f:TextArea ID="tbxRemark" runat="server" Label="备注" />
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
