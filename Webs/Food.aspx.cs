﻿using Common.Helper;
using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class Food : System.Web.UI.Page
    {
        protected string jsonStr = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }
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
            }
        }

        protected void GetFood()
        {
            int pageIndex = 1;
            int pageSize = 27;//27

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
            DataSet ds = Tool.ExecuteGetDs(sqlSelect);
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
            ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);
            jsonStr = Tool.GetJsonByDataset(ds);

            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + thePage + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
        }

        protected void AddFood()
        {
            var cache = MemCacheHelper.GetMyConfigInstance();
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
            Tool.ExecuteNon(insertFood);
        }

        protected void UpdateFood()
        {
            string foodId = Request["id"];
            string name = Request["name"];
            string condition = " and isDeleted = 'False' and id!=" + foodId;
            bool isExist = Tool.IsExist(name, "name", "Food", condition);
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string updateFood = "update Food set name = '" + name + "' where id = " + foodId;
            Tool.ExecuteNon(updateFood);
        }

        protected void DeleteFood()
        {
            string foodId = Request["id"];
            string sqlDelete = "update Food set isDeleted ='True' where id=" + foodId;
            Tool.ExecuteNon(sqlDelete);
        }

        protected void BatchDelete()
        {
            string ids = Request["ids[]"];
            string batchDelete = "update Food set isDeleted = 'True' where id in (" + ids + ")";
            Tool.ExecuteNon(batchDelete);
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void UpdateFoodCache()
        {
            Tool.UpdateCache<DbOpertion.Models.Food>("Food", "List_Food", true);
        }
    }
}