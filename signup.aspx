<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="signup.aspx.cs" Inherits="signup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- This page allows the user to sign-up -->
    <br />
    <div class="div-centerer">
        <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" OnContinueButtonClick="GoToMain" OnCreatedUser="CreateUserWizard1_CreatedUser">
            <WizardSteps>
                <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                </asp:CreateUserWizardStep>
                <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                </asp:CompleteWizardStep>
            </WizardSteps>
        </asp:CreateUserWizard>
    </div>

    <asp:Image ID="LoginImage" runat="server" ImageUrl="~/images/signup.png" BackColor="Transparent" CssClass="loginImage" />

</asp:Content>

