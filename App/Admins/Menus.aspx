<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menus.aspx.cs" Inherits="App.Admins.Menus" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="0px" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <f:GridPro ID="Grid1" runat="server"
                    AutoCreateFields="false" 
                    ShowNumberField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="true"
                    OnDelete="Grid1_Delete"
                    WinHeight="600" WinCloseAction="HidePostBack"
                    >
                    <Columns>
                        <f:ImageField DataImageUrlField="ImageUrl" Width="30px" />
                        <f:BoundField DataField="Name" HeaderText="菜单标题" DataSimulateTreeLevelField="TreeLevel" Width="250px" />
                        <f:BoundField DataField="NavigateUrl" HeaderText="链接" Width="250px" />
                        <f:BoundField DataField="ViewPower" HeaderText="浏览权限" Width="120px" />
                        <f:BoundField DataField="IsOpen" HeaderText="展开" Width="60px" />
                        <f:BoundField DataField="Visible" HeaderText="可见" Width="60px" />
                        <f:BoundField DataField="Seq" HeaderText="排序" Width="60px" />
                        <f:BoundField DataField="Remark" HeaderText="备注" ExpandUnusedSpace="true" />
                    </Columns>
                </f:GridPro>

            </Items>
        </f:Panel>
    </form>
</body>
</html>
