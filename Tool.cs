using Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;


namespace WebApplication1
{
    public class Tool
    {
        protected static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HealthConnection"].ConnectionString);

        public static string GetRestIdBySellerId(string sellerId) {
            string str = "select restaurantid from Seller where id =" + sellerId;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            string r = sqlCom.ExecuteScalar().ToString();
            conn.Close();
            return r;
        }

        public static string GetSellerIdByRestId(string restId) {
            string str = "select id from Seller where restaurantid =" + restId;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            string r = sqlCom.ExecuteScalar().ToString();
            conn.Close();
            return r;
        }

        public static string GetRestNameById(string restId) {
            string str = "select name from Restaurant where id ="+restId;
            conn.Open();
            SqlCommand sqlCom = new SqlCommand(str, conn);
            string r = sqlCom.ExecuteScalar().ToString() ;
            conn.Close();
            return r;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="tableName"></param>
        public static void UpdateCache<T>(string tableName,string listName,bool isHasDeleteField) where T : class,new()
        {
            conn.Open();
            string selectAll = "select * from " + tableName;
            if (isHasDeleteField)
            {
                //if (tableName == "Tag" || tableName == "Food")
                    selectAll += " where isDeleted = 'false'";
                //else
                //    selectAll += " where isDelete = 'false'";
            }
            SqlDataAdapter da = new SqlDataAdapter(selectAll, conn);
            DataTable table = new DataTable();
            da.Fill(table);
            var list = table.ConvertToList<T>();

            conn.Close();
            var MyCache = MemCacheHelper.GetMyConfigInstance();
            MyCache.Set(listName, list);
        }

        /// <summary>
        /// 判断添加或更新的名字是否存在
        /// </summary>
        /// <param name="value">要添加或更新的值</param>
        /// <param name="field">字段名称</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">额外的条件，可为空</param>
        /// <returns>true或false</returns>
        public static bool IsExist(string value, string field, string tableName, string condition)
        {
            conn.Open();
            string str = "select id from " + tableName + " where " + field + " = '" + value + "'" + condition;
            SqlCommand sqlCom = new SqlCommand(str, conn);
            var a = sqlCom.ExecuteScalar();
            conn.Close();
            if (a != null)
                return true;
            return false;
        }

        public static string[] GetTags()
        {
            string[] result = { };

            return result;
        }

        /// <summary>
        /// 生成字典
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <param name="key">字典的键</param>
        /// <param name="value">字典的值</param>
        /// <param name="conn">数据库连接实例</param>
        /// <returns>字典</returns>
        public static Dictionary<string, string> GetDict(string tableName, string key, string value, SqlConnection conn)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            DataSet ds = new DataSet();
            string str = "select " + key + "," + value + " from " + tableName;
            if (tableName == "Food" || tableName == "Tag")
            {
                str += " where isDeleted = 'False'";
            }
            conn.Open();
            SqlDataAdapter myda = new SqlDataAdapter(str, conn);
            myda.Fill(ds);
            conn.Close();
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                dict.Add(ds.Tables[0].Rows[i][key].ToString(), ds.Tables[0].Rows[i][value].ToString());
            }

            return dict;
        }

        /// <summary>
        /// 获取字典的键
        /// </summary>
        /// <param name="dict">字典</param>
        /// <param name="value">字典的值</param>
        /// <returns>查到返回字典的键，查不到返回"查不到id"</returns>
        public static string GetKey(Dictionary<string, string> dict, string value)
        {
            foreach (string key in dict.Keys)
            {
                if (dict[key] == value)
                    return key;
            }
            return "查不到这个id";
        }

