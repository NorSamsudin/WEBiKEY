<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UploadSpeciesDescriptionFile.aspx.cs" Inherits="WEBiKEY.Application.Admin.UploadSpeciesDescriptionFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <table>
        <tr>
            <td>
                <asp:DropDownList ID="cmbSpecies" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbSpecies_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr style="background-color: bisque;">
            <td>Species Description</td>
            <td>
                <asp:FileUpload ID="FileUploadDescriptionFile" runat="server" Enabled="False" />
                <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" />
                <asp:Button ID="btnRemoveCurrentFile" runat="server" Text="Remove Current File" OnClick="btnRemoveCurrentFile_Click" />
                <asp:Label ID="lblInfo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>

        <%-- 
                   <tr style="background-color: lightsteelblue">
            <td style="vertical-align: top;">Species Images</td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:FileUpload ID="FileUploadImage" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label Text="Description:" runat="server" />
                            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                            <asp:Button ID="btnUploadImage" Text="Upload" runat="server" OnClick="btnUploadImage_Click" />
                            <asp:Label ID="lblImageInfo" runat="server"></asp:Label>

                        </td>
                    </tr>

                    <tr>
                        <td>
                            <asp:ListView runat="server" ID="lstSpeciesImages">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button Text="Remove" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Image ImageUrl='<%# GetImageUrl(Eval("Image")) %>' runat="server" Height="100px" />
                                                <asp:Label Text='<%# Eval("SpeciesImageDescription") %>' runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
 
        --%>
    </table>

</asp:Content>
