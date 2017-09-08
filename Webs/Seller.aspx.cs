using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class accountSet : System.Web.UI.Page
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
                case "getRest":
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
            }
        }

        protected void BatchDelete()
        {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids)
            {
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
            int pageSize = 14;
            if (!string.IsNullOrEmpty(Request["thePage"]))
                pageIndex = Convert.ToInt32(Request["thePage"]);

            int pages = 0;

            string sqlSelect = "select s.id,r.name,s.loginname,s.password from Seller as s ,Restaurant as r where s.restaurantid=r.id and s.isDeleted='False'";

            if (!string.IsNullOrEmpty(Request["search"]))
                sqlSelect += " and r.name like '%" + Request["search"].Trim() + "%'";

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";

            DataSet ds = Tool.ExecuteGetDs(sqlSelect);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();

            if (allCount > 0)
            {
                if (pageIndex > pages)//最后一页的最后一个被删掉时，处理
                {
                    pageIndex = pages;
                    x = (pageIndex - 1) * pageSize;
                    sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
                }
            }

            ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);
            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + pageIndex + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
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
            string sqlInsert = "insert Seller (name,loginname,password,restaurantid,recipeids,balance,isDeleted) values ('此字段不使用','" + loginname + "','" + password + "'," + restId + ",' ',0,'False')";
            Tool.ExecuteNon(sqlInsert);
        }

        protected void UpdateSeller()
        {
            string id = Request["sellerId"];
            string sqlUpdate = "";
            string password = Request["password"];
            if (Request["loginname"] != null)//如果改用户名，要检测是否存在
            {
                string account = Request["loginname"];
                bool isExist = Tool.IsExist(account, "loginname", "Seller", " and IsDeleted='false' and id!=" + id);
                if (isExist)
                {
                    Response.Write("exist");
                    Response.End();
                    return;
                }
                sqlUpdate = "update Seller set loginname = '" + account + "',password = '" + password + "' where id = " + id;
            }
            else
            {
                sqlUpdate = "update Seller set password = '" + password + "' where id = " + id;
            }
            Tool.ExecuteNon(sqlUpdate);
        }

        protected void DeleteSeller(string id)
        {
            string sqlDelete = "update Seller set isDeleted = 'True' where id = " + id;
            Tool.ExecuteNon(sqlDelete);
        }

        protected void GetRest()
        {
            string str = "select r.id,r.name from Restaurant as r,Seller as s where r.id not in ( select r.id from Restaurant as r,Seller as s where  r.id = s.restaurantid and s.isDeleted='False' group by r.id )group by r.id,r.name order by id";
            DataSet ds = Tool.ExecuteGetDs(str);
            int count = ds.Tables[0].Rows.Count;
            string r1 = "";
            string r2 = "";
            for (int i = 0; i < count; i++)
            {
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