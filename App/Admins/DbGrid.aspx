<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DbGrid.aspx.cs" Inherits="App.Admins.DbGrid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="0px"  ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>

                <f:GridPro ID="Grid1" runat="server"
                    AutoCreateFields="true" RelayoutToolbar="false"
                    ShowNumberField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="true"
                    OnDelete="Grid1_Delete"
                    OnWindowClose="Grid1_WindowClose"
                    WinHeight="300"
                    >
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnClose"  Icon="SystemSaveClose" OnClick="btnClose_Click" runat="server" Text="关闭" Hidden="true" />
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:GridPro>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
