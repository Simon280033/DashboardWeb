using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Security;

public partial class manageusers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayUsers();
    }

    // This method populates the gridview with the users and their relevant data
    protected void DisplayUsers()
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                // Deleted users are NOT removed from aspnet_Users, but they are from membership. So we get the valid ones from there!
                cmd.CommandText = "SELECT aspnet_Users.UserName AS [User], COUNT(ISNULL(Project_Viewed.project_id,0)) AS [Total project views] FROM aspnet_Users JOIN aspnet_Membership ON aspnet_Users.UserId = aspnet_Membership.UserId JOIN Project_Viewed ON aspnet_Users.UserId = Project_Viewed.UserId WHERE aspnet_Membership.UserId IS NOT NULL GROUP BY aspnet_Users.UserName";
                cmd.Connection = conn;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        // We must assign unique IDs to our buttons manually, in order to get them to work
        SetButtonsAndRoles();
    }

    // This method searches for users based on input text
    protected void Search()
    {
        if (String.IsNullOrEmpty(TextBox1.Text))
        {
            DisplayUsers();
            return;
        }

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                // Deleted users are NOT removed from aspnet_Users, but they are from membership. So we get the valid ones from there!
                cmd.CommandText = "SELECT aspnet_Users.UserName AS [User], COUNT(ISNULL(Project_Viewed.project_id,0)) AS [Total project views] FROM aspnet_Users JOIN aspnet_Membership ON aspnet_Users.UserId = aspnet_Membership.UserId JOIN Project_Viewed ON aspnet_Users.UserId = Project_Viewed.UserId WHERE aspnet_Membership.UserId IS NOT NULL AND aspnet_Users.UserId = (SELECT Top 1 UserId FROM aspnet_Users WHERE UserName = @Username) GROUP BY aspnet_Users.UserName";
                cmd.Parameters.AddWithValue("@Username", TextBox1.Text);
                cmd.Connection = conn;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        // We must assign unique IDs to our buttons manually, in order to get them to work
        SetButtonsAndRoles();
    }

    // This method ensures that we can run the search method both from button click, but also from other methods more cleanly
    protected void SearchButtonClicked(object sender, EventArgs e)
    {
        Search();
    }

    // This method changes the user of the selected row's role, to the new one chosen on the dropdownlist
    protected void UpdateRoleForRow(object sender, EventArgs e)
    {
        // We get the username
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        string username = row.Cells[3].Text;

        // We get the desired role
        DropDownList rolesDd = (DropDownList)row.Cells[2].Controls[1];
        string role = rolesDd.SelectedValue;

        // We check if they have selected a new role at all
        if (Roles.IsUserInRole(username, role))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please select a new role for the user!')", true);
            return;
        }

        // We remove the user from his current role to avoid double roles
        string[] currentRoles = Roles.GetRolesForUser(username);
        if (currentRoles != null && currentRoles.Length > 0)
        {
            Roles.RemoveUserFromRoles(username, currentRoles);
        }

        Roles.AddUserToRole(username, role);

        // We alert the user
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Role updated successfully!')", true);
    }

    // This method stores the username for the user we want to see data for in a session variable, and redirects to the userdata page
    protected void ViewDataForSelectedRow(object sender, EventArgs e)
    {
        // We get the username from the selected row
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        string username = row.Cells[3].Text;

        // We set it as the session variable
        Session["username"] = username;

        // We change to the edit project page
        Response.Redirect("~/write/viewuserdata.aspx");
    }

    // Adding search suggestions
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetCompletionList(String prefixText, int count)
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select distinct UserName from aspnet_Users where " +
                "UserName like '%' + @SearchText + '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                cmd.Connection = conn;
                conn.Open();
                List<string> customers = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(sdr["UserName"].ToString());
                    }
                }
                conn.Close();
                return customers;
            }
        }
    }

    // This method is called when a user searches for a project
    protected void SearchUserOnEvent(object sender, EventArgs e)
    {
        Search();
    }


    // We need this method to enable paging since we don't use a source in the standard way, but bind it manually in the code behind
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();

        SetButtonsAndRoles();
    }

    // This method assigns each button in the gridview a unique ID and style. They need unique IDs to work properly!
    protected void SetButtonsAndRoles()
    {
        // We must assign unique IDs to our buttons manually, in order to get them to work
        foreach (GridViewRow grdRw in GridView1.Rows)
        {
            Button dataBtn = (Button)grdRw.Cells[0].Controls[1];

            dataBtn.ID = "dataBtn_" + grdRw.RowIndex.ToString();

            dataBtn.CssClass = "blueButton";

            Button editBtn = (Button)grdRw.Cells[1].Controls[1];

            editBtn.ID = "editBtn_" + grdRw.RowIndex.ToString();

            editBtn.CssClass = "greenButton";

            DropDownList rolesDd = (DropDownList)grdRw.Cells[2].Controls[1];

            rolesDd.ID = "rolesDd_" + grdRw.RowIndex.ToString();

            // We set the role in the dropdownlist
            if (Roles.IsUserInRole(grdRw.Cells[3].Text, "Admin"))
            {
                rolesDd.SelectedIndex = 2;
            }

            if (Roles.IsUserInRole(grdRw.Cells[3].Text, "Write"))
            {
                rolesDd.SelectedIndex = 1;
            }

            if (Roles.IsUserInRole(grdRw.Cells[3].Text, "Read"))
            {
                rolesDd.SelectedIndex = 0;
            }
        }
    }
}