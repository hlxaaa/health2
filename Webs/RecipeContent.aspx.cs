using Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

namespace WebApplication1.Webs
{
    public partial class RecipeContent : System.Web.UI.Page
    {
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
                dictTag = Tool.GetDict("Tag", "id", "name");
                switch (Request["method"])
                {
                    default:
                        dictRest = Tool.GetDict("Restaurant", "id", "name");
                        dictFood = Tool.GetDict("Food", "id", "name");
                        dictType = Tool.GetDict("FoodType", "id", "name");
                        id = Request["id"];
                        if (id != null)
                            GetContentById(id);
                        break;
                    case "addRecipe":
                        AddRecipe();
                        UpdateRecipeCache();
                        break;
                    case "updateRecipe":
                        UpdateRecipe();
                        UpdateRecipeCache();
                        break;
                    case "cutImg":
                        CutImg();
                        break;
                }

            }

        }


        protected void CutImg()
        {
            string path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);
            int x1 = Convert.ToInt32(Request["x1"]);
            int y1 = Convert.ToInt32(Request["y1"]);
            int w = Convert.ToInt32(Request["w"]);
            int h = Convert.ToInt32(Request["h"]);
            string imgName = Request["imgName"];
            string id = Request["id"];
            string sourceSrc = path + "\\img\\recipe\\" + imgName; ;
            if (!File.Exists(sourceSrc))
                sourceSrc = path + "\\img\\recipe\\temp\\" + imgName;
            string targetSrc = path + "\\img\\recipe\\temp\\n" + imgName;

            Tool.CutImage(sourceSrc, x1, y1, w, h, targetSrc);

            string res = "\\img\\recipe\\temp\\n" + imgName;
            Response.Write(res.Replace("\\", "//"));
            Response.End();

        }

        protected void GetContentById(string id)
        {
            string sql = "select  * from Recipe  where id=" + id;
            DataSet ds = Tool.ExecuteGetDs(sql);
            name = ds.Tables[0].Rows[0]["name"].ToString();
            available = ds.Tables[0].Rows[0]["available"].ToString();
            rest = dictRest[ds.Tables[0].Rows[0]["restaurantId"].ToString()];
            sales = ds.Tables[0].Rows[0]["sales"].ToString();
            price = ds.Tables[0].Rows[0]["price"].ToString();
            imgs = ds.Tables[0].Rows[0]["images"].ToString().Split('|');
            oImgs = ds.Tables[0].Rows[0]["images"].ToString();//Replace("//", "/")

            string selectTag = "select tagId  from Tag_Relation as tr ,Tag as t where typename='recipe' and relationId = " + id + " and t.id=tr.tagId and t.isDeleted='False'";
            ds = Tool.ExecuteGetDs(selectTag);
            int count = ds.Tables[0].Rows.Count;
            tags = new string[count];
            for (int i = 0; i < count; i++)
            {
                tags[i] = dictTag[ds.Tables[0].Rows[i]["tagId"].ToString()];
            }

            string selectFood = "select foodtypeId,foodId, weight from Recipe_foods as rf ,FoodType as ft,Food as f where recipeId = " + id + " and rf.foodtypeId = ft.id and f.id=foodId and f.IsDeleted='False' and ft.IsDeleted='false'";
            ds = Tool.ExecuteGetDs(selectFood);
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
                var a = f[i].ContentLength.ToString();
                string newname = now.ToString("yyyyMMddHHmmss") + i;
                /* startIndex */
                int index = fname.LastIndexOf("\\") + 1;
                /* length */
                int len = fname.Length - index;
                fname = fname.Substring(index, len);

                int fileLength = f[i].ContentLength;
                bool isThum = true;
                if (fileLength < 200000)
                {
                    isThum = false;
                }
                /* save to server */
                string nameCut = "img//recipe//temp//recipe" + Request["id"] + "-" + newname + newext;
                string newImg = "img//recipe//temp//recipe" + Request["id"] + "-new" + newname + newext;

                string sourceImg = this.Server.MapPath("..//" + newImg);
                string cutImg = this.Server.MapPath("..//" + nameCut);
                f[i].SaveAs(this.Server.MapPath("..//" + newImg));

                if (isThum)
                    Tool.getThumImage(sourceImg, 18, 1, cutImg);//quality:18
                else
                    File.Copy(sourceImg, cutImg);

                if (i != 0)
                    res += "|";
                res += nameCut;
                File.Delete(sourceImg);
            }
            Response.Write(res);
            Response.End();
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
            string[] imgNames = Request.Form.GetValues("imgNames[]");
            string[] imgs = new string[imgNames.Length];
            for (int i = 0; i < imgNames.Length; i++)
            {
                imgs[i] = "img//recipe//" + imgNames[i];
            }

            string[] oImgNames = Request.Form.GetValues("oImgNames[]");
            string[] oImgs = new string[oImgNames.Length];
            for (int i = 0; i < oImgNames.Length; i++)
            {
                oImgs[i] = "img//recipe//temp//" + oImgNames[i];
            }


            string img = "";
            img += imgs[imgIndex];
            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i] = imgs[i];
                if (imgIndex != i)
                    img += "|" + imgs[i];
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string str = "insert Recipe (name,available,restaurantId,sales,price,createTime,images,foodtypes,foods,tags) values (\'" + name + "\',\'" + available + "\',\'" + restId + "\',\'" + sales + "\',\'" + price + "\','" + time + "','" + img + "','此字段不使用','此字段不使用','此字段不使用') Select @@IDENTITY";


            Tool.CopyImg(imgs, oImgs, path);

            foreach (var i in oImgs)
            {
                File.Delete(path + "\\" + i.Replace("//", "\\"));
            }
            string id = Tool.ExecuteScalar(str);
            string insertTag = "insert Tag_Relation values ";
            for (int i = 0; i < tagIds.Length; i++)
            {
                if (i != 0)
                    insertTag += ",";
                insertTag += "(" + id + "," + tagIds[i] + ",'recipe')";
            }
            Tool.ExecuteNon(insertTag);
            string insertFood = "insert Recipe_foods values ";
            for (int i = 0; i < typeIds.Length; i++)
            {
                if (i != 0)
                    insertFood += ",";
                insertFood += "(" + id + "," + typeIds[i] + "," + foodIds[i] + "," + weights[i] + ")";
            }
            Tool.ExecuteNon(insertFood);
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

            string[] imgNames = Request.Form.GetValues("imgNames[]");
            string[] imgs = new string[imgNames.Length];
            for (int i = 0; i < imgNames.Length; i++)
            {
                imgs[i] = "img//recipe//" + imgNames[i];
            }

            string[] oImgNames = Request.Form.GetValues("oImgNames[]");
            string[] oImgs = new string[oImgNames.Length];
            for (int i = 0; i < oImgNames.Length; i++)
            {
                oImgs[i] = "img//recipe//temp//" + oImgNames[i];
            }

            string img = "";
            img += imgs[imgIndex];
            for (int i = 0; i < imgs.Length; i++)
            {
                if (imgIndex != i)
                    img += "|" + imgs[i];
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string str = "update Recipe set name='" + name + "',available='" + available + "',restaurantId=" + restId + ",sales=" + sales + ",price='" + price + "',createTime='" + time + "',images='" + img + "' where id =" + id;
            Tool.ExecuteNon(str);
            string deleteTag = "delete from Tag_Relation where typename='recipe' and relationId=" + id;
            Tool.ExecuteNon(deleteTag);
            Tool.CopyImg(imgs, oImgs, path);

            foreach (var i in oImgs)
            {
                string filePath = path + "\\" + i.Replace("//", "\\");
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Tool.ImgUpdate(oImgs, imgs, path + "//img//recipe//");

            string insertTag = "insert Tag_Relation values ";
            for (int i = 0; i < tagIds.Length; i++)
            {
                if (i != 0)
                    insertTag += ",";
                insertTag += "(" + id + "," + tagIds[i] + ",'recipe')";
            }
            Tool.ExecuteNon(insertTag);
            string deleteFood = "delete from Recipe_foods where recipeId=" + id;
            Tool.ExecuteNon(deleteFood);
            string insertFood = "insert Recipe_foods values ";
            for (int i = 0; i < typeIds.Length; i++)
            {
                if (i != 0)
                    insertFood += ",";
                insertFood += "(" + id + "," + typeIds[i] + "," + foodIds[i] + "," + weights[i] + ")";
            }
            Tool.ExecuteNon(insertFood);
        }

        public static void UpdateRecipeCache()
        {
            Tool.UpdateCache<DbOpertion.Models.Recipe>("Recipe", "List_Recipe", true);
            Tool.UpdateCache<DbOpertion.Models.Tag_Relation>("Tag_Relation", "List_Tag_Relation", false);
            Tool.UpdateCache<DbOpertion.Models.Recipe_foods>("Recipe_foods", "List_Recipe_Foods", false);
            var MyCache = MemCacheHelper.GetMyConfigInstance();
            var list = MyCache.GetModel<List<DbOpertion.Models.Tag_Relation>>("List_Tag_Relation");
            var list2 = MyCache.GetModel<List<DbOpertion.Models.Recipe_foods>>("List_Recipe_Foods");
            var list3 = MyCache.GetModel<List<DbOpertion.Models.Recipe>>("List_Recipe");
        }
    }
}