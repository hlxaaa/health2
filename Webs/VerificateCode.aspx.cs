using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Webs
{
    public partial class VerificateCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Code;
            Code = string.Empty;
            Bitmap bit = GetCode(out Code);
            Session["vCode"] = Code;
            Response.ClearContent();
            //Response.Write("<script>alert(1)</script>");
            bit.Save(Response.OutputStream, ImageFormat.Png);
            Response.ContentType = "image/png";
            
            bit.Dispose();
        }

        public static Bitmap GetCode(out string Code)
        {
            int imgWidth = 80;
            int imgHeight = 40;
            //获取随机字符
            Code = Str(4,false);
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };

            Bitmap bmp = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            Random rnd = new Random();
            //画噪线 
            for (int i = 0; i < 10; i++)
            {
                int x1 = rnd.Next(imgWidth);
                int y1 = rnd.Next(imgHeight);
                int x2 = rnd.Next(imgWidth);
                int y2 = rnd.Next(imgHeight);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < Code.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, 18);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(Code[i].ToString(), ft, new SolidBrush(clr), (float)i * 19, (float)8);
            }
            //画噪点 
            for (int i = 0; i < 100; i++)
            {
                int x = rnd.Next(bmp.Width);
                int y = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x, y, clr);
            }
            //显式释放资源 
            // bmp.Dispose();
            g.Dispose();
            return bmp;
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
    }
}