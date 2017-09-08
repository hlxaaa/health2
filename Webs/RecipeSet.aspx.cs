using System;
using System.Collections.Generic;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class RecipeSet : System.Web.UI.Page
    {
        protected Dictionary<string, string> dictRest = new Dictionary<string, string>();
        protected string jsonStr = "";
        protected string id = "";
        protected bool isAdmin = false;
        protected string loginName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null)
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }
            id = Session["restId"].ToString();
            if (id == "0")
            {
                isAdmin = true;
                id = Request["id"];
                loginName = "管理员";
            }
            else
            {
                loginName = Tool.GetRestNameById(id);
            }

            switch (Request["method"])
            {
                case "search":
                    dictRest = Tool.GetDict("Restaurant", "id", "name");
                    GetRecipes(id);
                    ResJsonStr();
                    break;
                case "setAvailable":
                    SetAvailable();
                    WebApplication1.Webs.RecipeContent.UpdateRecipeCache();
                    break;
            }
        }

        protected void SetAvailable()
        {
            string recipeId = Request["id"];
            string available = Request["available"];
            string str = "update Recipe set available = '" + available + "' where id =" + recipeId;
            Tool.ExecuteNon(str);
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void GetRecipes(string restId)
        {
            id = restId;
            int pageIndex = 1;
            int pageSize = 4 * 12;

            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            string sqlSelect = "select id,name,available from Recipe where isDeleted='False'";
            if (!string.IsNullOrEmpty(restId))
            {
                sqlSelect += " and restaurantId = " + restId;
            }
            if (!string.IsNullOrEmpty(Request["search"]))
            {
                string search = Request["search"].Trim();
                sqlSelect += " and id in (select id from Recipe where  ";
                string name = search;
                sqlSelect += " name like '%" + name + "%')";
            }

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
            DataSet ds = Tool.ExecuteGetDs(sqlSelect);
            int allCount = ds.Tables[0].Rows.Count;
            int pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();
            ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);
            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
        }
    }
}