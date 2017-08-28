using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Webs
{
    public partial class Tag : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);

        protected int ran = new Random().Next();
        protected string jsonStr = "";
        protected DataSet ds = new DataSet();
        protected JObject jo = new JObject();

        protected void Page_Load(object sender, EventArgs e)
        {
            //GetTag();
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
                case"updateTag":
                    UpdateTag();
                    UpdateTagCache();
                    break;
                case"batchDelete":
                    BatchDelete();
                    UpdateTagCache();
                    break;
            }
            GetTag();
        }

        protected void GetTag() {
            string sqlSelect = "select *from Tag where isDeleted ='False' order by id desc";
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, conn);
            da.Fill(ds);
            jsonStr = Tool.GetJsonByDataset(ds);
            jo = JObject.Parse(jsonStr);
            //var a = 1;
        }

        protected void AddTag() {
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
            string consTemp="";
            for(int i=0;i<constitutions.Length;i++){
                //if (i != 0)
                //    consTemp += ;
                consTemp +=","+ constitutions[i];
            }
            string sqlInsert = "insert Tag (name,pinghescore,qiyuscore,yinxuscore,tanshiscore,yangxuscore,tebingscore,shirescore,qixuscore,xueyuscore,isDeleted) values ('"+name+"'"+consTemp+",'False')";
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlInsert, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void UpdateTag() {
            string id = Request["id"];
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

            string sqlUpdate = "update Tag set name = '" + name + "',pinghescore = '" + constitutions[0] + "',qiyuscore = '" + constitutions[1] + "',yinxuscore = '" + constitutions[2] + "',tanshiscore = '" + constitutions[3] + "',yangxuscore = '" + constitutions[4] + "',tebingscore = '" + constitutions[5] + "',shirescore = '" + constitutions[6] + "',qixuscore = '" + constitutions[7] + "',xueyuscore = '" + constitutions[8] + "' where id = " + id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlUpdate, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void DeleteTag() {
            string id = Request["id"];
            string sqlDelete = "update Tag set isDeleted = 'True' where id="+id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlDelete, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void BatchDelete() {
            string ids = Request["ids[]"];
            string sqlBatchDelete = "update Tag set isDeleted = 'True' where id in ("+ids+")";
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlBatchDelete, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }
        //protected void ResJsonStr() {
        //    Response.Write(jsonStr);
        //    Response.End();
        //}

        protected void UpdateTagCache() {
            Tool.UpdateCache<DbOpertion.Models.Tag>("Tag", "List_Tag", true);
        }
    }
}