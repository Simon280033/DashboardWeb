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

    [System.Web.Script.Services.ScriptService]
public partial class _Default : System.Web.UI.Page
{
    private Control[] buttons;
    private List<Tuple<string, string>> favorites = new List<Tuple<string,string>>();
    private string pinnedId;
    private string userId;
    private string userName;
    private string userRole;

    protected void Page_Load(object sender, EventArgs e)
    {
        // We collect the URL buttons in a list, so we can quickly iterate through them in other methods
        buttons = new Control[] { EDocButton, NewPortalButton, OldPortalButton, NCRButton, ECNsButton, JiraButton, JiraQuotesButton, HttpLogsButton, ServiceNowButton, HoursAccountingButton, QuotesButton, H1Button };

        ProjectName.Text = "Search for Project";
        
        // We disable the buttons by default
        for (int i = 0; i < buttons.Length; i++)
        {
            LinkButton btn = (LinkButton)buttons[i];
            btn.CssClass = "btn-disabled";
            btn.Enabled = false;
        }

        // We only run this code the first time the page is loaded during the session
        if (!this.IsPostBack)
        {
            // We check if the user is logged in, so we can determine whether or not he needs access to controls
            if (isLoggedIn())
            {
                // We set the favorites 
                SetFavorites();

                // We check if the user has a pinned project
                if (pinnedId == null)
                {
                    pinnedId = GetPinnedProjectId();
                }

                // If there is not another project in session, we load the pinned
                if (Session.Count > 0 && Session["project_id"] != null)
                {
                   TextBox1.Text = Session["project_id"].ToString();
                   SearchProject();
                }
                else if (pinnedId != null)
                {
                    TextBox1.Text = pinnedId;
                    SearchProject();
                }
            }
        }      
    }

        // This method sets the user's favorites to the favorites dropdownlist
    private void SetFavorites()
    {
        // We set the favorites
        if (favorites == null || favorites.Count == 0)
        {
            favorites = GetFavoritesIdAndName();
        }

        // If there are no favorites, we exit this method
        if (favorites == null || favorites.Count == 0)
        {
            return;
        }

        FavoritesDropDownList.Width = 20;
        FavoritesDropDownList.Items.Clear();

        ListItem li = new ListItem("---Favorites---", null);
        li.Attributes.CssStyle.Add("background-color", "white");
        li.Attributes.CssStyle.Add("color", "black");
        li.Attributes.CssStyle.Add("text-align", "center");

        FavoritesDropDownList.Items.Add(li);

        foreach (Tuple<string, string> favorite in favorites)
        {
            li = new ListItem(favorite.Item1 + " - " + favorite.Item2, favorite.Item2);
            li.Attributes.CssStyle.Add("background-color", "white");
            li.Attributes.CssStyle.Add("color", "black");
            FavoritesDropDownList.Items.Add(li); // Show 'ID - name', store ID for search
        }

        if (favorites != null && favorites.Count > 0)
        {
            FavoritesDropDownList.CssClass = "favorited";
        }
        else
        {
            FavoritesDropDownList.CssClass = "disabled";
        }
    }

        // This method get's the users role, and sets it in a variable for other methods to use
    private string GetUserRole()
    {
        string theRole = "Read";

        // Users can have multiple roles, so we check if one of them is admin/write
        string[] userRoles = Roles.GetRolesForUser();

        foreach (string role in userRoles)
        {
            if (role.Equals("Write"))
            {
                theRole = role;
            }

            if (role.Equals("Admin"))
            {
                theRole = role;
            }
        }

        userRole = theRole;

        return theRole;
    }

