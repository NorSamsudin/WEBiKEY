<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ConfigureCharacterDependencies.aspx.cs"
    Inherits="WEBiKEY.Application.Admin.ConfigureCharacterDependencies" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #AddRemoveDependencies {
            height: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div id="AddRemoveDependencies">
        <h5>Manage Character Dependencies here.</h5>
        <table>
            <tr>
                <td>Character</td>
                <td>State</td>
                <td>Disabled Character</td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="cmbCharacter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbCharacter_SelectedIndexChanged"
                        Width="140px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cmbState" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbState_SelectedIndexChanged"
                        Width="120px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="cmbDisabledCharacter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cmbDisabledCharacter_SelectedIndexChanged"
                        Width="140px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnUpdate" runat="server" Text="Add" Enabled="False" OnClick="btnUpdate_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="ExistingDependencies">
        <asp:GridView ID="gvExistingDependencies" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            DataSourceID="EntityDataSource" OnRowCommand="gvExistingDependencies_RowCommand"
            OnRowDataBound="gvExistingDependencies_RowDataBound" CellPadding="3" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellSpacing="2">
            <Columns>
                <asp:BoundField DataField="CharacterCode" HeaderText="Character Code" ReadOnly="True" SortExpression="CharacterCode" />
                <asp:BoundField DataField="CharacterStateID" HeaderText="Character StateID" ReadOnly="True" SortExpression="CharacterStateID" Visible="False" />
                <asp:BoundField DataField="CharacterStateCode" HeaderText="Character State Code" ReadOnly="True" SortExpression="CharacterStateCode" />
                <asp:BoundField DataField="CharacterStateDescription" HeaderText="Character State Description" ReadOnly="True" SortExpression="CharacterStateDescription" />
                <asp:BoundField DataField="DisabledCharacterCode" HeaderText="Disabled Character Code" ReadOnly="True" SortExpression="DisabledCharacterCode" />
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1"
                            CommandArgument='<%# new Tuple<object, object>( Eval("CharacterStateID"), Eval("DisabledCharacterID")) %>'
                            CommandName="CustomDelete" runat="server">Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FFF1D4" />
            <SortedAscendingHeaderStyle BackColor="#B95C30" />
            <SortedDescendingCellStyle BackColor="#F1E5CE" />
            <SortedDescendingHeaderStyle BackColor="#93451F" />
        </asp:GridView>
        <asp:EntityDataSource ID="EntityDataSource" runat="server" ConnectionString="name=InteractiveKeyEntities"
            DefaultContainerName="InteractiveKeyEntities" EnableFlattening="False" EntitySetName="CharacterDependencyViews"
            EntityTypeFilter="CharacterDependencyView"
            Select="it.[CharacterID], it.[CharacterCode], it.[CharacterDescription], it.[CharacterStateID], it.[CharacterStateCode], it.[CharacterStateDescription], it.[DisabledCharacterID], it.[DisabledCharacterCode], it.[DisabledCharacterDescription]">
        </asp:EntityDataSource>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server">
        </asp:CheckBoxList>
    </div>
</asp:Content>
