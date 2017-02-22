<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="GoDex.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="laydate/laydate.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 810px; height: 30px; margin-top: 10px;">
                <ul>
                    <li style="width: 800px; height: 22px; line-height: 22px;">
                        <span style="float: left; width: 80px; float: left;">&nbsp;节点编号:&nbsp; </span>
                        <asp:DropDownList ID="cmbNodes" Width="50px" CssClass="dropdownlist" runat="server" Height="22px"></asp:DropDownList>
                        <span style="float: left; width: 80px; float: left;">&nbsp;查询日期:&nbsp;</span><span style="width: 30px; float: left; height: 20px;">From</span>
                        <input id="datefrom" name="datefrom" value="<%=dtfrom %>" type="text" style="border: 1px solid darkgray; height: 20px; width: 120px; float: left;" />
                        <span style="width: 30px; float: left; height: 20px;">To</span>
                        <input id="dateto" name="dateto" value="<%=dtto %>" type="text" style="border: 1px solid darkgray; height: 20px; width: 120px; float: left;" />
                        <span style="width: 150px; height: 22px; float: left;">
                            <asp:Button ID="btnFind" Width="60" Height="22" BorderStyle="Solid" BorderWidth="1" BorderColor="DarkGray" PostBackUrl="~/Report.aspx" runat="server" Text="查询" OnClick="btnFind_Click" />&nbsp;&nbsp;
                        <%--<asp:Button ID="btnExport" Width="60" Height="22" BorderStyle="Solid" BorderWidth="1" BorderColor="DarkGray" runat="server" Text="导出Excel" OnClick="btnExport_Click" />--%>

                        </span>
                    </li>
                </ul>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width: 800px; height: 600px; float: left; padding-left: 5px;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#006699" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging"
            AllowSorting="True" PageSize="20" BorderColor="#669999" AllowPaging="true" BorderStyle="None" BorderWidth="1px" Font-Size="Larger">
            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="#003366" Font-Size="28pt" />
            <Columns>
                <asp:BoundField DataField="machineNo" HeaderText="节点编号"  ItemStyle-Width="100" ReadOnly="True">
                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray" />
                    <HeaderStyle BorderWidth="1px" BorderColor="DarkGray" />
                </asp:BoundField> 
                <asp:BoundField DataField="warnDiscrib" HeaderText="警报等级" ItemStyle-Width="200" ReadOnly="True">
                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray" />
                    <HeaderStyle BorderWidth="1px" BorderColor="DarkGray" />
                </asp:BoundField>
                <asp:BoundField DataField="warnLeval" HeaderText="警报描述" ItemStyle-Width="700" ReadOnly="True">
                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray" />
                    <HeaderStyle BorderWidth="1px" BorderColor="DarkGray" />
                </asp:BoundField>               
                <asp:BoundField DataField="userName" HeaderText="确认人员"  ItemStyle-Width="100" ReadOnly="True">
                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray" />
                    <HeaderStyle BorderWidth="1px" BorderColor="DarkGray" />
                </asp:BoundField>
                <asp:BoundField DataField="warnDateTime" HeaderText="警报日期"  ItemStyle-Width="150" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" ReadOnly="True">
                    <ItemStyle BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray" />
                    <HeaderStyle BorderWidth="1px" BorderColor="DarkGray" />
                </asp:BoundField>
                <%--<asp:CommandField HeaderText="选择" ShowSelectButton="True" />
                            <asp:CommandField HeaderText="编辑" ShowEditButton="True" />
                            <asp:CommandField HeaderText="删除" ShowDeleteButton="True" />--%>
            </Columns>
            <RowStyle ForeColor="#000066" Height="30px" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <PagerSettings NextPageText="[下一页]" LastPageText="[最后一页]" Position="TopAndBottom" PreviousPageText="[上一页]" Visible="true" FirstPageText="[第一页]" Mode="NextPreviousFirstLast" PageButtonCount="20" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Height="40px" Font-Size="Medium" HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:GridView>
    </div>
    <script>
        ; !function () {
            laydate({
                elem: '#datefrom'
            })
        }();
        ; !function () {
            laydate({
                elem: '#dateto'
            })
        }();
    </script>
</asp:Content>
