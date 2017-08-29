using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Webs
{
    public partial class QuestionEasy : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
       protected DataSet ds = new DataSet();
        protected int ran = new Random().Next();
        protected int[] idQuestion = { };
        protected int[] idOption = { };
        protected string[] question = { };
        protected string[][] option = { };
        protected string[][] constitution = { };
        protected string[] constitutions = { "平和质", "气郁质", "阴虚质", "痰湿质", "阳虚质", "特禀质", "湿热质", "气虚质", "血瘀质" };
        


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
                case "addQues":
                    AddQues();
                    break;
                case "deleteQues":
                    DeleteQues(Request["id"]);
                    break;
                case "updateQues":
                    UpdateQues(Request["id"]);
                    break;
                default:
                    GetQues();
                    break;
            }
        }

        protected void GetQues() { 
            string sqlSelect = "select id,Content from Questionnaire where category = 'express' and QuesOrOp = 'question'";
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlSelect, conn);
            sqlCom.ExecuteScalar();
            string result = Convert.ToString(sqlCom.ExecuteScalar());
            DataSet ds = new DataSet();

            if (result != null)
            {
                SqlDataAdapter myda = new SqlDataAdapter(sqlSelect, conn);
                myda.Fill(ds);
                int count = ds.Tables[0].Rows.Count;
                idQuestion = new int[count];
                question = new string[count];
                option = new string[count][];
                constitution = new string[count][];
                for (int i = 0; i < count; i++)
                {
                    question[i] = ds.Tables[0].Rows[i]["content"].ToString();
                    idQuestion[i] = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                    DataSet ds2 = new DataSet();
                    int relationId = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                    string sqlSelectOption = "select content,constitution from Questionnaire where QuesOrOp = 'option' and RelationId = " + relationId;
                    SqlDataAdapter da2 = new SqlDataAdapter(sqlSelectOption, conn);
                    da2.Fill(ds2);
                    int count2 = ds2.Tables[0].Rows.Count;
                    option[i] = new string[count2];
                    constitution[i] = new string[count2];
                    for (int j = 0; j < count2; j++)
                    {
                        option[i][j] = ds2.Tables[0].Rows[j]["content"].ToString();
                        constitution[i][j] = ds2.Tables[0].Rows[j]["constitution"].ToString();
                    }
                }
                conn.Close();
            }
        }
        

        protected void AddQues() {
            string question = Request["title"];
            var options = Request.Form.GetValues("options[]");
            var constitutions = Request.Form.GetValues("constitutions[]");

            string sqlInsertQues = "insert Questionnaire (QuesOrOp,Content,category,sex) values ('question','" + question + "','express','currency') Select @@IDENTITY";
            conn.Open();
            SqlCommand comInsertQues = new SqlCommand(sqlInsertQues, conn);
            var relationId = comInsertQues.ExecuteScalar();
            string sqlUpdateRelationId = "update Questionnaire set RelationId = '" + relationId + "' where id = " + relationId;
            SqlCommand comUpdateRelationId = new SqlCommand(sqlUpdateRelationId, conn);
            comUpdateRelationId.ExecuteScalar();

            string sqlInsertOptions;
            SqlCommand comInsertOptions;
            for (int i = 0; i < options.Length; i++)
            {
                sqlInsertOptions = "insert Questionnaire (Constitution,QuesOrOp,Content,category,RelationId,sex) values ('" + constitutions[i] + "','option','" + options[i] + "','express'," + relationId + ",'currency') ";
                comInsertOptions = new SqlCommand(sqlInsertOptions, conn);
                comInsertOptions.ExecuteScalar();
            }
            conn.Close();
        }

        protected void UpdateQues(string id) {
            string sqlDelete = "delete from Questionnaire where QuesOrOp = 'option' and RelationId = " + id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlDelete, conn);
            sqlCom.ExecuteScalar();

            string title = Request.Form["title"];
            var options = Request.Form.GetValues("options[]");
            var constitutions = Request.Form.GetValues("constitutions[]");

            string strUpdate = "update Questionnaire set Content = '" + title + "' where id = " + id;
            SqlCommand sqlComUpQues = new SqlCommand(strUpdate, conn);
            sqlComUpQues.ExecuteScalar();

            for (int i = 0; i < options.Length; i++)
            {
                string sqlInsert = "insert Questionnaire (Constitution,QuesOrOp,Content,category,RelationId,sex) values ('" + constitutions[i] + "','option','" + options[i] + "','express'," + id + ",'currency') ";
                SqlCommand sqlComOp = new SqlCommand(sqlInsert, conn);
                sqlComOp.ExecuteScalar();
            }

            conn.Close();
        }

        protected void DeleteQues(string id) {
            string sqlDelete = "delete from Questionnaire where category = 'express' and RelationId = " + id;
            conn.Open();
            SqlCommand sqlComDel = new SqlCommand(sqlDelete, conn);
            sqlComDel.ExecuteScalar();
            conn.Close();
        }
    }
}