<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEChart.aspx.cs" Inherits="App.Controls.ECharts.TestEChart" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <f:EChart runat="server" ID="Chart1" Width="650px" Height="285px" ScriptPath="~/res/js/echarts.min.js"  />
        <f:EChart runat="server" ID="Chart2" Width="650px" Height="285px" Title="Title" />
    </div>
    </form>
</body>
</html>
