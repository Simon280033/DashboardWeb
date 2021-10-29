<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This is the main page of the application, and allows users - logged in or not, to search for projects in the database, and displays their info and relevant URLs-->
    <!-- Users who are logged in can edit/pin/favorite projects to make work easier on the projects relevant for the user -->
    <div class="projectName">
        <asp:Label ID="ProjectName" runat="server" Text="Search for Projects"></asp:Label>
    </div>

    <!-- We put the edit button inside an asp:Panel so we can easily toggle its availability depending on user's role (Read-only users can not edit projects!)-->
    <div class="EditPanelDiv">
        <asp:Panel ID="EditProjectPanel" runat="server">

            <asp:LinkButton ID="EditButton" CssClass="disabled" runat="server" OnClick="ChangeToEditProject">

                <asp:Image ID="EditButtonImage" runat="server" ImageUrl="~/images/updateSmall.png" BackColor="Transparent" />

            </asp:LinkButton>

            <asp:LinkButton ID="PinButton" CssClass="disabled" runat="server" OnClick="PinButtonPressed">

                <asp:Image ID="PinButtonImage" runat="server" ImageUrl="~/images/pin.png" BackColor="Transparent" />

            </asp:LinkButton>

            <asp:LinkButton ID="FavoriteButton" CssClass="disabled" runat="server" OnClick="FavoriteButtonPressed">

                <asp:Image ID="FavoriteButtonImage" runat="server" ImageUrl="~/images/favorite.png" BackColor="Transparent" />

            </asp:LinkButton>

            <asp:DropDownList ID="FavoritesDropDownList" CssClass="disabled" runat="server" OnSelectedIndexChanged="FavoriteSelected" AutoPostBack="true"></asp:DropDownList>

        </asp:Panel>
    </div>

    <!-- We enable users to search for projects in the database -->
    <!-- The autocompleteextender helps the user find the project they are looking for, if they can't remember its ID -->
    <div class="searchContainer">
        <asp:TextBox ID="TextBox1" CssClass="searchBox" placeholder="Project ID..." runat="server" AutoPostBack="true" OnTextChanged="SearchProjectOnEvent" onfocus="this.select();"></asp:TextBox>

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

        <asp:Button ID="Button1" runat="server" CssClass="searchButton" OnClick="SearchProjectOnEvent" Text="Search" />
    </div>

    <div class="div-centerer">
        <div class="colorGuide">
            <div class="circle-content">
                <div class="circle" id="customCircle"></div>
                <p>Custom</p>
            </div>
            <div class="circle-content">
                <div class="circle" id="defaultCircle"></div>
                <p>Default</p>
            </div>
            <div class="circle-content">
                <div class="circle" id="disableCircle"></div>
                <p>Disabled</p>
            </div>
        </div>
    </div>

    <!-- We make a grid containing the buttons.
         The buttons change style depending on whether or not their URL is unique or standard.
         Therefor we assign them each an ID so we can work with them in the code behind.
    -->

    <div class="urls-grid-container">

        <asp:LinkButton ID="EDocButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="EDocImage" runat="server" ImageUrl="~/images/edoc.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="EDocLabel" runat="server" Text="eDoc"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="NewPortalButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="NewPortalImage" runat="server" ImageUrl="~/images/newPortal.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="NewPortalLabel" runat="server" Text="NEW Portal"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="OldPortalButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="OldPortalImage" runat="server" ImageUrl="~/images/oldPortal.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="OldPortalLabel" runat="server" Text="OLD Portal"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="NCRButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="NCRImage" runat="server" ImageUrl="~/images/ncr.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="NCRLabel" runat="server" Text="NCR"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="ECNsButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="ECNsImage" runat="server" ImageUrl="~/images/service.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="ECNsLabel" runat="server" Text="ECNs"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="JiraButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="JiraImage" runat="server" ImageUrl="~/images/jira.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="JiraLabel" runat="server" Text="Jira"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="JiraQuotesButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="JiraQuotesImage" runat="server" ImageUrl="~/images/jiraQuotes.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="JiraQuotesLabel" runat="server" Text="Jira Quotes"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="HttpLogsButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="HttpLogsImage" runat="server" ImageUrl="~/images/httpLogs.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="HttpLogsLabel" runat="server" Text="httpLogs"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="ServiceNowButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="ServiceNowImage" runat="server" ImageUrl="~/images/service.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="ServiceNowLabel" runat="server" Text="ServiceNow"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="HoursAccountingButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="HoursAccountingImage" runat="server" ImageUrl="~/images/hoursAccounting.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="HoursAccountingLabel" runat="server" Text="HoursAccounting"></asp:Label>

        </asp:LinkButton>

    </div>

    <div class="folders-grid-container">

        <asp:LinkButton ID="QuotesButton" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="QuotesImage" runat="server" ImageUrl="~/images/quotes.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="QuotesLabel" runat="server" Text="Quotes"></asp:Label>

        </asp:LinkButton>

        <asp:LinkButton ID="H1Button" CssClass="btn" runat="server" Width="100%" onMouseOver="window.status='New Panel'; return true;"
            onMouseOut="window.status='Menu ready'; return true;">

            <asp:Image ID="H1Image" runat="server" ImageUrl="~/images/h1.png" BackColor="Transparent" />

            <br />

            <asp:Label ID="H1Label" runat="server" Text="h1"></asp:Label>

        </asp:LinkButton>

    </div>


</asp:Content>
