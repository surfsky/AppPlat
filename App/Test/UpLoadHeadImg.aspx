<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpLoadHeadImg.aspx.cs" Inherits="App.Test.UpLoadHeadImg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no" />
    <meta name="format-detection" content="telephone=no" searchtype="map" />
    <title></title>
    <link href="/Res/css/weui.min.css" rel="stylesheet" />
    <link href="/Res/css/jquery-weui.min.css" rel="stylesheet" />
    <script src="/Res/js/jquery.min.js"></script>
    <script src="/Res/js/jquery-weui.min.js"></script>
    <script src="/Res/js/jquery.custom.js"></script>
</head>
<body>
    <form id="form1" runat="server">
       
                            <div class="weui-uploader__input-box">
                                <input id="uploaderInput" class="weui-uploader__input" type="file" accept="image/*" multiple="">
                            </div>
    </form>
</body>
</html>
