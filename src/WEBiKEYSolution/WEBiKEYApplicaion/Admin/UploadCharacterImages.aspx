<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UploadCharacterImages.aspx.cs" Inherits="WEBiKEY.Application.Admin.UploadCharacterImages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server">Character Code</asp:Label>
                <asp:DropDownList ID="cmbCharacters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbCharacters_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:FileUpload ID="FileUploadCharacterImage" runat="server" Enabled="False" />
                <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />
                <asp:Button ID="btnRemoveImage" runat="server" OnClick="btnRemoveImage_Click" Text="Remove Image" Visible="false" />
                <asp:Label ID="lblInfo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="CharacterImage" runat="server" Height="250px" />
            </td>
        </tr>
    </table>
</asp:Content>
