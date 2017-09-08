using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class FoodType1 : System.Web.UI.Page
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
                case "batchDelete":
                    BatchDelete();
                    UpdateTypeCache();
                    break;
                case "deleteType":
                    DeleteType(Request["id"]);
                    UpdateTypeCache();
                    break;
                case "updateType":
                    UpdateType();
                    UpdateTypeCache();
                    break;
                case "addType":
                    AddType();
                    UpdateTypeCache();
                    break;
                case "search":
                    GetFoodType();
                    ResJsonStr();
                    break;
            }
        }

        protected void BatchDelete()
        {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids)
            {
                DeleteType(id);
            }
        }

        protected void DeleteType(string foodTypeId)
        {
            string str = "update FoodType set isDeleted = 'True' where id =" + foodTypeId;
            Tool.ExecuteNon(str);
        }

        protected void UpdateType()
        {
            string foodTypeId = Request["id"];
            string name = Request["name"];
            bool isExist = Tool.IsExist(name, "name", "FoodType", " and isDeleted='False'");
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string str = "update FoodType set name ='" + name + "' where id = " + foodTypeId;
            Tool.ExecuteNon(str);
        }

        protected void AddType()
        {
            string name = Request["name"];
            bool isExist = Tool.IsExist(name, "name", "FoodType", " and isDeleted='False'");
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string str = "insert FoodType values ('" + name + "','False')";
            Tool.ExecuteNon(str);
        }

        protected void GetFoodType()
        {
            string sqlSelect = "select * from FoodType where isDeleted='False' ";
            string search = " ";
            if (Request["search"] != null && Request["search"].Trim() != "")
            {
                search = Request["search"].Trim();
                sqlSelect += " and id in (select id from FoodType where  ";
                string name = search;
                sqlSelect += " name like '%" + name + "%'";
                sqlSelect += ")";
            }
            sqlSelect += " order by id";
            DataSet ds = Tool.ExecuteGetDs(sqlSelect);
            ds = Tool.DsToString(ds);
            jsonStr = Tool.GetJsonByDataset(ds);
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void UpdateTypeCache()
        {
            Tool.UpdateCache<DbOpertion.Models.FoodType>("FoodType", "List_Food_Type", false);
        }
    }
}