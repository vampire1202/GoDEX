<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Monitor.aspx.cs" Inherits="GoDex.Default" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Src="~/UControl/Node.ascx" TagPrefix="uc1" TagName="Node" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
     <meta http-equiv="refresh" content="5"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-color:white;">
    <asp:Panel ID="palNodes" runat="server"> 
    </asp:Panel> 
    </div>
</asp:Content>
