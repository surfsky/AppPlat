<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Resources.aspx.cs" Inherits="App.Admins.Resources" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="0px"  ShowBorder="false" Layout="Fit" ShowHeader="false" >
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:FileUpload runat="server" ID="filePhoto" 
                        ButtonText="上传图片" ButtonOnly="true" ButtonIcon="ImageAdd" 
                        AcceptFileTypes="image/*"
                        AutoPostBack="true" OnFileSelected="filePhoto_FileSelected"/>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:GridPro ID="Grid1" runat="server"
                AutoCreateFields="false" 
                ShowNumberField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="false" ShowViewField="false"
                OnDelete="Grid1_Delete"
                RelayoutToolbar="false"
                WinHeight="800"
                >
                <Columns>
                    <f:ImageField HeaderText="图片" DataImageUrlField="Url" Width="150px" MinWidth="30" ImageHeight="30" ImageWidth="30" />
                    <f:BoundField HeaderText="大小" DataField="Size" SortField="Size" Width="100px"  />
                    <f:BoundField HeaderText="时间" DataField="UploadDt" SortField="UploadDt" Width="200px" Hidden="false" />
                    <f:BoundField HeaderText="说明" DataField="Description" ExpandUnusedSpace="true" />
                </Columns>
            </f:GridPro>
        </Items>
    </f:Panel>
 
    </form>
</body>
</html>
