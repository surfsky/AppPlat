<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptUsers.aspx.cs" Inherits="App.Admin.DeptUsers" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>

                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" BodyPadding="0px" Width="200px" Position="Left" Layout="Fit" runat="server" Split="true">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" DataKeyNames="ID" 
                            AllowSorting="false" SortField="Name" SortDirection="DESC" AllowPaging="false" EnableMultiSelect="false" 
                            OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                            <Toolbars>
                                <f:Toolbar  runat="server">
                                    <Items>
                                        <f:Button runat="server" ID="btnEditDept" Text="编辑部门列表" OnClick="btnEditDept_Click" Icon="Pencil"/>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="Name" ExpandUnusedSpace="true" DataSimulateTreeLevelField="TreeLevel" HeaderText="部门名称"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>

                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="0px" runat="server">
                    <Items>
                        <f:GridPro ID="Grid2" runat="server"
                            AutoCreateFields="false" 
                            ShowNumberField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="false"
                            OnDelete="Grid2_Delete" RelayoutToolbar="false"
                            >
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnNewUser" runat="server" Icon="Add" EnablePostBack="true" OnClick="btnNewUser_Click" Text="添加" />
                                        <f:ToolbarFill runat="server" />
                                        <f:TwinTriggerBoxPro ID="ttbSearchUser" runat="server" ShowLabel="false" EmptyText="搜索用户" OnTriggerClick="ttbSearchUser_TriggerClick" />
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="用户名" />
                                <f:BoundField DataField="RealName" SortField="RealName" Width="100px" HeaderText="姓名" />
                                <f:CheckBoxField DataField="Enabled" SortField="Enabled" HeaderText="启用" RenderAsStaticField="true" Width="50px" />
                                <f:BoundField DataField="Gender" SortField="Gender" Width="50px" HeaderText="性别" />
                                <f:BoundField DataField="Email" SortField="Email" Width="150px" HeaderText="邮箱" />
                                <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注" />
                            </Columns>
                        </f:GridPro>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>

        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true" 
            EnableIFrame="true" IFrameUrl="about:blank" Width="650px" Height="450px" OnClose="Window1_Close"
            />
    </form>
</body>
</html>
