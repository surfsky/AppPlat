<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configs.aspx.cs" Inherits="App.Admins.Configs" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowHeader="false"  ShowBorder="false" Layout="Fit"
        BodyPadding="0px" AutoScroll="true" runat="server">
        <Items>
            <f:SimpleForm ID="SimpleForm1" runat="server" LabelWidth="120px" BodyPadding="5px"
                LabelAlign="Left"  ShowBorder="false"
                ShowHeader="false">
                <Toolbars>
                    <f:Toolbar runat="server">
                        <Items>
                            <f:Button ID="btnSave" runat="server" Icon="SystemSave" OnClick="btnSave_OnClick"
                                ValidateForms="SimpleForm1" ValidateTarget="Top" Text="保存设置"
                                />
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:TextBox ID="tbxTitle" runat="server" Label="网站标题" Required="true" ShowRedStar="true" />
                    <f:DropDownList ID="ddlTheme" Label="网站主题" runat="server" Required="true" ShowRedStar="true">
                        <f:ListItem Text="Neptune" Selected="true" Value="neptune" />
                        <f:ListItem Text="Blue" Value="blue" />
                        <f:ListItem Text="Gray" Value="gray" />
                        <f:ListItem Text="Access" Value="access" />
                    </f:DropDownList>
                    <f:DropDownList ID="ddlMenuType" Label="菜单样式" runat="server" Required="true" ShowRedStar="true" Hidden="true">
                        <f:ListItem Text="手风琴菜单" Value="accordion" />
                        <f:ListItem Text="树型菜单" Value="tree" />
                    </f:DropDownList>
                    <f:NumberBox ID="nbxPageSize" runat="server" Label="表格默认记录数" Required="true" ShowRedStar="true" />
                    <f:TextBox ID="tbDefaultPassword" runat="server" Label="默认密码" Required="true" ShowRedStar="true" />
                    <f:TextArea runat="server" ID="tbxHelpList" Height="300" Label="帮助下拉列表" Required="false"
                        ShowRedStar="false" />
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
