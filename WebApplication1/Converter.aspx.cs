using System;
using System.Net;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        static string filename;


        // Web Button that allows users to choose the scenario , it does validate that the file is not large and is a .side file 
        public void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {

                    int fileSize = FileUploadControl.PostedFile.ContentLength;
                    if (fileSize > (5 * 1024))
                    {

                        StatusLabel.Text = "Filesize of image is too large. Maximum file size permitted is 1 MB";
                        return;
                    }

                    filename = System.IO.Path.GetFileName(FileUploadControl.FileName);
                    if (CheckFileType(filename) == false)
                    {
                        StatusLabel.Text = "Upload status: File is not .side format!";
                        return;
                    }
                    else
                    {
                        FileUploadControl.SaveAs(Server.MapPath("~/") + filename);
                        StatusLabel.Text = "Upload status: File uploaded!";
                        filePath = Server.MapPath("~/") + filename;
                    }
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }

        }

        // function that validates that the file is of .side extension 
        bool CheckFileType(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
                case ".side":
                    return true;
                default:
                    return false;
            }
        }

        // created classes that will be used in order to parse the .side file 
        // The way its been structured is there is one object called Script Info that could have multiple Suites inside of them ( in our case its only one suite) and within each suite
        // there will be a number of commads ( class defined as Test) and each command will have command , target , value , "list of targets" which is a new feature that .side introduces where 
        // it saves all possible locators for that element in a separate object ( xpath by location , xpath by attribute , css , name etc...)

        public class ScriptInfo
        {
            public string name { get; set; }
            public string url { get; set; }
            public List<Suites> Tests { get; set; }
        }

        public class Suites
        {
            public string name { get; set; }
            public List<Test> commands { get; set; }
        }

        public class Test
        {
            public string command;
            public string target;
            public string value;
            public String[][] targets;
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        // After parsing the .side file into the object above i save the info in the below structure that has the main information of the script itself 
        // there will be an object of Script that will have the main URL , Script Title and Script Steps (command , target , value , comment)
        public class Script
        {
            public string MainURL { get; set; }
            public string ScriptTitle { get; set; }
            public List<Steps> ScriptSteps = new List<Steps>();
        }

        public class Steps
        {
            public string command { get; set; }
            public string target { get; set; }
            public string value { get; set; }
            public string comment { get; set; }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        static string filePath { set; get; }
        
        // function using the NewtonSoft library that will parse the JSON file (.side script) and save it in object named result which is of class Script and return that converted script
        public Script ExtractJSON()
        {

            using (StreamReader sr = new StreamReader(filePath))
            {

                Script Converted_Script = new Script();
                // Read the stream to a string
                String line = sr.ReadToEnd();
                ScriptInfo result = JsonConvert.DeserializeObject<ScriptInfo>(line);
                Converted_Script.MainURL = result.url;
                Converted_Script.ScriptTitle = result.name;
                string temp = result.Tests[0].commands[3].targets.Length.ToString();
                // since there is the assumption that there is only one suite / script I'm only accessing the first element [0] and then parsing the JSON File into the Converted_Script
                for (int i = 0; i < result.Tests[0].commands.Count; i++)
                {
                    Steps TempScript = new Steps
                    {
                        command = result.Tests[0].commands[i].command,
                        target = result.Tests[0].commands[i].target,
                        value = result.Tests[0].commands[i].value,
                        comment = " "
                    };
                    Converted_Script.ScriptSteps.Add(TempScript);
                }
                Console.WriteLine(line);
                return Converted_Script;
            }
        }

        public string ConvertedPath;
        // this function will be creating the HTML File that is passed by the ExtractJSON function 
        public void CreateHTMLScript(Script Converted_Script)
        {
            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

            using (writer = new HtmlTextWriter(stringwriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                // <html>
                writer.RenderBeginTag(HtmlTextWriterTag.Head);
                // <head>
                writer.AddAttribute(HtmlTextWriterAttribute.Rel, "selenium.base");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, Converted_Script.MainURL);
                writer.RenderBeginTag(HtmlTextWriterTag.Link);
                writer.RenderEndTag();
                // <link = ........ />
                writer.RenderBeginTag(HtmlTextWriterTag.Title);
                // <title>
                writer.Write(Converted_Script.ScriptTitle);
                // Script Name
                writer.RenderEndTag();
                // </title>
                writer.RenderEndTag();
                // </head>
                writer.RenderBeginTag(HtmlTextWriterTag.Body);
                // <body>
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "1");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                // <table> 
                writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                // <thead>
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                // <tr>
                writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, "1");
                writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                // <td>
                writer.Write(Converted_Script.ScriptTitle);
                // Script Name
                writer.RenderEndTag();
                // </td>
                writer.RenderEndTag();
                // </tr>
                writer.RenderEndTag();
                // </thead>
                writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
                // <tbody> 
                string body = stringwriter.ToString();
                for (int i = 0; i < Converted_Script.ScriptSteps.Count; i++)
                {   // in the new selenium there is a number of commands that Selenium doesn't support including linkText 
                    if (Converted_Script.ScriptSteps[i].command == "click" | Converted_Script.ScriptSteps[i].command == "select" | Converted_Script.ScriptSteps[i].command == "type" | | Converted_Script.ScriptSteps[i].command == "sendKeys")
                    {
                        if (Converted_Script.ScriptSteps[i].target.Contains("linkText") == true)
                            Converted_Script.ScriptSteps[i].target = Converted_Script.ScriptSteps[i].target.Replace("linkText", "link");
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                        // <tr>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        // <td>
                        // The New selenium automaically waits for elements before pressing them in this part any action item "click / select / type / sendKeys will add a wait for element 
                        // to the HTML file before adding the command itself 
                        writer.Write("waitForElementPresent");
                        writer.RenderEndTag();
                        // </td>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        // <td>
                        writer.Write(Converted_Script.ScriptSteps[i].target);
                        writer.RenderEndTag();
                        //</ td >
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        //<td>
                        writer.Write(Converted_Script.ScriptSteps[i].comment);
                        writer.RenderEndTag();
                        //</td>
                        writer.RenderEndTag();
                        // </tr>
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        // <tr>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        // <td>



                        writer.Write(Converted_Script.ScriptSteps[i].command);
                        writer.RenderEndTag();
                        // </td>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        // <td>
                        writer.Write(Converted_Script.ScriptSteps[i].target);
                        writer.RenderEndTag();
                        // </td>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        //<td>
                        writer.Write(Converted_Script.ScriptSteps[i].value);
                        writer.RenderEndTag();
                        //</td >
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        //<td>
                        writer.Write(Converted_Script.ScriptSteps[i].comment);
                        writer.RenderEndTag();
                        //</td>
                        writer.RenderEndTag();
                        // </tr>
                    }
                    else
                    {
                        if (Converted_Script.ScriptSteps[i].target.Contains("linkText") == true)
                            Converted_Script.ScriptSteps[i].target = Converted_Script.ScriptSteps[i].target.Replace("linkText", "link");
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        // <tr>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        // <td>
                        writer.Write(Converted_Script.ScriptSteps[i].command);
                        writer.RenderEndTag();
                        // </td>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        // <td>
                        writer.Write(Converted_Script.ScriptSteps[i].target);
                        writer.RenderEndTag();
                        // </td>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        //<td>
                        writer.Write(Converted_Script.ScriptSteps[i].value);
                        writer.RenderEndTag();
                        //</td>
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        //<td>
                        writer.Write(Converted_Script.ScriptSteps[i].comment);
                        writer.RenderEndTag();
                        //</td>
                        writer.RenderEndTag();
                        // </tr>
                    }
                }

                writer.RenderEndTag();
                // </tbody>
                writer.RenderEndTag();
                // </table>
                writer.RenderEndTag();
                // </body>
                writer.RenderEndTag();
                // </html>

            }
            Console.WriteLine(stringwriter.ToString());
            string exportPath = AppDomain.CurrentDomain.BaseDirectory + @"\" + filename.Replace(".side", ".html");
            System.IO.File.WriteAllText(exportPath, stringwriter.ToString());
            StatusLabel.Text = "Convert status: Scenario Converted!";
            DownLoad(exportPath);
            // by end of this fucntion a file will be already created that will have all the commands parsed from the .side file in the new html file 
            // there is a small possibility that the resulted html script will not working immediately when uploaded into the portal and that is because the recorded element may be 
            // accessible using xpath or the id or whatever locator 
            // the script may need a minor tweaking by replacing the target with another target 
            // there is a list of new Selenium IDE commands that are not supported by the old selenium IDE
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = filePath;
            Script Converted_Script = new Script();
            Converted_Script = ExtractJSON();
            CreateHTMLScript(Converted_Script);


        }

        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "text/html";
                Response.WriteFile(file.FullName);
                Response.End();

            }
            else
            {
                Response.Write("This file does not exist.");
            }

        }

    }
}