<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="App.Admins.Welcome" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../res/js/echarts.min.js" ></script>
    <style type="text/css">
        .main td.x-table-layout-cell {
            padding: 5px;
        }

        .main td.f-layout-table-cell {
            padding: 5px;
        }

        li{
            /*list-style: url('../res/images/arrow.png')*/
        }
        li a{
            text-decoration: none;
            color: gray;
        }
        li a:visited {
            color: gray;
        }
        li a:hover {
            color: #157FCC;
        }
        .createDt {
            color: #D6D5D5;
            font-size: 10px;
            float: right;
        }
        .x-panel-header-default {
            background-image: none;
            background-color: #FBFBFB;
        }
        .x-panel-header-title-default {
        color: #4c9dd8;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"  />
    <f:Panel ID="Panel1" runat="server" ShowBorder="True" ShowHeader="false" Layout="VBox" Title="自动平均分布" BodyPadding="5">
        <Items>
            <f:Panel ID="Panel5" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigChildMargin="0 5 0 0" Margin="0 0 5 0">
                <Items>
                    <f:ContentPanel BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="true" Title="新闻公告" BodyPadding="5px"  AutoScroll="true" >
                        <asp:Repeater runat="server" ID="rptNews">
                            <ItemTemplate>
                                <li>
                                    <a href="#"  onclick="window.top.addMainTab('Article-<%#Eval("ID") %>', 'Article.aspx?id=<%# Eval("ID") %>', '新闻') ">
                                        <%#Eval("Title") %>
                                    </a>
                                    <div class="createDt">
                                        <%# String.Format("{0:yyyy-MM-dd}", Eval("PostDt")) %>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </f:ContentPanel>
                    <f:Panel ID="Panel9" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="true" Title="报表" BodyPadding="5px"  Layout="Card"  >
                        <Items>
                            <f:Label runat="server" ID="Chart1" CssStyle="width: 300px;height:200px;" />
                        </Items>
                    </f:Panel>
                </Items>
            </f:Panel>

            <f:Panel ID="Panel11" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigChildMargin="0 5 0 0">
                <Items>
                    <f:Panel ID="Panel12" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="true" BodyPadding="5px" Title="报表" Layout="Card" >
                        <Items>
                            <f:Label runat="server" ID="Chart2" CssStyle="width: 300px;height:200px;" />
                        </Items>
                    </f:Panel>
                    <f:Panel ID="Panel13" BoxFlex="1" runat="server" ShowBorder="false" ShowHeader="true" BodyPadding="5px" Title="报表" Layout="Card" >
                        <Items>
                            <f:Label runat="server" ID="Chart3" CssStyle="width: 300px;height:200px;" />
                        </Items>
                    </f:Panel>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>


    <f:Timer runat="server" ID="timer" Interval="1" OnTick="timer_Tick" Enabled="false" EnableAjaxLoading="false"  />
    </form>
</body>
</html>