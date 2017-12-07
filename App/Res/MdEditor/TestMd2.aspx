<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMd2.aspx.cs" Inherits="TestFineUI.Plugins.MdEditor.TestMd2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button runat="server" ID="btnSet" Text="Set" OnClick="btnSet_Click" />
        <asp:Button runat="server" ID="btnGet" Text="Get" OnClick="btnGet_Click"/>
        <div id="edt" style="width:100%" >
            <textarea runat="server" id="edtArea" />
        </div>
        <br/>
    </form>


        
        <link rel="stylesheet" href="./editor.md/css/editormd.css" />
        <script src="./editor.md/examples/js/jquery.min.js"></script>
        <script src="./editor.md/editormd.js"></script> 
        <script type="text/javascript">
            var editor;
            $(function() {
                editor = editormd("edt", {
                    height: 640,
                    path : './editor.md/lib/',
                    watch : false,
                    toolbarIcons : function() {
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
            });
        </script>
</body>
</html>
