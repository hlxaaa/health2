﻿using System;
using System.Data;

namespace WebApplication1.Webs
{
    public partial class CustomerSport : System.Web.UI.Page
    {
        protected int cid = 0;
        protected string[] steps = { };
        protected string[] dates = { };
        protected string jsonStr = "";
        protected string name = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }
            cid = Convert.ToInt32(Request["id"]);

            switch (Request["method"])
            {
                case "getEveryDay":
                    int year = Convert.ToInt32(Request["year"]);
                    int month = Convert.ToInt32(Request["month"]);
                    int id = Convert.ToInt32(Request["cid"]);
                    GetEveryDay(year, month, id);
                    ResJsonStr();
                    break;
                case "getEveryMonth":
                    year = Convert.ToInt32(Request["year"]);
                    id = Convert.ToInt32(Request["cid"]);
                    GetEveryMonth(year, id);
                    ResJsonStr();
                    break;
                case "getEveryYear":
                    id = Convert.ToInt32(Request["cid"]);
                    GetEveryYear(id);
                    break;
                default:
                    GetSport(cid);
                    break;
            }
        }

        protected void GetEveryYear(int cid)
        {
            string str = "select top 1 sDate from Sport where cid =" + cid + " order by sDate ";
            var result = Tool.ExecuteScalar(str);
            string a = "";
            if (result != null)
            {
                DateTime dt = Convert.ToDateTime(result);
                int oldYear = dt.Year;

                string str2 = "select top 1 sDate from Sport where cid =" + cid + " order by sDate desc";
                result = Tool.ExecuteScalar(str2);
                dt = Convert.ToDateTime(result);
                int lastYear = dt.Year;
                a = "{\"Table1\":[";
                for (int i = oldYear; i < lastYear + 1; i++)
                {
                    string str3 = "select SUM(steps) from Sport where cid =" + cid + " and sDate >= '" + i + "' and sDate<'" + (i + 1) + "'";
                    result = Tool.ExecuteScalar(str3);
                    if (i != oldYear)
                        a += ",";
                    if (string.IsNullOrEmpty(result.ToString()))
                        result = "0";
                    a += "{\"steps\":\"" + result + "\",\"sDate\":\"" + i + "\"}";
                }
                a += "]}";
            }
            else
            {
                a = "{\"Table1\":[{\"sDate\":\"" + DateTime.Now.Year + "\",\"steps\":\"0\"}]}";
            }
            Response.Write(a);
            Response.End();
        }

        protected void GetEveryMonth(int year, int cid)
        {
            string str = "";
            string[] monthSteps = new string[12];
            string a = "{\"Table1\":[";
            for (int i = 1; i < 13; i++)
            {
                if (i == 12)
                    str = "select sum(steps) from Sport where cid = " + cid + " and sDate >= '" + year + "-" + i + "-01' and sDate <'" + (year + 1) + "-01-01'";
                else
                    str = "select sum(steps) from Sport where cid = " + cid + " and sDate >= '" + year + "-" + i + "-01' and sDate <'" + year + "-" + (i + 1) + "-01'";
                var steps = Tool.ExecuteScalar(str);
                if (i != 1)
                    a += ",";
                if (string.IsNullOrEmpty(steps.ToString()))
                    steps = "0";
                a += "{\"steps\":\"" + steps + "\",\"sDate\":\"" + i + "\"}";
            }
            string yearJson = ",\"year\":" + year + "";
            a += "]" + yearJson + "}";
            Response.Write(a);
            Response.End();
        }

        protected void GetEveryDay(int year, int month, int cid)
        {
            string str2 = "select steps,sDate from Sport where cid =" + cid + " and sDate >='" + year + "-" + month + "-01' and sDate <'" + year + "-" + (month + 1) + "-01' order by sDate ";
            DataSet ds = Tool.ExecuteGetDs(str2);
            ds = Tool.DsToString(ds);
            int l = ds.Tables[0].Rows.Count;
            if (l == 0)
            {
                jsonStr = "{\"Table1\":[{\"sDate\":\"" + DateTime.Now.Year + "\",\"steps\":\"0\"}]}";
                year = DateTime.Now.Year;
            }
            else
            {
                for (int i = 0; i < l; i++)
                {
                    string temp = ds.Tables[0].Rows[i]["sDate"].ToString();
                    DateTime dt2 = Convert.ToDateTime(temp);
                    ds.Tables[0].Rows[i]["sDate"] = dt2.Day.ToString();
                    string step = ds.Tables[0].Rows[i]["steps"].ToString();
                    if (string.IsNullOrEmpty(step))
                        ds.Tables[0].Rows[i]["steps"] = "0";
                }

                jsonStr = Tool.GetJsonByDataset(ds);
            }
            string yearJson = ",\"year\":" + year + "";
            string monthJson = ",\"month\":" + month + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + yearJson + monthJson + "}";
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected void GetSport(int cid)
        {
            string str = "select top 1 sDate from Sport where cid =" + cid + " order by sDate desc";
            var result = Tool.ExecuteScalar(str);
            DateTime dt = Convert.ToDateTime(result);
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;

            string getName = "select name from Customer  where id=" + cid;
            name = Tool.ExecuteScalar(getName);
            GetEveryDay(year, month, cid);
        }
    }
}