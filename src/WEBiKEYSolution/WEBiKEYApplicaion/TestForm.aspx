<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestForm.aspx.cs" Inherits="WEBiKEY.Application.TestForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Styles/styles.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="width: 200px; overflow: scroll">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <asp:DataList ID="DataListCategories" runat="server" ViewStateMode="Enabled">
                        <ItemTemplate>
                            <h4>
                                <asp:Label ClientIDMode="Static" ID="lblCategoryName" runat="server" Text='<%# Eval("Id")  %>' />
                            </h4>
                            <asp:DataList ClientIDMode="Static" ID="DataListCharacters" ViewStateMode="Enabled" runat="server" DataSource='<%# Eval("Sub")  %>'>
                                <ItemTemplate>
                                    <h5>
                                        <asp:Label ClientIDMode="Static" ID="lblCharacterCode" runat="server" Text="2" />:
                                        <asp:Button ID="btnResetCharacter" runat="server" Text="Reset" />
                                        <asp:Button ID="btnViewImage" runat="server" Text="View Image" />
                                    </h5>
                                    <asp:RadioButtonList ID="RadioButtonListCharacterStates" AutoPostBack="true" ViewStateMode="Enabled" runat="server">
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                            </asp:DataList>
                            <br />
                        </ItemTemplate>
                    </asp:DataList>



                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                        <asp:ListItem Text="item1" />
                        <asp:ListItem Text="item2" />
                    </asp:RadioButtonList>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
