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

public partial class write_viewuserdata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // We make sure that a valid session variable exists, as this determines which user we are viewing data for
        if (!(Session.Count > 0 && Session["username"] != null))
        {
            Response.Redirect("~/Default.aspx");
        }

        SetUserInfo();

        SetParameters();
        DisplayProjectViews();
        DisplayProjectEdits();
    }

    // This method sets the parameters to the datasources, so we can get the right results for the particular project
    protected void SetParameters()
    {
        ViewsSource.SelectParameters.Clear();
        EditsSource.SelectParameters.Clear();
        ViewsSource.SelectParameters.Add("UserName", Session["username"].ToString());
        EditsSource.SelectParameters.Add("UserName", Session["username"].ToString());
    }

    // We need this method to enable paging since we don't use a source in the standard way, but bind it manually in the code behind
    protected void OnViewedPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewedAtGridView.PageIndex = e.NewPageIndex;
        ViewedAtGridView.DataBind();
    }

    protected void OnEditedPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        EditedAtGridView.PageIndex = e.NewPageIndex;
        EditedAtGridView.DataBind();
    }

    // This methods adds data about the projects views to the gridview
    protected void DisplayProjectViews()
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT Projects.project_id AS [ID], Projects.customer AS [Customer], viewed_at AS [Viewed at] FROM Projects JOIN Project_Viewed ON Project_Viewed.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Viewed.UserId WHERE aspnet_Users.UserName = @UserName GROUP BY Projects.project_id, customer, aspnet_Users.UserName, Project_Viewed.viewed_at ORDER BY [Viewed at] DESC";
                cmd.Parameters.AddWithValue("@UserName", Session["username"].ToString());
                cmd.Connection = conn;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                ViewedAtGridView.DataSource = dt;
                ViewedAtGridView.DataBind();
            }
        }
    }

    // This methods adds data about the projects views to the gridview
    protected void DisplayProjectEdits()
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT Projects.project_id AS [ID], Projects.customer AS [Customer], edited_at AS [Edited at] FROM Projects JOIN Project_Edits ON Project_Edits.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Edits.UserId WHERE aspnet_Users.UserName = @UserName GROUP BY Projects.project_id, customer, aspnet_Users.UserName, Project_Edits.edited_at ORDER BY [Edited at] DESC";
                cmd.Parameters.AddWithValue("@UserName", Session["username"].ToString());
                cmd.Connection = conn;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                EditedAtGridView.DataSource = dt;
                EditedAtGridView.DataBind();
            }
        }
    }

    // This method sets the users overall data in the labels
    protected void SetUserInfo()
    {
        // We get the user's name
        string username = Session["username"].ToString();
        // We get the role
        string[] roles = Roles.GetRolesForUser(username);
        // We get the creation and last login date
        Tuple<string, string> userInfo = CreationAndLastLogin(username);

        // We set the labels
        NameLabel.Text = username;
        RoleLabel.Text = roles[0];
        CreatedLabel.Text = userInfo.Item1;
        LastLoginLabel.Text = userInfo.Item2;
    }

    // This method returns the user's creation data and last login date
    protected Tuple<string, string> CreationAndLastLogin(string username)
    {
        Tuple<string, string> userInfo = null;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT aspnet_Membership.LastLoginDate AS [LastLogin], aspnet_Membership.CreateDate AS [Created] FROM aspnet_Membership JOIN aspnet_Users ON aspnet_Users.UserId = aspnet_Membership.UserId WHERE aspnet_Users.UserId = (SELECT Top 1 UserId FROM aspnet_Users WHERE aspnet_Users.UserName = @UserName);";
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        userInfo = new Tuple<string, string>(sdr["Created"].ToString(), sdr["LastLogin"].ToString());
                    }
                }
                conn.Close();

                return userInfo;
            }
        }
    }
}