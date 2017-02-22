<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="GoDex.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>欢迎登录Go-DEX早期火警预警系统</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script lang="JavaScript" src="js/jquery.js"></script>
    <script src="js/cloud.js" type="text/javascript"></script>

    <script lang="javascript">
	$(function(){
    $('.loginbox').css({'position':'absolute','left':($(window).width()-692)/2});
	$(window).resize(function(){  
    $('.loginbox').css({'position':'absolute','left':($(window).width()-692)/2});
    })  
});  
    </script>
</head>
<body style="background-color: #1c77ac; background-image: url(images/light.png); background-repeat: no-repeat; background-position: center top; overflow: hidden;">
    <form id="form1" runat="server">
        <div id="mainBody">
            <div id="cloud1" class="cloud"></div>
            <div id="cloud2" class="cloud"></div>
        </div>
        <div class="logintop">
            <span>欢迎登录Go-Dex早期火警预警系统</span>          
        </div>
        <div class="loginbody">
            <span class="systemlogo"></span>
            <div class="loginbox">
                <ul>
                    <li><asp:TextBox ID="txtUser" CssClass="loginuser" runat="server" ToolTip="请输入用户名" onclick="JavaScript:this.value=''"></asp:TextBox></li>
                    <li>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="loginpwd" TextMode="Password" ToolTip="请输入密码"  onclick="JavaScript:this.value=''"></asp:TextBox></li>
                    <li style="width:375px;">
                        <asp:Button ID="btnLogin" runat="server" Text="登录" CssClass="loginbtn" OnClick="btnLogin_Click" />  
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  
                        <%--<asp:Button ID="btnSet" runat="server" Text="服务器设置" CssClass="loginbtn" OnClick="btnSet_Click" />--%> 
                    </li>
                </ul>
            </div>
        </div>
    </form>
</body>
</html>
