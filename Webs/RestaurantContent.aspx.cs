using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

namespace WebApplication1.Webs
{
    public partial class RestaurantContent : System.Web.UI.Page
    {
        protected string id = "";
        protected string name = "";
        protected string address = "";
        protected string coordinate = "";
        protected string category = "";
        protected string phone = "";
        protected string sales = "";
        protected string consumption = "";
        protected string startTime = "";
        protected string endTime = "";
        protected string[] discounts = { };
        protected string[] imgs = { };
        protected string oImgs = "";
        protected Dictionary<string, string> dictCate = new Dictionary<string, string>();
        protected Dictionary<string, string> dictRecipe = new Dictionary<string, string>();

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
                dictCate = GetDict();
                id = Request["id"];
                GetRest(id);
                switch (Request["method"])
                {
                    case "addRest":
                        AddRest();
                        UpdateRestCache();
                        break;
                    case "updateRest":
                        UpdateRest();
                        UpdateRestCache();
                        break;
                }
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

                int fileLength = f[i].ContentLength;
                bool isThum = true;
                if (fileLength < 200000)
                {
                    isThum = false;
                }
                string newname = now.ToString("yyyyMMddHHmmss") + i;
                /* startIndex */
                int index = fname.LastIndexOf("\\") + 1;
                /* length */
                int len = fname.Length - index;
                fname = fname.Substring(index, len);
                /* save to server */
                string nameCut = "img//restaurant//temp//restaurant" + Request["id"] + "-" + newname + newext;//先存到temp中
                string newImg = "img//restaurant//temp//restaurant" + Request["id"] + "-new" + newname + newext;

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

        protected void AddRest()
        {
            string path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);

            string name = Request["name"];
            string address = Request["address"];
            string coordinate = Request["coordinate"];
            string categoryId = Request["categoryId"];
            string phone = Request["phone"];
            string sales = Request["sales"];
            string consumption = Request["consumption"];
            string startTime = Request["startTime"];
            string endTime = Request["endTime"];
            int imgIndex = Convert.ToInt32(Request["imgIndex"]);
            string[] discounts = Request.Form.GetValues("discounts[]");
            string discount = "";
            if (discounts != null)
            {
                for (int i = 0; i < discounts.Length; i++)
                {
                    if (i != 0)
                        discount += '|';
                    discount += discounts[i];
                }
            }

            string[] imgNames = Request.Form.GetValues("imgNames[]");
            string[] imgs = new string[imgNames.Length];
            for (int i = 0; i < imgNames.Length; i++)
            {
                imgs[i] = "img//restaurant//" + imgNames[i];
            }

            string[] oImgNames = Request.Form.GetValues("oImgNames[]");
            string[] oImgs = new string[oImgNames.Length];
            for (int i = 0; i < oImgNames.Length; i++)
            {
                oImgs[i] = "img//restaurant//temp//" + oImgNames[i];
            }

