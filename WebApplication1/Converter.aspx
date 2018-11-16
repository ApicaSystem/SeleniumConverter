<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Converter.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #TextArea1 {
            height: 257px;
            width: 566px;
        }
        #TextArea2 {
            height: 371px;
            width: 576px;
            margin-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:FileUpload id="FileUploadControl" runat="server" />
    <asp:Button runat="server" id="UploadButton" text="Upload" onclick="UploadButton_Click" />
    <br /><br />
    <asp:Label runat="server" id="StatusLabel" text="Upload status: " />
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        </p>
</form>
    <div>

    </div>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
</body>
</html>
