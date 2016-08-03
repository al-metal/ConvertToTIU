using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using web;

namespace ConvertToTIU
{
    public partial class Form1 : Form
    {
        string otv = null;
        web.WebRequest webRequest = new web.WebRequest();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            CookieContainer cookie = Authorizacion();
            string token = tokenReturn(cookie);
            otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/index/5187992?status=0", cookie);
            otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie);
            otv = webRequest.getRequest("http://bike18.ru/");
            MatchCollection globalCategory = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
            MatchCollection nameGlobalCategory = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);

            for (int i = 0; globalCategory.Count > i; i++)
            {
                string strNameGlobalCategory = nameGlobalCategory[i].ToString();
                addGroupInTIU(strNameGlobalCategory, cookie, token);
                otv = webRequest.getRequest(globalCategory[i].ToString());
                MatchCollection podCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                MatchCollection namePodCategory = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                if(podCategoryURL.Count != 0)
                {
                    string strUpCategory = "1 " + strNameGlobalCategory;
                    for(int n = 0; podCategoryURL.Count > n; n++)
                    {
                        strNameGlobalCategory = namePodCategory[n].ToString();
                        addPodGroupInTIU(strNameGlobalCategory, cookie, token, strUpCategory);

                        otv = webRequest.getRequest(podCategoryURL[n].ToString());
                        MatchCollection subPodCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                        MatchCollection nameSubPodCategory = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                        if(subPodCategoryURL.Count != 0)
                        {
                            
                            for (int z = 0; subPodCategoryURL.Count > z; z++)
                            {
                                string subStrUpCategory = "1 " + namePodCategory[n].ToString();
                                string strNameSubPodCategory = nameSubPodCategory[z].ToString();
                                addPodGroupInTIU(strNameSubPodCategory, cookie, token, subStrUpCategory);

                                otv = webRequest.getRequest(subPodCategoryURL[z].ToString());
                                MatchCollection subSubPodCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                MatchCollection nameSubSubPodCategory = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                                if (subSubPodCategoryURL.Count != 0)
                                {
                                    for (int x = 0; subSubPodCategoryURL.Count > x; x++)
                                    {
                                        string subsubStrUpCategory = "1 " + subSubPodCategoryURL[z].ToString();
                                        string strsubNameSubPodCategory = nameSubSubPodCategory[x].ToString();
                                        addPodGroupInTIU(strsubNameSubPodCategory, cookie, token, subsubStrUpCategory);


                                        otv = webRequest.getRequest(subSubPodCategoryURL[x].ToString());
                                        MatchCollection subSubPodCategoryURL2 = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                        MatchCollection nameSubSubPodCategory2 = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                                        if (subPodCategoryURL.Count != 0)
                                        {
                                            for (int c = 0; subPodCategoryURL.Count > c; c++)
                                            {
                                                string subsubStrUpCategory2 = "1 " + namePodCategory[x].ToString();
                                                string strsubNameSubPodCategory2 = nameSubPodCategory[c].ToString();
                                                addPodGroupInTIU(strsubNameSubPodCategory2, cookie, token, subsubStrUpCategory2);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            
        }

        private void addGroupInTIU(string strNameGlobalCategory, CookieContainer cookie, string token)
        {
            Thread.Sleep(10000);
            bool b = false;
            otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/group_create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie);
            MatchCollection listGroupTovarsTIU = new Regex("(?<=<option  value=\").*?(?=</option>)").Matches(otv);
            for (int z = 0; listGroupTovarsTIU.Count > z; z++)
            {
                string strListGroupTovars = listGroupTovarsTIU[z].ToString();
                if (strListGroupTovars.Contains("1 " + strNameGlobalCategory))
                {
                    b = true;
                    break;
                }
            }

            if (!b)
            {
                string inquiry = "csrf_token=" + token + "&user_id=2269119&group-next=https://my.tiu.ru/cabinet/product2/index/5187992?status=0&group-parent_group=13525258&group-name=1 " + strNameGlobalCategory + "&group-selling_type=1&group-description=&group-group_seo_text=&group-managers=66604&group-use_default_seo_settings=1&group-submit_button=Сохранить группу&save_=1";
                otv = webRequest.PostRequest("https://my.tiu.ru/cabinet/product2/group_create", cookie, token, inquiry);

            }
        }

        private void addPodGroupInTIU(string strNameGlobalCategory, CookieContainer cookie, string token, string strUpCategory)
        {
            Thread.Sleep(5000);
            bool b = false;
            string codeGroupTIU = null;
            string subsection = null;
            otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/group_create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie);
            MatchCollection listGroupTovarsTIU = new Regex("(?<=<option  value=\").*?(?=</option>)").Matches(otv);
            for (int z = 0; listGroupTovarsTIU.Count > z; z++)
            {
                codeGroupTIU = new Regex(".*?(?=\">)").Match(listGroupTovarsTIU[z].ToString()).ToString();
                string strListGroupTovars = listGroupTovarsTIU[z].ToString();
                if (strListGroupTovars.Contains(strUpCategory))
                {
                    subsection = strUpCategory;
                    break;
                }
            }
            for (int z = 0; listGroupTovarsTIU.Count > z; z++)
            {
                string strListGroupTovars = listGroupTovarsTIU[z].ToString();
                if (strListGroupTovars.Contains("1 " + strNameGlobalCategory))
                {
                    b = true;
                    break;
                }
            }

            if (!b)
            {
                Thread.Sleep(10000);
                string inquiry = "csrf_token=" + token + "&user_id=2269119&group-next=https://my.tiu.ru/cabinet/product2/index/5187992?status=0&group-parent_group=" + codeGroupTIU + "&group-name=1 " + strNameGlobalCategory + "&group-selling_type=1&group-description=&group-group_seo_text=&group-managers=66604&group-use_default_seo_settings=1&group-submit_button=Сохранить группу&save_=1";
                otv = webRequest.PostRequest("https://my.tiu.ru/cabinet/product2/group_create", cookie, token, inquiry);

            }
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

            string str = res.Cookies[0].Value;

            return cookie;
        }

        public string PostRequest(string nethouseTovar, CookieContainer cookie1)
        {
            string otv = null;
            HttpWebResponse res = null;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(nethouseTovar);
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.CookieContainer = cookie1;
            res = (HttpWebResponse)req.GetResponse();
            StreamReader ressr = new StreamReader(res.GetResponseStream());
            otv = ressr.ReadToEnd();
  

            return otv;
        }

        public CookieContainer Authorizacion()
        {
            CookieContainer cookie = new CookieContainer();

            HttpWebResponse res = null;
            HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create("http://izhevsk.tiu.ru/");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            req.CookieContainer = cookie;
            res = (HttpWebResponse)req.GetResponse();
            StreamReader ressr = new StreamReader(res.GetResponseStream());
            String otv = ressr.ReadToEnd();
            res.GetResponseStream().Close();
            req.GetResponse().Close();
            Cookie ck = res.Cookies["csrf_token"];

            Cookie token = new Cookie("csrf_token", res.Cookies[0].Value);
            string toke = res.Cookies[0].Value;

            req = (HttpWebRequest)HttpWebRequest.Create("https://my.tiu.ru/cabinet/sign-in");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.CookieContainer = cookie;
            res = (HttpWebResponse)req.GetResponse();
            ressr = new StreamReader(res.GetResponseStream());
            otv = ressr.ReadToEnd();

            req = (HttpWebRequest)HttpWebRequest.Create("https://my.tiu.ru/cabinet/auth/phone_login");
            req.Accept = "application/json, text/javascript, */*; q=0.01";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            req.Method = "POST";
            req.Referer = "https://my.tiu.ru/cabinet/sign-in";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("X-CSRFToken", toke);
            req.Headers.Add("Origin", "https://my.tiu.ru");
            req.CookieContainer = cookie;
            byte[] ms = Encoding.ASCII.GetBytes("phone_email=moto%40bike18.ru&password=TIURU12345&csrf_token=" + toke + "&_save=YES&");
            req.ContentLength = ms.Length;
            Stream stre = req.GetRequestStream();
            stre.Write(ms, 0, ms.Length);
            stre.Close();
            res = (HttpWebResponse)req.GetResponse();

            return cookie;
        }

        public string tokenReturn (CookieContainer cookie)
        {
            string token = null;

            HttpWebResponse res = null;
            HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create("http://izhevsk.tiu.ru/");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            req.CookieContainer = cookie;
            res = (HttpWebResponse)req.GetResponse();
            StreamReader ressr = new StreamReader(res.GetResponseStream());
            String otv = ressr.ReadToEnd();
            res.GetResponseStream().Close();
            req.GetResponse().Close();
            Cookie ck = res.Cookies["csrf_token"];

            Cookie tokenn = new Cookie("csrf_token", res.Cookies[0].Value);
            token = res.Cookies[0].Value;

            return token;
        }
    }
}
