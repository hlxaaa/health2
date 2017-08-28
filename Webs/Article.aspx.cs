using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
//using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Webs
{
    public partial class Article : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);
        protected DataSet ds = new DataSet();
        protected int ran = new Random().Next();
        protected string jsonStr = "";
        protected string[] ids = { };
        protected string path = "";

        protected Dictionary<string, string> dictTags = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["restId"] == null || (Session["restId"] != null && Session["restId"].ToString() != "0"))
            {
                Response.Redirect("/error.aspx", false);
                Response.End();
                return;
            }      
            dictTags = Tool.GetDict("Tag", "id", "name", conn);
            GetArticle();
            switch (Request["method"])
            {
                case "search":
                    ResJsonStr();
                    break;
                case "addArticle":
                    AddArticle();
                    UpdateArticleCache();
                    break;
                case"deleteArticle":
                    DeleteArticle(Request["id"]);
                    UpdateArticleCache();
                    break;
                case"edit":
                    GetContentByid();
                    break;
                case"updateArticle":
                    UpdateArticle();
                    UpdateArticleCache();
                    break;
                case"batchDelete":
                    BatchDelete();
                    UpdateArticleCache();
                    break;
            }
        }

        protected void GetContentByid() {
            string id = Request["id"];
            string selectCont = "select content from Article where id = "+id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(selectCont, conn);
            string content = (string)sqlCom.ExecuteScalar();
            content = content.Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");
            Response.Write(content);
            Response.End();
            conn.Close();
        }

        protected void GetArticle()
        {
            
            int pageIndex = 1;
            int pageSize = 15;
            string index = Request["thePage"];
            if (index != null)
                pageIndex = Convert.ToInt32(index);
            string size = Request["size"];
            if (size != null)
                pageSize = Convert.ToInt32(size);

            string sqlSelect = "select id,title,content,tags,cilckCount,loveCount,aTime from Article where 1 = 1 ";

            //搜索条件
            int pages = 0;
            int thePage = 1;
            if (Request["thePage"] != null)
                thePage = Convert.ToInt32(Request["thePage"]);

            string search = " ";
            if (Request["search"] != null && Request["search"].Trim() != "")
            {
                search = Request["search"].Trim();
                sqlSelect += " and id in (select id from Article where  ";
                sqlSelect += " title like '%" + search + "%' or content like '%" + search + "%'";

                foreach (var v in dictTags.Values)
                {
                    if (v.IndexOf(search) > -1)
                    {
                        string id = Tool.GetKey(dictTags, v);
                        sqlSelect += " or id in (select r.id from Article as r,Tag_Relation as r2 ,Tag as t where r2.tagId = " + id + " and r2.relationId=r.id and r2.tagId = t.id and t.isDeleted = 'False')";
                    }
                }

                sqlSelect += ")";
            }

 

            //sqlSelect += " and (name like '%" + search.Trim() +"%' )";
            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
            //sqlSelect += " and id in ( select top "+pageSize+" id from Recipe where id not in (select top "+x+" id from Recipe))";

            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sqlSelect, conn);
            da.Fill(ds);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();

            SqlDataAdapter myda = new SqlDataAdapter(sqlPaging, conn);
            myda.Fill(ds);

            ds = Tool.DsToString(ds);
            int count = ds.Tables[0].Rows.Count;
            ids = new string[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] = ds.Tables[0].Rows[i]["id"].ToString();
                ds.Tables[0].Rows[i]["aTime"] = DateTime.Parse(ds.Tables[0].Rows[i]["aTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string temp = ds.Tables[0].Rows[i]["content"].ToString().Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");
                

                temp = Regex.Replace(temp, @"[^\u4e00-\u9fa5]+", "");
                int l = temp.Length > 20 ? 20 : temp.Length;
                ds.Tables[0].Rows[i]["content"] = temp.Substring(0, l);

                ds.Tables[0].Rows[i]["tags"] = GetTags(ids[i], dictTags);
                //string result = Regex.Match(content[i], "(?<=<p>).*?(?=</p>)").Value;
            }

            conn.Close();
            jsonStr = Tool.GetJsonByDataset(ds);
            string pagesStr = ",\"pages\":" + pages + "";
            string thePageStr = ",\"thePage\":" + thePage + "";
            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1) + pagesStr + thePageStr + "}";
        }

        protected void AddArticle()
        {
            path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);

            var title = Request["title"];
            var content = Request["content"];
            //var tags = Request["tags[]"];
            var img = "";
            string[] imgs = Request.Form.GetValues("img[]");
            for (int i = 0; i < imgs.Length; i++) {
                if (i != 0)
                    img += ",";
                img += path + imgs[i];
            }

            var imgTemp = "";
            string[] imgTemps = Request.Form.GetValues("imgTemp[]");
            for (int i = 0; i < imgTemps.Length; i++) {
                if (i != 0)
                    imgTemp += ",";
                imgTemp +=path+ imgTemps[i];
            }

            var thumbnail = Request["thumbnail"];
            string tempUrl = Server.MapPath("~/img/article/temp/");
            //ImgHandle(img, imgTemp);
            Tool.ImgHandle(img, imgTemp, tempUrl);

            //string[] tags = Request.Form["tags[]"].Split(',');
            string[] tags = Request.Form.GetValues("tags[]");
            string tagIds = "";
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i] != "")
                {
                    if (i != 0)
                        tagIds += "|";
                    tagIds += Tool.GetKey(dictTags, tags[i]);
                }
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sqlInsert = "insert Article (title,content,tags,aTime,thumbnail,cilckCount,loveCount) values ('" + title + "','" + content + "','" + tagIds + "','" + time + "','" + thumbnail + "',0,0) Select @@IDENTITY";

            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlInsert, conn);
            //sqlCom.Parameters.Add("@id", SqlDbType.Int);
            var id = sqlCom.ExecuteScalar();
            string url = "../webs/ArticleUrl.aspx?id=" + id;
            string urlStr = "update Article set url = '" + url + "' where id = " + id;
            SqlCommand sqlCom2 = new SqlCommand(urlStr, conn);
            sqlCom2.ExecuteScalar();

            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i] != "")
                {
                    string tagId= Tool.GetKey(dictTags, tags[i]);
                    string insertTag = "insert Tag_Relation (relationId,tagId,typename) values ("+id+","+tagId+",'article')";
                    SqlCommand sqlCom3 = new SqlCommand(insertTag, conn);
                    sqlCom3.ExecuteScalar();
                }
            }
            

            conn.Close();
        }

        protected void UpdateArticle()
        {
            path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);

            int id = Convert.ToInt32(Request["id"]);
            string title = Request["title"];
            string content = Request["content"];
            string thumbnail = Request["thumbnail"];
            string oriImg ="";
            string[] oriImgs = Request["oriImg"].Split(',');
            for (int i = 0; i < oriImgs.Length; i++) {
                if (i != 0)
                    oriImg += ",";
                oriImg += path + oriImgs[i];
            }
            //string img = Request["img[]"];
            //string imgTemp = Request["imgTemp[]"];

            var img = "";
            string[] imgs = Request.Form.GetValues("img[]");
            for (int i = 0; i < imgs.Length; i++)
            {
                if (i != 0)
                    img += ",";
                img += path + imgs[i];
            }

            var imgTemp = "";
            string[] imgTemps = Request.Form.GetValues("imgTemp[]");
            for (int i = 0; i < imgTemps.Length; i++)
            {
                if (i != 0)
                    imgTemp += ",";
                imgTemp += path + imgTemps[i];
            }

            Tool.ImgUpdate(oriImg.Split(','), img.Split(','));
            string tempUrl = Server.MapPath("~/img/article/temp/");
            Tool.ImgHandle(img, imgTemp,tempUrl);
            string[] tags = Request.Form.GetValues("tags[]");
            string tagIds = "";
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i] != "")
                {
                    if (i != 0)
                        tagIds += "|";
                    tagIds += Tool.GetKey(dictTags, tags[i]);
                }
            }
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string sqlUpdate = "update Article set title = '" + title + "',content = '" + content + "',tags = '" + tagIds + "',aTime = '" + time + "',thumbnail = '" + thumbnail + "' where id = " + id;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(sqlUpdate, conn);
            sqlCom.ExecuteScalar();
            conn.Close();
        }

        protected void DeleteArticle(string id)
        {
            //string id = Request["id"];
            conn.Open();

            string selectImgs = "select content from Article where id = " + id;
            SqlCommand sqlCom = new SqlCommand(selectImgs, conn);
            string content = (string)sqlCom.ExecuteScalar();
            content = content.Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");
            string[] imgs = GetImgPath(content);

            string sqlDelete = "delete from Article where id = " + id;
            SqlCommand sqlCom2 = new SqlCommand(sqlDelete, conn);
            sqlCom2.ExecuteScalar();
            conn.Close();
            DeleteImgFile(imgs);
        }

        protected void BatchDelete() {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (string id in ids) {
                DeleteArticle(id);
            }
            //conn.Open();
            //foreach (string i in id) {
            //    DeleteImgFile(i,conn);
            //}
            //string ids = Request["ids[]"];
            //string batchDelete = "delete from Article where id in (" + ids + ")";
            
            //SqlCommand sqlCom = new SqlCommand(batchDelete, conn);
            //sqlCom.ExecuteScalar();
            //conn.Close();
        }

        protected void DeleteImgFile(string[] imgs) {
            //string selectImgs = "select content from Article where id = " + id;
            //SqlCommand sqlCom = new SqlCommand(selectImgs, conn);
            //string content = (string)sqlCom.ExecuteScalar();
            //content = content.Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");
            //string[] imgs = GetImgPath(content);

            for (int i = 0; i < imgs.Length; i++)
            {
                if (imgs[i] != "")
                {
                    string path = Server.MapPath(imgs[i].Substring(2));
                    if (File.Exists(path))
                        File.Delete(path);
                }
            }
        }

        protected void ResJsonStr()
        {

            Response.Write(jsonStr);
            Response.End();
        }

        protected string GetTags(string id, Dictionary<string, string> dict)
        {
            string[] tags = { };
            string result = "";
            string str = "select tagId from Tag_Relation as t1,Tag as t2 where relationId = " + id + " and typename = 'article' and isDeleted = 'False' group by tagId";
            //string str = "select tagId from " + tbName + " where " + tbId + " = " + id +" and typename='recipe'";
            //conn.Open();
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

        protected void ImgUpdate(string oImgs, string imgs)
        {
            string[] oImg = oImgs.Split(',');
            string[] img = imgs.Split(',');
            for (int i = 0; i < oImg.Length; i++)
            {
                bool isExist = false;
                for (int j = 0; j < img.Length; j++)
                {
                    if (oImg[i] == img[j])
                        isExist = true;
                }
                if (!isExist)
                    File.Delete(oImg[i]);
            }
        }

        /// <summary>
        /// 从图文html中取到所有图片的路径
        /// </summary>
        /// <param name="content">图文html</param>
        /// <returns>所有图片的路径</returns>
        protected string[] GetImgPath(string content) {
            List<string> list = new List<string>();
            string[] temp = content.Split('<');
            for (int i = 0; i < temp.Length; i++)
            {
                string result = Regex.Match(temp[i], "(?<=src=\").*?(?=\")").Value;
                if (result != ""&&result!=null)
                    list.Add(result);
            }
            return list.ToArray();
        }

        public static void UpdateArticleCache()
        {
            Tool.UpdateCache<DbOpertion.Models.Article>("Article","List_Article", false);
        }

        //[WebMethod]  
        //public static string SayHello()
        //{
        //    return "Hello Ajax!";
        //} 
    }
}