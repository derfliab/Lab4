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

public partial class GivePoints : System.Web.UI.Page
{
    public static int valueIndex;
    public static int pointIndex;
    public static int selectedEmployeeIndex;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.SearchEmployee();

        }
        if (Session["employeeLoggedIn"] == null)
        {
            Response.Redirect("Login.aspx"); //check that the filepath is correct
        }
        if (Session["employeeLoggedIn"].ToString() != "True")
        {
            Response.Redirect("Login.aspx"); //check that the filepath works
        }

        if (DropDownCompanyValue.SelectedIndex > 0)
        {
            valueIndex = DropDownCompanyValue.SelectedIndex;
        }
        
         

        if(DropDownPointsGiven.SelectedIndex > 0)
        {
            pointIndex = int.Parse(DropDownPointsGiven.SelectedValue);
        }

        DropDownPointsGiven.SelectedIndex = 0;
        selectValue();
    }
    private void SearchEmployee()
    {
        if (txtSearchTeamMember.Text != "")
        {
            GVTeamMember.Visible = true;
        }
        SqlConnection conn = ProjectDB.connectToDB();
        using (System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand())
        {
            string commandText = "SELECT EmployeeID, Firstname, LastName FROM Employee";
            if (!string.IsNullOrEmpty(txtSearchTeamMember.Text.Trim()))
            {
                commandText += " WHERE FirstName LIKE '%' + @FirstName + '%' OR LastName LIKE '%' + @LastName + '%'";
                insert.Parameters.AddWithValue("@FirstName", txtSearchTeamMember.Text.Trim());
                insert.Parameters.AddWithValue("@LastName", txtSearchTeamMember.Text.Trim());
            }
            insert.CommandText = commandText;
            insert.Connection = conn;
            using (SqlDataAdapter sda = new SqlDataAdapter(insert))
            {
                DataTable dt = new DataTable();
                sda.Fill(dt);
                GVTeamMember.DataSource = dt;
                GVTeamMember.DataBind();
            }


            conn.Close();
        }
    }
     
    protected void Search(object sender, EventArgs e)
    {
        this.SearchEmployee();
    }
    protected void OnPaging(object sender, GridViewPageEventArgs e)
    {
        GVTeamMember.PageIndex = e.NewPageIndex;
        this.SearchEmployee();
    }
    //protected void SelectEmployeeIndex(object sender, EventArgs e)
    //{

    //    //selectedEmployeeIndex = int.Parse(GVTeamMember.SelectedRow.Cells[1].Text);
    //}
    private void selectValue()
    {

        try
        {
            SqlConnection conn = ProjectDB.connectToDB();
            string commandText = "select Name from [dbo].[Value]";
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand(commandText, conn);
            DropDownCompanyValue.DataSource = insert.ExecuteReader();
            DropDownCompanyValue.DataTextField = "Name";
            DropDownCompanyValue.DataBind();
            DropDownCompanyValue.Items.Insert(0, "Select");

            conn.Close();
        }
        catch (Exception s)
        {
            Label.Text += "Value Error";
            Label.Text += s.Message;
        }
    }
    protected void SubmitGivePointsBtn_Click(object sender, EventArgs e)
    {
        bool working = true;
        if (pointIndex == 0)
        {
            working = false;
            Error.Text += "Please select from Points" + "<br>";
            
        }
        if (valueIndex == 0)
        {
            working = false;
            Error.Text += "Please select from Values" + "<br>";
        }

        if (working == true)
        {
            CommittToDBPoints();
        }
    }
    private void CommittToDBPoints()
    {
 
        try
        {
            SqlConnection conn = ProjectDB.connectToDB();
            string commandText = "INSERT INTO [dbo].[Achievement] (Description, Date, PointsAmount, EmployeeID, ValueID, RecEmployee) " +
                "VALUES (@Description, @Date, @PointsAmount, @EmployeeID, @ValueID, @RecEmployee)";
            System.Data.SqlClient.SqlCommand insert = new System.Data.SqlClient.SqlCommand(commandText, conn);
            insert.Parameters.AddWithValue("@Description", txtDescription.Value);
            insert.Parameters.AddWithValue("@Date", DateTime.Parse(txtDate.Value));
            insert.Parameters.AddWithValue("@PointsAmount", pointIndex);
            insert.Parameters.AddWithValue("@EmployeeID", 1);
            insert.Parameters.AddWithValue("@ValueID", valueIndex);
            insert.Parameters.AddWithValue("@RecEmployee", int.Parse(GVTeamMember.SelectedRow.Cells[1].Text));
            insert.ExecuteNonQuery();
            Label.Text += insert.CommandText;
            conn.Close();
        }
        catch(Exception ea)
        {
            Label.Text += ea.Message;
             
        }

    }
}