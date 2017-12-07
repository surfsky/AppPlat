<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMdEditor1.aspx.cs" Inherits="App.Controls.MdEditors.TestMdEditor1" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
</head>
<body>
    <form id="form2" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" BodyPadding="10px" LabelWidth="60px" ShowHeader="false">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:Button ID="btnGet" runat="server" Text="获取编辑器的值" OnClick="btnGet_Click" />
                        <f:Button ID="btnSet" runat="server" CssClass="marginr" Text="设置编辑器的值" OnClick="btnSet_Click" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:TextBox ID="tbTitle" Label="标题"  runat="server" />
                <f:DatePicker runat="server" Label="开始日期" DateFormatString="yyyy-MM-dd" EmptyText="请选择日期" ID="DatePicker1" ShowRedStar="True" />
                <f:ContentPanel ID="ContentPanel1" CssStyle="padding-left:65px;"  runat="server" ShowBorder="false" ShowHeader="false">
                    <div id="edt"><textarea runat="server" id="edtArea" /></div>
                </f:ContentPanel>
            </Items>
        </f:SimpleForm>
    </form>

    <!-- editor.md -->
    <link rel="stylesheet" href="./editor.md/css/editormd.css" />
    <script src="./editor.md/examples/js/jquery.min.js"></script>
    <script src="./editor.md/editormd.js"></script> 
    <script type="text/javascript">
        // 创建
        var editor = editormd("edt", {
            width: "100%",
            height: 400,
            path : './editor.md/lib/',
            watch: false,
            tex: true,
            tocm: true,
            emoji: true,
            taskList: true,
            codeFold: true,
            searchReplace: true,
            flowChart: true,
            sequenceDiagram: true,
            htmlDecode: "style,script,iframe",
            toolbarIcons: function () {
                return [
                        "h1", "h2", "h3", "|", 
                        "bold", "del", "italic", "quote", "|", 
                        "list-ul", "list-ol", "hr", "table", "|",
                        "link", "image", "||",
                        "watch", "fullscreen"
                    ]
            },
            imageUpload    : true,
            imageFormats   : ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
            imageUploadURL : "Uploader.ashx"
        });

        // 更新编辑器内容
        function updateEditor(content) {
            editor.setMarkdown(content);
        }
    </script>
</body>
</html>
