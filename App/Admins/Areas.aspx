<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Areas.aspx.cs" Inherits="App.Admins.Areas" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="0px"  ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>

                <f:GridPro ID="Grid1" runat="server"
                    AutoCreateFields="false" RelayoutToolbar="false"
                    ShowNumberField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="true"
                    OnDelete="Grid1_Delete"  
                    WinHeight="300" WinCloseAction="HidePostBack"
                    >
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnClose"  Icon="SystemSaveClose" OnClick="btnClose_Click" runat="server" Text="关闭" Hidden="true" />
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="Name" HeaderText="名称" DataSimulateTreeLevelField="TreeLevel" Width="150px" />
                        <f:BoundField DataField="Remark" HeaderText="描述" ExpandUnusedSpace="true" />
                        <f:BoundField DataField="Seq" HeaderText="排序" Width="80px" />
                    </Columns>
                </f:GridPro>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
