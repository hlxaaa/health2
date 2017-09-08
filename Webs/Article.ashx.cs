using System;
using System.Web;

namespace WebApplication1.Webs
{
    /// <summary>
    /// Article1 的摘要说明
    /// </summary>
    public class Article1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            var files = context.Request.Files;
            if (files.Count <= 0)
            {
                return;
            }




            //System.Threading.Thread.Sleep(200);
            HttpPostedFile file = files[0];

            if (file == null)
            {
                context.Response.Write("error|file is null");
                return;
            }
            else
            {
                string time = DateTime.Now.Year.ToString() + DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string path = context.Server.MapPath("~/img/article/temp/");  //存储图片的文件夹
                string originalFileName = file.FileName;
                string fileExtension = originalFileName.Substring(originalFileName.LastIndexOf('.'), originalFileName.Length - originalFileName.LastIndexOf('.'));
                string currentFileName = time + file.ContentLength + fileExtension;  //文件名中不要带中文，否则会出错
                //生成文件路径
                string imagePath = path + currentFileName;
                //保存文件
                file.SaveAs(imagePath);
                //context.Response.he
                //获取图片url地址
                string imgUrl = path + currentFileName;
                context.Response.Headers.Add("Access-Control-Allow-Origin", path);
                context.Response.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With");
                context.Response.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
                //返回图片url地址
                context.Response.Write("../img/article/temp/" + currentFileName);

                return;
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}