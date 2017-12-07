<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserForm.aspx.cs" Inherits="App.Admins.UserForm" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <style>
        .photo {
            height: 150px;
            line-height: 150px;
            overflow: hidden;
        }
        .photo img {
            height: 150px;
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  AutoScroll="true" runat="server">
            <Toolbars>
                <f:Toolbar runat="server" ID="toolbar1" />
            </Toolbars>
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px" Title="SimpleForm">
                    <Items>
                        <f:SimpleForm runat="server" ShowBorder="false" ShowHeader="false" >
                            <Items>
                                <f:Image ID="imgPhoto" CssClass="photo" ImageUrl="~/res/images/blank.png" ShowEmptyLabel="true" runat="server"/>
                                <f:FileUpload runat="server" ID="uploader" ShowRedStar="false" ShowEmptyLabel="true"
                                    ButtonText="上传图像" ButtonOnly="true" Required="false" ButtonIcon="ImageAdd" AcceptFileTypes="image/*"
                                    AutoPostBack="true" OnFileSelected="uploader_FileSelected"/>
                            </Items>
                        </f:SimpleForm>
                        <f:TextBox ID="tbName" runat="server" Label="用户名" Required="true" ShowRedStar="true" EmptyText="建议用手机号码，避免重复。用户名一旦设置将不允许修改。" />
                        <f:TextBox ID="tbNickName" runat="server" Label="昵称" Required="true" ShowRedStar="true" EmptyText="可填写用户姓名" />
                        <f:RadioButtonList ID="ddlGender" Label="性别" Required="true" ShowRedStar="true" runat="server">
                            <f:RadioItem Text="男" Value="男" Selected="true" />
                            <f:RadioItem Text="女" Value="女" />
                        </f:RadioButtonList>
                        <f:TextBox ID="tbMobile" runat="server" Label="手机号" Required="true" ShowRedStar="true" />
                        <f:CheckBoxList runat="server" ID="cblRole" Label="拥有角色" />
                        <f:CheckBox ID="cbEnabled" runat="server" Label="是否启用" />
                        <f:Label ID="lblRegistDt" runat="server" Label="注册日期" />
                        <f:Panel runat="server" ShowHeader="true" EnableCollapse="true" Collapsed="true" Title="更多..." ShowBorder="false" BodyPadding="10" Layout="VBox" >
                            <Items>
                                <f:TextBox ID="tbRealName" runat="server" Label="真实姓名" />
                                <f:TextBox ID="tbIdCard" runat="server" Label="身份证" />
                                <f:TextBox ID="tbPhone" runat="server" Label="工作电话" />
                                <f:DropDownList runat="server" ID="ddlDept" Label="归属部门" />
                                <f:CheckBoxList runat="server" ID="cblTitle" Label="拥有职务" />
                                <f:TextBox ID="tbEmail" runat="server" Label="邮箱" Required="false" ShowRedStar="false" RegexPattern="EMAIL" />
                                <f:TextBox ID="tbQQ" runat="server" Label="QQ号" />
                                <f:TextBox ID="tbWechat" runat="server" Label="微信号" />
                                <f:DatePicker ID="dpBirthday" runat="server" Label="生日" />
                                <f:TextBox ID="tbSpecialty" runat="server" Label="擅长" />
                                <f:TextArea ID="tbRemark" runat="server" Label="备注" Height="80" />
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Form>
            </Items>
        </f:Panel>


        <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Top" IsModal="True" Width="550px"
            Height="350px"
            />
    </form>
</body>
</html>
