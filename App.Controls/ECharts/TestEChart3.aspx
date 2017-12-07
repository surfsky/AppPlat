<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEChart3.aspx.cs" Inherits="App.Controls.ECharts.TestEChart3" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/javascript" src="../res/js/echarts.min.js" ></script>
</head>
<body>
    <form id="form2" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" BodyPadding="10px" LabelWidth="60px" ShowHeader="false">
            <Items>
                <f:TextBox ID="tbTitle" Label="标题"  runat="server" />
                <f:DatePicker runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" EmptyText="请选择日期" ID="DatePicker1" ShowRedStar="True" />
                <f:Label runat="server" ID="Chart1" Label="图表" CssStyle="width: 600px;height:400px;" />
            </Items>
        </f:SimpleForm>
        <f:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1" />
    </form>
</body>
</html>
