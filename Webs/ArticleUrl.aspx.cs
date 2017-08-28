using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Webs
{
    public partial class ArticleUrl : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected string content = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Get(Convert.ToInt32(Request["id"]));
        }

        protected void Get(int id)
        {
            DataSet ds = new DataSet();
            string sqlSelect = "select content from Article where id = " + id;
            conn.Open();
            SqlDataAdapter myda = new SqlDataAdapter(sqlSelect, conn);
            myda.Fill(ds);
            conn.Close();
            content = ds.Tables[0].Rows[0]["content"].ToString();
            content = content.Replace("*gt;", ">");
            content = content.Replace("*lt;", "<");
            content = content.Replace("*amp", "&");
        }
    }
}