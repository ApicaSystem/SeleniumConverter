<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Converter.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>
<html lang="en-us">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" dddd content="ie=edge">
    <title>Selenium Converter</title>
    <!-- Reset CSS stylesheet -->
    <link rel="stylesheet" href="reset.css" media="screen">
    <!-- Reset CSS stylesheet -->
    <link rel="stylesheet" href="Converter.css" media="screen">
    <!-- Boostrap Stylesheet -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css">

</head>

<body>

    <!-- Header (Jumbotron)-->
    <div class="container">
        <div class="jumbotron jumbotron-fluid">
            <img src="https://www.apicasystems.com/wp-content/uploads/2017/10/Apica-Logo.svg">
            <h2>Selenium Converter</h2>
            <p> Amr Saleh <p>
        </div>

        <!-- Instructions / Form -->
        <div class="card">
            <div class="row">
                <div class="col-lg-6">
                    <h2>Instructions:</h2>
                    <p>Selenium Project Changed their file type that is created using Selenium IDE from html tables to a JSON file , this tool was creted to convert these scripts recorded by the new Chrome and Firefox entenstion to the html format that you can then upload to the Apica Portal.</p>
                </div>
                <div class="col-lg-6">
                    <h2>Convert Selenium Script:</h2>
                    <form id="form1" runat="server">
                        <asp:FileUpload id="FileUploadControl" class="btn btn-outline-warning" runat="server" />
                        <asp:Button runat="server" id="UploadButton" class="btn btn-outline-success" text="Upload" onclick="UploadButton_Click" />
                        <br /><br />
                        <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
                        <p>
                            <asp:Button ID="Button1" runat="server" class="btn btn-primary" OnClick="Button1_Click" Text="Convert" />
                        </p>
                    </form>


                </div>
            </div>
        </div>
    </div>

</body>

</html>