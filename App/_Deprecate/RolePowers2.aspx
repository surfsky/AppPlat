<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RolePowers2.aspx.cs" Inherits="App.Admins.RolePowers2" %>

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

                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Width="200px" Position="Left" Layout="Fit" BodyPadding="0px" runat="server" Split="true">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" DataKeyNames="ID" 
                            AllowSorting="false" SortField="Name" SortDirection="ASC" 
                            AllowPaging="false" EnableMultiSelect="false" 
                            OnRowClick="Grid1_RowClick" EnableRowClickEvent="true"
                            EnableRowSelectEvent="false"
                            >
                            <Toolbars>
                                <f:Toolbar  runat="server">
                                    <Items>
                                        <f:Button runat="server" ID="btnEditRole" Text="编辑角色列表" OnClick="btnEditRole_Click" Icon="Pencil" />
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="Name" SortField="Name" ExpandUnusedSpace="true" HeaderText="角色名称"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>


                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit" BodyPadding="0px" runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" ShowBorder="true" ShowHeader="false" EnableMultiSelect="true" EnableCheckBoxSelect="false" DataKeyNames="ModuleId,ModuleName" 
                            AllowSorting="false"  AllowPaging="false" OnRowDataBound="Grid2_RowDataBound" SortField="GroupName" SortDirection="DESC">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnSelectAll" EnablePostBack="true" runat="server" Text="全选" Icon="Accept" Hidden="false" OnClick="btnSelectAll_Click" />
                                        <f:Button ID="btnUnSelectAll" EnablePostBack="true" runat="server" Text="全不选" Icon="Cross"  Hidden="false" OnClick="btnUnSelectAll_Click" />
                                        <f:Button ID="btnGroupUpdate" Icon="GroupEdit" runat="server" Text="保存" OnClick="btnGroupUpdate_Click" />
                                        <f:ToolbarFill runat="server" />
                                        <f:Button ID="btnEditPower" runat="server" OnClick="btnEditPower_Click" Text="编辑权限列表" Icon="Pencil" />
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="GroupName" SortField="GroupName" HeaderText="分组名称" Width="120px" />
                                <f:TemplateField   ExpandUnusedSpace="true" ColumnID="Powers" HeaderText="权限列表" >
                                    <ItemTemplate>
                                        <f:CheckBoxList ID="ddlPowers" CssClass="powers" runat="server" ColumnWidth="500" />
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>

        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true" 
            EnableIFrame="true" IFrameUrl="about:blank" Width="700px" Height="450px" 
            OnClose="Window1_Close" />

        <f:Menu ID="Menu2" runat="server">
            <f:MenuButton ID="menuSelectRows" EnablePostBack="false" runat="server" Text="全选行" />
            <f:MenuButton ID="menuUnselectRows" EnablePostBack="false" runat="server" Text="取消行" />
        </f:Menu>
    </form>
</body>
</html>
