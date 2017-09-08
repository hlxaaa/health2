using Newtonsoft.Json.Linq;
using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class Tag : System.Web.UI.Page
    {
        protected string jsonStr = "";
        protected JObject jo = new JObject();

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
                case "deleteTag":
                    DeleteTag();
                    UpdateTagCache();
                    break;
                case "addTag":
                    AddTag();
                    UpdateTagCache();
                    break;
                case "updateTag":
                    UpdateTag();
                    UpdateTagCache();
                    break;
                case "batchDelete":
                    BatchDelete();
                    UpdateTagCache();
                    break;
                default:
                    GetTag();
                    break;
            }
        }

        protected void GetTag()
        {
            DataSet ds = new DataSet();
            string sqlSelect = "select *from Tag where isDeleted ='False' order by id desc";
            ds = Tool.ExecuteGetDs(sqlSelect);
            jsonStr = Tool.GetJsonByDataset(ds);
            jo = JObject.Parse(jsonStr);
        }

        protected void AddTag()
        {
            string name = Request["name"];
            string condition = " and isDeleted = 'False'";
            bool isExist = Tool.IsExist(name, "name", "Tag", condition);
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string[] constitutions = Request.Form.GetValues("constitutions[]");
            string consTemp = "";
            for (int i = 0; i < constitutions.Length; i++)
            {
                consTemp += "," + constitutions[i];
            }
            string sqlInsert = "insert Tag (name,pinghescore,qiyuscore,yinxuscore,tanshiscore,yangxuscore,tebingscore,shirescore,qixuscore,xueyuscore,isDeleted) values ('" + name + "'" + consTemp + ",'False')";
            Tool.ExecuteNon(sqlInsert);
        }

        protected void UpdateTag()
        {
            string id = Request["id"];
            string name = Request["name"];
            string condition = " and isDeleted = 'False' and id!=" + id;
            bool isExist = Tool.IsExist(name, "name", "Tag", condition);
            if (isExist)
            {
                Response.Write("exist");
                Response.End();
                return;
            }

            string[] constitutions = Request.Form.GetValues("constitutions[]");

            string sqlUpdate = "update Tag set name = '" + name + "',pinghescore = '" + constitutions[0] + "',qiyuscore = '" + constitutions[1] + "',yinxuscore = '" + constitutions[2] + "',tanshiscore = '" + constitutions[3] + "',yangxuscore = '" + constitutions[4] + "',tebingscore = '" + constitutions[5] + "',shirescore = '" + constitutions[6] + "',qixuscore = '" + constitutions[7] + "',xueyuscore = '" + constitutions[8] + "' where id = " + id;
            Tool.ExecuteNon(sqlUpdate);
        }

        protected void DeleteTag()
        {
            string id = Request["id"];
            string sqlDelete = "update Tag set isDeleted = 'True' where id=" + id;
            Tool.ExecuteNon(sqlDelete);
        }

        protected void BatchDelete()
        {
            string ids = Request["ids[]"];
            string sqlBatchDelete = "update Tag set isDeleted = 'True' where id in (" + ids + ")";
            Tool.ExecuteNon(sqlBatchDelete);
        }

        protected void UpdateTagCache()
        {
            Tool.UpdateCache<DbOpertion.Models.Tag>("Tag", "List_Tag", true);
        }
    }
}