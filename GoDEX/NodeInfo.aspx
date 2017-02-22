<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NodeInfo.aspx.cs" Inherits="GoDEX.NodeInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        trright {
            text-align: right;
            width: 200px;
        }

        td {
            font-size: 16px;
            font-family: 微软雅黑;
            color: navy;
        }
    </style>
</head>
<%--style="text-align:center;vertical-align:middle; background-color: #1c77ac; background-image: url(images/light.png); background-repeat: no-repeat; background-position: center top; overflow: hidden;"--%>
<body>
    <form id="form1" runat="server">
        <div> 
            <table border="1">
                <tr>
                    <td class="trright">
                        <asp:Label ID="Label1" runat="server" Text="节点编号:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMachineNo" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="设备类型:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMachineType" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="滤网日期:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLvwangDate" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="设备地址:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="区域地图:"></asp:Label>
                    </td>
                    <td>
                        <asp:Image ID="imgMap" Width="500" Height="300" ImageAlign="AbsMiddle" runat="server" />
                    </td>
                </tr>            
            </table>
        </div>
    </form>
</body>
</html>
