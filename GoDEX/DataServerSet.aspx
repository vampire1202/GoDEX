<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataServerSet.aspx.cs" Inherits="GoDex.DataServerSet" %>

<!DOCTYPE html>
<style type="text/css">
    ul, li {
        text-align: left;
        list-style-type: none;
        line-height: 28px;
        height: 28px;
        font-size: 16px;
        font-family: 微软雅黑;
        color: navy;
    }

    .v-mult { 
        margin:50px auto;
        width: 270px;
        height: 250px; 
        border: 2px solid #ddd;
        overflow: hidden;
        background-color:skyblue;
        display:block;
    }
</style>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body style="text-align:center;vertical-align:middle; background-color: #1c77ac; background-image: url(images/light.png); background-repeat: no-repeat; background-position: center top; overflow: hidden;">
    <form id="form1" runat="server">
        <div class="v-mult">
            <ul style="margin-top: 10px;">
                <li>
                    <asp:Label ID="lblSelect" runat="server" Text="请选择服务器:"></asp:Label></li>
                <li>
                    <asp:DropDownList ID="cmbServer" Width="190px" Font-Size="18px" runat="server"></asp:DropDownList>
                </li>
                <li>数据库用户名:</li>
                <li>
                    <asp:TextBox ID="txtUser" Width="190" runat="server" Font-Size="18px">sa</asp:TextBox></li>
                <li>数据库密码:</li>
                <li>
                    <asp:TextBox ID="txtPassword" Width="190" runat="server" TextMode="Password" Font-Size="18px"></asp:TextBox></li>
                <li></li>
                <li>
                    <span><asp:Button ID="btnSave" runat="server" Text="保存设置" Font-Names="微软雅黑" UseSubmitBehavior="false" Width="95" Font-Size="16px" OnClick="btnSave_Click" /></span>
                    <span><asp:Button ID="btnReturn" runat="server" Text="返回登录" Font-Names="微软雅黑" UseSubmitBehavior="false" Width="95" Font-Size="16px" OnClick="btnReturn_Click" /></span> 
                </li>
            </ul>
        </div>
    </form>
</body>
</html>
