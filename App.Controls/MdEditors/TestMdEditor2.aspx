<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMdEditor2.aspx.cs" Inherits="App.Controls.MdEditors.TestMdEditor2" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
</head>
<body>
    <form id="form2" runat="server">
        <!-- fineui form -->
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" BodyPadding="10px" LabelWidth="60px" ShowHeader="false">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:Button ID="btnGet" runat="server" Text="获取编辑器的值" OnClick="btnGet_Click" />
                        <f:Button ID="btnSet" runat="server" CssClass="marginr" Text="设置编辑器的值" OnClick="btnSet_Click" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TextBox ID="tbTitle" Label="标题"  runat="server" />
                <f:DatePicker runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" EmptyText="请选择日期" ID="DatePicker1" ShowRedStar="True" />
                <f:ContentPanel ID="ContentPanel1" CssStyle="padding-left:65px;"  runat="server" ShowBorder="false" ShowHeader="false">
                    <f:MdEditor runat="server" ID="edt1" Text="hello world" Width="100%" Height="300" ScriptPath="~/res/js/editor.md/" />
                </f:ContentPanel>
                <f:ContentPanel ID="ContentPanel2" CssStyle="padding-left:65px;"  runat="server" ShowBorder="false" ShowHeader="false">
                    <f:MdEditor runat="server" ID="edt2" Text="hello world" Width="100%" Height="300" ScriptPath="~/res/js/editor.md/" />
                </f:ContentPanel>
            </Items>
        </f:SimpleForm>
    </form>
</body>
</html>
