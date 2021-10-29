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

public partial class read_viewprojectdata : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // We make sure that a valid session variable exists, as it determines which project we are viewing data for
        if (!(Session.Count > 0 && Session["project_id"] != null))
        {
            Response.Redirect("~/Default.aspx");
        }

        // We set the header label
        viewProjectDataHeaderLabel.Text = "View Project Data for project '" + Session["project_id"].ToString() + "'";

        SetParameters();
        DisplayProjectViews();
        DisplayProjectEdits();
    }

    // This method sets the parameters to the datasources, so we can get the right results for the particular project
    protected void SetParameters()
    {
        // We get a double parameter exception if we go to another page on the gridviews, unless we clear the parameters on each pageload
        ViewsSource.SelectParameters.Clear();
        EditsSource.SelectParameters.Clear();

        ViewsSource.SelectParameters.Add("ProjectId", Session["project_id"].ToString());
        EditsSource.SelectParameters.Add("ProjectId", Session["project_id"].ToString());
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
                cmd.CommandText = "SELECT UserName AS [User], viewed_at AS [Viewed at] FROM Projects JOIN Project_Viewed ON Project_Viewed.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Viewed.UserId WHERE Projects.project_id = @ProjectId GROUP BY Projects.project_id, customer, aspnet_Users.UserName, Project_Viewed.viewed_at ORDER BY [Viewed at] DESC";
                cmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
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
                cmd.CommandText = "SELECT UserName AS [User], edited_at AS [Edited at] FROM Projects JOIN Project_Edits ON Project_Edits.project_id = Projects.project_id JOIN aspnet_Users ON aspnet_Users.UserId = Project_Edits.UserId WHERE Projects.project_id = @ProjectId GROUP BY Projects.project_id, customer, aspnet_Users.UserName, Project_Edits.edited_at ORDER BY [Edited at] DESC";
                cmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
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
}