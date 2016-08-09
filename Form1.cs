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
                MatchCollection podCategoryName = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                if(podCategoryURL.Count != 0)
                {
                    string strUpCategory = "1 " + strNameGlobalCategory;
                    for(int n = 0; podCategoryURL.Count > n; n++)
                    {
                        string thisCategory = podCategoryName[n].ToString();
                        addPodGroupInTIU(thisCategory, cookie, token, strUpCategory);

                        otv = webRequest.getRequest(podCategoryURL[n].ToString());
                        MatchCollection subPodCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                        MatchCollection subPodCategoryName = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                        if(subPodCategoryURL.Count != 0)
                        {
                            for (int z = 0; subPodCategoryURL.Count > z; z++)
                            {
                                string subStrUpCategory = "1 " + podCategoryName[n].ToString();
                                thisCategory = subPodCategoryName[z].ToString();
                                addPodGroupInTIU(thisCategory, cookie, token, subStrUpCategory);

                                otv = webRequest.getRequest(subPodCategoryURL[z].ToString());
                                MatchCollection subSubPodCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                MatchCollection subSubPodCategoryName = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                                if (subSubPodCategoryURL.Count != 0)
                                {
                                    for (int x = 0; subSubPodCategoryURL.Count > x; x++)
                                    {
                                        string subsubStrUpCategory = "1 " + subPodCategoryName[z].ToString();
                                        thisCategory = subSubPodCategoryName[x].ToString();
                                        addPodGroupInTIU(thisCategory, cookie, token, subsubStrUpCategory);

                                        otv = webRequest.getRequest(subSubPodCategoryURL[x].ToString());
                                        MatchCollection subSubPodCategoryURL2 = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                        MatchCollection subSubPodCategoryName2 = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                                        if (subSubPodCategoryURL2.Count != 0)
                                        {
                                            for (int c = 0; subSubPodCategoryURL2.Count > c; c++)
                                            {
                                                string subsubStrUpCategory2 = "1 " + subSubPodCategoryName[x].ToString();
                                                thisCategory = subSubPodCategoryName2[c].ToString();
                                                addPodGroupInTIU(thisCategory, cookie, token, subsubStrUpCategory2);

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

        private void btn_loadTovars_Click(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();
            CookieContainer cookie = Authorizacion();
            string token = tokenReturn(cookie);
            otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/create?next=%2Fcabinet%2Fproduct2%2Froot_group", cookie);
            MatchCollection groups = new Regex("(?<=<option value=)[\\w\\W]*?(?=</option>)").Matches(otv);

            otv = webRequest.getRequest("http://bike18.ru/");
            MatchCollection globalCategory = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
            MatchCollection nameGlobalCategory = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
            string idGroupTIU = null;

            for (int i = 0; globalCategory.Count > i; i++)
            {
                string categoryForTIU = "1 " + nameGlobalCategory[i].ToString();
                otv = webRequest.getRequest(globalCategory[i].ToString());
                MatchCollection podCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                MatchCollection tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                if(tovars.Count != 0)
                {
                    for(int t = 10; tovars.Count > t; t++)
                    {
                        Thread.Sleep(10000);


                        //cookie = Authorizacion();
                        //token = tokenReturn(cookie);
                        //otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie);



                        otv = webRequest.getRequest(tovars[t].ToString());
                        int v = 0;
                        foreach (Match str in groups)
                        {
                            
                            if (groups[v].ToString().Contains(categoryForTIU))
                            {
                                string group = groups[v].ToString();
                                idGroupTIU = new Regex("(?<=\").*?(?=\" data)").Match(group).ToString();
                                break;
                            }
                            v++;
                        }

                        string urlImg = new Regex("(?<=\"><a href=\")http://i..*?(?=\")").Match(otv).ToString();
                        List<string> tovar = webRequest.arraySaveimage(tovars[t].ToString());
                        string name = tovar[4];
                        string price = tovar[9];
                        string articl = tovar[6];
                        string nameImage = "pic\\" + articl + ".jpg";
                        string miniText = tovar[7];
                        string fullText = tovar[8];
                        if(tovar[7].Contains("</a>"))
                        {
                            MatchCollection urls = new Regex("<a.*?</a>").Matches(tovar[7]);
                            foreach(Match str in urls)
                            {
                                miniText = miniText.Replace(str.ToString(), "").Replace("&nbsp;", "");
                            }
                            MatchCollection ampers = new Regex("&.*?;").Matches(tovar[7]);
                            foreach (Match str in ampers)
                            {
                                miniText = miniText.Replace(str.ToString(), "").Replace("&nbsp;", "").Replace(" style=\"text-align: justify;\"", "").Replace(" class=\"field-name\"", "").Replace(" class=\"value place\"", "").Replace(" class=\"value\"", "").Replace("<p><span></span></p><p><span></span></p><p><span></span></p><p><span>.</span></p></li></ul><p><br /></p>", "").Replace("<span style=\"font-weight: bold; font-weight: bold;\">", "").Replace("<p>", "").Replace("</p>", "").Replace("<p>", "").Replace("</span>", "").Replace("<span>", "").Replace("<br />", "").Replace("<li>", "").Replace("<ul>", "");
                            }
                        }
                        if (tovar[8].Contains("</a>"))
                        {
                            MatchCollection urls = new Regex("<a.*?</a>").Matches(tovar[8]);
                            foreach (Match str in urls)
                            {
                                fullText = fullText.Replace(str.ToString(), "").Replace("&nbsp;", "");
                            }
                            MatchCollection ampers = new Regex("&.*?;").Matches(tovar[8]);
                            foreach (Match str in urls)
                            {
                                fullText = fullText.Replace(str.ToString(), "").Replace("&nbsp;", "").Replace(" style=\"text-align: justify;\"", "").Replace(" class=\"field-name\"", "").Replace(" class=\"value place\"", "").Replace(" class=\"value\"", "").Replace("<p><span></span></p><p><span></span></p><p><span></span></p><p><span>.</span></p></li></ul><p><br /></p>", "").Replace("<span style=\"font-weight: bold; font-weight: bold;\">", "").Replace("<p>", "").Replace("</p>", "").Replace("<p>", "").Replace("</span>", "").Replace("<span>", "").Replace("<br />", "").Replace("<li>", "").Replace("<ul>", "");
                            }
                        }
                        fullText = fullText.Replace("&nbsp;", "").Replace(" style=\"text-align: justify;\"", "").Replace(" class=\"field-name\"", "").Replace(" class=\"value place\"", "").Replace(" class=\"value\"", "").Replace("<p><span></span></p><p><span></span></p><p><span></span></p><p><span>.</span></p></li></ul><p><br /></p>", "").Replace("<span style=\"font-weight: bold; font-weight: bold;\">", "").Replace("<p>", "").Replace("</p>", "").Replace("<p>", "").Replace("</span>", "").Replace("<span>", "").Replace("<br />", "").Replace("<li>", "").Replace("<ul>", "").Replace("</li>", "").Replace("</ul>", "");
                        miniText = miniText.Replace("&nbsp;", "").Replace(" style=\"text-align: justify;\"", "").Replace(" class=\"field-name\"", "").Replace(" class=\"value place\"", "").Replace(" class=\"value\"", "").Replace("<p><span></span></p><p><span></span></p><p><span></span></p><p><span>.</span></p></li></ul><p><br /></p>", "").Replace("<span style=\"font-weight: bold; font-weight: bold;\">", "").Replace("<p>", "").Replace("</p>", "").Replace("<p>", "").Replace("</span>", "").Replace("<span>", "").Replace("<br />", "").Replace("<li>", "").Replace("<ul>", "").Replace("</li>", "").Replace("</ul>", "");
                        if (urlImg != "")
                        {
                            webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                            webClient.DownloadFile(urlImg, "pic\\" + articl + ".jpg");
                        }
                            
                        string[] template = File.ReadAllLines("TemplateLoadImageStart.txt");
                        template[3] = token;
                        template[84] = "Content-Disposition: form-data; name=\"images[]\"; filename=\"" + nameImage + "\"";
                        File.WriteAllLines("TemplateLoadImageStart.txt", template);
                        if(File.Exists(articl + ".jpg"))
                            otv = webRequest.PostRequestaddTovarTIUImage("https://my.tiu.ru/media/upload_image?profile=0", cookie, token, articl + ".jpg");
                        string imgId = new Regex("(?<=\"id\": ).*(?=, \"size)").Match(otv).ToString();
                        string strQuery = strQueryReturn(token, name, price, articl, miniText, fullText, idGroupTIU, imgId);
                        otv = webRequest.PostRequestaddTovarTIU("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie, token, strQuery);
                    }





















                }
                else
                {
                    for(int t = 0; podCategoryURL.Count > t; t++)
                    {
                        otv = webRequest.getRequest(podCategoryURL[t].ToString());
                        MatchCollection category2URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                        tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                        //-----------------------------------------
                        if (tovars.Count != 0)
                        {
                            for (int z = 0; tovars.Count > z; z++)
                            {
                                Thread.Sleep(5000);
                                otv = webRequest.getRequest(tovars[z].ToString());
                                int v = 0;
                                foreach (Match str in groups)
                                {

                                    if (groups[v].ToString().Contains(categoryForTIU))
                                    {
                                        string group = groups[v].ToString();
                                        idGroupTIU = new Regex("(?<=\").*?(?=\" data)").Match(group).ToString();
                                        break;
                                    }
                                    v++;
                                }

                                string urlImg = new Regex("(?<=\"><a href=\")http://i..*?(?=\")").Match(otv).ToString();
                                List<string> tovar = webRequest.arraySaveimage(tovars[z].ToString());
                                string name = tovar[4];
                                string price = tovar[9];
                                string articl = tovar[6];
                                string nameImage = "pic\\" + articl + ".jpg";
                                string miniText = tovar[7];
                                string fullText = tovar[8];

                                if (urlImg != "")
                                {
                                    webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                                    webClient.DownloadFile(urlImg, "pic\\" + articl + ".jpg");
                                }

                                string[] template = File.ReadAllLines("TemplateLoadImageStart.txt");
                                template[3] = token;
                                template[84] = "Content-Disposition: form-data; name=\"images[]\"; filename=\"" + nameImage + "\"";
                                File.WriteAllLines("TemplateLoadImageStart.txt", template);

                                otv = webRequest.PostRequestaddTovarTIUImage("https://my.tiu.ru/media/upload_image?profile=0", cookie, token, articl + ".jpg");
                                string imgId = new Regex("(?<=\"id\": ).*(?=, \"size)").Match(otv).ToString();
                                string strQuery = strQueryReturn(token, name, price, articl, miniText, fullText, idGroupTIU, imgId);
                                otv = webRequest.PostRequestaddTovarTIU("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie, token, strQuery);
                            }
                        }
                        else
                        {
                            for(int y = 0; category2URL.Count > y; y++)
                            {
                                otv = webRequest.getRequest(category2URL[y].ToString());
                                MatchCollection category3URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);

                                if (tovars.Count != 0)
                                {
                                    for (int z = 0; tovars.Count > z; z++)
                                    {
                                        Thread.Sleep(5000);
                                        otv = webRequest.getRequest(tovars[z].ToString());
                                        int v = 0;
                                        foreach (Match str in groups)
                                        {

                                            if (groups[v].ToString().Contains(categoryForTIU))
                                            {
                                                string group = groups[v].ToString();
                                                idGroupTIU = new Regex("(?<=\").*?(?=\" data)").Match(group).ToString();
                                                break;
                                            }
                                            v++;
                                        }

                                        string urlImg = new Regex("(?<=\"><a href=\")http://i..*?(?=\")").Match(otv).ToString();
                                        List<string> tovar = webRequest.arraySaveimage(tovars[z].ToString());
                                        string name = tovar[4];
                                        string price = tovar[9];
                                        string articl = tovar[6];
                                        string nameImage = "pic\\" + articl + ".jpg";
                                        string miniText = tovar[7];
                                        string fullText = tovar[8];

                                        if (urlImg != "")
                                        {
                                            webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                                            webClient.DownloadFile(urlImg, "pic\\" + articl + ".jpg");
                                        }

                                        string[] template = File.ReadAllLines("TemplateLoadImageStart.txt");
                                        template[3] = token;
                                        template[84] = "Content-Disposition: form-data; name=\"images[]\"; filename=\"" + nameImage + "\"";
                                        File.WriteAllLines("TemplateLoadImageStart.txt", template);

                                        otv = webRequest.PostRequestaddTovarTIUImage("https://my.tiu.ru/media/upload_image?profile=0", cookie, token, articl + ".jpg");
                                        string imgId = new Regex("(?<=\"id\": ).*(?=, \"size)").Match(otv).ToString();
                                        string strQuery = strQueryReturn(token, name, price, articl, miniText, fullText, idGroupTIU, imgId);
                                        otv = webRequest.PostRequestaddTovarTIU("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie, token, strQuery);
                                    }
                                }
                                else
                                {
                                    otv = webRequest.getRequest(category3URL[y].ToString());
                                    MatchCollection category4URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                    tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                                    if (tovars.Count != 0)
                                    {
                                        Thread.Sleep(5000);
                                        for (int z = 0; tovars.Count > z; z++)
                                        {
                                            otv = webRequest.getRequest(tovars[z].ToString());
                                            int v = 0;
                                            foreach (Match str in groups)
                                            {

                                                if (groups[v].ToString().Contains(categoryForTIU))
                                                {
                                                    string group = groups[v].ToString();
                                                    idGroupTIU = new Regex("(?<=\").*?(?=\" data)").Match(group).ToString();
                                                    break;
                                                }
                                                v++;
                                            }

                                            string urlImg = new Regex("(?<=\"><a href=\")http://i..*?(?=\")").Match(otv).ToString();
                                            List<string> tovar = webRequest.arraySaveimage(tovars[z].ToString());
                                            string name = tovar[4];
                                            string price = tovar[9];
                                            string articl = tovar[6];
                                            string nameImage = "pic\\" + articl + ".jpg";
                                            string miniText = tovar[7];
                                            string fullText = tovar[8];

                                            if (urlImg != "")
                                            {
                                                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                                                webClient.DownloadFile(urlImg, "pic\\" + articl + ".jpg");
                                            }

                                            string[] template = File.ReadAllLines("TemplateLoadImageStart.txt");
                                            template[3] = token;
                                            template[84] = "Content-Disposition: form-data; name=\"images[]\"; filename=\"" + nameImage + "\"";
                                            File.WriteAllLines("TemplateLoadImageStart.txt", template);

                                            otv = webRequest.PostRequestaddTovarTIUImage("https://my.tiu.ru/media/upload_image?profile=0", cookie, token, articl + ".jpg");
                                            string imgId = new Regex("(?<=\"id\": ).*(?=, \"size)").Match(otv).ToString();
                                            string strQuery = strQueryReturn(token, name, price, articl, miniText, fullText, idGroupTIU, imgId);
                                            otv = webRequest.PostRequestaddTovarTIU("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie, token, strQuery);
                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                        //----------------------------------------------

                    }

                }

            }










                otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/index/5187992?status=0", cookie);
            otv = webRequest.getRequest("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie);
            string nameTovar = "Тестовый товар666  попытка";
            string groupTovar = "13729006";
            string priceTovar = "777";
            string articlTovar = "А12366321";
            string DescriptionAttributeTovar = "Тестовый товар описание";
            string keywords = "Ключевые словики";
            
            //string stringloadImg = loadImg(token, name);

            

            
            
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
            string otv = ressr.ReadToEnd();
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
            byte[] ms = Encoding.ASCII.GetBytes("phone_email=moto%40bike18.ru&password=testTIU2016&csrf_token=" + toke + "&_save=YES&");
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

        public string loadImg(string token, string nameTovar)
        {
            string stringloadImg = "------WebKitFormBoundaryBEJupAZNoLqBlkbt\n\rContent -Disposition: form-data; name=\"csrf_token\"\r\n\r\n" + token + "\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"user_id\"\r\n\r\n2269119\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"next\"\r\n\r\nhttps://my.tiu.ru/cabinet/product2/index/5187992?status=0\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"model_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"model_binding_source\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"category\"\r\n\r\n120302\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"name\"\r\n\r\n" + nameTovar + "\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"group\"\r\n\r\n13729006\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"selling_type\"\r\n\r\n1\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"price\"\r\n\r\n123\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"price_currency\"\r\n\r\n4003\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"measure_unit\"\r\n\r\n1000\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"presence\"\r\n\r\navail\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"sku\"\r\n\r\nа11111111111111111\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"description\"\r\n\r\nОписание\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"keywords\"\r\n\r\nсловечки\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"keywords\"\r\n\r\nсловечки\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_present\"\r\n\r\n1\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom_present\"\r\n\r\n1\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attribute_18\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attribute_228\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attribute_11217\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent-Disposition: form-data; name=\"attributes_custom -0-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-0-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-0-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-0-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-1-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-1-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-1-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-1-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-2-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-2-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-2-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-2-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-3-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-3-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-3-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-3-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-4-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-4-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-4-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-4-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-5-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-5-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -5-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-5-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-6-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-6-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -6-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-6-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-7-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-7-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-7-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-7-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-8-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-8-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-8-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-8-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-9-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-9-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-9-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-9-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-10-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-10-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-10-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-10-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-11-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-11-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-11-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-11-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-12-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-12-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-12-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-12-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-13-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-13-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-13-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-13-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-14-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-14-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-14-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-14-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-15-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-15-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-15-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-15-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-16-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-16-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -16-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-16-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-17-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-17-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-17-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-17-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-18-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-18-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-18-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-18-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-19-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-19-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -19-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-19-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-20-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-20-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-20-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-20-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-21-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-21-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-21-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-21-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-22-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-22-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-22-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-22-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-23-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-23-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-23-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-23-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-24-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-24-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-24-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-24-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-25-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-25-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-25-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-25-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-26-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-26-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-26-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-26-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-27-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-27-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-27-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-27-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-28-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-28-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-28-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-28-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-29-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-29-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-29-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-29-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -30-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-30-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-30-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-30-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-31-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-31-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-31-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -31-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-32-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-32-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-32-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-32-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-33-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-33-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-33-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-33-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-34-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-34-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-34-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-34-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-35-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-35-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-35-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-35-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-36-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-36-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-36-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-36-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-37-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -37-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-37-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-37-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-38-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -38-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-38-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-38-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-39-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-39-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-39-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-39-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-40-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-40-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-40-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-40-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-41-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-41-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-41-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-41-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-42-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-42-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-42-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-42-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-43-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-43-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-43-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-43-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-44-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-44-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-44-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-44-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-45-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-45-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-45-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-45-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-46-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-46-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-46-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-46-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-47-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-47-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-47-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-47-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-48-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-48-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent-Disposition: form-data; name=\"attributes_custom -48-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-48-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-49-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-49-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-49-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-49-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-50-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -50-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-50-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-50-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-51-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-51-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-51-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-51-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-52-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-52-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-52-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-52-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-53-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-53-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-53-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-53-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-54-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-54-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-54-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-54-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-55-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-55-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-55-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -55-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-56-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-56-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-56-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-56-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-57-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-57-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-57-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-57-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-58-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-58-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-58-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-58-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-59-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-59-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-59-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-59-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-60-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-60-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-60-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-60-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-61-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-61-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-61-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-61-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-62-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-62-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-62-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-62-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-63-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-63-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-63-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-63-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-64-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-64-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -64-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-64-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-65-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-65-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-65-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-65-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-66-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-66-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-66-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-66-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-67-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-67-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-67-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom -67-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-68-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-68-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-68-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-68-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-69-attribute_name\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-69-attribute_value\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-69-attribute_id\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"attributes_custom-69-attribute_exists\"\r\n\r\ny\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"use_default_seo_settings\"\r\n\r\n1\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"packaging\"\r\n\r\n\r\n------WebKitFormBoundaryBEJupAZNoLqBlkbt\r\nContent -Disposition: form-data; name=\"images[]\"; filename=\"8729512120.jpg\"\r\nContent -Type: image/jpeg\r\n\r\n";
            return stringloadImg;
        }

        public string strQueryReturn(string token, string nameTovar, string price, string articl, string miniText, string fullText, string idGroupTIU, string imgId)
        {
           string str = "csrf_token=" + token + "&user_id=2269119&next=%2Fcabinet%2Fproduct2%2Froot_group&model_id=&model_binding_source=&category=120302&name=" + nameTovar + "&group=" + idGroupTIU + "&selling_type=1&price=" + price + "&price_currency=4003&measure_unit=1000&presence=avail&sku=" + articl + "&images=" + imgId + "&description=" + miniText + fullText + "&keywords=" + nameTovar + "&keywords=" + nameTovar + "&attributes_present=1&attributes_custom_present=1&attribute_18=&attribute_228=&attribute_11217=&attributes_custom-0-attribute_name=&attributes_custom-0-attribute_value=&attributes_custom-0-attribute_id=&attributes_custom-0-attribute_exists=y&attributes_custom-1-attribute_name=&attributes_custom-1-attribute_value=&attributes_custom-1-attribute_id=&attributes_custom-1-attribute_exists=y&attributes_custom-2-attribute_name=&attributes_custom-2-attribute_value=&attributes_custom-2-attribute_id=&attributes_custom-2-attribute_exists=y&attributes_custom-3-attribute_name=&attributes_custom-3-attribute_value=&attributes_custom-3-attribute_id=&attributes_custom-3-attribute_exists=y&attributes_custom-4-attribute_name=&attributes_custom-4-attribute_value=&attributes_custom-4-attribute_id=&attributes_custom-4-attribute_exists=y&attributes_custom-5-attribute_name=&attributes_custom-5-attribute_value=&attributes_custom-5-attribute_id=&attributes_custom-5-attribute_exists=y&attributes_custom-6-attribute_name=&attributes_custom-6-attribute_value=&attributes_custom-6-attribute_id=&attributes_custom-6-attribute_exists=y&attributes_custom-7-attribute_name=&attributes_custom-7-attribute_value=&attributes_custom-7-attribute_id=&attributes_custom-7-attribute_exists=y&attributes_custom-8-attribute_name=&attributes_custom-8-attribute_value=&attributes_custom-8-attribute_id=&attributes_custom-8-attribute_exists=y&attributes_custom-9-attribute_name=&attributes_custom-9-attribute_value=&attributes_custom-9-attribute_id=&attributes_custom-9-attribute_exists=y&attributes_custom-10-attribute_name=&attributes_custom-10-attribute_value=&attributes_custom-10-attribute_id=&attributes_custom-10-attribute_exists=y&attributes_custom-11-attribute_name=&attributes_custom-11-attribute_value=&attributes_custom-11-attribute_id=&attributes_custom-11-attribute_exists=y&attributes_custom-12-attribute_name=&attributes_custom-12-attribute_value=&attributes_custom-12-attribute_id=&attributes_custom-12-attribute_exists=y&attributes_custom-13-attribute_name=&attributes_custom-13-attribute_value=&attributes_custom-13-attribute_id=&attributes_custom-13-attribute_exists=y&attributes_custom-14-attribute_name=&attributes_custom-14-attribute_value=&attributes_custom-14-attribute_id=&attributes_custom-14-attribute_exists=y&attributes_custom-15-attribute_name=&attributes_custom-15-attribute_value=&attributes_custom-15-attribute_id=&attributes_custom-15-attribute_exists=y&attributes_custom-16-attribute_name=&attributes_custom-16-attribute_value=&attributes_custom-16-attribute_id=&attributes_custom-16-attribute_exists=y&attributes_custom-17-attribute_name=&attributes_custom-17-attribute_value=&attributes_custom-17-attribute_id=&attributes_custom-17-attribute_exists=y&attributes_custom-18-attribute_name=&attributes_custom-18-attribute_value=&attributes_custom-18-attribute_id=&attributes_custom-18-attribute_exists=y&attributes_custom-19-attribute_name=&attributes_custom-19-attribute_value=&attributes_custom-19-attribute_id=&attributes_custom-19-attribute_exists=y&attributes_custom-20-attribute_name=&attributes_custom-20-attribute_value=&attributes_custom-20-attribute_id=&attributes_custom-20-attribute_exists=y&attributes_custom-21-attribute_name=&attributes_custom-21-attribute_value=&attributes_custom-21-attribute_id=&attributes_custom-21-attribute_exists=y&attributes_custom-22-attribute_name=&attributes_custom-22-attribute_value=&attributes_custom-22-attribute_id=&attributes_custom-22-attribute_exists=y&attributes_custom-23-attribute_name=&attributes_custom-23-attribute_value=&attributes_custom-23-attribute_id=&attributes_custom-23-attribute_exists=y&attributes_custom-24-attribute_name=&attributes_custom-24-attribute_value=&attributes_custom-24-attribute_id=&attributes_custom-24-attribute_exists=y&attributes_custom-25-attribute_name=&attributes_custom-25-attribute_value=&attributes_custom-25-attribute_id=&attributes_custom-25-attribute_exists=y&attributes_custom-26-attribute_name=&attributes_custom-26-attribute_value=&attributes_custom-26-attribute_id=&attributes_custom-26-attribute_exists=y&attributes_custom-27-attribute_name=&attributes_custom-27-attribute_value=&attributes_custom-27-attribute_id=&attributes_custom-27-attribute_exists=y&attributes_custom-28-attribute_name=&attributes_custom-28-attribute_value=&attributes_custom-28-attribute_id=&attributes_custom-28-attribute_exists=y&attributes_custom-29-attribute_name=&attributes_custom-29-attribute_value=&attributes_custom-29-attribute_id=&attributes_custom-29-attribute_exists=y&attributes_custom-30-attribute_name=&attributes_custom-30-attribute_value=&attributes_custom-30-attribute_id=&attributes_custom-30-attribute_exists=y&attributes_custom-31-attribute_name=&attributes_custom-31-attribute_value=&attributes_custom-31-attribute_id=&attributes_custom-31-attribute_exists=y&attributes_custom-32-attribute_name=&attributes_custom-32-attribute_value=&attributes_custom-32-attribute_id=&attributes_custom-32-attribute_exists=y&attributes_custom-33-attribute_name=&attributes_custom-33-attribute_value=&attributes_custom-33-attribute_id=&attributes_custom-33-attribute_exists=y&attributes_custom-34-attribute_name=&attributes_custom-34-attribute_value=&attributes_custom-34-attribute_id=&attributes_custom-34-attribute_exists=y&attributes_custom-35-attribute_name=&attributes_custom-35-attribute_value=&attributes_custom-35-attribute_id=&attributes_custom-35-attribute_exists=y&attributes_custom-36-attribute_name=&attributes_custom-36-attribute_value=&attributes_custom-36-attribute_id=&attributes_custom-36-attribute_exists=y&attributes_custom-37-attribute_name=&attributes_custom-37-attribute_value=&attributes_custom-37-attribute_id=&attributes_custom-37-attribute_exists=y&attributes_custom-38-attribute_name=&attributes_custom-38-attribute_value=&attributes_custom-38-attribute_id=&attributes_custom-38-attribute_exists=y&attributes_custom-39-attribute_name=&attributes_custom-39-attribute_value=&attributes_custom-39-attribute_id=&attributes_custom-39-attribute_exists=y&attributes_custom-40-attribute_name=&attributes_custom-40-attribute_value=&attributes_custom-40-attribute_id=&attributes_custom-40-attribute_exists=y&attributes_custom-41-attribute_name=&attributes_custom-41-attribute_value=&attributes_custom-41-attribute_id=&attributes_custom-41-attribute_exists=y&attributes_custom-42-attribute_name=&attributes_custom-42-attribute_value=&attributes_custom-42-attribute_id=&attributes_custom-42-attribute_exists=y&attributes_custom-43-attribute_name=&attributes_custom-43-attribute_value=&attributes_custom-43-attribute_id=&attributes_custom-43-attribute_exists=y&attributes_custom-44-attribute_name=&attributes_custom-44-attribute_value=&attributes_custom-44-attribute_id=&attributes_custom-44-attribute_exists=y&attributes_custom-45-attribute_name=&attributes_custom-45-attribute_value=&attributes_custom-45-attribute_id=&attributes_custom-45-attribute_exists=y&attributes_custom-46-attribute_name=&attributes_custom-46-attribute_value=&attributes_custom-46-attribute_id=&attributes_custom-46-attribute_exists=y&attributes_custom-47-attribute_name=&attributes_custom-47-attribute_value=&attributes_custom-47-attribute_id=&attributes_custom-47-attribute_exists=y&attributes_custom-48-attribute_name=&attributes_custom-48-attribute_value=&attributes_custom-48-attribute_id=&attributes_custom-48-attribute_exists=y&attributes_custom-49-attribute_name=&attributes_custom-49-attribute_value=&attributes_custom-49-attribute_id=&attributes_custom-49-attribute_exists=y&attributes_custom-50-attribute_name=&attributes_custom-50-attribute_value=&attributes_custom-50-attribute_id=&attributes_custom-50-attribute_exists=y&attributes_custom-51-attribute_name=&attributes_custom-51-attribute_value=&attributes_custom-51-attribute_id=&attributes_custom-51-attribute_exists=y&attributes_custom-52-attribute_name=&attributes_custom-52-attribute_value=&attributes_custom-52-attribute_id=&attributes_custom-52-attribute_exists=y&attributes_custom-53-attribute_name=&attributes_custom-53-attribute_value=&attributes_custom-53-attribute_id=&attributes_custom-53-attribute_exists=y&attributes_custom-54-attribute_name=&attributes_custom-54-attribute_value=&attributes_custom-54-attribute_id=&attributes_custom-54-attribute_exists=y&attributes_custom-55-attribute_name=&attributes_custom-55-attribute_value=&attributes_custom-55-attribute_id=&attributes_custom-55-attribute_exists=y&attributes_custom-56-attribute_name=&attributes_custom-56-attribute_value=&attributes_custom-56-attribute_id=&attributes_custom-56-attribute_exists=y&attributes_custom-57-attribute_name=&attributes_custom-57-attribute_value=&attributes_custom-57-attribute_id=&attributes_custom-57-attribute_exists=y&attributes_custom-58-attribute_name=&attributes_custom-58-attribute_value=&attributes_custom-58-attribute_id=&attributes_custom-58-attribute_exists=y&attributes_custom-59-attribute_name=&attributes_custom-59-attribute_value=&attributes_custom-59-attribute_id=&attributes_custom-59-attribute_exists=y&attributes_custom-60-attribute_name=&attributes_custom-60-attribute_value=&attributes_custom-60-attribute_id=&attributes_custom-60-attribute_exists=y&attributes_custom-61-attribute_name=&attributes_custom-61-attribute_value=&attributes_custom-61-attribute_id=&attributes_custom-61-attribute_exists=y&attributes_custom-62-attribute_name=&attributes_custom-62-attribute_value=&attributes_custom-62-attribute_id=&attributes_custom-62-attribute_exists=y&attributes_custom-63-attribute_name=&attributes_custom-63-attribute_value=&attributes_custom-63-attribute_id=&attributes_custom-63-attribute_exists=y&attributes_custom-64-attribute_name=&attributes_custom-64-attribute_value=&attributes_custom-64-attribute_id=&attributes_custom-64-attribute_exists=y&attributes_custom-65-attribute_name=&attributes_custom-65-attribute_value=&attributes_custom-65-attribute_id=&attributes_custom-65-attribute_exists=y&attributes_custom-66-attribute_name=&attributes_custom-66-attribute_value=&attributes_custom-66-attribute_id=&attributes_custom-66-attribute_exists=y&attributes_custom-67-attribute_name=&attributes_custom-67-attribute_value=&attributes_custom-67-attribute_id=&attributes_custom-67-attribute_exists=y&attributes_custom-68-attribute_name=&attributes_custom-68-attribute_value=&attributes_custom-68-attribute_id=&attributes_custom-68-attribute_exists=y&attributes_custom-69-attribute_name=&attributes_custom-69-attribute_value=&attributes_custom-69-attribute_id=&attributes_custom-69-attribute_exists=y&use_default_seo_settings=1&packaging=&ajax_create=true&status=0&redirect_to_list=true";
            return str;
        }        
    }
}
