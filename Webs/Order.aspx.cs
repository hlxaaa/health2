using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Webs
{
    public partial class seller : System.Web.UI.Page
    {
        protected int ran = new Random().Next();
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected string id = "";
        protected string jsonStr = "";
        protected DataSet ds = new DataSet();
        protected bool isAdmin = false;
        protected string loginName = "";

        protected Dictionary<string, string> dictRest = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null)
            {
                Response.Redirect("/error.aspx", false);
                Response.Clear();
                Response.Write("no-login");
                Response.End();
                return;
            }

            //dictSeller = Tool.GetDict("Seller", "id", "restaurantid", conn);
            dictRest = Tool.GetDict("Restaurant", "id", "name", conn);
            //dictRecipe = Tool.GetDict("Recipe", "id", "name", conn);
            //dictCus = Tool.GetDict("Customer", "id", "name", conn);
            //Get();
            id = Session["restId"].ToString() ;
            if (id == "0")
            {
                isAdmin = true;
                id = Request["id"];
                loginName = "管理员";
            }
            else {
                loginName = Tool.GetRestNameById(id);
            }
            GetOrder(id, isAdmin);
            switch (Request["method"])
            {
                case "search":
                    ResJsonStr();
                    break;
                case "clickOrder":
                    ClickOrder();
                    break;
            }
        }

        protected void ClickOrder() {
            string id = Request["id"];
            string str = "update Orders set IsNew = 'false' where id = "+id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void GetOrder(string sellerId,bool isAdmin)
        {
            id = sellerId;
            int pageIndex = 1;
            int pageSize = 4;

            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            string sqlSelect = "select o.isNew,o.id,r.name as rest,r2.name as recipe,o.Pay,c.name as customer,o.ShopTime,o.PayType,c.phone,o.CreateTime from Orders as o,Restaurant as r,Recipe as r2,Customer as c where o.SellerId=r.id and o.RecipeId=r2.id and o.CustomerId=c.id ";
            if (!isAdmin)
                sqlSelect += " and r.id="+sellerId;
            string startTime = "2017-05-03";
            string endTime = "2079-06-05";
            if (!string.IsNullOrEmpty(Request["startTime"]))
                startTime = Request["startTime"];
            if (!string.IsNullOrEmpty(Request["endTime"]))
                endTime = Request["endTime"];
            sqlSelect += " and o.CreateTime between '" + startTime+" 00:00:00" + "' and '" + endTime+" 23:59:59'";



            if (!string.IsNullOrEmpty(sellerId))
            {
                sqlSelect += " and o.SellerId = " + sellerId;
            }

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by isNew desc, id desc) order by isNew desc, id desc";
            conn.Open();

            SqlDataAdapter da2 = new SqlDataAdapter(sqlSelect, conn);
            da2.Fill(ds);
            int allCount = ds.Tables[0].Rows.Count;
            int pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();

            SqlDataAdapter da = new SqlDataAdapter(sqlPaging, conn);
            da.Fill(ds);
            ds = Tool.DsToString(ds);
            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";

            conn.Close();
            //DataSet ds = new DataSet();
            //string sqlSelect = "select * from Orders";
            //conn.Open();
            //SqlDataAdapter myda = new SqlDataAdapter(sqlSelect, conn);
            //myda.Fill(ds);
            //conn.Close();
            //int count = ds.Tables[0].Rows.Count;
            //id = new int[count];
            //sellerName = new string[count];
            //customer = new string[count];
            //recipe = new string[count];
            //time = new string[count];
            //price = new string[count];
            //pay = new string[count];
            //payType = new string[count];
            //for (int i = 0; i < count; i++)
            //{
            //    id[i] = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
            //    sellerName[i] = dictRest[dictSeller[ds.Tables[0].Rows[i]["sid"].ToString()]];
            //    customer[i] = dictCus[ds.Tables[0].Rows[i]["cid"].ToString()];
            //    recipe[i] = dictRecipe[ds.Tables[0].Rows[i]["recipeId"].ToString()];
            //    time[i] = ds.Tables[0].Rows[i]["time"].ToString();
            //    price[i] = ds.Tables[0].Rows[i]["recipePrice"].ToString();
            //    pay[i] = ds.Tables[0].Rows[i]["pay"].ToString();
            //    payType[i] = ds.Tables[0].Rows[i]["payType"].ToString();
            //}
            //conn.Close();
        }
    }
}