            string img = "";
            img += imgs[imgIndex];
            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i] = imgs[i];
                if (imgIndex != i)
                    img += "|" + imgs[i];
            }
            string thumb = imgs[imgIndex];

            string str = "insert Restaurant values ('" + name + "','" + thumb + "','" + img + "','" + address + "','" + phone + "','" + startTime + "-" + endTime + "','" + categoryId + "','" + coordinate + "','" + sales + "'," + consumption + ",'" + discount + "','False') ";
            Tool.ExecuteNon(str);
            Tool.CopyImg(imgs, oImgs, path);//图片存在temp中，复制到article中。删除temp中的图片
            foreach (var i in oImgs)
            {
                File.Delete(path + "\\" + i.Replace("//", "\\"));
            }
        }

        protected void UpdateRest()
        {
            string path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);

            string name = Request["name"];
            string address = Request["address"];
            string coordinate = Request["coordinate"];
            string categoryId = Request["categoryId"];
            string phone = Request["phone"];
            string sales = Request["sales"];
            string consumption = Request["consumption"];
            string startTime = Request["startTime"];
            string endTime = Request["endTime"];
            int imgIndex = Convert.ToInt32(Request["imgIndex"]);
            string[] discounts = Request.Form.GetValues("discounts[]");
            string discount = "";
            if (discounts != null)
            {
                for (int i = 0; i < discounts.Length; i++)
                {
                    if (i != 0)
                        discount += '|';
                    discount += discounts[i];
                }
            }
            string[] imgNames = Request.Form.GetValues("imgNames[]");
            string[] imgs = new string[imgNames.Length];
            for (int i = 0; i < imgNames.Length; i++)
            {
                imgs[i] = "img//restaurant//" + imgNames[i];
            }

            string[] oImgNames = Request.Form.GetValues("oImgNames[]");
            string[] oImgs = new string[oImgNames.Length];
            for (int i = 0; i < oImgNames.Length; i++)
            {
                oImgs[i] = "img//restaurant//temp//" + oImgNames[i];
            }

            string img = "";
            img += imgs[imgIndex];
            for (int i = 0; i < imgs.Length; i++)
            {
                if (imgIndex != i)
                    img += "|" + imgs[i];
            }

            string thumb = imgs[imgIndex];
            string id = Request["id"];

            string str = "update Restaurant set name = '" + name + "',thumbnail='" + thumb + "',images='" + img + "',address='" + address + "',phone='" + phone + "',businesshours='" + startTime + "-" + endTime + "',category='" + categoryId + "',coordinate='" + coordinate + "',sales='" + sales + "',consumption='" + consumption + "',discount='" + discount + "' where id = " + id;
            Tool.ExecuteNon(str);
            Tool.CopyImg(imgs, oImgs, path);

            foreach (var i in oImgs)
            {
                string filePath = path + "\\" + i.Replace("//", "\\");
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            Tool.ImgUpdate(oImgs, imgs, path + "//img//restaurant//");
        }

        protected void GetRest(string id)
        {
            if (id != "" && id != null)
            {
                string sql = "select * from Restaurant where id = " + id;
                DataSet ds = Tool.ExecuteGetDs(sql);
                id = ds.Tables[0].Rows[0]["id"].ToString();
                name = ds.Tables[0].Rows[0]["name"].ToString();
                address = ds.Tables[0].Rows[0]["address"].ToString();
                phone = ds.Tables[0].Rows[0]["phone"].ToString();
                coordinate = ds.Tables[0].Rows[0]["coordinate"].ToString();
                category = dictCate[ds.Tables[0].Rows[0]["category"].ToString()];
                sales = ds.Tables[0].Rows[0]["sales"].ToString();
                consumption = ds.Tables[0].Rows[0]["consumption"].ToString();
                string times = ds.Tables[0].Rows[0]["businesshours"].ToString();
                startTime = times.Split('-')[0];
                endTime = times.Split('-')[1];
                discounts = ds.Tables[0].Rows[0]["discount"].ToString().Split('|');
                imgs = ds.Tables[0].Rows[0]["images"].ToString().Split('|');
                oImgs = ds.Tables[0].Rows[0]["images"].ToString();
                dictRecipe = GetRecipes(id);
            }
        }

        protected Dictionary<string, string> GetRecipes(string restId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string sql = "select id,name from Recipe where isDeleted = 'False' and restaurantId = " + restId;
            DataSet ds = Tool.ExecuteGetDs(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string idTemp = ds.Tables[0].Rows[i]["id"].ToString();
                string name = ds.Tables[0].Rows[i]["name"].ToString();
                dict.Add(idTemp, name);
            }
            return dict;
        }

        protected Dictionary<string, string> GetDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string sql = "select * from DataDictionary where typename ='餐厅类型'";
            DataSet ds = new DataSet();

            ds = Tool.ExecuteGetDs(sql);
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                string id = ds.Tables[0].Rows[i]["id"].ToString();
                string value = ds.Tables[0].Rows[i]["typeValue"].ToString();
                dict.Add(id, value);
            }
            return dict;
        }

        public static void UpdateRestCache()
        {
            Tool.UpdateCache<DbOpertion.Models.Restaurant>("Restaurant", "List_Restaurant", true);
        }
    }
}