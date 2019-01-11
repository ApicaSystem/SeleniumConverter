<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FetchGroups.aspx.cs" Inherits="WebApplication1.FetchGroups" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 267px">
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
            <asp:Label ID="Label1" runat="server" Text="Auth Ticket"></asp:Label>
        </div>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Label ID="Label2" runat="server" Text="UserID"></asp:Label>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </form>
</body>
</html>
