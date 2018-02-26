
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using database;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class AdminAddFunds : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["employeeLoggedIn"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (Session["employeeLoggedIn"].ToString() != "True")
        {
            Response.Redirect("Login.aspx");
        }

        Employee user = (Employee)Session["user"];

        if (user.Admin != true)
        {
            Response.Redirect("HomePage.aspx");
        }
    }

    protected void SubmitFunds_OnClick(object sender, EventArgs e)
    {
        try
        {
            string commandText = "INSERT INTO [dbo].[Fund] (AccountTo, AccountFrom, Amount) Values (@AccountTo, @AccountFrom, @Amount)";
            SqlConnection conn = ProjectDB.connectToDB();
            SqlCommand insert = new SqlCommand(commandText, conn);
            insert.Parameters.AddWithValue("@AccountTo", txtDepositTo.Text);
            insert.Parameters.AddWithValue("@AccountFrom", txtWithdrawFrom.Text);
            insert.Parameters.AddWithValue("@Amount", txtAmount.Text);
            insert.ExecuteNonQuery();
            conn.Close();
        }
        catch(Exception)
        {

        }
    }



}