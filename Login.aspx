<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This page is shown when the user needs to login -->
    <br />
    <br />
    <div class="div-centerer">
        <asp:Login ID="Login1" runat="server"></asp:Login>
    </div>
    <div class="div-centerer">
        <p>To create a new user, please </p>
        <p><a href="signup.aspx">‎‏‏‎ ‏‏‎ ‎‎signup.</a></p>
    </div>

    <asp:Image ID="LoginImage" runat="server" ImageUrl="~/images/login.png" BackColor="Transparent" CssClass="loginImage" />

</asp:Content>

