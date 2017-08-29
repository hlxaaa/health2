using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common.Helper;

namespace WebApplication1.Webs
{
    public partial class Food : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);

        protected int ran = new Random().Next();
        protected string jsonStr = "";
        protected DataSet ds = new DataSet();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }      
            //GetFood();
            switch (Request["method"])
            {
                case "search":
                    GetFood();
                    ResJsonStr();
                    break;
                case "addFood":
                    AddFood();
                    UpdateFoodCache();
                    GetFood();
                    ResJsonStr();
                    break;
                case "updateFood":
                    UpdateFood();
                    UpdateFoodCache();
                    GetFood();
                    ResJsonStr();
                    break;
                case "deleteFood":
                    DeleteFood();
                    UpdateFoodCache();
                    GetFood();
                    ResJsonStr();
                    break;
                case "batchDelete":
                    BatchDelete();
                    UpdateFoodCache();
                    break;
                default:
                    GetFood();
                    break;
            }
        }

        protected void GetFood()
        {
            int pageIndex = 1;
            int pageSize = 27;

            int thePage = 1;
            if (Request["thePage"] != null)
                thePage = Convert.ToInt32(Request["thePage"]);

            string index = Request["thePage"];
            if (index != null)
                pageIndex = Convert.ToInt32(index);
            string size = Request["size"];
            if (size != null)
                pageSize = Convert.ToInt32(size);


            string sqlSelect = "select id,name from Food where isDeleted = 'False' ";

            //查询条件添加区
            string search = " ";
            if (Request["search"] != null && Request["search"].Trim() != "")
            {
                search = Request["search"].Trim();
                sqlSelect += " and id in (select id from Food where  ";
                string name = search;
                sqlSelect += " name like '%" + name + "%'";
                sqlSelect += ") ";
            }

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
            conn.Open();

            SqlDataAdapter daPage = new SqlDataAdapter(sqlSelect, conn);
            daPage.Fill(ds);
            int allCount = ds.Tables[0].Rows.Count;
            var pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();
            if (allCount > 0)
            {
                if (thePage > pages)//最后一页的最后一个被删掉时，处理
                {
                    thePage = pages;
                    x = (pageIndex - 2) * pageSize;
                    sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(sqlPaging, conn);
            da.Fill(ds);
            ds = Tool.DsToString(ds);
            conn.Close();
            jsonStr = Tool.GetJsonByDataset(ds);

            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + thePage + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
        }

        protected void AddFood()
        {
            var cache = MemCacheHelper.GetMyConfigInstance();
            //cache.Set("tongxiaoyi", "nande");
            //var a = cache.Get("tongxiaoyi");
            string name = Request["name"];
            string condition = " and isDeleted = 'False'";
            bool isExist = Tool.IsExist(name, "name", "Food", condition);
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string insertFood = "insert Food (name,isDeleted) values ('" + name + "','False')";
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(insertFood, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void UpdateFood()
        {
            string foodId = Request["id"];
            string name = Request["name"];
            string condition = " and isDeleted = 'False'";
            bool isExist = Tool.IsExist(name, "name", "Food", condition);
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string updateFood = "update Food set name = '" + name + "' where id = " + foodId;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(updateFood, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void DeleteFood()
        {
            string foodId = Request["id"];
            string sqlDelete = "update Food set isDeleted ='True' where id=" + foodId;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlDelete, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void BatchDelete()
        {
            string ids = Request["ids[]"];
            string batchDelete = "update Food set isDeleted = 'True' where id in (" + ids + ")";
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(batchDelete, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void UpdateFoodCache() {
            Tool.UpdateCache<DbOpertion.Models.Food>("Food", "List_Food", true);
        }
    }
}