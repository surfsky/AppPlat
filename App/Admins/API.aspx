<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="API.aspx.cs" Inherits="App.Admins.API" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="0px"  ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" >
                    <Columns>
                        <f:RowNumberField />
                        <f:BoundField DataField="Name" Width="200" HeaderText="名称" />
                        <f:BoundField DataField="Description"  Width="200" HeaderText="描述" />
                        <f:HyperLinkField DataNavigateUrlFields="URL" DataTextField="URL"  Target="_blank" HeaderText="URL" ExpandUnusedSpace="true" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
