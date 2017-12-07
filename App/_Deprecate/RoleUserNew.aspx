<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleUserNew.aspx.cs" Inherits="App.Admin.RoleUserNew" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="0px"  Layout="Fit">
        <Items>
            <f:GridPro ID="Grid1" runat="server"  EnableCheckBoxSelect="true" AutoCreateFields="false">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server" Text="关闭" />
                            <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                            <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click" runat="server" Text="选择后关闭" />
                            <f:ToolbarFill runat="server" />
                            <f:TwinTriggerBoxPro ID="ttbSearchMessage" Width="160px" runat="server" ShowLabel="false"
                                EmptyText="搜索用户名" 
                                OnTriggerClick="ttbSearchMessage_TriggerClick" />
                        </Items>
                    </f:Toolbar>
                </Toolbars>

                <Columns>
                    <f:RowNumberField />
                    <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="用户名" />
                    <f:BoundField DataField="RealName" SortField="RealName" Width="100px" HeaderText="姓名" />
                    <f:CheckBoxField DataField="Enabled" SortField="Enabled" HeaderText="启用" RenderAsStaticField="true"  Width="50px" />
                    <f:BoundField DataField="Gender" SortField="Gender" Width="50px" HeaderText="性别" />
                    <f:BoundField DataField="Email" SortField="Email" Width="150px" HeaderText="邮箱" />
                    <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注" />
                </Columns>
            </f:GridPro>
        </Items>
    </f:Panel>

    </form>
</body>
</html>
