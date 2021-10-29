<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="createproject.aspx.cs" Inherits="write_createproject" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This page allows users with write privileges or above to create new projects, if they have a unique ID -->
    <div class="pageHeader">
        <asp:Label ID="createProjectHeaderLabel" runat="server">Create Project</asp:Label>
    </div>

    <div class="contentCenterer">
        <p>To create a new project, please fill out all details below, and press the 'Create Project' button.</p>
    </div>

    <div class="contentCenterer">
        <asp:Button ID="Button1" runat="server" Text="Create Project" CssClass="greenButton" OnClick="CreateButton_Click" />
    </div>

    <!-- Below are all the forms for the required info to create a new project -->
    <div class="contentFrame">
        <div class="div-centerer">
            <div class="grid-container">

                <asp:Panel ID="IdPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image13" runat="server" ImageUrl="~/images/id.png" BackColor="Transparent" />
                        <p><b>ID: (Must be unique!) </b></p>
                        <div class="InsideContent">
                            <div class="InsideContent">
                                <asp:TextBox ID="ProjectIdTextBox" CssClass="textbox" placeholder="Project ID..." runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="TitleEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="QuotesImage" runat="server" ImageUrl="~/images/title.png" BackColor="Transparent" />
                        <p><b>Title: </b></p>
                        <div class="InsideContent">
                            <div class="InsideContent">
                                <asp:TextBox ID="ProjectTitleBox" CssClass="textbox" placeholder="Project name..." runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

            </div>
        </div>

        <div class="contentCenterer">
        <!-- User is notified if the ID they have entered is invalid (if it contains non-numerical characters) -->
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ProjectIdTextBox" ErrorMessage="Alert: Only numbers allowed in IDs!" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
        </div>

        <div class="contentCenterer">

            <div class="project-data-grid-container">

                <asp:Panel ID="eDocEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/edoc.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>eDoc URL:</p>

                        <div class="InsideContent">
                            <asp:TextBox ID="eDocUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                            <asp:Button ID="edoc" runat="server" CssClass="blueButton" Text="Generate default" OnClick="GetUrl" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="NewPortalEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/newPortal.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>New Portal URL: </p>

                        <div class="InsideContent">
                            <div class="InsideContent">
                                <asp:TextBox ID="NewPortalUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="newportal" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="OldPortalEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/oldPortal.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>Old Portal URL: </p>

                        <div class="InsideContent">
                            <div class="InsideContent">
                                <asp:TextBox ID="OldPortalUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="oldportal" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="NcrEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image4" runat="server" ImageUrl="~/images/ncr.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>NCR URL: </p>
                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="NcrUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="ncr" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="EcnsEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image5" runat="server" ImageUrl="~/images/ecns.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>ECNs URL: </p>

                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="EcnsUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="ecns" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="JiraEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image6" runat="server" ImageUrl="~/images/jira.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>Jira URL: </p>
                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="JiraUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="jira" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="JiraQuotesEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image7" runat="server" ImageUrl="~/images/jiraQuotes.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>Jira Quotes URL: </p>

                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="JiraQuotesUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="jiraquotes" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="HttpLogsEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image8" runat="server" ImageUrl="~/images/httpLogs.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>HTTP Logs URL: </p>

                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="HttpLogsUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="httplogs" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="ServiceNowEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image9" runat="server" ImageUrl="~/images/service.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>ServiceNow URL: </p>
                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="ServiceNowUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="servicenow" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="HoursAccountingEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image10" runat="server" ImageUrl="~/images/hoursAccounting.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>HoursAccounting URL: </p>

                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="HoursAccountingUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="hoursaccounting" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="QuotesEditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image11" runat="server" ImageUrl="~/images/quotes.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>Quotes URL: </p>

                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="QuotesUrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="quotes" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="H1EditPanel" CssClass="ProjectData" runat="server" HorizontalAlign="center">
                    <div class="InsideContent">
                        <asp:Image ID="Image12" runat="server" ImageUrl="~/images/h1.png" BackColor="Transparent" Width="50" Height="50" />
                        <p>h1 URL: </p>

                        <div class="InsideContent">

                            <div class="InsideContent">
                                <asp:TextBox ID="H1UrlBox" CssClass="textbox" runat="server"></asp:TextBox>
                                <asp:Button ID="h1" runat="server" CssClass="blueButton" Text="Generate default" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>

            </div>
        </div>
    </div>
</asp:Content>
