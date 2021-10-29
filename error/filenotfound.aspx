<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="filenotfound.aspx.cs" Inherits="error_filenotfound" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This page is shown when the user encounters a 404 error -->
    <br />
    <div class="div-centerer">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/error.png" Height="500px" Width="500px" />
    </div>
    <br />

    <div class="div-centerer">
        <h3>The page you are looking for is currently (or perhaps permanently) not available.<br />
            <br />
            Please return to the
            <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Default.aspx" runat="server">home page</asp:HyperLink>
            and try again. 
        </h3>
    </div>
</asp:Content>

