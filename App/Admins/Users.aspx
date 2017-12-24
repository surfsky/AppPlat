<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="App.Admins.Users" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="0px"  
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="用户管理">
            <Items>


                <f:GridPro ID="Grid1" runat="server" 
                    AutoCreateFields="false"
                    ShowNumberField="true"  ShowEditField="true" ShowDeleteField="true" ShowViewField="true"
                    RelayoutToolbar="false"
                    OnDelete="Grid1_Delete"
                    WinHeight="700"
                    OnPreRowDataBound="Grid1_PreRowDataBound"
                    >

                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <f:DropDownList runat="server" ID="ddlDept" EmptyText="部门" />
                                <f:DropDownList runat="server" ID="ddlTitle" EmptyText="职称" Width="100" />
                                <f:DropDownList runat="server" ID="ddlRole" EmptyText="角色" Width="100" />
                                <f:DropDownList runat="server" ID="ddlStatus" EmptyText="用户状态" Width="100" />
                                <f:TextBox runat="server" ID="tbName" EmptyText="用户名" Width="100" />
                                <f:TextBox runat="server" ID="tbMobile" EmptyText="手机号" Width="100" />
                                <f:Button runat="server" ID="btnSearch" Text="查找" OnClick="btnSearch_Click"  Type="Submit" />
                            </Items>
                        </f:Toolbar>
                    </Toolbars>

                    <Columns>
                        <f:WindowField ColumnID="changePasswordField" TextAlign="Center" Icon="Key" ToolTip="修改密码"
                            WindowID="Window1" Title="修改密码" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/admins/UserChangePwd.aspx?id={0}"
                            Width="30px"  />
                        <f:ImageField DataImageUrlField="Photo" HeaderText="照片" Hidden="false" ImageHeight="30" ImageWidth="30" MinWidth="30" />
                        <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="用户名" />
                        <f:BoundField DataField="NickName" SortField="NickName" Width="100px" HeaderText="昵称" />
                        <f:BoundField DataField="RealName" SortField="RealName" Width="100px" HeaderText="真实姓名" />
                        <f:CheckBoxField DataField="Enabled" SortField="Enabled" HeaderText="启用" RenderAsStaticField="true"  Width="50px" />
                        <f:BoundField DataField="Name" Width="200px" HeaderText="角色" Hidden="false"  ColumnID="Roles" />
                        <f:BoundField DataField="Gender" SortField="Gender" Width="50px" HeaderText="性别" />
                        <f:BoundField DataField="Mobile" SortField="Mobile" Width="150px" HeaderText="移动电话" />

                        <f:BoundField DataField="Dept.Name"  Width="100px" HeaderText="部门" Hidden="true" />
                        <f:BoundField DataField="Name" Width="100px" HeaderText="职务" ColumnID="Titles" Hidden="true"  />
                        <f:BoundField DataField="Email" SortField="Email" Width="150px" HeaderText="邮箱" Hidden="true" />
                        <f:BoundField DataField="Phone" SortField="Phone" Width="150px" HeaderText="办公电话" Hidden="true" />
                        <f:BoundField DataField="IdentityCard" SortField="IdentityCard" Width="150px" HeaderText="身份证" Hidden="true" />
                        <f:BoundField DataField="Birthday" SortField="Birthday" Width="150px" HeaderText="生日" Hidden="true" />
                        <f:BoundField DataField="TakeOfficeDt" SortField="TakeOfficeDt" Width="150px" HeaderText="就职日期" Hidden="true" />
                        <f:BoundField DataField="LastLoginDt" SortField="LastLoginDt" Width="150px" HeaderText="最后登录日期" Hidden="true" />
                        <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注" Hidden="true" />
                    </Columns>
                </f:GridPro>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
