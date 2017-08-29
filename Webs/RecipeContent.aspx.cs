using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common.Helper;
using System.IO;

namespace WebApplication1.Webs
{
    public partial class RecipeContent : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected DataSet ds = new DataSet();
        protected int ran = new Random().Next();
        //protected string jsonStr = "";
        protected string id = "";
        protected string name = "";
        protected string available = "";
        protected string rest = "";
        protected string price = "";
        protected string sales = "";
        protected string[] tags = { };
        protected string[] foodtypes = { };
        protected string[] foods = { };
        protected string[] weights = { };
        protected string[] imgs = { };
        protected string oImgs = "";

        protected Dictionary<string, string> dictTag = new Dictionary<string, string>();
        protected Dictionary<string, string> dictType = new Dictionary<string, string>();
        protected Dictionary<string, string> dictFood = new Dictionary<string, string>();
        protected Dictionary<string, string> dictRest = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }
            if (Request.Files != null && Request.Files.Count > 0)
                UpImg();
            else
            {
                switch (Request["method"])
                {
                    default:
                        dictRest = Tool.GetDict("Restaurant", "id", "name", conn);
                        dictFood = Tool.GetDict("Food", "id", "name", conn);
                        dictType = Tool.GetDict("FoodType", "id", "name", conn);
                        dictTag = Tool.GetDict("Tag", "id", "name", conn);
                        id = Request["id"];
                        //id = "1";
                        if (id != null)
                            GetContentById(id);
                        break;
                    case "addRecipe":
                        dictTag = Tool.GetDict("Tag", "id", "name", conn);
                        AddRecipe();
                        UpdateRecipeCache();
                        break;
                    case "updateRecipe":
                        dictTag = Tool.GetDict("Tag", "id", "name", conn);
                        UpdateRecipe();
                        UpdateRecipeCache();
                        break;
                }

            }

        }

        protected void GetContentById(string id)
        {
            string sql = "select  * from Recipe  where id=" + id;
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(ds);
            name = ds.Tables[0].Rows[0]["name"].ToString();
            available = ds.Tables[0].Rows[0]["available"].ToString();
            rest = dictRest[ds.Tables[0].Rows[0]["restaurantId"].ToString()];
            sales = ds.Tables[0].Rows[0]["sales"].ToString();
            price = ds.Tables[0].Rows[0]["price"].ToString();
            imgs = ds.Tables[0].Rows[0]["images"].ToString().Split('|');
            oImgs = ds.Tables[0].Rows[0]["images"].ToString();//Replace("//", "/")


            string selectTag = "select tagId  from Tag_Relation as tr ,Tag as t where typename='recipe' and relationId = " + id + " and t.id=tr.tagId and t.isDeleted='False'";
            SqlDataAdapter da2 = new SqlDataAdapter(selectTag, conn);
            ds.Clear();
            da2.Fill(ds);
            int count = ds.Tables[0].Rows.Count;
            tags = new string[count];
            for (int i = 0; i < count; i++)
            {
                tags[i] = dictTag[ds.Tables[0].Rows[i]["tagId"].ToString()];
            }

            string selectFood = "select foodtypeId,foodId, weight from Recipe_foods as rf ,FoodType as ft,Food as f where recipeId = " + id + " and rf.foodtypeId = ft.id and f.id=foodId and f.IsDeleted='False'";
            SqlDataAdapter da3 = new SqlDataAdapter(selectFood, conn);
            ds.Clear();
            da3.Fill(ds);
            count = ds.Tables[0].Rows.Count;
            foodtypes = new string[count];
            foods = new string[count];
            weights = new string[count];
            for (int i = 0; i < count; i++)
            {
                foodtypes[i] = dictType[ds.Tables[0].Rows[i]["foodtypeId"].ToString()];
                foods[i] = dictFood[ds.Tables[0].Rows[i]["foodId"].ToString()];
                weights[i] = ds.Tables[0].Rows[i]["weight"].ToString();
            }
            conn.Close();
        }

        protected void UpImg()
        {
            HttpFileCollection f = Request.Files;
            string res = "";
            for (int i = 0; i < f.Count; i++)
            {
                string fname = f[i].FileName;

                int j = f[i].FileName.LastIndexOf(".");
                if (f[i].FileName == "")
                    return;
                string newext = f[i].FileName.Substring(j);

                DateTime now = DateTime.Now;
                //string newname = now.DayOfYear.ToString() + f.ContentLength.ToString();
                var a = f[i].ContentLength.ToString();
                string newname = now.ToString("yyyyMMddHHmmss") + i;
                /* startIndex */
                int index = fname.LastIndexOf("\\") + 1;
                /* length */
                int len = fname.Length - index;
                fname = fname.Substring(index, len);
                /* save to server */
                string nameCut = "img//recipe//recipe" + Request["id"] + "-" + newname + newext;
                string newImg = "img//recipe//temp//recipe" + Request["id"] + "-" + newname + newext;

                string sourceImg = this.Server.MapPath("..//" + newImg);
                string cutImg = this.Server.MapPath("..//" + nameCut);
                f[i].SaveAs(this.Server.MapPath("..//" + newImg));

                Tool.getThumImage(sourceImg, 18, 1, cutImg);
                //File.Create(sourceImg).Close(); 
                if (i != 0)
                    res += "|";
                res += nameCut;
                File.Delete(sourceImg);
            }
            Response.Write(res);
            Response.End();
            //Response.Redirect("Restaurant.aspx");
            //HttpPostedFileBase pathfile = Request.Files["filepath"];

        }

        protected void AddRecipe()
        {
            string path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);

            int imgIndex = Convert.ToInt32(Request["imgIndex"]);
            string name = Request["name"];
            string available = Request["available"];
            string restId = Request["restId"];
            string price = Request["price"];
            string sales = Request["sales"];
            string[] tags = Request.Form.GetValues("tags[]");
            string[] tagIds = new string[tags.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                tagIds[i] = Tool.GetKey(dictTag, tags[i]);
            }
            string[] typeIds = Request.Form.GetValues("typeIds[]");
            string[] foodIds = Request.Form.GetValues("foodIds[]");
            string[] weights = Request.Form.GetValues("weights[]");
            string[] imgs = Request.Form.GetValues("imgs[]");

            string img = "";
            img += imgs[imgIndex].Substring(1);
            for (int i = 0; i < imgs.Length; i++)
            {
                if (imgIndex != i)
                    img += "|" + imgs[i].Substring(1);
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string str = "insert Recipe (name,available,restaurantId,sales,price,createTime,images,foodtypes,foods,tags) values (\'" + name + "\',\'" + available + "\',\'" + restId + "\',\'" + sales + "\',\'" + price + "\','" + time + "','" + img + "','此字段不使用','此字段不使用','此字段不使用') Select @@IDENTITY";

            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i] = path + "\\" + imgs[i].Substring(1).Replace("//", "\\");
            }
            string[] oImgs = Request["oImgs"].Split('|');
            for (int i = 0; i < oImgs.Length; i++)
            {
                oImgs[i] = path + "\\" + oImgs[i].Replace("//", "\\");
            }
            //return;
            Tool.ImgUpdate(oImgs, imgs);

            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            var id = sqlCom.ExecuteScalar();

            string insertTag = "insert Tag_Relation values ";
            for (int i = 0; i < tagIds.Length; i++)
            {
                if (i != 0)
                    insertTag += ",";
                insertTag += "(" + id + "," + tagIds[i] + ",'recipe')";
            }
            SqlCommand sqlCom2 = new SqlCommand(insertTag, conn);
            sqlCom2.ExecuteScalar();

            string insertFood = "insert Recipe_foods values ";
            for (int i = 0; i < typeIds.Length; i++)
            {
                if (i != 0)
                    insertFood += ",";
                insertFood += "(" + id + "," + typeIds[i] + "," + foodIds[i] + "," + weights[i] + ")";
            }

            SqlCommand sqlCom3 = new SqlCommand(insertFood, conn);
            sqlCom3.ExecuteScalar();
            conn.Close();

            //Tool.UpdateCache<DbOpertion.Models.Recipe>("Recipe", "List_Recipe");
            //var MyCache = MemCacheHelper.GetMyConfigInstance();
            //var list = MyCache.GetModel<List<DbOpertion.Models.Recipe>>("List_Recipe");
        }

        protected void UpdateRecipe()
        {
            string path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);

            int imgIndex = Convert.ToInt32(Request["imgIndex"]);
            string id = Request["id"];
            string name = Request["name"];
            string available = Request["available"];
            string restId = Request["restId"];
            string price = Request["price"];
            string sales = Request["sales"];
            string[] tags = Request.Form.GetValues("tags[]");
            string[] tagIds = new string[tags.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                tagIds[i] = Tool.GetKey(dictTag, tags[i]);
            }
            string[] typeIds = Request.Form.GetValues("typeIds[]");
            string[] foodIds = Request.Form.GetValues("foodIds[]");
            string[] weights = Request.Form.GetValues("weights[]");
            string[] imgs = Request.Form.GetValues("imgs[]");
            string img = "";
            img += imgs[imgIndex].Substring(1);
            for (int i = 0; i < imgs.Length; i++)
            {
                if (imgIndex != i)
                    img += "|" + imgs[i].Substring(1);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string str = "update Recipe set name='" + name + "',available='" + available + "',restaurantId=" + restId + ",sales=" + sales + ",price='" + price + "',createTime='" + time + "',images='" + img + "' where id =" + id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            sqlCom.ExecuteScalar();

            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i] = path + "\\" + imgs[i].Substring(1).Replace("//", "\\");
            }
            string[] oImgs = Request["oImgs"].Split('|');
            for (int i = 0; i < oImgs.Length; i++)
            {
                oImgs[i] = path + "\\" + oImgs[i].Replace("//", "\\");
            }
            Tool.ImgUpdate(oImgs, imgs);


            string deleteTag = "delete from Tag_Relation where typename='recipe' and relationId=" + id;
            SqlCommand sqlCom4 = new SqlCommand(deleteTag, conn);
            sqlCom4.ExecuteScalar();

            string insertTag = "insert Tag_Relation values ";
            for (int i = 0; i < tagIds.Length; i++)
            {
                if (i != 0)
                    insertTag += ",";
                insertTag += "(" + id + "," + tagIds[i] + ",'recipe')";
            }
            SqlCommand sqlCom2 = new SqlCommand(insertTag, conn);
            sqlCom2.ExecuteScalar();

            string deleteFood = "delete from Recipe_foods where recipeId=" + id;
            SqlCommand sqlCom5 = new SqlCommand(deleteFood, conn);
            sqlCom5.ExecuteScalar();

            string insertFood = "insert Recipe_foods values ";
            for (int i = 0; i < typeIds.Length; i++)
            {
                if (i != 0)
                    insertFood += ",";
                insertFood += "(" + id + "," + typeIds[i] + "," + foodIds[i] + "," + weights[i] + ")";
            }
            SqlCommand sqlCom3 = new SqlCommand(insertFood, conn);
            sqlCom3.ExecuteScalar();

            conn.Close();
        }

        public static void UpdateRecipeCache()
        {
            Tool.UpdateCache<DbOpertion.Models.Recipe>("Recipe", "List_Recipe", true);
            Tool.UpdateCache<DbOpertion.Models.Tag_Relation>("Tag_Relation", "List_Tag_Relation", false);
            Tool.UpdateCache<DbOpertion.Models.Recipe_foods>("Recipe_foods", "List_Recipe_Foods", false);
            var MyCache = MemCacheHelper.GetMyConfigInstance();
            var list = MyCache.GetModel<List<DbOpertion.Models.Tag_Relation>>("List_Tag_Relation");
            //var list2 = MyCache.GetModel<List<DbOpertion.Models.Recipe_foods>>("List_Recipe_Foods");
        }
    }
}