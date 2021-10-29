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

public partial class write_manageprojects : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayProjects();
    }

    // This method populates the gridview with data about the projects
    protected void DisplayProjects()
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT Projects.project_id AS [Project ID], customer AS [Customer name], COUNT(ISNULL(Projects.project_id,0)) - 1 AS [Views] FROM Projects LEFT JOIN Project_Viewed ON Project_Viewed.project_id = Projects.project_id GROUP BY Projects.project_id, customer ORDER BY [Views] DESC";
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
        SetButtons();
    }

    // This method finds the particular project searched for
    protected void Search()
    {
        if (String.IsNullOrEmpty(TextBox1.Text))
        {
            DisplayProjects();
            return;
        }

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT Projects.project_id AS [Project ID], customer AS [Customer name], COUNT(ISNULL(Projects.project_id,0)) - 1 AS [Views] FROM Projects LEFT JOIN Project_Viewed ON Project_Viewed.project_id = Projects.project_id WHERE Projects.project_id = @ProjectId GROUP BY Projects.project_id, customer ORDER BY [Views] DESC;";
                cmd.Parameters.AddWithValue("@ProjectId", CleanId(TextBox1.Text));
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
        SetButtons();
    }

    protected void SearchButtonClicked(object sender, EventArgs e)
    {
        Search();
    }

    protected void EditSelectedRow(object sender, EventArgs e)
    {
        // We get the project ID from the selected row
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        string projectId = row.Cells[2].Text;

        // We set it as the session variable
        Session["project_id"] = projectId;

        // We change to the edit project page
        Response.Redirect("editproject.aspx");
    }

    protected void ViewDataForSelectedRow(object sender, EventArgs e)
    {
        // We get the project ID from the selected row
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        string projectId = row.Cells[2].Text;

        // We set it as the session variable
        Session["project_id"] = projectId;

        // We change to the edit project page
        Response.Redirect("~/write/viewprojectdata.aspx");
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
                cmd.CommandText = "select project_id, customer from Projects where " +
                "project_id like '%' + @SearchText + '%' OR customer like '%' + @SearchText + '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                cmd.Connection = conn;
                conn.Open();
                List<string> customers = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(sdr["project_id"].ToString() + " - " + sdr["customer"].ToString());
                    }
                }
                conn.Close();
                return customers;
            }
        }
    }

    // This method attempts to figure to get an ID from the user's search
    public string CleanId(String searchText)
    {
        string[] parts = searchText.Split(new[] { " - " }, StringSplitOptions.None); // The string is split 

        return parts[0];
    }

    // This method is called when a user searches for a project
    protected void SearchProjectOnEvent(object sender, EventArgs e)
    {
        TextBox1.Text = CleanId(TextBox1.Text);
        Search();
    }


    // We need this method to enable paging since we don't use a source in the standard way, but bind it manually in the code behind
    protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();

        // We must assign unique IDs to our buttons manually, in order to get them to work
        SetButtons();
    }

    // This method styles and assigns IDs to buttons, to get them to work properly
    protected void SetButtons()
    {
        foreach (GridViewRow grdRw in GridView1.Rows)
        {
            Button dataBtn = (Button)grdRw.Cells[0].Controls[1];

            dataBtn.ID = "dataBtn_" + grdRw.RowIndex.ToString();

            dataBtn.CssClass = "blueButton";

            Button editBtn = (Button)grdRw.Cells[1].Controls[1];

            editBtn.ID = "editBtn_" + grdRw.RowIndex.ToString();

            editBtn.CssClass = "greenButton";
        }
    }
}