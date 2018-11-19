using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace WebApplication1
{
    public partial class FetchGroups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<TopMonitorGroup> results = UserAssignedTopMonitorGroup();
            if (!IsPostBack)
            {
                // Load this data only once.
                GridView1.DataSource = CreateDataSource(results);
                GridView1.DataBind();
            }
            for (int i= 0;i <results.Count; i++)
            {

                PostData(results[i].id);
            }

        }


        public void PostData(string groupID)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api-wpm.apicasystem.com/v3/groups/" + groupID + "/users?auth_ticket=EE0975AC-B42E-41A8-BF6E-CC650D7F6EA6");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "[\"f7d29c5e-e541-4a1d-830b-2e02a84ef589\"]";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

       

        ICollection CreateDataSource(List<TopMonitorGroup> results)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Monitor Group ID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Monitor Group Name", typeof(string)));

            

            for (int i = 0; i < results.Count; i++)
            {
                TopMonitorGroup Temp = results[i];
                dr = dt.NewRow();

                dr[0] = Temp.id ;
                dr[1] = Temp.name;
             

                dt.Rows.Add(dr);
            }

            DataView dv = new DataView(dt);
            return dv;
        }
        public class TopMonitorGroup
        {
            [JsonProperty("id")]
            public string id { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
        }

        public List<TopMonitorGroup> UserAssignedTopMonitorGroup()
        {
            string html = string.Empty;
            string url = @"https://api-wpm.apicasystem.com/v3/groups?auth_ticket=EE0975AC-B42E-41A8-BF6E-CC650D7F6EA6";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.PreAuthenticate = true;
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            List<TopMonitorGroup> results = JsonConvert.DeserializeObject<List<TopMonitorGroup>>(html);

            return results;

           
        }

    }
}