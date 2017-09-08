using System;

namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Server.MapPath("");
            Tool.ClearTempPic(path + "\\img\\article\\temp");
            Tool.ClearTempPic(path + "\\img\\restaurant\\temp");
            Tool.ClearTempPic(path + "\\img\\recipe\\temp");
            Session.Timeout = 30;
            switch (Request["method"])
            {
                case "login":
                    LoginIn();
                    break;
                case "loginOut":
                    LoginOut();
                    break;
            }
        }

        protected void LoginIn()
        {
            string vCode = Request["vCode"];
            if (Session["vCode"].ToString().ToLower() != vCode.ToLower().Trim())
            {
                Response.Write("vCodeError");
                Response.End();
                return;
            }

            string account = Request["account"];
            string selectAccount = "select restaurantid from Seller where loginname ='" + account + "'";
            string isAccount = Tool.ExecuteScalar(selectAccount);
            if (isAccount == null)
            {
                Response.Write("noAccount");
                Response.End();
                return;
            }

            string password = Request["password"];
            string str = "select restaurantid from Seller where loginname = '" + account + "' and password = '" + password + "'";

            string restId = Tool.ExecuteScalar(str);
            if (restId == null)
            {
                Response.Write("passwordError");
                Response.End();

                return;
            }
            Session["restId"] = restId;//管理员为0，普通商家有自己的餐厅ID
        }

        protected void LoginOut()
        {
            Session.Abandon();
            Response.Redirect("/Login.aspx", false);
        }
    }
}