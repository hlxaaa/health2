﻿using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class QuestionPro : System.Web.UI.Page
    {
        protected string[] id = { };
        protected string[] constitution = { };
        protected string[] content = { };
        protected string[] sex = { };
        protected string[] constitutions = { "平和质", "气郁质", "阴虚质", "痰湿质", "阳虚质", "特禀质", "湿热质", "气虚质", "血瘀质" };
        protected string[] sexs = { "通用", "限男性", "限女性" };

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
                case "deleteQues":
                    DeleteQues(Request["id"]);
                    break;
                case "updateQues":
                    UpdateQues();
                    break;
                case "batchDelete":
                    BatchDelete();
                    break;
                default:
                    GetQues();
                    break;
            }

        }

        protected void GetQues()
        {
            string strSelect = "select id,Constitution,Content,sex from Questionnaire where category = 'profession' and QuesOrOp = 'question' order by Constitution";
            DataSet ds = Tool.ExecuteGetDs(strSelect);
            int count = ds.Tables[0].Rows.Count;
            id = new string[count];
            constitution = new string[count];
            content = new string[count];
            sex = new string[count];
            for (int i = 0; i < count; i++)
            {
                id[i] = ds.Tables[0].Rows[i]["id"].ToString();
                constitution[i] = ds.Tables[0].Rows[i]["constitution"].ToString();
                content[i] = ds.Tables[0].Rows[i]["content"].ToString();
                string sexTemp = ds.Tables[0].Rows[i]["sex"].ToString();
                if (sexTemp == "male")
                    sex[i] = "限男性";
                else if (sexTemp == "female")
                    sex[i] = "限女性";
                else
                    sex[i] = "通用";
            }
        }

        protected void DeleteQues(string id)
        {
            string sqlDelete = "delete from Questionnaire where id = " + id;
            Tool.ExecuteNon(sqlDelete);
        }

        protected void BatchDelete()
        {
            string ids = Request["ids[]"];
            string sqlDelete = "delete from Questionnaire where id in (" + ids + ")";
            Tool.ExecuteNon(sqlDelete);
        }

        protected void UpdateQues()
        {
            string[] ids = Request.Form.GetValues("ids[]");
            string[] titles = Request.Form.GetValues("titles[]");
            string[] constitutions = Request.Form.GetValues("constitutions[]");
            string[] sexs = GetSex(Request.Form.GetValues("sexs[]"));

            for (int i = 0; i < ids.Length; i++)
            {

                if (ids[i] == "0")
                {
                    string sqlInsert = "insert Questionnaire (Constitution,QuesOrOp,Content,category,sex) values ('" + constitutions[i] + "','question','" + titles[i] + "','profession','" + sexs[i] + "')";
                    Tool.ExecuteNon(sqlInsert);
                }
                else
                {
                    string sqlUpdate = "update Questionnaire set Constitution = '" + constitutions[i] + "',Content = '" + titles[i] + "',sex = '" + sexs[i] + "' where id = " + ids[i];
                    Tool.ExecuteNon(sqlUpdate);
                }
            }
        }

        protected string[] GetSex(string[] strs)
        {
            string[] r = new string[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i] == "限女性")
                    r[i] = "female";
                else if (strs[i] == "限男性")
                    r[i] = "male";
                else
                    r[i] = "currency";
            }
            return r;
        }
    }
}