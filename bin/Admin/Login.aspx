<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SiteMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WEBiKEY.Application.Admin.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:Login ID="Login1" runat="server" DisplayRememberMe="False" OnAuthenticate="Login1_Authenticate"></asp:Login>
</asp:Content>
