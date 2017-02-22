<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Nodes.aspx.cs" Inherits="GoDex.ManageNodes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<meta http-equiv="refresh" content="5"/>--%>
    <script type="text/javascript">
        $(function () {
            //导航切换
            $(".menuson li").click(function () {
                $(".menuson li.active").removeClass("active")
                $(this).addClass("active");
            });

            $('.title').click(function () {
                var $ul = $(this).next('ul');
                $('dd').find('ul').slideUp();
                if ($ul.is(':visible')) {
                    $(this).next('ul').slideUp();
                } else {
                    $(this).next('ul').slideDown();
                }
            });
        })
    </script>
<%--<script type="text/javascript" language="javascript">

    function iFrameHeight() {

        var ifm = document.getElementById("nodeiframe");

        var subWeb = document.frames ? document.frames["nodeiframe"].document :

ifm.contentDocument;

        if (ifm != null && subWeb != null) {

            ifm.height = subWeb.body.scrollHeight;

        }

    }

</script> --%> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
        <div style="width:200px;float:left;" >
            <dl class="leftmenu">
                <dd>
                    <div class="title">
                        <span>
                            <img src="images/node.png" /></span>主机节点
                    </div>
                    <ul class="menuson"> 
                        <%=nodesList %>
                    </ul>                    
                </dd> 
            </dl>
        </div> 
      <%--  onLoad="iFrameHeight()" --%>
<%--        <div style="background-color:red;width:810px; height:510px; text-align:left;">--%>
            <iframe name="nodeiframe" src="NodeInfo.aspx" id="nodeiframe" width="780" height="500"  frameborder="1" scrolling="no" marginheight="0" marginwidth="0"style="background-color: #FFFFFF; position: relative"></iframe>
        <%--</div>--%> 
    

</asp:Content>
