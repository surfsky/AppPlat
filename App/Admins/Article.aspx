<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Article.aspx.cs"  Inherits="App.Admins.Article" ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1><asp:Literal runat="server" ID="lblTitle" /></h1>
        <p>
            <small><asp:Literal runat="server" ID="lblAuthor" /></small>
            <small><asp:Literal runat="server" ID="lblPostDt" /></small>
            <small><asp:Literal runat="server" ID="lblVisitCnt" /></small>
        </p>
        <div>
            <asp:Literal runat="server" ID="lblContent" />
        </div>

        <asp:Repeater runat="server" ID="rptImage" >
            <ItemTemplate>
                <img src="<%#Eval("Url") %>" />
                <br />
                <div class="img-desc"> <%#Eval("Description") %> </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
