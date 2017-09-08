using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class balance : System.Web.UI.Page
    {
        protected string jsonStr = "";
        protected string[] name = { };
        protected string[] balance1 = { };

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
                    GetBalance();
                    ResJsonStr();
                    break;
            }
        }

        protected void GetBalance()
        {
            int pageIndex = 1;
            int pageSize = 3 * 14;
            if (!string.IsNullOrEmpty(Request["pageIndex"]))
                pageIndex = Convert.ToInt32(Request["pageIndex"]);
            string sqlSelect = "select a.id ,b.name,a.balance from Seller as a,Restaurant as b where a.restaurantid = b.id";
            int pages = 0;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            if (!string.IsNullOrEmpty(Request["search"]))
            {
                sqlSelect += " and b.name like '%" + Request["search"] + "%'";
            }
            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id )  order by id";
            DataSet ds = new DataSet();
            ds = Tool.ExecuteGetDs(sqlSelect);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();
            ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);
            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

    }
}