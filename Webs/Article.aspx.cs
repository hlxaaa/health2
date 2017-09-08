using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
//using System.Web.Services;

namespace WebApplication1.Webs
{
    public partial class Article : System.Web.UI.Page
    {
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
            dictTags = Tool.GetDict("Tag", "id", "name");
            //GetArticle();
            switch (Request["method"])
            {
                case "search":
                    GetArticle();
                    ResJsonStr();
                    break;
                case "addArticle":
                    AddArticle();
                    UpdateArticleCache();
                    break;
                case "deleteArticle":
                    DeleteArticle(Request["id"]);
                    UpdateArticleCache();
                    break;
                case "edit":
                    GetContentByid();
                    break;
                case "updateArticle":
                    UpdateArticle();
                    UpdateArticleCache();
                    break;
                case "batchDelete":
                    BatchDelete();
                    UpdateArticleCache();
                    break;
                //default:
                //    GetArticle();
                //    break;
            }
        }

        protected void GetContentByid()
        {
            string id = Request["id"];
            string selectCont = "select content from Article where id = " + id;
            string content = Tool.ExecuteScalar(selectCont);
            content = content.Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");
            Response.Write(content);
            Response.End();
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

            string sqlSelect = "select id,title,content,cilckCount,loveCount,aTime from Article where 1 = 1 ";
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

            int x = (pageIndex - 1) * pageSize;
            string sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
            DataSet ds = Tool.ExecuteGetDs(sqlSelect);
            int allCount = ds.Tables[0].Rows.Count;
            pages = allCount / pageSize;
            pages = pages * pageSize == allCount ? pages : pages + 1;
            ds.Clear();

            if (allCount > 0)
            {
                if (thePage > pages)//最后一页的最后一个被删掉时，处理
                {
                    thePage = pages;
                    x = (pageIndex - 2) * pageSize;
                    sqlPaging = "select top " + pageSize + " * from (" + sqlSelect + ") r where id not in (select top " + x + " id from (" + sqlSelect + ") r order by id desc) order by id desc";
                }
            }
            ds = Tool.ExecuteGetDs(sqlPaging);
            ds = Tool.DsToString(ds);
            int count = ds.Tables[0].Rows.Count;
            ids = new string[count];
            DataColumn dc = new DataColumn("tags");
            ds.Tables[0].Columns.Add(dc);
            for (int i = 0; i < count; i++)
            {
                ids[i] = ds.Tables[0].Rows[i]["id"].ToString();
                ds.Tables[0].Rows[i]["aTime"] = DateTime.Parse(ds.Tables[0].Rows[i]["aTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                string temp = ds.Tables[0].Rows[i]["content"].ToString().Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");

                temp = Regex.Replace(temp, @"[^\u4e00-\u9fa5]+", "");
                int l = temp.Length > 20 ? 20 : temp.Length;
                ds.Tables[0].Rows[i]["content"] = temp.Substring(0, l);

                ds.Tables[0].Rows[i]["tags"] = GetTags(ids[i]);
            }

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
          
            string[] imgNames = Request.Form.GetValues("imgName[]");
            string[] imgs = new string[imgNames.Length];
            for (int i = 0; i < imgNames.Length; i++)
            {
                imgs[i] = "img//article//" + imgNames[i];
            }
            string[] oImgs = new string[imgNames.Length];
            for (int i = 0; i < imgNames.Length; i++)
            {
                oImgs[i] = "img//article//temp//" + imgNames[i];
            }

            var thumbnail = Request["thumbnail"];
            string[] tags = Request.Form.GetValues("tags[]");
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sqlInsert = "insert Article (title,content,tags,aTime,thumbnail,cilckCount,loveCount) values ('" + title + "','" + content + "','此字段不使用','" + time + "','" + thumbnail + "',0,0) Select @@IDENTITY";//tags不能为空，但这个字段已经不适用了-txy
            var id = Tool.ExecuteScalar(sqlInsert);
            string url = "../webs/ArticleUrl.aspx?id=" + id;
            string urlStr = "update Article set url = '" + url + "' where id = " + id;
            Tool.ExecuteNon(urlStr);
            string insertTag = "insert Tag_Relation (relationId,tagId,typename) values ";
            for (int i = 0; i < tags.Length; i++)
            {
                if (tags[i] != "")
                {
                    if (i != 0)
                        insertTag += ",";
                    string tagId = Tool.GetKey(dictTags, tags[i]);
                    insertTag += " (" + id + "," + tagId + ",'article')";
                }
            }
            Tool.ExecuteNon(insertTag);
            Tool.CopyImg(imgs, oImgs, path);
        }

        protected void UpdateArticle()
        {
            path = Server.MapPath("");
            path = path.Substring(0, path.Length - 5);

            string articleId = Request["id"];
            string title = Request["title"];
            string content = Request["content"];
            string thumbnail = Request["thumbnail"];
            string[] oriImgs = Request["oriImg"].Split(',');
            string[] oImgs = new string[oriImgs.Length];
            for (int i = 0; i < oriImgs.Length; i++)
            {
                oImgs[i] = "img//article//" + oriImgs[i];
            }
            string[] imgNames = Request.Form.GetValues("imgName[]");
            string[] imgs = new string[imgNames.Length];
            for (int i = 0; i < imgNames.Length; i++)
            {
                imgs[i] = "img//article//" + imgNames[i];
            }

            string[] imgTemps = new string[imgNames.Length];
            for (int i = 0; i < imgTemps.Length; i++)
            {
                imgTemps[i] = "img//article//temp//" + imgNames[i];
            }
        
            Tool.CopyImg(imgs, imgTemps, path);
            Tool.ImgUpdate(oImgs, imgs, path + "//img//article//");
            string tempUrl = Server.MapPath("~/img/article/temp/");
            string[] tags = Request.Form.GetValues("tags[]");
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = Tool.GetKey(dictTags, tags[i]);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string sqlUpdate = "update Article set title = '" + title + "',content = '" + content + "',aTime = '" + time + "',thumbnail = '" + thumbnail + "' where id = " + articleId;
            Tool.ExecuteNon(sqlUpdate);
            Tool.UpdateTag_Relation(articleId, "article", tags);
        }

        protected void DeleteArticle(string articleId)
        {
            string selectImgs = "select content from Article where id = " + articleId;
            string content = Tool.ExecuteScalar(selectImgs);
            content = content.Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");
            string[] imgs = GetImgPath(content);

            string sqlDelete = "delete from Article where id = " + articleId;
            Tool.ExecuteNon(sqlDelete);
            DeleteImgFile(imgs);
        }

        protected void BatchDelete()
        {
            string[] ids = Request.Form.GetValues("ids[]");
            foreach (string id in ids)
            {
                DeleteArticle(id);
            }
        }

        protected void DeleteImgFile(string[] imgs)
        {
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

        /// <summary>
        /// 由文章id获取标签字符串
        /// </summary>
        /// <param name="articleId">文章id</param>
        /// <param name="dict">标签 id,名称 字典</param>
        /// <returns></returns>
        protected string GetTags(string articleId)
        {
            string str = "select stuff ((select ' '+ t2.name from Tag_Relation as t1,Tag as t2 where relationId = "+articleId+" and typename = 'article' and isDeleted = 'False' and t2.id=t1.tagId group by t2.name for xml path('')),1,1,'')";
            return Tool.ExecuteScalar(str);
        }

        /// <summary>
        /// 从图文html中取到所有图片的路径
        /// </summary>
        /// <param name="content">图文html</param>
        /// <returns>所有图片的路径</returns>
        protected string[] GetImgPath(string content)
        {
            List<string> list = new List<string>();
            string[] temp = content.Split('<');
            for (int i = 0; i < temp.Length; i++)
            {
                string result = Regex.Match(temp[i], "(?<=src=\").*?(?=\")").Value;
                if (result != "" && result != null)
                    list.Add(result);
            }
            return list.ToArray();
        }

        public static void UpdateArticleCache()
        {
            Tool.UpdateCache<DbOpertion.Models.Article>("Article", "List_Article", false);
        }
    }
}