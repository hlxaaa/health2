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
    public partial class BalanceSeller : System.Web.UI.Page
    {
        protected string id = "";
        protected string restId = "";
        protected string jsonDetail = "";
        protected string jsonWithdraw = "";
        protected double balance = 0;
        protected bool isAdmin = false;
        protected string loginName = "";
        protected double availableCash = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null)
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }

            string sellerId = "";
            restId = Session["restId"].ToString();

            if (restId == "0")
            {
                isAdmin = true;
                sellerId = Request["sellerId"];//管理员，sellerId通过参数来获得
                loginName = "管理员";
            }
            else
            {
                loginName = Tool.GetRestNameById(restId);
                sellerId = Tool.GetSellerIdByRestId(restId);//不是管理员的时候，通过session的restId来获取sellerId
            }

            //下一页什么的，会有sellerId传过来。
            id = sellerId;//传到前端
            availableCash = GetAvailableCash(Tool.GetRestIdBySellerId(sellerId), sellerId);
            balance = GetBalance(sellerId);

            switch (Request["method"])
            {
                case "search":
                    GetDetail(sellerId);
                    Response.Write(jsonDetail);
                    Response.End();
                    break;
                case "search2":
                    GetWithdraw(sellerId);
                    Response.Write(jsonWithdraw);
                    Response.End();
                    break;
                case "withdraw":
                    DoWithdraw(sellerId);
                    break;
            }
        }

        protected void DoWithdraw(string sellerId)
        {
            var number = Request["number"];
            if (availableCash < Convert.ToDouble(number))
            {
                Response.Write("no-money");
                Response.End();
                return;
            }
            string insertWithdraw = "insert Withdraw values(" + sellerId + ",'" + number + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','False')";
            Tool.ExecuteNon(insertWithdraw);
        }

        protected double GetBalance(string id)
        {
            string str = "select balance from Seller where id=" + id;
            return Convert.ToDouble(Tool.ExecuteScalar(str));
        }

        protected void GetDetail(string sellerId)
        {
            int pageIndex = 1;
            int pageSize = 10;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            string sqlSelect = "select o.Id, o.CreateTime,o.Pay,r.name as recipe,c.name as customer from Recipe as r,Orders as o ,Customer as c,Restaurant as r2 ,seller as s  where s.id=" + sellerId + " and c.id=o.CustomerId and r.id = o.RecipeId and s.restaurantid = o.SellerId and o.SellerId=r2.id";

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
            DataSet ds = Tool.ExecuteGetDs(sqlSelect);
            int allCount = ds.Tables[0].Rows.Count;
            int pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();
            ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);
            jsonDetail = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonDetail = jsonDetail.Substring(0, jsonDetail.Length - 1) + pagesStr + thePageStr + "}";
        }

        protected void GetWithdraw(string sellerId)
        {
            int pageIndex = 1;
            int pageSize = 10;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            string sqlSelect = "select id,applyTime,applyMoney, applyState from Withdraw where sellerId=" + sellerId;

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by applyState,applyTime desc) order by  applyState,applyTime desc";
            DataSet ds2 = new DataSet();
            ds2 = Tool.ExecuteGetDs(sqlSelect);
            int allCount = ds2.Tables[0].Rows.Count;
            int pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds2.Clear();
            ds2 = Tool.ExecuteGetDs(sqlPaging);
            ds2 = Tool.DsToString(ds2);
            jsonWithdraw = Tool.GetJsonByDataset(ds2);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonWithdraw = jsonWithdraw.Substring(0, jsonWithdraw.Length - 1) + pagesStr + thePageStr + "}";
        }

        /// <summary>
        /// 获取可提取的额度
        /// </summary>
        /// <param name="restId">餐厅id</param>
        /// <param name="sellerId">商家id</param>
        /// <returns>可提取的额度</returns>
        protected double GetAvailableCash(string restId, string sellerId)
        {
            string str2 = "select * from Withdraw where sellerId = " + sellerId + " and applyState='False'";
            double r = 0;
            var result = Tool.ExecuteScalar(str2);
            if (result != null)
            {
                string str = "select (hasmoney.balance-applyingmoney.money2)as availablecash from  (select balance from Seller where restaurantid=" + restId + ") hasmoney ,(select SUM(applyMoney)as money2 from Withdraw where applyState='False' and sellerId=" + sellerId + ") applyingmoney";
                r = Convert.ToDouble(Tool.ExecuteScalar(str));
            }
            else
            {
                string str = "select balance from Seller where id = " + sellerId;
                r = Convert.ToDouble(Tool.ExecuteScalar(str));
            }
            return r;
        }
    }
}