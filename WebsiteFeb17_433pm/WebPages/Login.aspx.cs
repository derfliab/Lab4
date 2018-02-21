using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using database;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["databaseName"] = "Lab4";
        
    }

    protected void EmployeeLogin_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
            //the Login object has both UserName and Password properties
            string userName = employeeLogin.UserName;
            string password = employeeLogin.Password;

            //the authenticated property of the AutheticateEventArgs object is what
            //determines whether to authenticate the login or not...here we assume no
            e.Authenticated = false;

            //setting up SqlConnection and SqlCommand
            SqlConnection conn = ProjectDB.connectToDB();
            if (conn != null)
            {
                string commandText = "SELECT TOP 1 UserName, PasswordHash FROM [dbo].[EmployeeLogin] WHERE UserName = @UserName";

                SqlCommand select = new SqlCommand(commandText, conn);

                select.Parameters.AddWithValue("@UserName", userName);

                SqlDataReader reader = select.ExecuteReader();

                //if there is such a record, read it
                if (reader.HasRows)
                {
                    reader.Read();
                    String pwHash = reader["PasswordHash"].ToString(); //retrieve the password hash

                    String user = reader["UserName"].ToString();
                    Session["loggedInAs"] = user;
                    if (user == "admin") //check if the user is an admin
                    {
                        Session["admin"] = true;
                    }
                    else
                    {
                        Session["admin"] = false;
                    }
                    //user the SimpleHash object to verify the user's entered password
                    bool verify = SimpleHash.VerifyHash(password, "MD5", pwHash);

                    //the result of the VerifyHash is boolean; we use this to determine authentication
                    e.Authenticated = verify;
                }

                conn.Close();

                Session["employeeLoggedIn"] = e.Authenticated.ToString();
            }
            else
            {
                errorMessage.Text += "\nThe connection to the database failed: " + conn;
            }
        }
        catch (Exception ex)
        {
            employeeLogin.FailureText = ex.ToString();
        }
    }

    protected void employeeLogin_LoggedIn(object sender, EventArgs e)
    {

        Response.Redirect("HomePage.aspx");   
    }
}