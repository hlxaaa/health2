using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class ArticleUrl : System.Web.UI.Page
    {
        protected string content = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Get(Convert.ToInt32(Request["id"]));
        }

        protected void Get(int id)
        {
            DataSet ds = new DataSet();
            string sqlSelect = "select content from Article where id = " + id;
            ds = Tool.ExecuteGetDs(sqlSelect);
            content = ds.Tables[0].Rows[0]["content"].ToString();
            content = content.Replace("*gt;", ">");
            content = content.Replace("*lt;", "<");
            content = content.Replace("*amp", "&");
        }
    }
}