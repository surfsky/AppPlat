<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogForm.aspx.cs" Inherits="App.Admins.LogForm"  %>


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
                <f:SimpleFormPro ID="SimpleForm1" runat="server" ShowCloseButton="true" />
            </Items>
        </f:Panel>
    </form>
</body>
</html>
