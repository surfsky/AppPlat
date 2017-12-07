<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMdEditor.aspx.cs" Inherits="App.Controls.MdEditors.TestMdEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:MdEditor runat="server" ID="edt" Text="hello world" Width="100%" Height="300" ScriptPath="~/res/js/editor.md/" />
        <asp:Button runat="server" ID="btnSet" Text="设置" OnClick="btnSet_Click"  />
        <asp:Button runat="server" ID="btnGet" Text="获取" OnClick="btnGet_Click"  />
        <asp:Label runat="server" ID="lblInfo" Text="" />
    </form>
</body>
</html>
