using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace WebApplication1.Webs
{
    public partial class Recipe : System.Web.UI.Page
    {
        protected string[] name = { };
        protected string[] recipeId = { };
        protected string[] images = { };
        protected string jsonStr = "";
        protected Dictionary<string, string> dictRest = new Dictionary<string, string>();
        protected Dictionary<string, string> dictFood = new Dictionary<string, string>();
        protected Dictionary<string, string> dictTags = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }
            dictRest = Tool.GetDict("Restaurant", "id", "name");
            dictFood = Tool.GetDict("Food", "id", "name");
            dictTags = Tool.GetDict("Tag", "id", "name");

            switch (Request["method"])
            {
                case "search":

                    GetRecipe();
                    ResJsonStr();
                    break;
                case "deleteRecipe":
                    DeleteRecipe(Request["id"]);
                    WebApplication1.Webs.RecipeContent.UpdateRecipeCache();
                    break;
                case "batchDelete":
                    BatchDelete();
                    WebApplication1.Webs.RecipeContent.UpdateRecipeCache();
                    break;
            }
        }

        protected void BatchDelete()
        {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids)
            {
                DeleteRecipe(id);
            }
        }

        protected void DeleteRecipe(string id)
        {
            string getImg = "select images from Recipe where id = " + id;
            string img = Tool.ExecuteScalar(getImg);
            string[] imgs = img.Split('|');
            string path = Server.MapPath("").Replace("\\", "/") + "/";
            path = path.Substring(0, path.Length - 5);
            foreach (var i in imgs)
            {
                if (File.Exists(path + i))
                    File.Delete(path + i);
            }

            string delRecipe = "update Recipe set isDeleted = 'True' where id = " + id;
            Tool.ExecuteNon(delRecipe);
        }

        protected void GetRecipe()
        {
            int pageIndex = 1;
            int pageSize = 10;
            if (Request["pageSize"] != null)
                pageSize = Convert.ToInt32(Request["pageSize"]);
            string index = Request["thePage"];
            if (index != null)
                pageIndex = Convert.ToInt32(index);
            StringBuilder sqlSelect = new StringBuilder();
            sqlSelect.Append(" select id,name,restaurantId,available,sales,price,a.foods, b.tags from Recipe,( select recipeId, stuff((select ' '+ren1+','+CONVERT(nvarchar(50),rew)+'g' from One where One.recipeId=Recipe_foods.recipeId for xml path('')),1,1,'') as foods   from Recipe_foods  group by recipeId   ) as A, ( select relationId, stuff((select ' '+CONVERT(nvarchar(50),Ren) from t2 where t2.relationId=Tag_Relation.relationId for xml path('')),1,1,'') as tags   from Tag_Relation  group by relationId   ) as b where isDeleted='False'  and a.recipeId=Recipe.id and b.relationId=a.recipeId");
            StringBuilder selectAll = new StringBuilder();
            selectAll.Append("select count(id) from recipe where isDeleted = 'False'");
            //搜索条件
            int pages = 0;
            int thePage = 1;
            if (Request["thePage"] != null)
                thePage = Convert.ToInt32(Request["thePage"]);

            string search = " ";
            if (Request["search"] != null && Request["search"].Trim() != "")
            {
                search = Request["search"].Trim();
                sqlSelect.Append(" and id in (select id from Recipe where  ");
                selectAll.Append(" and id in (select id from Recipe where  ");

                string name = search;
                sqlSelect.Append(" name like '%" + name + "%'");
                selectAll.Append(" name like '%" + name + "%'");

                foreach (var v in dictFood.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictFood, v);
                        sqlSelect.Append(" or id in (select r.id from Recipe as r,Recipe_foods as r2 where r2.foodId = " + id + " and r2.recipeId=r.id)");
                        selectAll.Append(" or id in (select r.id from Recipe as r,Recipe_foods as r2 where r2.foodId = " + id + " and r2.recipeId=r.id)");
                    }
                }

                foreach (var v in dictTags.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictTags, v);
                        sqlSelect.Append(" or id in (select r.id from Recipe as r,tag_relation as r2 where r2.tagId = " + id + " and r2.relationId=r.id and typename='recipe')");
                        selectAll.Append(" or id in (select r.id from Recipe as r,tag_relation as r2 where r2.tagId = " + id + " and r2.relationId=r.id and typename='recipe')");
                    }
                }

                foreach (var v in dictRest.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictRest, v);
                        sqlSelect.Append(" or id in (select r.id from Recipe as r,Restaurant as r2 where r2.id = " + id + " and r2.id=r.restaurantId and r2.isDeleted='False')");
                        selectAll.Append(" or id in (select r.id from Recipe as r,Restaurant as r2 where r2.id = " + id + " and r2.id=r.restaurantId and r2.isDeleted='False')");
                    }
                }

                sqlSelect.Append(")");
                selectAll.Append(")");
            }

            int sale1 = 0;
            int sale2 = 9999999;
            if (Request.Form.GetValues("saleRange[]") != null)
            {
                if (Request.Form.GetValues("saleRange[]")[0] != "")
                    sale1 = Convert.ToInt32(Request.Form.GetValues("saleRange[]")[0]);
                if (Request.Form.GetValues("saleRange[]")[1] != "")
                    sale2 = Convert.ToInt32(Request.Form.GetValues("saleRange[]")[1]);
                sqlSelect.Append(" and sales between " + sale1 + " and " + sale2 + "");
                selectAll.Append(" and sales between " + sale1 + " and " + sale2 + "");
            }

            int price1 = 0;
            int price2 = 999999;
            if (Request.Form.GetValues("priceRange[]") != null)
            {
                if (Request.Form.GetValues("priceRange[]")[0] != "")
                    price1 = Convert.ToInt32(Request.Form.GetValues("priceRange[]")[0]);
                if (Request.Form.GetValues("priceRange[]")[1] != "")
                    price2 = Convert.ToInt32(Request.Form.GetValues("priceRange[]")[1]);
                sqlSelect.Append(" and price between " + price1 + " and " + price2 + "");
                selectAll.Append(" and sales between " + sale1 + " and " + sale2 + "");
            }

            string startTime = "2017-05-03";
            string endTime = "2099-12-31";
            if (Request.Form.GetValues("timeRange[]") != null)
            {
                if (Request.Form.GetValues("timeRange[]")[0] != "")
                    startTime = Request.Form.GetValues("timeRange[]")[0];
                if (Request.Form.GetValues("timeRange[]")[1] != "")
                    endTime = Request.Form.GetValues("timeRange[]")[1];
                sqlSelect.Append(" and createTime between '" + startTime + "' and '" + endTime + "'");
                selectAll.Append(" and createTime between '" + startTime + "' and '" + endTime + "'");
            }

            if (Request["available"] != null)
            {
                string available = Request["available"];
                sqlSelect.Append(" and available = '" + available + "'");
                selectAll.Append(" and available = '" + available + "'");
            }

            string[] tags = Request.Form.GetValues("tags[]");
            if (tags != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    string id = Tool.GetKey(dictTags, tags[i]);
                    sqlSelect.Append(" and  id in (select r.id from Recipe as r,tag_relation as r2 where r2.tagId=" + id + " and r2.relationId=r.id and typename='recipe')");
                    selectAll.Append(" and  id in (select r.id from Recipe as r,tag_relation as r2 where r2.tagId=" + id + " and r2.relationId=r.id and typename='recipe')");

                }
            }

            string[] foods = Request.Form.GetValues("foods[]");
            if (foods != null)
            {
                for (int i = 0; i < foods.Length; i++)
                {
                    string id = Tool.GetKey(dictFood, foods[i]);
                    sqlSelect.Append(" and  id in (select r.id from Recipe as r,Recipe_foods as r2 where r2.foodId=" + id + " and r2.recipeId=r.id)");
                    selectAll.Append(" and  id in (select r.id from Recipe as r,Recipe_foods as r2 where r2.foodId=" + id + " and r2.recipeId=r.id)");
                }
            }

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = " with t2 as ( 	select tr.relationId,(t.name) as ren from Tag_Relation as tr , Tag as t where typename='recipe' and tr.tagId=t.id ) ,One AS ( select rf.recipeId,f.name as ren1,rf.weight as rew from Recipe_foods as rf , food as f where rf.foodId=f.id ) ";
            sqlPaging += "select top " + pageSize + " * from (" + sqlSelect.ToString() + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";

            int allCount = Convert.ToInt32(Tool.ExecuteScalar(selectAll.ToString()));
            pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;

            if (allCount > 0)
            {
                if (thePage > pages)//最后一页的最后一个被删掉时，处理
                {
                    thePage = pages;
                    x = (pageIndex - 2) * pageSize;
                    sqlPaging = " with t2 as ( 	select tr.relationId,(t.name) as ren from Tag_Relation as tr , Tag as t where typename='recipe' and tr.tagId=t.id ) ,One AS ( select rf.recipeId,f.name as ren1,rf.weight as rew from Recipe_foods as rf , food as f where rf.foodId=f.id ) ";
                    sqlPaging += "select top " + pageSize + " * from (" + sqlSelect.ToString() + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
                }
            }
            DataSet ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);
            int count = ds.Tables[0].Rows.Count;
            recipeId = new string[count];
            images = new string[count];
            for (int i = 0; i < count; i++)
            {
                recipeId[i] = ds.Tables[0].Rows[i]["id"].ToString();
                ds.Tables[0].Rows[i]["available"] = (ds.Tables[0].Rows[i]["available"].ToString() == "True") ? "有" : "没有";
                ds.Tables[0].Rows[i]["restaurantId"] = dictRest.ContainsKey(ds.Tables[0].Rows[i]["restaurantId"].ToString()) ? dictRest[ds.Tables[0].Rows[i]["restaurantId"].ToString()] : "该餐厅已被删除";
            }

            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + thePage + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

    }
}