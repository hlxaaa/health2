using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common.Helper;


namespace WebApplication1.Webs
{
    public partial class Customer : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);

        //protected int[] id = { };
        //protected string[] name = { };
        //protected string[] phone = { };
        //protected string[] wechat = { };
        //protected string[] password = { };
        //protected string[] height = { };
        //protected string[] weight = { };
        //protected string[] sex = { };
        //protected int[] age = { };
        //protected string[] labour = { };
        //protected string[] constitution = { };
        //protected string[] score = { };

        //protected string[] sTime = { };//分钟
        //protected string[] sDate = { };
        //protected string[] distance = { };//米
        //protected string[] steps = { };

        protected int ran = new Random().Next();
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
            GetCustomer();
            switch (Request["method"])
            {
                case "search":
                    ResJsonStr();
                    break;
                case "updateCustomer":
                    string userId = Request["id"];
                    UpdateCustomer(userId);
                    UpdateCustomerCacheById(userId);
                    break;
            }
        }

        protected void UpdateCustomer(string userId) {
            
            string name = Request["name"];
            string phone = Request["phone"];
            string wechat = Request["wechat"];

            string str = "update Customer set phone='" + phone + "'";

         

            if (!string.IsNullOrEmpty(name))
                str += ",name='" + name + "'";

            if(!string.IsNullOrEmpty(wechat))
                str+=",wechat='" + wechat + "'";

            string password = Request["password"];
            if (!string.IsNullOrEmpty(password))
                str += ",password='"+password+"'";

            string height = Request["height"];
            if (!string.IsNullOrEmpty(height))
                str += ",height='"+height+"'";

            string weight = Request["weight"];
            if (!string.IsNullOrEmpty(weight))
                str += ",weight='" + weight + "'";

            string sex = Request["sex"];
            if (!string.IsNullOrEmpty(sex))
                str+=",sex='"+sex+"'";

            string age = Request["age"];
            if (!string.IsNullOrEmpty(age))
                str += ",birthday='"+age+"'";

            string labour = Request["labour"];
            if (!string.IsNullOrEmpty(labour))
                str += ",labourIntensity='"+labour+"'";

            string constitution = Request["constitution"];
            if (!string.IsNullOrEmpty(constitution))
                str += ",constitution='"+constitution+"'";

            string score = Request["score"];
            if (!string.IsNullOrEmpty(score))
                str += ",UserScore='"+score+"'";

            str += " where id ="+userId;
            //return;
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


        protected void GetCustomer()
        {
            int pageIndex = 1;
            int pageSize = 3;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);
            string sqlSelect = "select * from Customer where 1=1";
            if (!string.IsNullOrEmpty(Request["search"]))
            {
                string search = Request["search"].Trim();
                sqlSelect += " and name like '%" + search + "%'";
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

            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++) {
                var sex = ds.Tables[0].Rows[i]["sex"].ToString();
                ds.Tables[0].Rows[i]["sex"] = sex==""?"未填写":sex == "True" ? "男" : "女";

                try
                {
                    var birthday = Convert.ToDateTime(ds.Tables[0].Rows[i]["birthday"].ToString());
                    int ageTemp = DateTime.Now.Year - birthday.Year;
                    if (birthday > DateTime.Now.AddYears(-ageTemp))
                        ageTemp--;
                    ds.Tables[0].Rows[i]["birthday"] = ageTemp;
                }
                catch
                {
                    ds.Tables[0].Rows[i]["birthday"] = "未填写";
                }

            }

            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
            conn.Close();
        }

        protected void UpdateCustomerCacheById(string userId) {
            conn.Open();
            string selectAll = "select * from Customer where id = " + userId;
            
            SqlDataAdapter da = new SqlDataAdapter(selectAll, conn);
            DataTable table = new DataTable();
            da.Fill(table);
            var list = table.ConvertToList<DbOpertion.Models.Customer>();

            conn.Close();
            var MyCache = MemCacheHelper.GetMyConfigInstance();
            MyCache.Set("UserInfoModel_" + userId, list);
        }
    }
}