﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using database;
using System.Data.SqlClient;

public partial class Rewards : System.Web.UI.Page
{
    static Employee user;
    static int index;

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
        user = (Employee)Session["user"];

        if (!IsPostBack)
        {
            try
            {
                SqlConnection conn = ProjectDB.connectToDB();
                System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
                insert.Connection = conn;

                //insert.CommandText = "select concat([RewardID],' ',[Name],' ',[Description], ' ',[Price],' ',[StartDate]) AS search_RewardItems from [dbo].[RewardItem]";
                ////////////////////////////PARAMATERIZE////////////////////////////////////
                insert.CommandText = "select concat([dbo].[RewardItem].[RewardID],' ',[dbo].[RewardItem].[Name],' ',[dbo].[RewardItem].[Price],' ',[dbo].[RewardProvider].[ProviderName]) AS search_RewardItems FROM [dbo].[RewardItem] INNER JOIN [dbo].[RewardProvider] ON [dbo].[RewardItem].[ProviderID] = [dbo].[RewardProvider].[ProviderID]";

                lstRewardsView.DataSource = insert.ExecuteReader();

                lstRewardsView.DataTextField = "search_RewardItems";
                lstRewardsView.DataBind();
                lstElligable.Visible = false;
                lstSearchName.Visible = false;
                lstSearchProvider.Visible = false;
                conn.Close();
            }
            //Shows an error message if there is a problem connecting to the database
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ERROR')", true);

            }
        }
    }
    //purchases a reward
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        //gets the rewardID of the selected item
        String id = "";
        try
        {
            if (lstRewardsView.Visible == true)
            {
                String search = lstRewardsView.SelectedItem.ToString();
                id = findItemID(search);
            }
            else if (lstSearchName.Visible == true)
            {
                String search = lstSearchName.SelectedItem.ToString();
                id = findItemID(search);
            }
            else if (lstSearchProvider.Visible == true)
            {
                String search = lstSearchProvider.SelectedItem.ToString();
                id = findItemID(search);
            }
            else if (lstElligable.Visible == true)
            {
                String search = lstElligable.SelectedItem.ToString();
                id = findItemID(search);
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You must select a reward to purchase')", true);
        }
        if (user.Points >= FindCost(id))
        {
            //enters data into the transaction table in the DB
            try
            {
                SqlConnection conn = ProjectDB.connectToDB();
                System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
                insert.Connection = conn;

                insert.CommandText = "insert into [dbo].[Transaction] values (@cost, @purchaseTime, @empID, @rewardID)";
                insert.Parameters.AddWithValue("@cost", FindCost(id));
                insert.Parameters.AddWithValue("@purchaseTime", DateTime.Now);
                insert.Parameters.AddWithValue("@empID", FindID(user.EmpLoginID));
                insert.Parameters.AddWithValue("@rewardID", id);
                insert.ExecuteNonQuery();

                conn.Close();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Reward Purchased Successfully')", true);

                //update points
                SubtractPoints(pointsSubtraction(FindCost(id), Convert.ToDecimal(user.Points)), FindID(user.EmpLoginID));
                user.Points -= FindCost(id);
                //update Quantity in the database
                SubtractQuantity(quantitySubtraction(findQuantity(id)), id);

            }
            //Shows an error message if there is a problem connecting to the database
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('data connection error')", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('You do not have enough points to purchase this reward')", true);
        }
    }

    public String findItemID(String search)
    {
        String id = "";
        for (int i = 0; i < search.Length; i++)
        {
            if (search.Substring(i, 1) != " ")
            {
                id += search.Substring(i, 1);
            }
            else
            {
                break;
            }
        }
        return id;
    }
    //allows employee to search the rewards page based on name or company name
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hideLstViews();
        try
        {
            lstRewardsView.DataSource = null;
            lstRewardsView.Items.Clear();
            SqlConnection conn = ProjectDB.connectToDB();
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
            insert.Connection = conn;

            //see if you are searching for reward name or reward provider
            if (rdoName.Checked)
            {
                lstSearchName.Visible = true;
                insert.CommandText = "select concat([RewardID],' ',[Name],' ',[Description], ' ',[Price],' ',[StartDate]) AS search_RewardItems from [dbo].[RewardItem] where lower([Name]) like lower (@rewardsearch)";
                insert.Parameters.AddWithValue("@rewardsearch", "%" + txtSearch.Text + "%");
                lstSearchName.DataSource = insert.ExecuteReader();
                lstSearchName.DataTextField = "search_RewardItems";
                lstSearchName.DataBind();
                conn.Close();
                if (lstSearchName.Items.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('There were no rewards matching your search!!')", true);
                    lstRewardsView.Visible = true;
                    lstSearchName.Visible = false;
                    return;
                }
            }
            else if (rdoCompany.Checked)
            {
                lstSearchProvider.Visible = true;
                insert.CommandText = "select concat([RewardID],' ',[Name],' ',[Description], ' ',[Price],' ',[StartDate]) AS search_RewardProvider from [dbo].[RewardItem] where lower([Name]) like lower(@providersearch)";
                insert.Parameters.AddWithValue("@providersearch", "%" + txtSearch.Text + "%");
                lstSearchProvider.DataSource = insert.ExecuteReader();
                lstSearchProvider.DataTextField = "search_RewardProvider";
                lstSearchProvider.DataBind();
                conn.Close();
                if (lstSearchProvider.Items.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('There were no rewards matching your search!!')", true);
                    lstRewardsView.Visible = true;
                    lstSearchProvider.Visible = false;
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('PLEASE SELECT A RADIO BUTTON')", true);
            }

        }
        //Shows an error message if there is a problem connecting to the database
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ERROR2')", true);
        }
    }
    //Displays only the rewards the employee is elligable for
    protected void btnElligable_Click(object sender, EventArgs e)
    {
        hideLstViews();
        try
        {
            SqlConnection conn = ProjectDB.connectToDB();
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand();
            insert.Connection = conn;

            insert.CommandText = "SELECT concat(RewardItem.RewardID,' ',RewardItem.Name,' ',RewardItem.Description,' ',RewardItem.Price,' ',RewardItem.StartDate) as search_Elligable FROM RewardItem WHERE RewardItem.Price <= @userpoints";
            insert.Parameters.AddWithValue("@userpoints", user.Points);
            lstElligable.DataSource = insert.ExecuteReader();
            lstElligable.Visible = true;
            lstElligable.DataTextField = "search_Elligable";
            lstElligable.DataBind();
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
    //finds the EmployeeID
    public int FindID(int id)
    {
        try
        {
            String commandText = "Select EmployeeID from [dbo].[EMPLOYEE] WHERE EmpLoginID = @EmpLoginID";
            SqlConnection conn = ProjectDB.connectToDB();
            SqlCommand select = new SqlCommand(commandText, conn);

            select.Parameters.AddWithValue("@EmpLoginID", id);
            SqlDataReader reader = select.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                int employeeID = (int)reader["EmployeeID"];
                conn.Close();
                return employeeID;
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('eligable ERROR1')", true);
            return -1;
        }
        return -1;
    }

    //Finds the cost of the reward
    public Decimal FindCost(String id)
    {
        try
        {
            String commandText = "Select Price from [dbo].[RewardItem] WHERE RewardID = @RewardID";
            SqlConnection conn = ProjectDB.connectToDB();
            SqlCommand select = new SqlCommand(commandText, conn);

            select.Parameters.AddWithValue("@RewardID", id);
            SqlDataReader reader = select.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                Decimal price = (Decimal)reader["Price"];
                conn.Close();
                return price;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ex')", true);
            return -1;
        }
        return -1;
    }

    //math behind the subtraction method
    public Decimal pointsSubtraction(Decimal cost, Decimal currentPoints)
    {
        Decimal answer = (currentPoints - cost);
        return answer;
    }

    //Subtracts the points in the Employee Table on the DB
    public void SubtractPoints(Decimal points, int id)
    {
        try
        {
            String commandText = "update Employee set points = @pointsValue where EmployeeID = @EmployeeID";
            SqlConnection conn = ProjectDB.connectToDB();
            SqlCommand select = new SqlCommand(commandText, conn);

            select.Parameters.AddWithValue("@pointsValue", points);
            select.Parameters.AddWithValue("@EmployeeID", id);
            select.ExecuteNonQuery();
            conn.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ex')", true);
        }
    }

    //math behind the quantity subtraction method
    public int quantitySubtraction(int quantity)
    {
        int answer = (quantity - 1);
        return answer;
    }

    //Subtracts the quantity in the RewardItem Table on the DB
    public void SubtractQuantity(int quantity, String id)
    {
        try
        {
            String commandText = "update RewardItem set quantity = @quantity where RewardID = @RewardID";
            SqlConnection conn = ProjectDB.connectToDB();
            SqlCommand select = new SqlCommand(commandText, conn);

            select.Parameters.AddWithValue("@quantity", quantity);
            select.Parameters.AddWithValue("@RewardID", id);
            select.ExecuteNonQuery();
            conn.Close();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ex')", true);
        }
    }

    public void hideLstViews()
    {
        lstRewardsView.Visible = false;
        lstElligable.Visible = false;
        lstSearchName.Visible = false;
        lstSearchProvider.Visible = false;
    }
    public int findQuantity(String RewardID)
    {
        try
        {
            String commandText = "Select Quantity from [dbo].[RewardItem] WHERE RewardID = @RewardID";
            SqlConnection conn = ProjectDB.connectToDB();
            SqlCommand select = new SqlCommand(commandText, conn);

            select.Parameters.AddWithValue("@RewardID", RewardID);
            SqlDataReader reader = select.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                int quantity = (int)reader["Quantity"];
                conn.Close();
                return quantity;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ex')", true);
            return -1;
        }
        return -1;
    }
}