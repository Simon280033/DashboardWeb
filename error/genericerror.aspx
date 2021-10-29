<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="genericerror.aspx.cs" Inherits="error_genericerror" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This page is shown whenever the user encounters an error that is not 404 -->
    <br />
    <div class="div-centerer">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/error.png" Height="500px" Width="500px" />
    </div>
    <br />

    <div class="div-centerer">
        <h3>An error occured.
            <br />
            <br />

            Please return to the
            <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Default.aspx" runat="server">home page</asp:HyperLink>
            and try again. 
        </h3>
    </div>
</asp:Content>

