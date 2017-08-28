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
    public partial class RecipeSet : System.Web.UI.Page
    {
        protected int ran = new Random().Next();
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        //protected string[] rest = { };
        //protected string[] restId = { };

        //protected string[] recipeId = { };
        //protected string[] recipe = { };
        //protected string[] available = { };
        //protected string restaurant = "";
        //protected string restaurantId = "";

        protected Dictionary<string, string> dictRest = new Dictionary<string, string>();
        protected DataSet ds = new DataSet();
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
                //id = Request["id"];
            }


            dictRest = Tool.GetDict("Restaurant", "id", "name", conn);
            GetRecipes(id);

            switch (Request["method"])
            {
                case "change":
                    //Change();
                    break;
                case "search":
                    ResJsonStr();
                    break;
                case "setAvailable":
                    SetAvailable();
                    break;
            }
        }

        protected void SetAvailable()
        {
            string id = Request["id"];
            string available = Request["available"];
            string str = "update Recipe set available = '" + available + "' where id =" + id;
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


        protected void GetRecipes(string restId)
        {
            id = restId;
            int pageIndex = 1;
            int pageSize = 4;

            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            //restId = "101";
            string sqlSelect = "select id,name,available from Recipe where 1=1";
            if (!string.IsNullOrEmpty(restId)) {
                sqlSelect += " and restaurantId = " + restId;
            }
            if (!string.IsNullOrEmpty(Request["search"]))
            {

                string search = Request["search"].Trim();
                sqlSelect += " and id in (select id from Recipe where  ";
                string name = search;
                sqlSelect += " name like '%" + name + "%')";
            }

            conn.Open();
            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";

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

            //string sqlSelectRest = "select id,name from Restaurant";
            //conn.Open();

            //DataSet ds2 = new DataSet();
            //SqlDataAdapter da2 = new SqlDataAdapter(sqlSelectRest, conn);
            //da2.Fill(ds2);
            //int count2 = ds2.Tables[0].Rows.Count;
            //rest = new string[count2];
            //restId = new string[count2];
            //for (int i = 0; i < count2; i++)
            //{
            //    rest[i] = ds2.Tables[0].Rows[i]["name"].ToString();
            //    restId[i] = ds2.Tables[0].Rows[i]["id"].ToString();
            //}
            //if (id != null)
            //    restaurantId = id;
            //else
            //{
            //    restaurantId = restId[0];
            //    id = restId[0];
            //}
            //string sqlSelectAvailable = "select id,name,available from Recipe where restaurantId = " + id;
            //SqlCommand sqlCom = new SqlCommand(sqlSelectAvailable, conn);
            //sqlCom.ExecuteScalar();
            //string result = Convert.ToString(sqlCom.ExecuteScalar());
            //DataSet ds = new DataSet();
            //if (result != null)
            //{
            //    SqlDataAdapter da = new SqlDataAdapter(sqlSelectAvailable, conn);
            //    da.Fill(ds);
            //    int count = ds.Tables[0].Rows.Count;
            //    recipe = new string[count];
            //    recipeId = new string[count];
            //    available = new string[count];
            //    for (int i = 0; i < count; i++)
            //    {
            //        recipe[i] = ds.Tables[0].Rows[i]["name"].ToString();
            //        recipeId[i] = ds.Tables[0].Rows[i]["id"].ToString();
            //        available[i] = ds.Tables[0].Rows[i]["available"].ToString();
            //    }
            //}
            //conn.Close();
        }

        //protected void Change()
        //{
        //    string id = Request["id"];
        //    string available = Request["available"];
        //    string sqlUpdate = "update Recipe set available = '" + available + "' where id = " + id;
        //    conn.Open();
        //    SqlCommand sqlCom = new SqlCommand(sqlUpdate, conn);
        //    sqlCom.ExecuteScalar();
        //    conn.Close();
        //}
    }
}