using Common.Helper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;


namespace WebApplication1.Webs
{
    public partial class Customer : System.Web.UI.Page
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
                    GetCustomer();
                    ResJsonStr();
                    break;
                case "updateCustomer":
                    string userId = Request["id"];
                    UpdateCustomer(userId);
                    UpdateCustomerCacheById(userId);
                    break;
            }
        }

        protected void UpdateCustomer(string userId)
        {
            string name = Request["name"];
            string phone = Request["phone"];
            string wechat = Request["wechat"];

            string str = "update Customer set phone='" + phone + "'";

            if (!string.IsNullOrEmpty(name))
                str += ",name='" + name + "'";

            if (!string.IsNullOrEmpty(wechat))
            {
                var regexStr = "^[a-zA-Z]{1}[-_a-zA-Z0-9]{5,19}$";
                Regex regex = new Regex(regexStr);
                if (!regex.IsMatch(wechat))
                {
                    Response.Write("wechat-format-error");
                    Response.End();
                    return;
                }
                str += ",wechat='" + wechat + "'";

            }
            string password = Request["password"];
            if (!string.IsNullOrEmpty(password))
            {
                var RegexStr = @"^(?![^A-Za-z]+$)(?![^0-9]+$)[\x21-x7e]{6,12}$";
                Regex regex = new Regex(RegexStr);
                if (!regex.IsMatch(password))
                {
                    Response.Write("password-format-error");
                    Response.End();
                    return;
                }
                str += ",password='" + password + "'";
            }

            string height = Request["height"];
            if (!string.IsNullOrEmpty(height))
                str += ",height='" + height + "'";

            string weight = Request["weight"];
            if (!string.IsNullOrEmpty(weight))
                str += ",weight='" + weight + "'";

            string sex = Request["sex"];
            if (!string.IsNullOrEmpty(sex))
                str += ",sex='" + sex + "'";

            string age = Request["age"];
            if (!string.IsNullOrEmpty(age))
                str += ",birthday='" + age + "'";

            string labour = Request["labour"];
            if (!string.IsNullOrEmpty(labour))
                str += ",labourIntensity='" + labour + "'";

            string constitution = Request["constitution"];
            if (!string.IsNullOrEmpty(constitution))
                str += ",constitution='" + constitution + "'";

            string score = Request["score"];
            if (!string.IsNullOrEmpty(score))
                str += ",UserScore='" + score + "'";

            str += " where id =" + userId;
            Tool.ExecuteNon(str);
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void GetCustomer()
        {
            int pageIndex = 1;
            int pageSize = 16;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);
            string sqlSelect = "select * from Customer where 1=1";
            if (!string.IsNullOrEmpty(Request["search"]))
            {
                string search = Request["search"].Trim();
                sqlSelect += " and name like '%" + search + "%'";
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

            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                var sex = ds.Tables[0].Rows[i]["sex"].ToString();
                ds.Tables[0].Rows[i]["sex"] = sex == "" ? "未填写" : sex == "True" ? "男" : "女";
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
        }

        protected void UpdateCustomerCacheById(string userId)
        {
            string selectAll = "select * from Customer where id = " + userId;

            SqlDataAdapter da = new SqlDataAdapter(selectAll, ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
            DataTable table = new DataTable();
            da.Fill(table);
            var list = table.ConvertToList<DbOpertion.Models.Customer>().FirstOrDefault();
            UpdateCache.Tool.lockCache("User");
            var MyCache = MemCacheHelper.GetMyConfigInstance();
            MyCache.Set("UserInfoModel_" + userId, list);
            UpdateCache.Tool.ReleaseLock("User");
        }
    }
}