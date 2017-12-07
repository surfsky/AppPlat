<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuForm.aspx.cs" Inherits="App.Admins.MenuForm" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  AutoScroll="true" runat="server">
            <Items>
                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"  Title="SimpleForm">
                    <Items>
                        <f:TextBox ID="tbName" runat="server" Label="菜单名称" Required="true" ShowRedStar="true" />
                        <f:DropDownList ID="ddlParentMenu" Label="上级菜单" Required="false" ShowRedStar="false" runat="server" />
                        <f:NumberBox ID="tbSeq" Label="排序" Required="true" ShowRedStar="true" runat="server" />
                        <f:DropDownList ID="ddlViewPower" runat="server" Label="浏览权限" EnableEdit="false" />
                        <f:TextBox ID="tbUrl" runat="server" Label="链接" />
                        <f:TextBox ID="tbIcon" runat="server" Label="图标" />
                        <f:RadioButtonList ID="iconList" ColumnNumber="4" ShowEmptyLabel="true" runat="server" />
                        <f:TextArea ID="tbRemark" runat="server" Label="备注" Height="100" />
                        <f:CheckBox ID="chkOpen" runat="server" Label="是否展开" Checked="false" />
                        <f:CheckBox ID="chkVisible" runat="server" Label="是否可见" Checked="true" />
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>

    <script type="text/javascript">
        F.ready(function () {
            var iconList = F('<%= iconList.ClientID %>');
            var tbxIcon = F('<%= tbIcon.ClientID %>');

            iconList.on('change', function (field) {
                tbxIcon.setValue(field.f_getSelectedValues()[0]);
            });

            tbxIcon.on('change', function (field, newValue, oldValue) {
                iconList.f_setValue(newValue);
            });
        });
    </script>
</body>
</html>
