﻿using System;
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

            dictRest = Tool.GetDict("Restaurant", "id", "name", conn);
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
            //GetOrder(id, isAdmin);
            switch (Request["method"])
            {
                case "search":
                    GetOrder(id, isAdmin);
                    ResJsonStr();
                    break;
                case "clickOrder":
                    ClickOrder();
                    break;
                default:
                    GetOrder(id, isAdmin);
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
        }
    }
}