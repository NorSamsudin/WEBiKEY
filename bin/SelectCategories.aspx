<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SiteMaster.Master" AutoEventWireup="true" CodeBehind="SelectCategories.aspx.cs" Inherits="WEBiKEY.Application.SelectCategories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div id="cat_desc" class="shadow_round">
        <h3>Select the Characters you have in your specimen</h3>
    </div>
    <div style="float: right; margin-right:100px;">
        <img src="Images/bamboo_abstract.png" />
    </div>
    <div id="cats">
        <asp:CheckBoxList ID="chkLstCategories" runat="server"></asp:CheckBoxList>
    </div>
    <div id="cat_buttons">
        <asp:Button CssClass="button_style" ID="btnSelectAll" runat="server" Text="Select all" OnClick="btnSelectAll_Click" />
        <asp:Button CssClass="button_style" ID="btnSelectNone" runat="server" Text="Select none" OnClick="btnSelectNone_Click" />
        <asp:Button CssClass="button_style" ID="btnNext" runat="server" Text="Next..." OnClick="btnNext_Click" />
        <asp:Label ID="lblInfo" runat="server" ForeColor="#FF3300"></asp:Label>
    </div>
</asp:Content>
