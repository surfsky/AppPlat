<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="App.Main" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>首页</title>
    <link href="~/res/css/main.css" rel="stylesheet" runat="server" />
    <script type="text/javascript">
        // 切换全屏(ie8不支持)
        function switchFullScreen() {
            if (isFullscreen())     exitFullScreen();
            else                    enterFullScreen();
        }
        // 是否全屏
        function isFullscreen() {
            return document.fullscreenElement ||
                document.msFullscreenElement ||
                document.mozFullScreenElement ||
                document.webkitFullscreenElement ||
                false;
        }
        //进入全屏
        function enterFullScreen() {
            var de = document.documentElement;
            if (de.requestFullscreen)             de.requestFullscreen();
            else if (de.mozRequestFullScreen)     de.mozRequestFullScreen();
            else if (de.webkitRequestFullScreen)  de.webkitRequestFullScreen();
            else if (de.msRequestFullscreen)      de.msRequestFullscreen();
        }
        //退出全屏
        function exitFullScreen() {
            var de = document;
            if (de.exitFullscreen)                de.exitFullscreen();
            else if (de.mozCancelFullScreen)      de.mozCancelFullScreen();
            else if (de.webkitCancelFullScreen)   de.webkitCancelFullScreen();
            else if (de.msExitFullscreen)         de.msExitFullscreen();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="regionPanel" runat="server" />
        <f:RegionPanel ID="regionPanel" ShowBorder="false" runat="server">
            <Regions>

                <f:Region ID="regionTop" ShowBorder="false" ShowHeader="false" Position="Top"  Layout="Fit" runat="server">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Bottom" runat="server" CssClass="topbar">
                            <Items>
                                <f:ToolbarText runat="server" ID="txtTitle" Text="SiteTitle"  CssStyle="font-size:20px; color:white;" />
                                <f:ToolbarFill runat="server" />
                                <f:ToolbarText runat="server" ID="txtUser" Text="欢迎" />
                                <f:Button ID="btnRefresh" runat="server" Icon="Reload" Text="刷新标签" ToolTip="刷新主区域内容" EnablePostBack="false"/>
				                <f:Button ID="btnCloseAll" runat="server" Icon="Cross" Text="关闭所有标签" ToolTip="关闭所有标签页面" EnablePostBack="false"  OnClientClick="window.removeAllTab();" Hidden="false"/>
                                <f:Button ID="btnFullScreen" runat="server" Icon="ArrowOut" Text="全屏" ToolTip="全屏（不支持IE)" EnablePostBack="false"/>
                                <f:Button ID="btnHelp" runat="server" Icon="Help" Text="帮助" EnablePostBack="false" />
                                <f:Button ID="btnExit" runat="server" Icon="DoorOut" Text="安全退出" ConfirmText="确定退出系统？" OnClick="btnExit_Click"/>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Region>


                <f:Region ID="regionLeft" Split="true" EnableCollapse="true" Width="200px"
                    ShowHeader="true" Title="系统菜单" Layout="Fit" Position="Left" runat="server" >
                    <Items>
                        <f:Tree runat="server" ShowBorder="false" ShowHeader="false" EnableArrows="true" EnableLines="true" ID="treeMenu" />
                    </Items>
                </f:Region>

                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Position="Center" runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="首页" EnableIFrame="true" IFrameUrl="~/admins/Welcome.aspx" Icon="House" runat="server" EnableClose="true" />
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>

                <f:Region ID="bottomPanel" RegionPosition="Bottom" ShowBorder="false" ShowHeader="false" EnableCollapse="false" runat="server" Layout="Fit">
                    <Items>
                        <f:ContentPanel ID="ContentPanel3" runat="server" ShowBorder="false" ShowHeader="false">
                            <table class="bottombar">
                                <tr>
                                    <td style="width: 300px;">&nbsp;版本：<a target="_blank" href="http://surfsky.cnblogs.com">
                                        <asp:Literal runat="server" ID="lblVersion"></asp:Literal></a>
                                        &nbsp;&nbsp; </td>
                                    <td style="text-align: center;">Copyright &copy; 2017 </td>
                                    <td style="width: 300px; text-align: right;">
                                        &nbsp;<f:Label runat="server" ID="txtOnlineUserCount" />&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Timer runat="server" ID="Timer1" Interval="30" OnTick="Timer1_Tick" />
    </form>

    <script>
    // F相关的方法请查源码 xxx.js 文件
    F.ready(function () {
        var treeMenu = F('<%= treeMenu.ClientID %>');
        var mainTabStrip = F('<%= mainTabStrip.ClientID %>');
        var btnRefresh = F('<%= btnRefresh.ClientID %>');
        var btnFullScreen = F('<%= btnFullScreen.ClientID %>');

        // 初始化主框架中的树和选项卡互动，及地址栏的更新逻辑
        F.util.initTreeTabStrip(treeMenu, mainTabStrip, null, true, false, false);

        // 刷新按钮
        btnRefresh.on('click', function () {
            var iframe = Ext.DomQuery.selectNode('iframe', mainTabStrip.getActiveTab().body.dom);
            iframe.contentWindow.location.reload(false);
        });

        // 刷新按钮
        btnFullScreen.on('click', function () {
            switchFullScreen();
        });

        // 添加标签(id是标签的唯一性标志，若该标签已经打开，再次操作时会跳到已经打开的tab上)
        window.addMainTab = function (id, url, text, icon, refreshWhenExist) {
            F.util.addMainTab(mainTabStrip, id, url, text, icon, null, refreshWhenExist);
        };

        // 删除活动标签
        window.removeActiveTab = function () {
            var activeTab = mainTabStrip.getActiveTab();
            mainTabStrip.removeTab(activeTab.id);
        };

        // 删除所有标签
        window.removeAllTab = function () {
            mainTabStrip.removeAll();
        };
    });
    </script>
</body>
</html>
