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
using System.IO;

namespace WebApplication1.Webs
{
    public partial class Restaurant : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        //protected string[] name = { };
        //protected string[] id = { };
        //protected string[] available = { };
        //protected string[] foodtypes = { };
        //protected string[] foods = { };
        //protected string[] restaurant = { };
        //protected string[] tags = { };
        //protected string[] images = { };
        //protected string[] sales = { };
        //protected string[] price = { };
        protected DataSet ds = new DataSet();
        protected int ran = new Random().Next();
        protected string jsonStr = "";
        protected JObject jo = new JObject();
        protected Dictionary<string, string> dictCategory = new Dictionary<string, string>();

        //protected Dictionary<string, string> dictRest = new Dictionary<string, string>();
        //protected Dictionary<string, string> dictFood = new Dictionary<string, string>();
        //protected Dictionary<string, string> dictFoodType = new Dictionary<string, string>();
        //protected Dictionary<string, string> dictTags = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }      
            dictCategory = GetDict(conn);
            GetRest();

            switch (Request["method"])
            {
                case "search":
                    ResJsonStr();
                    break;
                case "deleteRest":
                    DeleteRest(Request["id"]);
                    WebApplication1.Webs.RestaurantContent.UpdateRestCache();
                    break;
                case "batchDelete":
                    BatchDelete();
                    WebApplication1.Webs.RestaurantContent.UpdateRestCache();
                    break;
            }
        }

        protected void BatchDelete() { 
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids) {
                DeleteRest(id);
            }
        }

        protected void DeleteRest(string restId) {
            conn.Open();
            string getImg = "select images from Restaurant where id = " + restId;
            SqlCommand sqlCom = new SqlCommand(getImg, conn);
            string img = (string)sqlCom.ExecuteScalar();
            string[] imgs = img.Split('|');
            string path = Server.MapPath("").Replace("\\", "/") + "/";
            path = path.Substring(0, path.Length - 5);
            foreach (var i in imgs)
            {
                if (File.Exists(path + i))
                    File.Delete(path + i);
            }

            string delRest = "update  Restaurant set isDeleted ='true' where id = " + restId;
            SqlCommand sc2 = new SqlCommand(delRest, conn);
            sc2.ExecuteScalar();
            string delRecipe = "update Recipe set isDeleted ='true' where restaurantId="+restId;
            sc2 = new SqlCommand(delRecipe, conn);
            sc2.ExecuteScalar();
            conn.Close();
        }

        protected void GetRest() {
            int pageIndex = 1;
            int pageSize = 14;
            string index = Request["thePage"];
            if (index != null)
                pageIndex = Convert.ToInt32(index);
            string size = Request["size"];
            if (size != null)
                pageSize = Convert.ToInt32(size);

            
            string sqlSelect = "select * from Restaurant where isDeleted ='false' ";

            string cate = "";
            if (Request["cate"]!=null&&Request["cate"]!="")
            {
                cate=Request["cate"];
                sqlSelect += "and id in ( select r.id from Restaurant as r,DataDictionary as d where r.category=d.id and r.category= "+Tool.GetKey(dictCategory,cate)+") ";
            }

            //搜索条件
            int pages = 0;
            int thePage = 1;
            if (Request["thePage"] != null)
                thePage = Convert.ToInt32(Request["thePage"]);

            string search = " ";
            if (Request["search"] != null&&Request["search"].Trim() != "")
            {
                search = Request["search"].Trim();
                sqlSelect += " and id in (select id from Restaurant where  ";
                string name = search;
                sqlSelect += " name like '%" + name + "%'";

                sqlSelect += " or address like '%"+search+"%'";

                foreach (var v in dictCategory.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictCategory, v);
                        sqlSelect += " or category="+id;
                    }
                }

                sqlSelect += ")";
            }
            
            //if (Request["category"] != null)
            //{
            //    string category = "";
            //    sqlSelect += " and category = " + category;
            //}

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc ) order by id desc";
            conn.Open();
            SqlDataAdapter daCount = new SqlDataAdapter(sqlSelect, conn);
            daCount.Fill(ds);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();

            SqlDataAdapter da = new SqlDataAdapter(sqlPaging, conn);
            da.Fill(ds);
            ds = Tool.DsToString(ds);

            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++) {
                ds.Tables[0].Rows[i]["category"]=dictCategory[ds.Tables[0].Rows[i]["category"].ToString()];
            }


            conn.Close();
            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + thePage + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";

            jo = JObject.Parse(jsonStr);
            //var a = jo["Table1"][0]["name"];
            //var b = jo["Table1"];
        }

        protected void GetRecipes() {
            string restId = Request["restId"];
            string selectRecipes = "select id,name from Recipe where restaurantId = "+restId;
            //得到这些食谱的id和name，餐厅内页的食谱估计点击之后要转到食谱的内页的
        }

        protected void AddRest(){
            string name = Request["name"];
            string address = Request["address"];
            string coordinate = Request["coordinate"];
            string category = Request["category"];
            string phone = Request["phone"];
            string sales = Request["sales"];
            string consumption = Request["consumption"];
            string startTime = Request["startTime"];
            string endTime = Request["endTime"];
            string[] discounts = Request.Form.GetValues("discounts[]");
            foreach (HttpPostedFile f in Request.Files)
            {
                //f...
            }

        }

        protected void UpdateRest() { 
            
        }

        protected void ResJsonStr()
        {
          
            Response.Write(jsonStr);
            Response.End();
        }

        protected Dictionary<string, string> GetDict(SqlConnection conn)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            DataSet ds = new DataSet();
            string str = "select id,typeValue from DataDictionary where typename ='餐厅类型'";
            conn.Open();
            SqlDataAdapter myda = new SqlDataAdapter(str, conn);
            myda.Fill(ds);
            conn.Close();
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                dict.Add(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["typeValue"].ToString());
            }

            return dict;
        }
    }
}