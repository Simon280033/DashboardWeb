<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loggedout.aspx.cs" Inherits="loggedout" %>

<!DOCTYPE html>
<!-- This page is shown when a user logs out -->
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logged out</title>
    <meta http-equiv="refresh" content="2;url=Default.aspx" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="div-centerer">
            <asp:Image ID="QuotesImage" runat="server" ImageUrl="~/images/logout.png" BackColor="Transparent" Height="300px" Width="300px" />
        </div>
        <div class="div-centerer">
            <h2>You are now logged out, see you! Redirecting to front page in 2 seconds...</h2>
        </div>
    </form>
</body>
</html>
