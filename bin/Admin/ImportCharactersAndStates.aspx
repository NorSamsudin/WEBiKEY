<%@ Page Title="" Language="C#" MasterPageFile="~/Master/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ImportCharactersAndStates.aspx.cs" Inherits="WEBiKEY.Application.Admin.ImportCharactersAndStates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:Wizard ID="ExcelImportWizard" runat="server" ActiveStepIndex="0" BackColor="#E6E2D8" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CancelDestinationPageUrl="~/Default.aspx"
        DisplayCancelButton="True" Font-Names="Verdana" Font-Size="Small" HeaderText="Import Characters and States" Height="300px" OnActiveStepChanged="ExcelImportWizard_ActiveStepChanged"
        OnFinishButtonClick="ExcelImportWizard_FinishButtonClick" OnNextButtonClick="ExcelImportWizard_NextButtonClick" Width="100%" OnPreviousButtonClick="ExcelImportWizard_PreviousButtonClick">
        <HeaderStyle BackColor="#666666" BorderColor="#E6E2D8" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
        <NavigationButtonStyle BackColor="White" BorderColor="#C5BBAF" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#1C5E55" />
        <SideBarButtonStyle ForeColor="White" />
        <SideBarStyle BackColor="#666666" Font-Size="Small" VerticalAlign="Top" Width="200px" BorderStyle="None" />
        <StepStyle BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderStyle="Solid" BorderWidth="2px" />
        <WizardSteps>
            <asp:WizardStep ID="WizardStep1" runat="server" Title="Select Excel File">
                <table class="cream" style="width: 100%;">

                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Excel File (2007 or Newer)"></asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="fileUploadExcel" runat="server" ViewStateMode="Enabled" />
                            <asp:Label ID="lblExcelFileInfo" runat="server" Font-Bold="False" Font-Size="XX-Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="(1st Sheet must contain data)" Font-Size="X-Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="RadioButtonListOptions" runat="server" CellSpacing="0" RepeatLayout="Flow">
                                <asp:ListItem>Append</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Overwrite">Overwrite Existing Data <strong style="color: red;">(This will delete all species info including images and description files)</strong></asp:ListItem>
                            </asp:RadioButtonList>

                        </td>
                    </tr>

                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Configure Columns">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                <asp:Label ID="lblInfo" runat="server" Text="Select an Excel File."></asp:Label>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <div id="loading" class="loading_progress">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                        <asp:GridView ID="dgvColumns" runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                            GridLines="Vertical" AutoGenerateColumns="False">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="!" HeaderText="Excel Column" />
                                <asp:TemplateField HeaderText="Database Column">
                                    <ItemTemplate>
                                        <asp:DropDownList OnSelectedIndexChanged="ItemDropDown_SelectedIndexChanged" DataSource='<%# this.items %>' DataTextField="Value" DataValueField="Key" ID="ItemDropDown" runat="server" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                        <asp:Button ID="btnValidate" runat="server" Text="Validate" OnClick="btnValidate_Click" BackColor="#333333" ForeColor="White" />
                        <asp:CustomValidator ID="CustomValidatorValidateMapping" runat="server" ErrorMessage="Error!" OnServerValidate="CustomValidatorValidateMapping_ServerValidate" Font-Bold="True" Font-Size="X-Small" ForeColor="Red"></asp:CustomValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep3" runat="server" StepType="Complete" Title="Completed">
                Import Process Completed Successfully!
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
