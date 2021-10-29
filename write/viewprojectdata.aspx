<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewprojectdata.aspx.cs" Inherits="read_viewprojectdata" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- On this page, the user can see the view/edit history for a project and its most viewing/editing users -->
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>

    <div class="pageHeader">
        <asp:Label ID="viewProjectDataHeaderLabel" runat="server">View Project Data</asp:Label>
    </div>

    <div class="contentCenterer">
        <p>Get an overview of the editing/viewing activity for the given project.</p>
    </div>

    <!-- Charts and gridviews allow the user to get both a micro/macro overview over the data -->
    <div class="contentCenterer">

        <div class="stats-grid-container">

            <p class="centered-grid-item"><b>Views by user (Top 5 most viewed by)</b></p>
            <p class="centered-grid-item"><b>Views overview</b></p>

            <asp:Chart ID="ViewsChart" runat="server" DataSourceID="ViewsSource" CssClass="centered-grid-item">
                <Series>
                    <asp:Series Name="ViewSeries" XValueMember="User" YValueMembers="Views"></asp:Series>
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

            <p class="centered-grid-item"><b>Edits by user (Top 5)</b></p>
            <p class="centered-grid-item"><b>Edits overview</b></p>

            <asp:Chart ID="Chart1" runat="server" DataSourceID="EditsSource" CssClass="centered-grid-item">
                <Series>
                    <asp:Series Name="ViewSeries" XValueMember="User" YValueMembers="Edits"></asp:Series>
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

    <!-- This datasource gets top 5 viewers of the project, and their individual view counts-->
    <asp:SqlDataSource ID="ViewsSource" runat="server"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        SelectCommand="SELECT TOP 5 aspnet_Users.UserName AS [User], COUNT(ISNULL(Projects.project_id,0)) AS [Views] FROM Projects JOIN Project_Viewed ON Project_Viewed.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Viewed.UserId WHERE Projects.project_id = @ProjectId GROUP BY aspnet_Users.UserName"></asp:SqlDataSource>

    <!-- This datasource gets top 5 editors of the project, and their individual edit counts-->
    <asp:SqlDataSource ID="EditsSource" runat="server"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        SelectCommand="SELECT TOP 5 aspnet_Users.UserName AS [User], COUNT(ISNULL(Projects.project_id,0)) AS [Edits] FROM Projects JOIN Project_Edits ON Project_Edits.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Edits.UserId WHERE Projects.project_id = @ProjectId GROUP BY aspnet_Users.UserName"></asp:SqlDataSource>

</asp:Content>
