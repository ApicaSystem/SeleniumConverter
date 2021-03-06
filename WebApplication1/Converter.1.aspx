﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Converter.aspx.cs" Inherits="WebApplication1.WebForm1" %>

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
                    <p>Lorem Ipsum är en utfyllnadstext från tryck- och förlagsindustrin. Lorem ipsum har varit
                        standard ända sedan 1500-talet, när en okänd boksättare tog att antal bokstäver och blandade
                        dem för att göra ett provexemplar av en bok. Lorem ipsum har inte bara överlevt fem
                        århundraden, utan även övergången till elektronisk typografi utan större förändringar. Det blev
                        allmänt känt på 1960-talet i samband med lanseringen av Letraset-ark med avsnitt av Lorem
                        Ipsum, och senare med mjukvaror som Aldus PageMaker.</p>
                </div>
                <div class="col-lg-6">
                    <h2>Form:</h2>
                    <button type="button" class="btn btn-outline-warning">Choose File</button>
                    <br>
                    <br>
                    <button type="button" class="btn btn-outline-success">Upload</button>
                    <br>
                    <br>
                    <button type="button" class="btn btn-primary">Output</button>

                    <form id="form1" runat="server">
                        <asp:FileUpload id="FileUploadControl" runat="server" />
                        <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
                        <br /><br />
                        <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
                        <p>
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
                        </p>
                    </form>


                </div>
            </div>
        </div>
    </div>

</body>

</html>