    private bool isLoggedIn()
    {
        bool loggedIn = (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

        return loggedIn;
    }

        // This method checks whether the project is pinned or favorited, and styles the buttons accordingly
    protected void SetUserButtons() 
    {
        if (!isLoggedIn())
        {
            EditButton.CssClass = "disabled";
            PinButton.CssClass = "disabled";
            FavoriteButton.CssClass = "disabled";

            PinButtonImage.ImageUrl = "~/images/pin.png";
            FavoriteButtonImage.ImageUrl = "~/images/favorite.png";
            return;
        }

        if (favorites != null && favorites.Count == 0)
        {
            favorites = GetFavoritesIdAndName();
        }

        if (Session.Count > 0 && Session["project_id"] != null)
        {
            EditButton.CssClass = "editButton";

            // We check if the project is pinned
            if (pinnedId == null)
            {
                pinnedId = GetPinnedProjectId();
            }

            if (Session["project_id"].ToString().Equals(pinnedId))
            {
                PinButtonImage.ImageUrl = "~/images/pinned.png";
                PinButton.CssClass = "pinned";
            }
            else
            {
                PinButtonImage.ImageUrl = "~/images/pin.png";
                PinButton.CssClass = "unpinned";
            }

            // We check if the project is favorited
            bool favorited = false;

            if (favorites != null)
            {
                foreach (Tuple<string, string> favorite in favorites)
                {
                    if (Session["project_id"].ToString().Equals(favorite.Item1))
                    {
                        favorited = true;
                        break;
                    }
                }
            }

            if (favorited)
            {
                FavoriteButtonImage.ImageUrl = "~/images/favorited.png";
                FavoriteButton.CssClass = "favorited";
            }
            else
            {
                FavoriteButtonImage.ImageUrl = "~/images/favorite.png";
                FavoriteButton.CssClass = "favoriteButton";
            } 
        }
        else
        {
            // If there is no project, we disable the edit/pin/favorite buttons
            EditButton.CssClass = "disabled";
            PinButton.CssClass = "disabled";
            FavoriteButton.CssClass = "disabled";

            PinButtonImage.ImageUrl = "~/images/pin.png";
            FavoriteButtonImage.ImageUrl = "~/images/favorite.png";
        }

        // We set the favorite dropdownlist
        SetFavorites();
    }

        // When a favorite is selected from the dropdownlist
    public void FavoriteSelected(object sender, EventArgs e)
    {
            // ID of selected project is searched for
            TextBox1.Text = CleanId(FavoritesDropDownList.SelectedItem.Text);

            SearchProject();
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

    public void SearchProject()
    {
        // If the search bar is empty (maybe change this so it happens when there are no results?)
        if (TextBox1.Text.Length == 0)
        {
            ProjectName.Text = "Search for Project";

            // the textbox is empty
            for (int i = 0; i < buttons.Length; i++)
            {
                LinkButton btn = (LinkButton)buttons[i];
                btn.CssClass = "btn-disabled";
                btn.Enabled = false;
                Session["project_id"] = null;
            }
            return;
        }

        // We attempt to get the ID from the search
        string projectId = CleanId(TextBox1.Text);

        // We store the project ID in a session variable, so we can use it on other pages (If the user wants to edit this project)
        Session["project_id"] = projectId; // MAKE SOME SORT OF cleanID METHOD FOR THIS LATER!!!!

        ProjectName.Visible = true;

        // URLs
        string edoc = null;
        string newPortal = null;
        string oldPortal = null;
        string NCR = null;
        string ECNs = null;
        string jira = null;
        string jiraQuotes = null;
        string httpLogs = null;
        string serviceNow = null;
        string hoursAccounting = null;
        string quotes = null;
        string h1 = null;

        int rowCount = 0;

        //Connection is made to the SQL server
        var con = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();

        using (SqlConnection myConnection = new SqlConnection(con))
        {
            string oString = "Select * from Projects where project_id = '" + projectId + "'"; // FIX TO PARAMETERED TO AVOID INJECTIONS!!!
            SqlCommand oCmd = new SqlCommand(oString, myConnection);
            myConnection.Open();
            using (SqlDataReader oReader = oCmd.ExecuteReader())
            {
                while (oReader.Read())
                {
                    rowCount++;

                    // We set title
                    ProjectName.Text = oReader["customer"].ToString();

                    edoc = oReader["edoc"].ToString();
                    newPortal = oReader["new_portal"].ToString();
                    oldPortal = oReader["old_portal"].ToString();
                    NCR = oReader["ncr"].ToString();
                    ECNs = oReader["ecns"].ToString();
                    jira = oReader["jira"].ToString();
                    jiraQuotes = oReader["jira_quotes"].ToString();
                    httpLogs = oReader["http_logs"].ToString();
                    serviceNow = oReader["service_now"].ToString();
                    hoursAccounting = oReader["hours_accounting"].ToString();
                    quotes = oReader["quotes"].ToString();
                    h1 = oReader["h1"].ToString();
                }

                myConnection.Close();
            }
        }

        if (rowCount > 0)
        {
            // We add the URLs to a list
            string[] urls = { edoc, newPortal, oldPortal, NCR, ECNs, jira, jiraQuotes, httpLogs, serviceNow, hoursAccounting, quotes, h1 };

            SetButtons(urls);

        }
        else
        {
            ProjectName.Text = "No results found!";
            Session["project_id"] = null;

            for (int i = 0; i < buttons.Length; i++)
            {
                LinkButton btn = (LinkButton)buttons[i];
                btn.CssClass = "btn-disabled";
                btn.Enabled = false;
                btn.Attributes.Remove("href");
            }
        }

        if (isLoggedIn())
        {
            // We enable editing of the project if the user has privileges
            SetUserButtons();

            // We log the viewing by the user in the DB
            LogProjectView();
        }
    }

    // This method is called when a user searches for a project
    protected void SearchProjectOnEvent(object sender, EventArgs e)
    {
        SearchProject();
    }

    // This method sets the URLs and styles of the buttons
    private void SetButtons(string[] urls)
    {
        // We iterate the URLs
        // If the URL is a default one, we make the button blue
        // Otherwise we make it orange
        for (int i = 0; i < urls.Length; i++)
        {
            string classToSet;

            if (!urls[i].Contains(Session["project_id"].ToString()))
            {
                classToSet = "btn-default";
            }
            else
            {
                classToSet = "btn";
            }


                LinkButton btn = (LinkButton)buttons[i];
                btn.Attributes.Add("href", urls[i]); // Sets URL
                btn.Attributes.Add("target", "_blank"); // Opens in a new tab
                btn.CssClass = classToSet;

        }
    }

        // This method attempts to figure to get an ID from the user's search
    public string CleanId(String searchText)
    {
        string[] parts = searchText.Split(new[] { " - " }, StringSplitOptions.None); // The string is split 

        return parts[0];
    }

        // This method changes to the edit project page
    public void ChangeToEditProject(object sender, EventArgs e)
    {
        // We make sure to only do it if we are on a valid project
        if (Session.Count > 0 && Session["project_id"] != null)
        {
            if (userRole == null)
            {
                GetUserRole();
            }

            if (userRole.Equals("Read"))
            {
                Response.Write("<script>alert('" + "Insufficient privileges! You must have write privileges to edit projects." + "')</script>");
            }
            else
            {
                Response.Redirect("write/editproject.aspx");
            }
        }
    }

        // This method get's the user's pinned project
    private string GetPinnedProjectId()
    {
        if (!isLoggedIn())
        {
            return null;
        }
        MembershipUser currentUser = Membership.GetUser();
        string userId = currentUser.ProviderUserKey.ToString();

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT TOP 1 Pinned FROM Pins WHERE UserId = @UserId;";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Connection = conn;
                conn.Open();
                List<string> pinList = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        pinList.Add(sdr["Pinned"].ToString());
                    }
                }
                conn.Close();

                if (pinList.Count != 0)
                {
                    pinnedId = pinList[0];
                    return pinList[0];
                }
                else
                {
                    return null;
                }
            }
        }
    }

    // This method get's the user's favorited projects
    private List<Tuple<string, string>> GetFavoritesIdAndName()
    {
        if (!isLoggedIn())
        {
            return null;
        }
        MembershipUser currentUser = Membership.GetUser();
        string userId = currentUser.ProviderUserKey.ToString();

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT * FROM Favorites JOIN Projects on Favorites.project_id = Projects.project_id WHERE UserId = @UserId;";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Connection = conn;
                conn.Open();
                List<Tuple<string, string>> favoriteList = new List<Tuple<string, string>>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Tuple<string, string> favorite = new Tuple<string, string>(sdr["project_id"].ToString(), sdr["customer"].ToString());
                        favoriteList.Add(favorite);
                    }
                }
                conn.Close();

                if (favoriteList.Count != 0)
                {
                    return favoriteList;
                }
                else
                {
                    return null;
                }
            }
        }
    }

        // This method does the pin action depending on the current circumstances
    public void PinButtonPressed(object sender, EventArgs e)
    {
        if (Session.Count > 0 && Session["project_id"] != null)
        {
            bool pinned = false;

            // We check if the current project is already pinned
            if (PinButton.CssClass.Equals("pinned"))
            {
                pinned = true;
            }

            // We run the method depending on its current status
            if (pinned)
            {
                UnpinProject();
            }
            else
            {
                PinProject();
            }

            SearchProject();
        }
    }


        // This method attaches the current project to the user's pinned
    public void PinProject()
    {
        if (!isLoggedIn())
        {
            return;
        }

        if (userId == null)
        {
            MembershipUser currentUser = Membership.GetUser();
            userId = currentUser.ProviderUserKey.ToString();
        }  

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "UPDATE Pins SET Pinned = @ProjectId WHERE UserId = @UserId IF @@ROWCOUNT = 0 INSERT INTO Pins VALUES(@UserId, @ProjectId);";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // We change the button image to denote that it is pinned
        PinButtonImage.ImageUrl = "~/images/pinned.png";
    }

        // This method unpins the active project
    private void UnpinProject()
    {
        if (!isLoggedIn())
        {
            return;
        }

        if (userId == null)
        {
            MembershipUser currentUser = Membership.GetUser();
            userId = currentUser.ProviderUserKey.ToString();
        }  

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "UPDATE Pins SET Pinned = NULL WHERE UserId = @UserId IF @@ROWCOUNT = 0 INSERT INTO Pins VALUES(@UserId, NULL);";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // We change the button image to denote that it is unpinned
        PinButtonImage.ImageUrl = "~/images/pin.png";
    }

    // This method does the favorite action depending on the current circumstances
    public void FavoriteButtonPressed(object sender, EventArgs e)
    {
        if (Session.Count > 0 && Session["project_id"] != null)
        {
            bool favorited = false;

            // We check if the current project is already favorited
            if (FavoriteButton.CssClass.Equals("favorited"))
            {
                favorited = true;
            }

            // We run the method depending on its current status
            if (favorited)
            {
                UnfavoriteProject();
            }
            else
            {
                FavoriteProject();
            }

            SearchProject();
        }
    }


    // This method attaches the current project to the user's favorites
    public void FavoriteProject()
    {
        if (!isLoggedIn())
        {
            return;
        }

        if (userId == null)
        {
            MembershipUser currentUser = Membership.GetUser();
            userId = currentUser.ProviderUserKey.ToString();
        }  

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "INSERT INTO Favorites VALUES(@UserId, @ProjectId);";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // We change the button image to denote that it is favorited
        FavoriteButtonImage.ImageUrl = "~/images/favorited.png";

        // We add the favorite to the dropdown list
        ListItem li = new ListItem(Session["project_id"].ToString() + " - " + ProjectName.Text, Session["project_id"].ToString());
        li.Attributes.CssStyle.Add("background-color", "white");
        li.Attributes.CssStyle.Add("color", "black");
        FavoritesDropDownList.Items.Add(li); // Show 'ID - name', store ID for search

        // And to the favorites list
        favorites = GetFavoritesIdAndName();

        if (favorites != null && favorites.Count > 0)
        {
            FavoritesDropDownList.CssClass = "favorited";
        }
        else
        {
            FavoritesDropDownList.CssClass = "disabled";
        }
    }

    // This method unpins the active project
    private void UnfavoriteProject()
    {
        if (!isLoggedIn())
        {
            return;
        }

        if (userId == null)
        {
            MembershipUser currentUser = Membership.GetUser();
            userId = currentUser.ProviderUserKey.ToString();
        }    

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "DELETE FROM Favorites WHERE UserId = @UserID AND project_id = @ProjectId ;";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // We change the button image to denote that it is unfavorited
        FavoriteButtonImage.ImageUrl = "~/images/favorite.png";

        // We remove the favorite from the dropdownlist
        foreach (ListItem li in FavoritesDropDownList.Items)
        {
            if (CleanId(li.Text).Equals(Session["project_id"].ToString()))
            {
                FavoritesDropDownList.Items.Remove(li);
                break;
            }
        }

        // And from the favorites list
        favorites = GetFavoritesIdAndName();

        if (favorites != null && favorites.Count > 0)
        {
            FavoritesDropDownList.CssClass = "favorited";
        }
        else
        {
            FavoritesDropDownList.CssClass = "disabled";
        }
    }

        // This method logs the view data for this project/user
    private void LogProjectView()
    {
        if (!isLoggedIn())
        {
            return;
        }

        if (Session["project_id"] == null)
        {
            return;
        }  

        if (userId == null)
        {
            MembershipUser currentUser = Membership.GetUser();
            userId = currentUser.ProviderUserKey.ToString();
        }

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager
                    .ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "INSERT INTO Project_Viewed VALUES(@ProjectId, @UserId, @TimeStamp);";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProjectId", Session["project_id"].ToString());
                cmd.Parameters.AddWithValue("@TimeStamp", TimeStamp());
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

        // This method creates a timestamp in the correct format
    private string TimeStamp()
    {
        DateTime myDateTime = DateTime.Now;
        return myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }

}