﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminRewards.aspx.cs" Inherits="AdminRewards" %>

<!DOCTYPE html>
<html>
    <head>
        <title>W3.CSS Template</title>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
        <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins">
        <style>
        body,h1,h2,h3,h4,h5 {font-family: "Poppins", sans-serif}
        #btnSave {
            background-color: #4CAF50;
            color: white;
            padding: 12px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        #btnSave:hover {
            background-color: #45a049;
        }
        input[type=text], .ddl, textarea {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
            margin-top: 6px;
            margin-bottom: 16px;
            resize: vertical;
        }
        body {font-size:16px;}
        .w3-half img{margin-bottom:-6px;margin-top:16px;opacity:0.8;cursor:pointer}
        .w3-half img:hover{opacity:1}
        </style>
    </head>
    <body>

    <!-- Sidebar/menu -->
    <nav class="w3-sidebar w3-red w3-collapse w3-top w3-large w3-padding" style="z-index:3;width:300px;font-weight:bold;" id="mySidebar"><br>
      <a href="javascript:void(0)" onclick="w3_close()" class="w3-button w3-hide-large w3-display-topleft" style="width:100%;font-size:22px">Close Menu</a>
      <div class="w3-container">
        <h3 class="w3-padding-64"><b>Top 10<br>Solutions</b></h3>
      </div>
      <div class="w3-bar-block">
        <a href="Admin.aspx" onclick="w3_close()" class="w3-bar-item w3-button w3-hover-white">Home</a>  
        <a href="AdminRewards.aspx" onclick="w3_close()" class="w3-bar-item w3-button w3-hover-white">Add Rewards</a> 
        <a href="AdminCreate.aspx" onclick="w3_close()" class="w3-bar-item w3-button w3-hover-white">Create/Edit Users</a> 
        <a href="AdminAnalytics.aspx" onclick="w3_close()" class="w3-bar-item w3-button w3-hover-white">View Analytics</a>  
        <a href="Logout.aspx" onclick="w3_close()" class="w3-bar-item w3-button w3-hover-white">Logout</a>
      </div>
    </nav>

    <!-- Top menu on small screens -->
    <header class="w3-container w3-top w3-hide-large w3-red w3-xlarge w3-padding">
      <a href="javascript:void(0)" class="w3-button w3-red w3-margin-right" onclick="w3_open()">☰</a>
      <span>Top 10 Solutions</span>
    </header>

    <!-- Overlay effect when opening sidebar on small screens -->
    <div class="w3-overlay w3-hide-large" onclick="w3_close()" style="cursor:pointer" title="close side menu" id="myOverlay"></div>

    <!-- !PAGE CONTENT! -->
    <div class="w3-main" style="margin-left:340px;margin-right:40px">

      <!-- Header -->
      <div class="w3-container" style="margin-top:80px" id="showcase">
        <h1 class="w3-jumbo"><b>Administration</b></h1>
        <h1 class="w3-xxxlarge w3-text-red"><b>Add a Reward</b></h1>
        <hr style="width:50px;border:5px solid red; float: left;" class="w3-round">
      </div>

    <div class="w3-container" id="administration" style="margin-top: 75px;">
        <form id="feed" runat="server">
            &nbsp;<asp:Label ID="lblName" runat="server" Text="Reward Name"></asp:Label>
            <br />
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <br /><br />
            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
            <br />
            <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
            <br /><br />
            <asp:Label ID="lblPrice" runat="server" Text="Price"></asp:Label>
            <br />
            <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
            <br /><br />
            <asp:Label ID="lblStart"  runat="server" Text="Start Date"></asp:Label>
            <br />
            <asp:TextBox ID="txtStartDate" placeholder="mm/dd/yyyy" runat="server"></asp:TextBox>
            <br /><br />
            <asp:Label ID="lblEnd" runat="server" Text="End Date"></asp:Label>
            <br />
            <asp:TextBox ID="txtEndDate" placeholder="mm/dd/yyyy" runat="server"></asp:TextBox>
            <br /><br />
            <asp:Label ID="lblQuantity" runat="server" Text="Quantity"></asp:Label>
            <br />
            <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
            <br /><br />
            <asp:Label ID="lblProvider" runat="server" Text="Reward Provider"></asp:Label>
            <br />
            <asp:DropDownList CssClass="ddl" ID="txtProvider" runat="server">
            </asp:DropDownList>
            <br /><br />
            <asp:Label ID="lblCategory" runat="server" Text="Reward Category"></asp:Label>
            <br />
            <asp:DropDownList CssClass="ddl" ID="txtCategory" runat="server">
            </asp:DropDownList>
            <br /><br />
            <asp:Button ID="btnSave" runat="server" Text="Save Reward" OnClick="btnSave_Click" />
            <br /><br />
            <div class="w3-container" style="margin-top:80px" id="editshowcase">       
                    <h1 class="w3-xxxlarge w3-text-red">Edit and Delete Rewards</h1>
                    <hr style="width:50px;border:5px solid red; float: left;" class="w3-round">
                </div>
            <br />
            <asp:GridView CssClass="ddl" ID="rewardGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataKeyNames="RewardID" DataSourceID="SqlDataSource2">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    <asp:BoundField DataField="RewardID" HeaderText="RewardID" InsertVisible="False" ReadOnly="True" SortExpression="RewardID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                    <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" />
                    <asp:BoundField DataField="EndDate" HeaderText="EndDate" SortExpression="EndDate" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Lab4ConnectionString %>" DeleteCommand="DELETE FROM [RewardItem] WHERE [RewardID] = @RewardID" InsertCommand="INSERT INTO [RewardItem] ([Name], [Description], [Price], [StartDate], [EndDate], [Quantity]) VALUES (@Name, @Description, @Price, @StartDate, @EndDate, @Quantity)" SelectCommand="SELECT [RewardID], [Name], [Description], [Price], [StartDate], [EndDate], [Quantity] FROM [RewardItem]" UpdateCommand="UPDATE [RewardItem] SET [Name] = @Name, [Description] = @Description, [Price] = @Price, [StartDate] = @StartDate, [EndDate] = @EndDate, [Quantity] = @Quantity WHERE [RewardID] = @RewardID">
                <DeleteParameters>
                    <asp:Parameter Name="RewardID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="Price" Type="Decimal" />
                    <asp:Parameter Name="StartDate" Type="DateTime" />
                    <asp:Parameter Name="EndDate" Type="DateTime" />
                    <asp:Parameter Name="Quantity" Type="Int32" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="Description" Type="String" />
                    <asp:Parameter Name="Price" Type="Decimal" />
                    <asp:Parameter Name="StartDate" Type="DateTime" />
                    <asp:Parameter Name="EndDate" Type="DateTime" />
                    <asp:Parameter Name="Quantity" Type="Int32" />
                    <asp:Parameter Name="RewardID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </form>
    </div>
  

    <!-- End page content -->
    </div>

    <!-- W3.CSS Container -->
    <div class="w3-light-grey w3-container w3-padding-32" style="margin-top:75px;padding-right:58px"><p class="w3-right">Powered by <a href="https://www.w3schools.com/w3css/default.asp" title="W3.CSS" target="_blank" class="w3-hover-opacity">w3.css</a></p></div>

    <script>
        // Script to open and close sidebar
        function w3_open() {
            document.getElementById("mySidebar").style.display = "block";
            document.getElementById("myOverlay").style.display = "block";
        }

        function w3_close() {
            document.getElementById("mySidebar").style.display = "none";
            document.getElementById("myOverlay").style.display = "none";
        }

        // Modal Image Gallery
        function onClick(element) {
            document.getElementById("img01").src = element.src;
            document.getElementById("modal01").style.display = "block";
            var captionText = document.getElementById("caption");
            captionText.innerHTML = element.alt;
        }
    </script>

    </body>
</html>

