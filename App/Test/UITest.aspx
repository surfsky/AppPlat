<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UITest.aspx.cs" Inherits="App.Test.UITest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0" />
    <title></title>
    <link href="../Res/WeiXin/css/card.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ul class="card_list">
                <li>
                    <div class="card_wrap">
                        <!--卡-->
                        <img src="../Handlers/QRCode.ashx" width="48px" height="48px" alt="">
                        <a href="http://www.styd.cn/m/order/buy/?sy=card&amp;uid=378564&amp;id=3324079&amp;type=1" class="measured_card">
                            <b>三体系统培训专用会员卡<label class="discount" style="display: none">8折</label></b>
                            <span style=">支持 1 家场馆：
                                        模拟健身房（徐汇店）&nbsp;
                            </span>
                            <p class="card_remark">
                                <i class="card_type_des">次卡类型</i>
                                <em>特别说明：每人限购5张，每张可听10节课。
仅限三体云动系统YY语音培训课程使用</em>
                            </p>
                        </a>
                    </div>
                    <div class="view_course"><a href="http://www.styd.cn/m/378564/default/card_class/3324079?from=card_buy">查看支持的课程</a></div>
                </li>
                <li>
                    <div class="card_wrap">
                        <!--卡-->
                        <img src="https://static-s.styd.cn/201606240956/qika.png?imageView2/1/w/610/h/340/interlace/1" alt="" class="card_bg">
                        <a href="http://www.styd.cn/m/order/buy/?sy=card&amp;uid=378564&amp;id=1000013&amp;type=1" class="monthly_card">
                            <b>期限卡<label class="discount" style="display: none">8折</label></b>
                            <span>支持 4 家场馆：
                                        模拟健身房（徐汇店）&nbsp;
                                        模拟健身房（浦东店）&nbsp;
                                        模拟健身房（北京店）&nbsp;
                                        模拟健身房（广州天河店）&nbsp;
                            </span>
                            <p class="card_remark">
                                <i class="card_type_des">期限类型</i>
                            </p>
                        </a>
                    </div>
                    <div class="view_course"><a href="http://www.styd.cn/m/378564/default/card_class/1000013?from=card_buy">查看支持的课程</a></div>
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
