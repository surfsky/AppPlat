<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEChart2.aspx.cs" Inherits="App.Controls.ECharts.TestEChart2" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
</head>
<body>
    <form id="form2" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" BodyPadding="10px" LabelWidth="60px" ShowHeader="false">
            <Items>
                <f:TextBox ID="tbTitle" Label="标题"  runat="server" />
                <f:DatePicker runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" EmptyText="请选择日期" ID="DatePicker1" ShowRedStar="True" />
                <f:ContentPanel ID="ContentPanel1" CssStyle="padding-left:65px;"  runat="server" ShowBorder="false" ShowHeader="false">
                    <f:EChart runat="server" ID="Chart1" Width="650px" Height="285px" ScriptPath="~/res/js/echarts.min.js"  />
                </f:ContentPanel>
            </Items>
        </f:SimpleForm>
    </form>
</body>
</html>
