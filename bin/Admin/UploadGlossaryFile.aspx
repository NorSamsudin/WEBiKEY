<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UploadGlossaryFile.aspx.cs" Inherits="WEBiKEY.Application.Admin.UploadGlossaryFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:FileUpload ID="FileUploadDescriptionFile" runat="server" />
    <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />

    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
