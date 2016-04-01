<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SiteMaster.Master" ViewStateMode="Enabled" AutoEventWireup="true" CodeBehind="Key.aspx.cs" Inherits="WEBiKEY.Application.Key" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanelBtns" runat="server">
        <ContentTemplate>
            <asp:Button CssClass="button_style" ID="btnResetAllStates" runat="server" Text="Reset All" OnClick="btnResetAllStates_Click" />
            <asp:Button CssClass="button_style" ID="btnHideImage" runat="server" Text="Hide Image" OnClick="btnHideImage_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server">
        <ProgressTemplate>
            <div class="loading-panel">
                <div class="loading-indicator">
                    Loading....
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="characters">
        <div id="characterList">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:DataList ID="DataListCategories" runat="server" ViewStateMode="Enabled" ClientIDMode="Predictable">
                        <ItemTemplate>
                            <div class="charCat">
                                <h4>
                                    <asp:Label ClientIDMode="Predictable" ID="lblCategoryName" runat="server" Text='<%#  Eval("CategoryName") %>' />
                                </h4>
                                <asp:DataList ClientIDMode="Predictable" ID="DataListCharacters" ViewStateMode="Enabled" runat="server" DataSource='<%# (IEnumerable<WEBiKEY.Application.Character>)Eval("Characters") %>'>
                                    <ItemTemplate>
                                        <h5>
                                            <asp:Label ClientIDMode="Static" ID="lblCharacterCode" runat="server" Text='<%#  Eval("CharacterCode") %>' />:
                                            <asp:Label ClientIDMode="Static" ID="lblCharacterId" runat="server" Text='<%#  Eval("CharacterID") %>' Visible="False" />
                                            <asp:Label ClientIDMode="Static" ID="lblCharacterDescription" runat="server" Text='<%#  Eval("CharacterDescription") %>' />
                                            <asp:Button CssClass="button_style" ID="btnResetCharacter" runat="server" Text="Reset" OnClick="btnResetCharacter_Click" />
                                            <asp:Button CssClass="button_style" ID="btnViewImage" runat="server" Text="View Image" OnClick="btnViewImage_Click" Visible='<%# (Eval("Image")!=null) %>' />
                                        </h5>
                                        <div class="charStateSet">
                                            <asp:RadioButtonList ID="RadioButtonListCharacterStates" OnSelectedIndexChanged="RadioButtonListCharacterStates_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Predictable"
                                                ViewStateMode="Enabled" DataTextField="CharacterStateDescriptionWithCode"
                                                DataValueField="CharacterStateID" DataSource='<%# (IEnumerable<WEBiKEY.Application.CharacterState>)Eval("CharacterStates") %>' runat="server" />
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="characterImages">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <h4 id="h4Images" runat="server"></h4>
                    <asp:Image ID="imgCharacterImage" EnableViewState="true" runat="server" ClientIDMode="Static" />
                    <h5 id="h4ImageCopyrights" Visible="False" runat="server">Image courtesy of Bamboo Phylogeny Group, 2005.<br/><a href="http://www.eeob.iastate.edu/research/bamboo/characters.html" target="_blank">Bamboo Biodiversity, Iowa State University.</a></h5>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
    <div class="clear"></div>
    <div id="species">
        <div id="speciesIncluded">
            <h5>Selected Species</h5>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:ListBox ID="ListBoxSpeciesIncl" runat="server" Height="120px" Width="100%"></asp:ListBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="speciesExcluded">
            <h5>Eliminated Species</h5>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:ListBox ID="ListBoxSpeciesExcl" runat="server" Height="120px" Width="100%"></asp:ListBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="clear"></div>

</asp:Content>
