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

public partial class editproject : System.Web.UI.Page
{
    private Control[] textBoxes;
    private Control[] buttons;

    protected void Page_Load(object sender, EventArgs e)
    {
        // We make sure that a valid session variable exists, as this holds the info about which project we are editing
        if (!(Session.Count > 0 && Session["project_id"] != null))
        {
            Response.Redirect("~/Default.aspx");
        }

        // We collect textboxes and buttons in lists, so we can iterate over them for shorter and more readable/maintainable code
        textBoxes = new Control[] { ProjectTitleBox, eDocUrlBox, NewPortalUrlBox, OldPortalUrlBox, NcrUrlBox, EcnsUrlBox, JiraUrlBox, JiraQuotesUrlBox, HttpLogsUrlBox, ServiceNowUrlBox, HoursAccountingUrlBox, QuotesUrlBox, H1UrlBox };
        buttons = new Control[] { edoc, newportal, oldportal, ncr, ecns, jira, jiraquotes, httplogs, servicenow, hoursaccounting, quotes, h1 };

        // If we don't do this, the values entered to our textboxes will not be read
        if (!this.IsPostBack)
        {
            SetTitle();
            PopulateProjectDataTextBoxes();
        }

        AddMethodsToButtons();
    }

    protected void AddMethodsToButtons()
    {
        foreach (Control ctrl in buttons)
        {
            Button btn = (Button)ctrl;
            btn.Click += new EventHandler(GetUrl);
        }
    }

    protected void GetUrl(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int index = Array.FindIndex(buttons, x => x.Equals(btn));

        TextBox tb = (TextBox)textBoxes[index + 1];
        tb.Text = GenerateDefaultUrlForButton(btn.ID);
    }

    private string GenerateDefaultUrlForButton(string id)
    {
        string projectId = Session["project_id"].ToString();
        // This is method is just for demonstration
        return "https://www." + id + ".com/" + projectId;
    }

    protected void SetTitle()
    {
        if (Session.Count > 0 && Session["project_id"].ToString() != null)
        {
            roomValueLabel.Text = "Edit Project '" + Session["project_id"].ToString() + "'";
        }
        else
        {
            roomValueLabel.Text = "ERROR";
        }
    }

    // We get the current values for the project and inser them
    protected void PopulateProjectDataTextBoxes()
    {
        //Connection is made to the SQL server
        var con = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();

        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string oString = "Select * from Projects where project_id = @ProjectId ;";
            SqlCommand oCmd = new SqlCommand(oString, myConnection);
            oCmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
            myConnection.Open();
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {

                    // We set title
                    ProjectTitleBox.Text = oReader["customer"].ToString();

                    eDocUrlBox.Text = oReader["edoc"].ToString();
                    NewPortalUrlBox.Text = oReader["new_portal"].ToString();
                    OldPortalUrlBox.Text = oReader["old_portal"].ToString();
                    NcrUrlBox.Text = oReader["ncr"].ToString();
                    EcnsUrlBox.Text = oReader["ecns"].ToString();
                    JiraUrlBox.Text = oReader["jira"].ToString();
                    JiraQuotesUrlBox.Text = oReader["jira_quotes"].ToString();
                    HttpLogsUrlBox.Text = oReader["http_logs"].ToString();
                    ServiceNowUrlBox.Text = oReader["service_now"].ToString();
                    HoursAccountingUrlBox.Text = oReader["hours_accounting"].ToString();
                    QuotesUrlBox.Text = oReader["quotes"].ToString();
                    H1UrlBox.Text = oReader["h1"].ToString();
                }

                myConnection.Close();
            }
        }
    }

    protected void UpdateUrl(object sender, EventArgs e)
    {
        // We find the ID of the button clicked
        Button clickedButton = sender as Button;

        if (clickedButton == null) // just to be on the safe side
            return;

        if (clickedButton.ID.Equals("TitleButton"))
        {
            // We check if all values are valid
            if (AllFilled())
            {
                UpdateProjectColumn();
            }
            else
            {
                Response.Write("<script>alert('" + "Please make sure that all textfields are filled!" + "')</script>");
            }
        }
    }

    // This method updates the project with the new values. It also saves info about the edit in the Project_Edits table
    protected void UpdateProjectColumn()
    {
        MembershipUser currentUser = Membership.GetUser();
        string userId = currentUser.ProviderUserKey.ToString();

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "UPDATE Projects SET " +
                    "customer = @Customer , " +
                    "edoc = @edoc , " +
                    "new_portal = @new_portal , " +
                    "old_portal = @old_portal , " +
                    "ncr = @ncr , " +
                    "ecns = @ecns , " +
                    "jira = @jira , " +
                    "jira_quotes = @jira_quotes , " +
                    "http_logs = @http_logs , " +
                    "service_now = @service_now , " +
                    "hours_accounting = @hours_accounting , " +
                    "quotes = @quotes , " +
                    "h1 = @h1 WHERE project_id = @ProjectId; INSERT INTO Project_Edits VALUES(@ProjectId, @UserId, @TimeStamp);";
                cmd.Parameters.AddWithValue("@Customer", ProjectTitleBox.Text);
                cmd.Parameters.AddWithValue("@edoc", eDocUrlBox.Text);
                cmd.Parameters.AddWithValue("@new_portal", NewPortalUrlBox.Text);
                cmd.Parameters.AddWithValue("@old_portal", OldPortalUrlBox.Text);
                cmd.Parameters.AddWithValue("@ncr", NcrUrlBox.Text);
                cmd.Parameters.AddWithValue("@ecns", EcnsUrlBox.Text);
                cmd.Parameters.AddWithValue("@jira", JiraUrlBox.Text);
                cmd.Parameters.AddWithValue("@jira_quotes", JiraQuotesUrlBox.Text);
                cmd.Parameters.AddWithValue("@http_logs", HttpLogsUrlBox.Text);
                cmd.Parameters.AddWithValue("@service_now", ServiceNowUrlBox.Text);
                cmd.Parameters.AddWithValue("@hours_accounting", HoursAccountingUrlBox.Text);
                cmd.Parameters.AddWithValue("@quotes", QuotesUrlBox.Text);
                cmd.Parameters.AddWithValue("@h1", H1UrlBox.Text);
                cmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@TimeStamp", TimeStamp());

                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        Response.Write("<script>alert('" + "Succesfully updated project " + ProjectTitleBox.Text + "!" + "')</script>");
    }

    private bool AllFilled()
    {
        bool allFilled = true;

        foreach (Control ctrl in textBoxes)
        {
            TextBox txt = (TextBox)ctrl;
            if (String.IsNullOrEmpty(txt.Text))
            {
                allFilled = false;
            }
        }
        return allFilled;
    }

    // This method creates a timestamp in the correct format
    private string TimeStamp()
    {
        DateTime myDateTime = DateTime.Now;
        return myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}