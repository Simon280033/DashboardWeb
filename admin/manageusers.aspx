<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="manageusers.aspx.cs" Inherits="manageusers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This page allows admins to manage users - Get an overview over all the users, change their roles and view their viewing/editing history -->
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

    <div class="pageHeader">
        <asp:Label ID="manageUsersHeaderLabel" runat="server">Manage Users</asp:Label>
    </div>

    <div class="contentCenterer">
        <p>Alter the roles of users or view their activity.</p>
    </div>

    <div class="contentCenterer">
        <!-- We make sure that the search method is called when the user picks a suggestion from the autocompleteextender -->
        <asp:TextBox ID="TextBox1" CssClass="searchBox" placeholder="Project ID..." runat="server" AutoPostBack="true" OnTextChanged="SearchUserOnEvent" onfocus="this.select();"></asp:TextBox>

        <!-- The user is given search suggestions based on their input, makes it much easier to find projects if you are unsure of their exact ID -->
        <cc1:AutoCompleteExtender
            ServiceMethod="GetCompletionList"
            MinimumPrefixLength="2"
            CompletionInterval="100"
            EnableCaching="false"
            CompletionSetCount="10"
            TargetControlID="TextBox1"
            ID="AutoCompleteExtender1"
            runat="server"
            FirstRowSelected="false">
        </cc1:AutoCompleteExtender>

        <asp:Button ID="Button1" runat="server" CssClass="searchButton" OnClick="SearchButtonClicked" Text="Search" />
    </div>

    <div class="contentCenterer">
        <!-- We set AutoGenerateColumns="False" so we can specify the columns ourselves. This way, we can get the role in the same query and use its value without showing it as a standard column -->
        <!-- When the gridview index is changed, the page is reloaded. Therefor, we must run a method that sets everything again, so that it all works as intended -->
        <!-- We set the columns manually, so we can get them exactly how we want -->
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText="No users found!" AllowPaging="true" PageSize="12" OnPageIndexChanging="OnPageIndexChanging">
            <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="viewDataButton" runat="server" OnClick="ViewDataForSelectedRow" Text="View data" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="editButton" runat="server" OnClick="UpdateRoleForRow" Text="Update role" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <asp:DropDownList ID="RolesDropDownList" runat="server">
                            <asp:ListItem Text="Read" Value="Read"></asp:ListItem>
                            <asp:ListItem Text="Write" Value="Write"></asp:ListItem>
                            <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="User" HeaderText="User" />
                <asp:BoundField DataField="Total Project views" HeaderText="Total Project views" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
