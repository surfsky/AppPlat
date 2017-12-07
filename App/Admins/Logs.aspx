<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="App.Admins.Logs" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="0px" ShowBorder="false" ShowHeader="false" Layout="Fit">
        <Items>

            <f:GridPro ID="Grid1" runat="server" 
                SortDirection="DESC" ShowNumberField="true"  
                OnPreRowDataBound="Grid1_PreRowDataBound" 
                >
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnDelete" Icon="Delete" runat="server" Text="删除一个月前的记录" OnClick="btnDelete_Click" />
                            <f:ToolbarFill runat="server" />
                            <f:DropDownList ID="ddlSearchLevel" runat="server" Width="120px" />
                            <f:DropDownList ID="ddlSearchRange" runat="server"  Width="120px"  EnableEdit="true"
                                EmptyText="-- 时间范围 --" AutoSelectFirstItem="false" ForceSelection="true"
                                >
                                <f:ListItem Text="今天" Value="TODAY"/>
                                <f:ListItem Text="最近一周" Value="LASTWEEK"  Selected="true" />
                                <f:ListItem Text="最近一个月" Value="LASTMONTH" />
                                <f:ListItem Text="最近一年" Value="LASTYEAR" />
                            </f:DropDownList>
                            <f:TextBox runat="server" ID="tbOperator" EmptyText="操作人" Width="100px" />
                            <f:TextBox runat="server" ID="tbxMessage" EmptyText="错误信息" Width="200px" />
                            <f:Button runat="server" ID="btnSearch" Icon="SystemSearch" Text="查找" OnClick="btnSearch_Click" Type="Submit"  />
                        </Items>
                    </f:Toolbar>
                </Toolbars>


                <Columns>
                    <f:ImageField DataImageUrlField="Lvl" ColumnID="Icon" SortField="Lvl" Width="30px" DataToolTipField="Level" />
                    <f:BoundField DataField="LogDt" SortField="LogDt" DataFormatString="{0:yyyy-MM-dd HH:mm}" Width="150px" HeaderText="时间" />
                    <f:BoundField DataField="Lvl" SortField="Lvl" Width="80px" HeaderText="级别" />
                    <f:BoundField DataField="Operator" SortField="Logger" Width="100px" HeaderText="操作人" />
                    <f:BoundField DataField="From"  SortField="From" HeaderText="来源" Width="100px" />
                    <f:BoundField DataField="IP"    SortField="IP" HeaderText="IP" Width="140px" />
                    <f:BoundField DataField="Summary"  HeaderText="信息" Width="300px" ExpandUnusedSpace="true" />
                </Columns>
            </f:GridPro>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
