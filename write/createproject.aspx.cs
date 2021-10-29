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

public partial class write_createproject : System.Web.UI.Page
{
    private Control[] textBoxes;
    private Control[] buttons;

    protected void Page_Load(object sender, EventArgs e)
    {
        // We collect textboxes and buttons in lists, so we can iterate over them for shorter and more readable/maintainable code
        textBoxes = new Control[] { ProjectIdTextBox, ProjectTitleBox, eDocUrlBox, NewPortalUrlBox, OldPortalUrlBox, NcrUrlBox, EcnsUrlBox, JiraUrlBox, JiraQuotesUrlBox, HttpLogsUrlBox, ServiceNowUrlBox, HoursAccountingUrlBox, QuotesUrlBox, H1UrlBox };
        buttons = new Control[] { edoc, newportal, oldportal, ncr, ecns, jira, jiraquotes, httplogs, servicenow, hoursaccounting, quotes, h1};

        // We add methods the "generate default url" buttons
        AddMethodsToButtons();

        // If we don't do this, the values entered to our textboxes will not be read
        if (!this.IsPostBack)
        {
            PopulateProjectDataTextBoxes();
        }
    }

    // This method adds methods to the buttons in the list
    protected void AddMethodsToButtons()
    {
        foreach (Control ctrl in buttons) 
        {
            Button btn = (Button)ctrl;
            btn.Click += new EventHandler(GetUrl);
        }
    }

    // This method sets the generated URL in the right textbox upon click
    protected void GetUrl(object sender, EventArgs e)
    {
        // We find which button was clicked
        Button btn = (Button)sender;
        int index = Array.FindIndex(buttons, x => x.Equals(btn));

        // We know that there are two textboxes without generator buttons (ID and title)
        // Otherwise, they are in the same order in the lists. Therefor, we simply add +2 to the index to get the corresponding textbox to the button
        TextBox tb = (TextBox)textBoxes[index + 2];
        tb.Text = GenerateDefaultUrlForButton(btn.ID);
    }

    // This method generates a default URL for the project
    private string GenerateDefaultUrlForButton(string id)
    {
        string projectId = ProjectIdTextBox.Text;
        // This is method is just for demonstration, in "real-life" there is a lot more backend local logic with access to lists and folders, which generates real URLs
        // For this purpose, we simply make something up
        return "https://www." + id + ".com/" + projectId;
    }

    protected void PopulateProjectDataTextBoxes()
    {
        // We skip the first two, because they are ID and title
        for (int i = 2; i < textBoxes.Length; i++)
        {
            TextBox tb = (TextBox)textBoxes[i];
            tb.Attributes.Add("placeholder", "URL...");
        }
    }

    // This method checks if all the necessary data is filled out
    public Boolean allFilled()
    {
        Boolean allFilled = true;

        for (int i = 0; i < textBoxes.Length; i++)
        {
            TextBox tb = (TextBox)textBoxes[i];

            if (String.IsNullOrEmpty(tb.Text))
            {
                allFilled = false;
            }
        }

        return allFilled;
    }

    // This method is called when the "Create" button is clicked, and creates the project if the conditions are met
    protected void CreateButton_Click(object sender, EventArgs e)
    {
        // We check if all the textboxes are filled
        if (!allFilled())
        {
            // If not, we notify the user, and end the method
            Response.Write("<script>alert('" + "Please fill out ALL the textboxes to create a new project!" + "')</script>");
            return;
        }

        // We get the DB connection and make the query
        string dbstring = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        SqlConnection con = new SqlConnection(dbstring);

        string sqlStr = "SELECT project_id FROM Projects WHERE project_id = @theId";

        SqlCommand sqlCmd = new SqlCommand(sqlStr, con);

        sqlCmd.Parameters.AddWithValue("@theId", ProjectIdTextBox.Text);

        con.Open();

        //I only want to know whether or not the query returns something, i.e. if a project with the 
        //ID alrady exists. So instead of dealing with a DataSet, I will use the method 
        //ExecuteScalar, that only returns a single value (first column in the first row of the query result)
        object result = sqlCmd.ExecuteScalar();

        //If the result is null, a project with that name does not already exist, so add it to the DB
        if (result == null)
        {
            // The SQL statement to insert a new project into the projects table

            string sqlStr2 = "INSERT INTO Projects (project_id, customer, edoc, new_portal, old_portal, ncr, ecns,"
            + "jira, jira_quotes, http_logs, service_now, hours_accounting, quotes, h1) VALUES (@project_id, @customer, @edoc, @new_portal, @old_portal, @ncr, @ecns,"
            + "@jira, @jira_quotes, @http_logs, @service_now, @hours_accounting, @quotes, @h1)";

            // Create an executable SQL command containing our SQL statement and the database connection
            SqlCommand sqlCmd2 = new SqlCommand(sqlStr2, con);

            // Fill in the parameters in our prepared SQL statement
            sqlCmd2.Parameters.AddWithValue("@project_id", ProjectIdTextBox.Text);
            sqlCmd2.Parameters.AddWithValue("@customer", ProjectTitleBox.Text);
            sqlCmd2.Parameters.AddWithValue("@edoc", eDocUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@new_portal", NewPortalUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@old_portal", OldPortalUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@ncr", NcrUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@ecns", EcnsUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@jira", JiraUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@jira_quotes", JiraQuotesUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@http_logs", HttpLogsUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@service_now", ServiceNowUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@hours_accounting", HoursAccountingUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@quotes", QuotesUrlBox.Text);
            sqlCmd2.Parameters.AddWithValue("@h1", H1UrlBox.Text);

            // Execute the SQL command
            sqlCmd2.ExecuteNonQuery();

            // Close the connection to the database
            con.Close();

            Session["project_id"] = ProjectIdTextBox.Text;

            // We notify the user of the success
            Response.Write("<script>alert('" + "Succesfully created the project: " + ProjectTitleBox.Text + "!" + "')</script>");

            // We send them back to the projects page
            Response.Redirect("~/Default.aspx");

        }
        else
        {
            // If we fail, the user is also notified
            Response.Write("<script>alert('" + "FAILED to create project: " + ProjectTitleBox.Text + "!" + "')</script>");
        }
    }

}