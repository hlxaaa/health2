using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace WebApplication1.Webs
{
    public partial class Recipe : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected string[] name = { };
        protected string[] id = { };
        protected string[] images = { };
    
        protected DataSet ds = new DataSet();
        protected int ran = new Random().Next();
        protected string jsonStr = "";
        //protected JObject jo = new JObject();

        protected Dictionary<string, string> dictRest = new Dictionary<string, string>();
        protected Dictionary<string, string> dictFood = new Dictionary<string, string>();
        protected Dictionary<string, string> dictFoodType = new Dictionary<string, string>();
        protected Dictionary<string, string> dictTags = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }      
            dictRest = Tool.GetDict("Restaurant", "id", "name", conn);
            dictFood = Tool.GetDict("Food", "id", "name", conn);
            dictFoodType = Tool.GetDict("FoodType", "id", "name", conn);
            dictTags = Tool.GetDict("Tag", "id", "name", conn);
            //GetRecipe();

            switch (Request["method"])
            {
                case "search":
                    GetRecipe();
                    ResJsonStr();
                    break;
                case"deleteRecipe":
                    DeleteRecipe(Request["id"]);
                    WebApplication1.Webs.RecipeContent.UpdateRecipeCache();
                    break;
                case "batchDelete":
                    BatchDelete();
                    WebApplication1.Webs.RecipeContent.UpdateRecipeCache();
                    break;
                default:
                    GetRecipe();
                    break;
            }
        }

        protected void BatchDelete() {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (var id in ids)
            {
                DeleteRecipe(id);
            }
        }

        protected void DeleteRecipe(string id) {
            conn.Open();
            //string id = Request["id"];
            string getImg = "select images from Recipe where id = "+id;
            SqlCommand sqlCom = new SqlCommand(getImg, conn);
            string img = (string)sqlCom.ExecuteScalar();
            string[] imgs = img.Split('|');
            string path = Server.MapPath("").Replace("\\", "/")+"/";
            path = path.Substring(0, path.Length - 5);
            foreach (var i in imgs)
            {
                if (File.Exists(path+i))
                    File.Delete(path+ i);
            }

            string delRecipe = "update Recipe set isDeleted = 'True' where id = "+id;
            SqlCommand sc2 = new SqlCommand(delRecipe, conn);
            sc2.ExecuteScalar();
            conn.Close();
        }

        protected void GetRecipe()
        {
            int pageIndex = 1;
            int pageSize = 10;
            if (Request["pageSize"] != null)
                pageSize = Convert.ToInt32(Request["pageSize"]);
            string index = Request["thePage"];
            if (index != null)
                pageIndex = Convert.ToInt32(index);
            string size = Request["size"];
            if (size != null)
                pageSize = Convert.ToInt32(size);
            
            string sqlSelect = "select * from Recipe where isDeleted='False' ";

            //搜索条件
            int pages = 0;
            int thePage = 1;
            if (Request["thePage"] != null)
                thePage = Convert.ToInt32(Request["thePage"]);

            string search = " ";
            if (Request["search"] != null&&Request["search"].Trim()!="")
            {
                search = Request["search"].Trim() ;
                sqlSelect += " and id in (select id from Recipe where  ";
                string name = search;
                sqlSelect += " name like '%"+name+"%'";

                foreach (var v in dictFood.Values) {
                    if (v.IndexOf(search) > -1) {
                        string id = Tool.GetKey(dictFood, v);
                        sqlSelect += " or id in (select r.id from Recipe as r,Recipe_foods as r2 where r2.foodId = "+id+" and r2.recipeId=r.id)";
                    }
                }

                foreach (var v in dictTags.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictTags, v);
                        sqlSelect += " or id in (select r.id from Recipe as r,tag_relation as r2 where r2.tagId = " + id + " and r2.relationId=r.id and typename='recipe')";
                    }
                }

                foreach (var v in dictRest.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictRest, v);
                        sqlSelect += " or id in (select r.id from Recipe as r,Restaurant as r2 where r2.id = " + id + " and r2.id=r.restaurantId and r2.isDeleted='False')";
                    }
                }

                sqlSelect += ")";
            }

            int sale1 = 0;
            int sale2 = 9999999;
            if (Request.Form.GetValues("saleRange[]") != null)
            {
                if (Request.Form.GetValues("saleRange[]")[0] != "")
                    sale1 = Convert.ToInt32(Request.Form.GetValues("saleRange[]")[0]);
                if (Request.Form.GetValues("saleRange[]")[1] != "")
                    sale2 = Convert.ToInt32(Request.Form.GetValues("saleRange[]")[1]);
                sqlSelect += " and sales between "+sale1+" and "+sale2+"";
            }

            int price1 = 0;
            int price2 = 999999;
            if (Request.Form.GetValues("priceRange[]") != null)
            {
                if (Request.Form.GetValues("priceRange[]")[0] != "")
                    price1 = Convert.ToInt32(Request.Form.GetValues("priceRange[]")[0]);
                if (Request.Form.GetValues("priceRange[]")[1] != "")
                    price2 = Convert.ToInt32(Request.Form.GetValues("priceRange[]")[1]);
                sqlSelect += " and price between " + price1 + " and " + price2 + "";
            }

            string startTime = "2017-05-03";
            string endTime = "2099-12-31";
            if (Request.Form.GetValues("timeRange[]") != null) {
                if (Request.Form.GetValues("timeRange[]")[0] != "")
                    startTime = Request.Form.GetValues("timeRange[]")[0];
                if (Request.Form.GetValues("timeRange[]")[1] != "")
                    endTime = Request.Form.GetValues("timeRange[]")[1];
                sqlSelect += " and createTime between '" + startTime + "' and '" + endTime + "'";
            }

            //string available = "";
            if (Request["available"] != null) {
                string available = Request["available"];
                sqlSelect += " and available = '"+available+"'";
            }

            string[] tags = Request.Form.GetValues("tags[]");
            if (tags != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    string id = Tool.GetKey(dictTags, tags[i]);
                    sqlSelect += " and  id in (select r.id from Recipe as r,tag_relation as r2 where r2.tagId=" + id + " and r2.relationId=r.id and typename='recipe')";
                }
            }

            string[] foods = Request.Form.GetValues("foods[]");
            if (foods != null) {
                for (int i = 0; i < foods.Length; i++) {
                    string id = Tool.GetKey(dictFood,foods[i]);
                    sqlSelect += " and  id in (select r.id from Recipe as r,Recipe_foods as r2 where r2.foodId="+id+" and r2.recipeId=r.id)";
                }
            }
            //


            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top "+pageSize+" * from (" + sqlSelect + ") r where id not in (select top "+x+" id from (" + sqlSelect + ") r order by id desc) order by id desc";
            
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, conn);
            da.Fill(ds);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();

            SqlCommand sqlCom = new SqlCommand(sqlPaging, conn);
            string result = Convert.ToString(sqlCom.ExecuteScalar());
            if (result != null)
            {
                SqlDataAdapter myda = new SqlDataAdapter(sqlPaging, conn);
                myda.Fill(ds);

                ds = Tool.DsToString(ds);
                int count = ds.Tables[0].Rows.Count;
                id = new string[count];
                images = new string[count];

                for (int i = 0; i < count; i++) {
                    id[i] = ds.Tables[0].Rows[i]["id"].ToString();
                    ds.Tables[0].Rows[i]["available"] = (ds.Tables[0].Rows[i]["available"].ToString() == "True") ? "有" : "没有";
                    ds.Tables[0].Rows[i]["foods"] = GetFoods(id[i], conn);
                    ds.Tables[0].Rows[i]["restaurantId"] = dictRest.ContainsKey(ds.Tables[0].Rows[i]["restaurantId"].ToString()) ? dictRest[ds.Tables[0].Rows[i]["restaurantId"].ToString()] : "该餐厅已被删除";
                    ds.Tables[0].Rows[i]["tags"] = GetTags(id[i], dictTags);
                    images[i] = ds.Tables[0].Rows[i]["images"].ToString();
                }

            }
            conn.Close();
            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":"+thePage+"";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr+"}";
            //jo = JObject.Parse(jsonStr);
        }

        protected string GetTags(string id,Dictionary<string, string> dict)
        {
            string[] tags = { };
            string result = "";
            string str = "select tagId from Tag_Relation as t1,Tag as t2 where relationId = "+id+" and typename = 'recipe' and isDeleted = 'False' group by tagId";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(str, conn);
            da.Fill(ds);
            int count = ds.Tables[0].Rows.Count;
            tags = new string[count];
            for (int i = 0; i < count; i++)
            {
                if (i != 0)
                    result += " ";
                string tagId = ds.Tables[0].Rows[i]["tagId"].ToString();
                if (dict.ContainsKey(tagId))
                {
                    tags[i] = dict[ds.Tables[0].Rows[i]["tagId"].ToString()];

                    result += tags[i];
                }
                else
                {
                    //result += "标签被删除";
                }
            }
            return result;
        }

        protected void ResJsonStr()
        {
            Response.Write(jsonStr);
            Response.End();
        }

        protected string GetFoodName(string food,Dictionary<string,string> dict) {
            string result = "";
            string[] foodAndWeights = food.Split('|');
            for (int i = 0; i < foodAndWeights.Length; i++)
            {
                if (i != 0)
                    result += "|";
                string[] fAndW = foodAndWeights[i].Split(';');
                for (int j = 0; j < fAndW.Length; j++) {
                    string foodId = fAndW[j].Split(',')[0];
                    string name = dict[foodId];
                    if (j != 0)
                        result += " ";
                    result+= name + fAndW[j].Substring(foodId.Length);
                }
            }
            return result;
        }

        protected string GetFoods(string id,SqlConnection conn) {
            string[] food = {};
            string[] weight = { };
            string result = "";
            string str = "select r.id,r.recipeId,r.foodtypeId,r.foodId,r.weight from Recipe_foods as r ,Food as f where recipeId ="+id+" and r.foodId = f.id and f.isDeleted='False'";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(str, conn);
            da.Fill(ds);
            int count = ds.Tables[0].Rows.Count;
            food = new string[count];
            weight = new string[count];
            for (int i = 0; i < count; i++)
            {
                if (i != 0)
                    result += " ";
                string foodId = ds.Tables[0].Rows[i]["foodId"].ToString();
                if (dictFood.ContainsKey(foodId))
                {
                    weight[i] = ds.Tables[0].Rows[i]["weight"].ToString();
                    food[i] = dictFood[foodId];
                    result += food[i] + "," + weight[i] + "g";
                }
                else {
                    //result += "该菜品已被删除";
                     //result += "该菜品已被删除";
                }
                
            }
            return result;
        }
    }
}