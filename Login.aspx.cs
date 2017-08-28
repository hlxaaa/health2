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
    public partial class Login : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected int ran = new Random().Next();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Timeout = 30;
            switch (Request["method"]) { 
                case "login":
                    LoginIn();
                    break;
                case "loginOut":
                    LoginOut();
                    break;
            }
        }

        protected void LoginIn() {
            //Session["id"] = "101";
            string vCode = Request["vCode"];
            if (Session["vCode"].ToString().ToLower() != vCode.ToLower().Trim()) {
                Response.Write("vCodeError");
                Response.End();
                return;
            }

            string account = Request["account"];
            string selectAccount = "select restaurantid from Seller where loginname ='" + account + "'";
            conn.Open();
            SqlCommand sqlCom1 = new SqlCommand(selectAccount, conn);
            if (sqlCom1.ExecuteScalar() == null) {
                Response.Write("noAccount");
                Response.End();
                conn.Close();
                return;
            }

            string password = Request["password"];
            string str = "select restaurantid from Seller where loginname = '"+account+"' and password = '"+password+"'";
           
            SqlCommand sqlCom = new SqlCommand(str, conn);
            var restId = sqlCom.ExecuteScalar();
            conn.Close();
            if (restId == null) {
                Response.Write("passwordError");
                Response.End();
              
                return;
            }
            
            //if (restId != "0")
            //{
                Session["restId"] = restId;//管理员为0，普通商家有自己的餐厅ID
                conn.Close();
            //    Response.Write("userLogin");
            //    Response.End();
            //    //Response.Redirect("/WebSeller/Home.aspx", false);
            //    return;
            //}
            //Session["admin"] = "True";
        }

        protected void LoginOut() {
            Session.Abandon();
            Response.Redirect("/Login.aspx", false);
        }
    }
}