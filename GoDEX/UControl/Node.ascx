<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Node.ascx.cs" Inherits="GoDex.UControl.Node" %>
<div style="width:158px;height:68px;background-color:aliceblue;border:2px solid skyblue ; float:left;margin:1px 1px 1px 1px;">
   <table style="width:158px;height:68px; border:none;border-collapse: collapse; ">
       <tr style="height:22px;">
           <td style="width:25px;  text-align:center;">
               <asp:Label ID="lblMachineNo" runat="server" Text="1" ForeColor="Blue" Font-Size="18px" Font-Bold="true"></asp:Label>
           </td>
           <td style="width:35px;text-align:center;">
               <asp:Label ID="Label1" runat="server" Font-Names="微软雅黑" ForeColor="DarkBlue" Text="火警"></asp:Label>
           </td>
           <td  style="width:18px;">
               <asp:Panel ID="fire1" Width="16px" Height="16px" BorderStyle="Solid" BorderColor="Gray" BorderWidth="1" runat="server" BackColor="LightCyan">
               </asp:Panel>
           </td>
           <td style="width:18px;">   <asp:Panel ID="fire2" Width="16px" Height="16px"  BorderStyle="Solid" BorderColor="Gray" BorderWidth="1"  runat="server" BackColor="LightCyan">
               </asp:Panel></td>
           <td style="width:18px;">   <asp:Panel ID="fire3" Width="16px" Height="16px"  BorderStyle="Solid" BorderColor="Gray" BorderWidth="1"  runat="server" BackColor="LightCyan">
               </asp:Panel></td>
           <td style="width:18px;">   <asp:Panel ID="fire4" Width="16px" Height="16px"  BorderStyle="Solid" BorderColor="Gray" BorderWidth="1"  runat="server" BackColor="LightCyan">
               </asp:Panel></td>
       </tr>
       <tr style="height:22px;"> 
           <td>
               <asp:Label ID="lblOnline" runat="server" Font-Size="14px" Text="在线" ForeColor="Green"></asp:Label>
           </td>
           <td>
               <asp:Label ID="Label2" runat="server" Font-Names="微软雅黑" ForeColor="DarkBlue" Text="气流"></asp:Label>
           </td>
            <td ><asp:Panel ID="air1" Width="16px" Height="16px"  BorderStyle="Solid" BorderColor="Gray" BorderWidth="1"  runat="server" BackColor="LightCyan">
               </asp:Panel></td>
           <td ><asp:Panel ID="air2" Width="16px" Height="16px"  BorderStyle="Solid" BorderColor="Gray" BorderWidth="1"  runat="server" BackColor="LightCyan">
               </asp:Panel></td>
           <td><asp:Panel ID="air3" Width="16px" Height="16px"  BorderStyle="Solid" BorderColor="Gray" BorderWidth="1"  runat="server" BackColor="LightCyan">
               </asp:Panel></td>
           <td ><asp:Panel ID="air4" Width="16px" Height="16px"  BorderStyle="Solid" BorderColor="Gray" BorderWidth="1"  runat="server" BackColor="LightCyan">
               </asp:Panel></td>
       </tr>
       <tr  style="height:22px;text-align:left;">
           <td colspan="6">
               <asp:Label ID="lblMachineType" Font-Names="微软雅黑" ForeColor="Navy" runat="server" Text="设备类型:单管单区"></asp:Label>
           </td> 
       </tr>
   </table>
</div>