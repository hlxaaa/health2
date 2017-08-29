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
    public partial class FoodType1 : System.Web.UI.Page
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
            switch (Request["method"]) { 
                case"batchDelete":
                    BatchDelete();
                    UpdateTypeCache();
                    break;
                case"deleteType":
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
                default:
                    GetFoodType();
                    break;
            }
        }

        protected void BatchDelete() {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids) {
                DeleteType(id);
            }
        }

        protected void DeleteType(string foodTypeId) {
            string str = "update FoodType set isDeleted = 'True' where id ="+foodTypeId;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void UpdateType() {
            string foodTypeId = Request["id"];
            string name = Request["name"];
            bool isExist = Tool.IsExist(name, "name", "FoodType", " and isDeleted='False'");
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string str = "update FoodType set name ='"+name+"' where id = "+foodTypeId;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void AddType() {
            string name = Request["name"];
            bool isExist = Tool.IsExist(name, "name", "FoodType", " and isDeleted='False'");
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string str = "insert FoodType values ('"+name+"','False')";
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void GetFoodType() {
            string sqlSelect = "select * from FoodType where isDeleted='False' ";

            //查询条件添加区
            string search = " ";
            if (Request["search"] != null && Request["search"].Trim() != "")
            {
                search = Request["search"].Trim();
                sqlSelect += " and id in (select id from FoodType where  ";
                string name = search;
                sqlSelect += " name like '%" + name + "%'";
                sqlSelect += ")";
            }
            conn.Open();

            sqlSelect += " order by id";
            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, conn);
            da.Fill(ds);
            ds = Tool.DsToString(ds);
            conn.Close();
            jsonStr = Tool.GetJsonByDataset(ds);
            //string pagesStr = ",\"pages\":" + pageSize + "";
            //string thePageStr = ",\"thePage\":" + thePage + "";
            //jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
        }

        protected void ResJsonStr() {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void UpdateTypeCache() {
            Tool.UpdateCache<DbOpertion.Models.FoodType>("FoodType", "List_Food_Type", false);
        }
    }
}