using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class Withdraw : System.Web.UI.Page
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
                    GetWithdraw();
                    Response.Write(jsonStr);
                    Response.End();
                    break;
                case "doWithdraw":
                    DoWithdraw();
                    break;
            }
        }

        protected void DoWithdraw()
        {//可能需要整体事务
            string withdrawId = Request["withdrawId"];
            string str1 = "update Withdraw set applyState='True' where id=" + withdrawId;
            Tool.ExecuteNon(str1);
            string str2 = "select applyMoney from Withdraw where id = " + withdrawId;
            double withdraw = Convert.ToDouble(Tool.ExecuteScalar(str2));
            string str3 = "select sellerid from Withdraw where id =" + withdrawId;
            var sellerId = Tool.ExecuteScalar(str3);
            string str4 = "update Seller set balance =(balance-" + withdraw + ") where id = " + sellerId;
            Tool.ExecuteNon(str4);
        }

        protected void GetWithdraw()
        {
            int pageIndex = 1;
            int pageSize = 13;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            string sqlSelect = "select w.id,r.name,s.balance,w.applyMoney,w.applyTime,w.applyState from Withdraw as w,Restaurant as r,Seller  as s where  s.restaurantid=r.id and w.sellerId=s.id";

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by applyState,id desc) order by applyState,id desc";
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