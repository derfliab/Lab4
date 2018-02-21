using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using database;
using System.Data.SqlClient;

public partial class Rewards : System.Web.UI.Page
{
    static int index;
    protected void Page_Load(object sender, EventArgs e)
    {
        //index = lstRewardsView.SelectedIndex;
        //lstRewardsView.SelectedIndex = index;

        if (Session["employeeLoggedIn"] == null)
        {
            Response.Redirect("Login.aspx"); //check that the filepath is correct
        }
        if (Session["employeeLoggedIn"].ToString() != "True")
        {
            Response.Redirect("Login.aspx"); //check that the filepath works
        }

        ////loop for it to only load this on the first time the page loads
        //ispostback
        //for (int i = 0; i < 1; i++)
        //{
        try
        {
            SqlConnection conn = ProjectDB.connectToDB();
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
            insert.Connection = conn;

            insert.CommandText = "select concat([RewardID],' ',[Name],' ',[Description], ' ',[Price],' ',[StartDate]) AS search_RewardItems from [dbo].[Reward]";
            lstRewardsView.DataSource = insert.ExecuteReader();

            lstRewardsView.DataTextField = "search_RewardItems";
            lstRewardsView.DataBind();
            conn.Close();
        }

        //Shows an error message if there is a problem connecting to the database
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ERROR')", true);
        }
        //}
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection conn = ProjectDB.connectToDB();
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
            insert.Connection = conn;

            //see if you are searching for reward name or reward provider
            if (rdoName.Checked)
            {
                insert.CommandText = "select concat([RewardID],' ',[RewardName],' ',[RewardDescription], ' ',[Price],' ',[StartDate]) AS search_RewardItems from [dbo].[RewardItems] where lower([RewardName]) like lower (@rewardsearch)";
                insert.Parameters.AddWithValue("@rewardsearch", "%" + txtSearch.Text + "%");
                lstRewardsView.DataSource = insert.ExecuteReader();
                lstRewardsView.DataTextField = "search_RewardItems";
                lstRewardsView.DataBind();
                conn.Close();
            }
            else if (rdoCompany.Checked)
            {
                insert.CommandText = "select concat([RewardID],' ',[RewardName],' ',[RewardDescription], ' ',[Price],' ',[StartDate]) AS search_RewardProvider from [dbo].[RewardItems] where lower([RewardProvider]) like lower (@providersearch)";
                insert.Parameters.AddWithValue("@providersearch", "%" + txtSearch.Text + "%");
                lstRewardsView.DataSource = insert.ExecuteReader();
                lstRewardsView.DataTextField = "search_RewardProvider";
                lstRewardsView.DataBind();
                conn.Close();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PLEASE SELECT A RADIO BUTTON')", true);
            }

            if (lstRewardsView.Items.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('There were no rewards matching your search!!')", true);
                return;
            }
        }
        //Shows an error message if there is a problem connecting to the database
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ERROR2')", true);
        }
    }

    //DOES NOT WORK YET////////////////////////////////////////////////////////////////////////
    protected void btnEligible_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection conn = ProjectDB.connectToDB();
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
            insert.Connection = conn;

            insert.CommandText = "select concat([RewardID],' ',[RewardName],' ',[RewardDescription], ' ',[Price],' ',[StartDate]) AS search_RewardItems from [dbo].[RewardItems] where "; //person's points are >= reward price
            insert.Parameters.AddWithValue("@rewardsearch", "%" + txtSearch.Text + "%");
            lstRewardsView.DataSource = insert.ExecuteReader();
            //lstRewardsView.DataTextField = "search_RewardItems";
            lstRewardsView.DataBind();
            conn.Close();

            if (lstRewardsView.Items.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('There were no rewards matching your search!!')", true);
                return;
            }
        }
        //Shows an error message if there is a problem connecting to the database
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('eligable ERROR')", true);
        }
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        //String id = "";
        //String search = lstRewardsView.SelectedItem.ToString();

        //for (int i = 0; i < search.Length; i++)
        //{
        //    if (search.Substring(i, 1) != " ")
        //    {
        //        id += search.Substring(i, 1);
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You must select a reward to purchase')", true);
        //        break;
        //    }
        //}

        try
        {
            SqlConnection conn = ProjectDB.connectToDB();
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
            insert.Connection = conn;

            insert.CommandText = "insert into [dbo].[Transaction] values (@id, @cost, @purchaseTime, @empID)";
            insert.Parameters.AddWithValue("@id", 0); //use getters and setters here//////////////////////////////////////////////////////////
            insert.Parameters.AddWithValue("@cost", 0);
            insert.Parameters.AddWithValue("@purchaseTime", 2012 - 02 - 12);
            insert.Parameters.AddWithValue("@empID", 100);
            insert.Parameters.AddWithValue("@rewardID", 0);
            insert.ExecuteNonQuery();
            //SEND EMAIL????///////////////////////////////////////////////////
            conn.Close();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('reward purchased successfully')", true);
        }
        //Shows an error message if there is a problem connecting to the database
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('data connection error')", true);
        }
    }
}