        /// <summary>  
        /// 生成缩略图  
        /// </summary>  
        /// <param name="sourceFile">原始图片文件</param>  
        /// <param name="quality">质量压缩比</param>  
        /// <param name="multiple">收缩倍数</param>  
        /// <param name="outputFile">输出文件名</param>  
        /// <returns>成功返回true,失败则返回false</returns>  
        public static bool getThumImage(String sourceFile, long quality, int multiple, String outputFile)
        {
            try
            {
                long imageQuality = quality;
                Bitmap sourceImage = new Bitmap(sourceFile);
                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, imageQuality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                float xWidth = sourceImage.Width;
                float yWidth = sourceImage.Height;
                Bitmap newImage = new Bitmap((int)(xWidth / multiple), (int)(yWidth / multiple));
                Graphics g = Graphics.FromImage(newImage);

                g.DrawImage(sourceImage, 0, 0, xWidth / multiple, yWidth / multiple);
                g.Dispose();
                sourceImage.Dispose();
                newImage.Save(outputFile, myImageCodecInfo, myEncoderParameters);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// temp图片复制到目标文件夹中
        /// </summary>
        /// <param name="imgs">目标文件的路径</param>
        /// <param name="imgTemps">temp文件的路径</param>
        /// <param name="tempUrl">temp文件夹路径</param>
        public static void ImgHandle(string imgs, string imgTemps,string tempUrl)
        {
            string[] img = imgs.Split(',');
            string[] imgTemp = imgTemps.Split(',');
            for (int i = 0; i < img.Length; i++)
            {
                //imgTemp[i] = imgTemp[i].Replace("\\\\", "\\");
                //if (!File.Exists(img[i]))
                    Tool.getThumImage(imgTemp[i], 18, 1, img[i]);
                //File.Copy(imgTemp[i], img[i]);
            }
            //string tempUrl = Server.MapPath("~/img/article/temp/");
            DirectoryInfo dir = new DirectoryInfo(tempUrl);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);
                }
                else
                    File.Delete(i.FullName);
            }
        }

        /// <summary>
        /// 图片更新文件操作
        /// </summary>
        /// <param name="oImgs">原来图片的路径集合</param>
        /// <param name="imgs">最新图片的路径集合</param>
        public static void ImgUpdate(string[] oImg, string[] img)
        {
            //string[] oImg = oImgs.Split(',');
            //string[] img = imgs.Split(',');
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
        /// 获取图片编码信息  
        /// </summary>  
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 把dataset数据转换成json的格式
        /// </summary>
        /// <param name="ds">dataset数据集</param>
        /// <returns>json格式的字符串</returns>
        public static string GetJsonByDataset(DataSet ds)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            {
                //如果查询到的数据为空则返回标记ok:false
                return "{\"ok\":false}";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"ok\":true,");
            foreach (DataTable dt in ds.Tables)
            {
                sb.Append(string.Format("\"{0}\":[", dt.TableName));

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }

                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("],");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 将object转换成为string
        /// </summary>
        /// <param name="ob">obj对象</param>
        /// <returns></returns>
        public static string ObjToStr(object ob)
        {
            if (ob == null)
            {
                return string.Empty;
            }
            else
                return ob.ToString();
        }

        /// <summary>
        /// DataSet的字段类型都转为string
        /// </summary>
        /// <param name="ds">任意DataSet</param>
        /// <returns>全是string字段的DataSet</returns>
        public static DataSet DsToString(DataSet ds)
        {
            DataSet result = new DataSet();
            DataTable dt = new DataTable();
            result.Tables.Add(dt);
            int colCount = ds.Tables[0].Columns.Count;
            for (int i = 0; i < colCount; i++)
            {
                result.Tables[0].Columns.Add(ds.Tables[0].Columns[i].ColumnName, typeof(string));
            }
            int l = ds.Tables[0].Rows.Count;
            for (int i = 0; i < l; i++)
            {
                result.Tables[0].Rows.Add();
                for (int j = 0; j < colCount; j++)
                {

                    result.Tables[0].Rows[i][j] = ds.Tables[0].Rows[i][j].ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 数据库中"id1|id2|id3"的字符串转为"name1 name2 name3"
        /// </summary>
        /// <param name="ids">"id1|id2|id3"的字符串</param>
        /// <param name="dict">以id,name为键值对的字典</param>
        /// <returns>"name1 name2 name3"的字符串</returns>
        public static string IdToName(string ids, Dictionary<string, string> dict)
        {
            string names = "";
            string[] id = ids.Split('|');
            for (int i = 0; i < id.Length; i++)
            {
                if (i != 0)
                    names += " ";
                names += dict[id[i]];
            }
            return names;
        }

        
        public static string GetTags(SqlConnection conn, string id, string tbId, string tbName, Dictionary<string, string> dict)
        {
            string[] tags = { };
            string result = "";
            string str = "select tagId from " + tbName + " where " + tbId + " = " + id;
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
                    result += "标签被删除";
                }
            }

            return result;
        }


    }
}