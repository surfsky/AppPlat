<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Titles.aspx.cs" Inherits="App.Admins.Titles" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="0px"  ShowBorder="false" Layout="Fit" ShowHeader="false" >
        <Items>
            <f:GridPro ID="Grid1" runat="server"
                AutoCreateFields="false" 
                ShowNumberField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="false"
                OnDelete="Grid1_Delete"
                RelayoutToolbar="false"
                WinHeight="300"
                >
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnClose_Click" runat="server" Text="关闭" Hidden="true" />
                            <f:ToolbarFill runat="server" />
                            <f:TwinTriggerBoxPro ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="搜索职称"   OnTriggerClick="ttbSearchMessage_TriggerClick" />
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Columns>
                    <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="职务名称" />
                    <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注" />
                </Columns>
            </f:GridPro>
        </Items>
    </f:Panel>
 
    </form>
</body>
</html>
