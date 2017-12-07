<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleForm.aspx.cs"  Inherits="App.Admins.ArticleForm" ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>

                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Width="700px" Position="Left" Layout="Fit" BodyPadding="0px" runat="server" Split="true">
                    <Items>
                        <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"  Title="SimpleForm">
                            <Items>
                                <f:TextBox runat="server" Label="标题" ID="tbTitle"   Required="true" ShowRedStar="true" />
                                <f:TextBox runat="server" Label="作者" ID="tbAuthor"  Required="true" ShowRedStar="true" />
                                <f:NumberBox runat="server" Label="访问次数" ID="tbVisitCnt" />
                                <f:Label   runat="server" Label="发布日期" ID="lblPostDt" />
                                <f:DropDownList runat="server" Label="类别" ID="ddlType" EmptyText="类别" />
                                <f:HtmlEditor runat="server" Label="内容" ID="tbContent" Height="400px" />
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:Region>
                
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit" BodyPadding="0px" runat="server" Width="200" MinWidth="200">
                    <Items>
                        <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false"  AutoScroll="true" runat="server" 
                            EnableIFrame="true" IFrameUrl="~/Admins/Resources.aspx?cate=News&key=News-001"
                            />
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>

    </form>
</body>
</html>
