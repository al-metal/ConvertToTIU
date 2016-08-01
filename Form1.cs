using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using web;

namespace ConvertToTIU
{
    public partial class Form1 : Form
    {
        web.WebRequest webRequest = new web.WebRequest();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string otv = null;
            CookieContainer cookie = cookieURL("http://izhevsk.tiu.ru/");
            otv = PostRequest("https://my.tiu.ru/cabinet/sign-in", cookie);
        }

        public CookieContainer cookieURL(string url)
        {
            CookieContainer cookie = new CookieContainer();

            HttpWebResponse res = null;
            HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(url);
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            req.CookieContainer = cookie;
            res = (HttpWebResponse)req.GetResponse();
            StreamReader ressr = new StreamReader(res.GetResponseStream());
            String otv = ressr.ReadToEnd();
            res.GetResponseStream().Close();
            req.GetResponse().Close();

            return cookie;
        }

        public string PostRequest(string nethouseTovar, CookieContainer cookie1)
        {
            string otv = null;
            HttpWebResponse res = null;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(nethouseTovar);
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.CookieContainer = cookie1;
            res = (HttpWebResponse)req.GetResponse();
            StreamReader ressr = new StreamReader(res.GetResponseStream());
            otv = ressr.ReadToEnd();
  

            return otv;
        }
    }
}
