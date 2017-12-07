<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Onlines.aspx.cs" Inherits="App.Admins.Onlines" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="0px" ShowBorder="false" ShowHeader="false" Title="日志管理" Layout="Fit">
        <Items>

            <f:GridPro ID="Grid1" runat="server" DataKeyNames="ID"  SortDirection="DESC"  EnableCheckBoxSelect="false">

                <Toolbars>
                    <f:Toolbar runat="server" ID="Toolbar1">
                        <Items>
                            <f:TwinTriggerBoxPro ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="搜索用户名" OnTriggerClick="ttbSearchMessage_TriggerClick" />
                        </Items>
                    </f:Toolbar>
                </Toolbars>

                <Columns>
                    <f:RowNumberField />
                    <f:WindowField WindowID="Window1" ToolTip="查看" HeaderText="用户" Width="100px" 
                        DataTextField="User.Name" SortField="User.Name"
                        DataIFrameUrlFields="User.ID"  DataIFrameUrlFormatString="~/admins/UserForm.aspx?mode=view&id={0}" 
                        />
                    <f:BoundField DataField="UpdateDt" SortField="UpdateDt" DataFormatString="{0:yyyy-MM-dd HH:mm}" Width="120px" HeaderText="最后操作时间" />
                    <f:BoundField DataField="LoginDt" SortField="LoginDt" Width="120px" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="登录时间" />
                    <f:BoundField DataField="User.RealName" SortField="User.RealName" Width="100px" HeaderText="姓名" />
                    <f:BoundField DataField="IPAdddress" ExpandUnusedSpace="true" HeaderText="IP地址" />
                </Columns>
            </f:GridPro>
        </Items>
    </f:Panel>

    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="650px"
        Height="450px" Title="用户信息"
        />
    </form>
</body>
</html>
