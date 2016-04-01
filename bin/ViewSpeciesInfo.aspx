<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SiteMaster.Master" AutoEventWireup="true" CodeBehind="ViewSpeciesInfo.aspx.cs" Inherits="WEBiKEY.Application.ViewSpeciesInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">

    <asp:EntityDataSource ID="EntityDataSourceSpecies" runat="server" ConnectionString="name=InteractiveKeyEntities" DefaultContainerName="InteractiveKeyEntities" EnableFlattening="False"
        EntitySetName="Species" EntityTypeFilter="Species" OrderBy="it.SpeciesName">
    </asp:EntityDataSource>
    <div style="padding: 0px 20px 0px 20px;">
        <table>
            <asp:DataList ID="DataList1" runat="server" DataKeyField="SpeciesID" DataSourceID="EntityDataSourceSpecies">
                <ItemTemplate>
                    <tr>
                        <td>
                            <em>
                                <asp:Label ID="SpeciesNameLabel" runat="server" Text='<%# Eval("SpeciesName") %>' /></em></td>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# (Eval("DescriptionFileName")==null? "": Eval("DescriptionFileName").ToString().Substring (Eval("DescriptionFileName").ToString().Length-3, 3).ToUpper()=="PDF"? "PDF": "DOC")=="PDF"? "~/Images/pdf-icon32.png":"~/Images/Document-icon32.png"  %>' />
                        </td>
                        <td>
                            <asp:Button ID="DownloadButton" ValidationGroup='<%# Eval("SpeciesID") %>' Enabled='<%# IsDescriptionFileAvailable(Eval("DescriptionFile")) %>' Text='<%# GetDownloadButtonText(Eval("DescriptionFile")) %>'
                                runat="server" OnClick="ButtonDownload_Click"></asp:Button></td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
    </div>
</asp:Content>
