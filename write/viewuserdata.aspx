<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewuserdata.aspx.cs" Inherits="write_viewuserdata" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This page allows admins to view data about a particular user, including name/role/creation date/last login and their view/edit history for projects -->
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

    <div class="pageHeader">
        <asp:Label ID="viewUserDataHeaderLabel" runat="server">View User Data</asp:Label>
    </div>

    <div class="contentCenterer">
        <p>Get an overview of the users activity.</p>
    </div>

    <!-- Charts and gridviews allow the user to get both a micro/macro overview over the data -->
    <div class="contentCenterer">
        <div class="grid-container">
            <div class="user-data-grid">
                <div class="user-data-grid-item">
                    <asp:Label ID="Label1" runat="server" Text="Username: " Font-Bold="true"></asp:Label>
                </div>

                <div class="user-data-grid-item">
                    <asp:Label ID="NameLabel" runat="server" Text="Label"></asp:Label>
                </div>

                <div class="user-data-grid-item">
                    <asp:Label ID="Label2" runat="server" Text="Role: " Font-Bold="true"></asp:Label>
                </div>
                <div class="user-data-grid-item">
                    <asp:Label ID="RoleLabel" runat="server" Text="Label"></asp:Label>
                </div>
            </div>

            <div class="user-data-grid">
                <div class="user-data-grid-item">
                    <asp:Label ID="Label3" runat="server" Text="Created: " Font-Bold="true"></asp:Label>
                </div>

                <div class="user-data-grid-item">
                    <asp:Label ID="CreatedLabel" runat="server" Text="Label"></asp:Label>
                </div>

                <div class="user-data-grid-item">
                    <asp:Label ID="Label5" runat="server" Text="Last login: " Font-Bold="true"></asp:Label>
                </div>
                <div class="user-data-grid-item">
                    <asp:Label ID="LastLoginLabel" runat="server" Text="Label"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <div class="contentCenterer">

        <div class="stats-grid-container">

            <p class="centered-grid-item"><b>Views by project (Top 5 most viewed)</b></p>
            <p class="centered-grid-item"><b>View history</b></p>

            <asp:Chart ID="ViewsChart" runat="server" DataSourceID="ViewsSource" CssClass="centered-grid-item">
                <Series>
                    <asp:Series Name="ViewSeries" XValueMember="Project" YValueMembers="Views"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea"></asp:ChartArea>
                </ChartAreas>
            </asp:Chart>

            <asp:GridView ID="ViewedAtGridView" CssClass="centered-grid-item" runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No views found!" AllowPaging="true" PageSize="8" OnPageIndexChanging="OnViewedPageIndexChanging">
                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
            </asp:GridView>
        </div>
    </div>
    <div class="contentCenterer">

        <div class="stats-grid-container">

            <p class="centered-grid-item"><b>Edits by project (Top 5 most edited)</b></p>
            <p class="centered-grid-item"><b>Edits overview</b></p>

            <asp:Chart ID="Chart1" runat="server" DataSourceID="EditsSource" CssClass="centered-grid-item">
                <Series>
                    <asp:Series Name="ViewSeries" XValueMember="Project" YValueMembers="Edits"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea"></asp:ChartArea>
                </ChartAreas>
            </asp:Chart>

            <asp:GridView ID="EditedAtGridView" CssClass="centered-grid-item" runat="server" ShowHeaderWhenEmpty="True" EmptyDataText="No edits found!" AllowPaging="true" PageSize="8" OnPageIndexChanging="OnEditedPageIndexChanging">
                <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
            </asp:GridView>

        </div>
    </div>

    <!-- This datasource gets top 5 most viewed projects for this user, and the amount they have viewed each-->
    <asp:SqlDataSource ID="ViewsSource" runat="server"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        SelectCommand="SELECT TOP 5 Projects.customer AS [Project], COUNT(ISNULL(Projects.project_id,0)) AS [Views] FROM Projects JOIN Project_Viewed ON Project_Viewed.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Viewed.UserId WHERE aspnet_Users.UserName = @UserName GROUP BY Projects.customer"></asp:SqlDataSource>
    
    <!-- This datasource gets top 5 most edited projects for this user, and the amount they have edited each-->
    <asp:SqlDataSource ID="EditsSource" runat="server"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        SelectCommand="SELECT TOP 5 Projects.customer AS [Project], COUNT(ISNULL(Projects.project_id,0)) AS [Edits] FROM Projects JOIN Project_Edits ON Project_Edits.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Edits.UserId WHERE aspnet_Users.UserName = @UserName GROUP BY Projects.customer"></asp:SqlDataSource>

</asp:Content>
