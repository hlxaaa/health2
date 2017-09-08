using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace WebApplication1.Webs
{
    public partial class Restaurant : System.Web.UI.Page
    {
        protected Dictionary<string, string> dictCategory = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }
            dictCategory = GetDict();

            switch (Request["method"])
            {
                case "search":
                    Response.Write(GetRest());
                    Response.End();
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

        protected void BatchDelete()
        {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids)
            {
                DeleteRest(id);
            }
        }

        protected void DeleteRest(string restId)
        {
            string getImg = "select images from Restaurant where id = " + restId;
            string img = Tool.ExecuteScalar(getImg);
            string[] imgs = img.Split('|');
            string path = Server.MapPath("").Replace("\\", "/") + "/";
            path = path.Substring(0, path.Length - 5);
            foreach (var i in imgs)
            {
                if (File.Exists(path + i))
                    File.Delete(path + i);
            }

            string delRest = "update  Restaurant set isDeleted ='true' where id = " + restId;
            Tool.ExecuteNon(delRest);
            string delRecipe = "update Recipe set isDeleted ='true' where restaurantId=" + restId;
            Tool.ExecuteNon(delRecipe);
        }

        protected string GetRest()
        {
            int pageIndex = 1;
            int pageSize = 14;
            string index = Request["thePage"];
            if (index != null)
                pageIndex = Convert.ToInt32(index);
            string size = Request["size"];
            if (size != null)
                pageSize = Convert.ToInt32(size);

            string sqlSelect = "select r.id,name,address,phone,dd.typeValue as category,sales,consumption,businesshours,discount from Restaurant as r,DataDictionary as dd where dd.typename='餐厅类型' and dd.id=r.category and r.IsDeleted='False' ";

            string cate = "";
            if (Request["cate"] != null && Request["cate"] != "")
            {
                cate = Request["cate"];
                sqlSelect += "and r.id in ( select r.id from Restaurant as r,DataDictionary as d where r.category=d.id and r.category= " + Tool.GetKey(dictCategory, cate) + ") ";
            }

            //搜索条件
            int pages = 0;
            int thePage = 1;
            if (Request["thePage"] != null)
                thePage = Convert.ToInt32(Request["thePage"]);

            string search = " ";
            if (Request["search"] != null && Request["search"].Trim() != "")
            {
                search = Request["search"].Trim();
                sqlSelect += " and r.id in (select id from Restaurant where  ";
                string name = search;
                sqlSelect += " name like '%" + name + "%'";

                sqlSelect += " or address like '%" + search + "%'";

                foreach (var v in dictCategory.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictCategory, v);
                        sqlSelect += " or category=" + id;
                    }
                }

                sqlSelect += ")";
            }

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc ) order by id desc";
            DataSet ds = Tool.ExecuteGetDs(sqlSelect);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
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
            ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);

            //int count = ds.Tables[0].Rows.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    ds.Tables[0].Rows[i]["category"] = dictCategory[ds.Tables[0].Rows[i]["category"].ToString()];
            //}

            string jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + thePage + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
            return jsonStr;
        }

        protected Dictionary<string, string> GetDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            DataSet ds = new DataSet();
            string str = "select id,typeValue from DataDictionary where typename ='餐厅类型'";
            ds = Tool.ExecuteGetDs(str);
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                dict.Add(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["typeValue"].ToString());
            }

            return dict;
        }
    }
}