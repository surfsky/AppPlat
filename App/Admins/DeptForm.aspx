<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptForm.aspx.cs" Inherits="App.Admins.DeptForm" %>

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
                        <f:Label runat="server" ID="lblId" Label="ID" />
                        <f:TextBox ID="tbName" runat="server" Label="名称" Required="true" ShowRedStar="true" />
                        <f:NumberBox ID="tbSeq" Label="排序" Required="true" ShowRedStar="true" runat="server" />
                        <f:DropDownList ID="ddlParent" Label="上级部门" ShowRedStar="true" runat="server" />
                        <f:TextArea ID="tbRemark" runat="server" Label="备注" />
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
