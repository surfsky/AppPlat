<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GDP.aspx.cs" Inherits="App.Reports.GDP" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../res/js/echarts.min.js" ></script>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"  />
        <f:Panel ID="Panel1" runat="server" BodyPadding="0px" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:TextBox ID="txtFrom" runat="server" TextMode="Text" EmptyText="开始季度（如2000Q1）" />
                        <f:TextBox ID="txtTo" runat="server" TextMode="Text" EmptyText="结束季度（如2000Q1）" />
                        <f:Button ID="btnSearch" runat="server" Icon="SystemSearch" Text="查找" OnClick="btnSearch_Click"  Type="Submit" />
                        <f:Button ID="btnChart1" runat="server" Icon="ChartLine" Text="图表1" OnClick="btnChart1_Click" />
                        <f:Button ID="btnChart2" runat="server" Icon="ChartLine" Text="图表2" OnClick="btnChart2_Click" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:GridPro ID="Grid1" runat="server" AutoCreateFields="true"
                    ShowNumberField="true" ShowIDField="false" ShowViewField="true" ShowEditField="true" ShowDeleteField="false"
                    AllowNew="true"  AllowEdit="true"  AllowDelete="true" AllowBatchDelete="true" AllowExport="true"
                    OnExport="Grid1_Export"
                    />
                <f:Label runat="server" ID="Chart1" Hidden="true"  CssStyle="width: 600px;height:400px;" />
            </Items>
        </f:Panel>
    </form>
</body>
</html>
