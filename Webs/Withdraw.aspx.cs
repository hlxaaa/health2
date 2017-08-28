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
    public partial class Withdraw : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected string sellerId = "";
        protected string jsonStr = "";
        protected DataSet ds = new DataSet();

        protected int ran = new Random().Next();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString()!="0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }      

            sellerId = Request["id"];
            sellerId = "1";
            GetWithdraw(sellerId);
            switch (Request["method"]) { 
                case"search":
                    Response.Write(jsonStr);
                    Response.End();
                    break;
                case"doWithdraw":
                    DoWithdraw();
                    break;
            }
        }

        protected void DoWithdraw() {
            string withdrawId = Request["withdrawId"];
            conn.Open();
            string str1 = "update Withdraw set applyState='True' where id=" + withdrawId;
            SqlCommand sqlCom = new SqlCommand(str1, conn);
            sqlCom.ExecuteScalar();
            string str2 = "select applyMoney from Withdraw where id = " + withdrawId;
            sqlCom = new SqlCommand(str2, conn);
            double withdraw = Convert.ToDouble(sqlCom.ExecuteScalar());
            string str3 = "select sellerid from Withdraw where id =" + withdrawId;
            sqlCom = new SqlCommand(str3, conn);
            var sellerId = sqlCom.ExecuteScalar();
            string str4 = "update Seller set balance =(balance-" + withdraw + ") where id = " + sellerId;
            sqlCom = new SqlCommand(str4, conn);
            sqlCom.ExecuteScalar();

            conn.Close();
        }

        protected void GetWithdraw(string sellerId) {
            int pageIndex = 1;
            int pageSize = 3;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            string sqlSelect = "select w.id,r.name,s.balance,w.applyMoney,w.applyTime,w.applyState from Withdraw as w,Restaurant as r,Seller  as s where  s.restaurantid=r.id and w.sellerId=s.id";

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by applyState,id desc) order by applyState,id desc";
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, conn);

            DataSet ds2 = new DataSet();
            da.Fill(ds2);
            int allCount = ds2.Tables[0].Rows.Count;
            int pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds2.Clear();

            da = new SqlDataAdapter(sqlPaging, conn);
            da.Fill(ds2);
            ds2 = Tool.DsToString(ds2);
            jsonStr = Tool.GetJsonByDataset(ds2);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";

            conn.Close();
        }
    }
}