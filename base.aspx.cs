using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _base : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected int ran = new Random().Next();

        protected string str = ""; 
        protected void Page_Load(object sender, EventArgs e)
        {
            str = "我,魔";
        }

        protected string[] test() {
            string[] a = str.Split(',');
            return a;
        }

        protected void getSocket() { 
            
        }
    }
}