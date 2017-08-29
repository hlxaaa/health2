using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Webs
{
    public partial class accountSet : System.Web.UI.Page
    {
        protected int ran = new Random().Next();
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        //protected int[] id = { };
        //protected string[] name = { };
        //protected string[] loginname = { };
        //protected string[] password = { };
        //protected string[] restId = { };
        //protected string[] rest = { };
        protected Dictionary<string, string> dictRest = new Dictionary<string, string>();
        protected DataSet ds = new DataSet();
        protected string jsonStr = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }      
            //dictRest = Tool.GetDict("Restaurant", "id", "name", conn);
            //GetSeller();
            switch (Request["method"])
            {
                case"getRest":
                    GetRest();
                    break;
                case "search":
                    GetSeller();
                    ResJsonStr();
                    break;
                case "addSeller":
                    AddSeller();
                    break;
                case "updateSeller":
                    UpdateSeller();
                    break;
                case "deleteSeller":
                    DeleteSeller(Request["id"]);
                    break;
                case "batchDelete":
                    BatchDelete();
                    break;
                default:
                    GetSeller();
                    break;
            }
        }

        protected void BatchDelete() {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids) {
                DeleteSeller(id);
            }
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void GetSeller()
        {
            int pageIndex = 1;
            int pageSize = 5;
            if (!string.IsNullOrEmpty( Request["thePage"]))
                pageIndex = Convert.ToInt32( Request["thePage"]);

            int pages = 0;

            string sqlSelect = "select s.id,r.name,s.loginname,s.password from Seller as s ,Restaurant as r where s.restaurantid=r.id and s.isDeleted='False'";

            if (!string.IsNullOrEmpty(Request["search"]))
            {
                sqlSelect += " and r.name like '%" + Request["search"].Trim() + "%'";
            }

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";

            conn.Open();

            SqlDataAdapter da2 = new SqlDataAdapter(sqlSelect, conn);
            da2.Fill(ds);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
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
            //string sqlSelect = "select b.name,a.id,restaurantid,loginname,password,balance from Seller as a,Restaurant as b where a.restaurantid = b.id";
            //conn.Open();
            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter(sqlSelect, conn);
            //da.Fill(ds);
            //int count = ds.Tables[0].Rows.Count;
            //id = new int[count];
            //name = new string[count];
            //loginname = new string[count];
            //password = new string[count];
            //restId = new string[count];
            //rest = new string[count];
            //for (int i = 0; i < count; i++)
            //{
            //    id[i] = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
            //    name[i] = ds.Tables[0].Rows[i]["name"].ToString();
            //    loginname[i] = ds.Tables[0].Rows[i]["loginname"].ToString();
            //    password[i] = ds.Tables[0].Rows[i]["password"].ToString();
            //    restId[i] = ds.Tables[0].Rows[i]["restaurantid"].ToString();
            //    dictRest.Remove(restId[i]);
            //    //rest[i]=dictRest[restId[i]];
            //}
            //conn.Close();
        }

        protected void AddSeller()
        {
            string loginname = Request["loginname"];
            bool isExist = Tool.IsExist(loginname, "loginname", "Seller", " and IsDeleted='false'");
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }
            string password = Request["password"];
            string restId = Request["restId"];
            string sqlInsert = "insert Seller (name,loginname,password,restaurantid,recipeids,balance,isDeleted) values ('暂留字段','" +loginname + "','" + password + "'," + restId + ",' ',0,'False')";
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlInsert, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void UpdateSeller()
        {
            string id = Request["sellerId"];
            string sqlUpdate = "";
            string password = Request["password"];
            //string restId = Request["restId"];
            if (Request["loginname"] != null)
            {
                string account = Request["loginname"];
                bool isExist = Tool.IsExist(account, "loginname", "Seller", " and IsDeleted='false'");
                if (isExist)
                {
                    Response.Write("exist");
                    Response.End();
                    return;
                }
                sqlUpdate = "update Seller set loginname = '" + account + "',password = '" + password + "' where id = " + id;
            }
            else {
                sqlUpdate = "update Seller set password = '" + password + "' where id = " + id;
            }
            
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlUpdate, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void DeleteSeller(string id)
        {
            //string id = Request["id"];
            string sqlDelete = "update Seller set isDeleted = 'True' where id = " + id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlDelete, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void GetRest() {
            string str = "select r.id,r.name from Restaurant as r,Seller as s where r.id not in ( select r.id from Restaurant as r,Seller as s where  r.id = s.restaurantid and s.isDeleted='False' group by r.id )group by r.id,r.name order by id";
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(str, conn);
            da.Fill(ds);
            int count = ds.Tables[0].Rows.Count;
            string r1 = "";
            string r2 = "";
            for (int i = 0; i < count; i++) {
                if (i != 0)
                    r1 += ",";
                r1 += ds.Tables[0].Rows[i]["id"].ToString();
                if (i != 0)
                    r2 += ",";
                r2 += ds.Tables[0].Rows[i]["name"].ToString();
            }
            Response.Write(r1 + "|" + r2);
            Response.End();
        }
    }

}