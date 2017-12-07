<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Articles.aspx.cs" Inherits="App.Admins.Articles" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="0px"  ShowBorder="false" Layout="Fit" ShowHeader="false" >
        <Items>
            <f:GridPro ID="Grid1" runat="server"
                AutoCreateFields="false" 
                ShowNumberField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="false" ShowViewField="false"
                OnDelete="Grid1_Delete"
                RelayoutToolbar="false"
                WinHeight="600"
                WinWidth="800"
                OnPreRowDataBound="Grid1_PreRowDataBound"
                >
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button runat="server" ID="btnNew" Text="新增" OnClick="btnNew_Click" Icon="SystemNew"/>
                            <f:ToolbarFill runat="server" />
                            <f:DropDownList runat="server" ID="ddlType" EmptyText="类别" />
                            <f:TextBox runat="server" ID="tbTitle" EmptyText="标题" Width="100" />
                            <f:TextBox runat="server" ID="tbAuthor" EmptyText="作者" Width="100" />
                            <f:DatePicker runat="server" ID="dpStart" EmptyText="开始时间" Width="100" />
                            <f:DatePicker runat="server" ID="dpEnd" EmptyText="结束时间" Width="100" />
                            <f:Button runat="server" ID="btnSearch" Text="查找" OnClick="btnSearch_Click"  Type="Submit" />
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Columns>
                    <f:HyperLinkField HeaderText="" Text="<img src='../res/icon/eye.png'/>" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="Article.aspx?id={0}" Width="30" Target="_blank" ToolTip="查看" />
                    <f:BoundField HeaderText="类别" DataField="Type" SortField="Type" Width="100px" ColumnID="Type" />
                    <f:BoundField HeaderText="标题" DataField="Title" SortField="Title" Width="300px" />
                    <f:BoundField HeaderText="作者" DataField="Author" SortField="Author" Width="100px" />
                    <f:BoundField HeaderText="访问次数" DataField="VisitCnt" SortField="VisitCnt" Width="100px" />
                    <f:BoundField HeaderText="发布日期" DataField="PostDt" SortField="PostDt" Width="150px" />
                    <f:BoundField HeaderText="内容摘要" DataField="Summary" SortField="Summary" Width="100px" ExpandUnusedSpace="true" />
                </Columns>
            </f:GridPro>
        </Items>
    </f:Panel>
 
    </form>
</body>
</html>
