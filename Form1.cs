﻿using Bike18;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ConvertToTIU
{
    public partial class Form1 : Form
    {
        int countTovar = new int();
        string otv = null;
        httpRequest webRequest = new httpRequest();
        nethouse nethouse = new nethouse();
        WebClient webClient = new WebClient();

        CookieContainer cookie;
        CookieContainer cookieBike;
        public Form1()
        {
            InitializeComponent();

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            CookieContainer cookie = Authorizacion(tbLoginTIU.Text, tbPassTIU.Text);
            string token = tokenReturn(cookie);
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/index/5187992?status=0");
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0");
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
                if (podCategoryURL.Count != 0)
                {
                    string strUpCategory = "1 " + strNameGlobalCategory;
                    for (int n = 0; podCategoryURL.Count > n; n++)
                    {
                        string thisCategory = podCategoryName[n].ToString();
                        addPodGroupInTIU(thisCategory, cookie, token, strUpCategory);

                        otv = webRequest.getRequest(podCategoryURL[n].ToString());
                        MatchCollection subPodCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                        MatchCollection subPodCategoryName = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);
                        if (subPodCategoryURL.Count != 0)
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
            Properties.Settings.Default.loginTIU = tbLoginTIU.Text;
            Properties.Settings.Default.passTIU = tbPassTIU.Text;
            Properties.Settings.Default.loginNethause = tbLoginNethause.Text;
            Properties.Settings.Default.passNethause = tbPassNethause.Text;
            Properties.Settings.Default.Save();

            CookieContainer cookie = Authorizacion(tbLoginTIU.Text, tbPassTIU.Text);
            string token = tokenReturn(cookie);
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/create?next=%2Fcabinet%2Fproduct2%2Froot_group");
            cookieBike = nethouse.CookieNethouse(tbLoginNethause.Text, tbPassNethause.Text);
            if (cookie.Count != 9 || cookieBike.Count != 4)
            {
                MessageBox.Show("Логин или пароль не верные");
                return;
            }

            MatchCollection groups = new Regex("(?<=<option value=)[\\w\\W]*?(?=</option>)").Matches(otv);

            otv = webRequest.getRequest("http://bike18.ru/");
            MatchCollection globalCategory = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
            MatchCollection nameGlobalCategory = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);

            for (int i = 13; globalCategory.Count > i; i++)
            {
                Thread.Sleep(10000);
                string categoryForTIU = /*"1 " +*/ nameGlobalCategory[i].ToString();
                otv = webRequest.getRequest("https://bike18.ru" + globalCategory[i].ToString() + "?page=all");
                MatchCollection podCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                MatchCollection podCategoryName = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                MatchCollection tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                if (tovars.Count != 0)
                {
                    for (int z = 0; tovars.Count > z; z++)
                    {
                        Thread.Sleep(15000);
                        CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                    }
                }
                else
                {
                    for (int l = 0; podCategoryURL.Count > l; l++)
                    {
                        categoryForTIU = /*"1 " + */podCategoryName[l].ToString();

                        /*if (categoryForTIU != "Снегоходы новые (Китай, Россия)" && categoryForTIU != "Снегоходы и аксессуары ALPINA SHERPA" && categoryForTIU != "Снегоходы Stels")
                            continue;*/

                        otv = webRequest.getRequest("https://bike18.ru" + podCategoryURL[l].ToString() + "?page=all");
                        MatchCollection category2Name = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                        MatchCollection category2URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                        tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                        if (tovars.Count != 0)
                        {
                            for (int z = 0; tovars.Count > z; z++)
                            {
                                Thread.Sleep(15000);
                                CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                            }
                        }
                        else
                        {
                            for (int t = 0; category2URL.Count > t; t++)
                            {
                                categoryForTIU = /*"1 " + */category2Name[t].ToString();
                                otv = webRequest.getRequest("https://bike18.ru" + category2URL[t].ToString() + "?page=all");
                                MatchCollection category3URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                MatchCollection category3Name = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                                tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                                if (tovars.Count != 0)
                                {

                                    for (int z = 0; tovars.Count > z; z++)
                                    {
                                        Thread.Sleep(15000);
                                        CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                                    }
                                }
                                else
                                {
                                    for (int r = 0; category3URL.Count > r; r++)
                                    {
                                        categoryForTIU = "1 " + category3Name[r].ToString();
                                        otv = webRequest.getRequest(category3URL[r].ToString() + "/page/all");
                                        MatchCollection category4URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                        MatchCollection category4Name = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                                        tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                                        if (tovars.Count != 0)
                                        {

                                            for (int z = 0; tovars.Count > z; z++)
                                            {
                                                Thread.Sleep(8000);
                                                CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }










            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/index/5187992?status=0");
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0");
            string nameTovar = "Тестовый товар666  попытка";
            string groupTovar = "13729006";
            string priceTovar = "777";
            string articlTovar = "А12366321";
            string DescriptionAttributeTovar = "Тестовый товар описание";
            string keywords = "Ключевые словики";

            //string stringloadImg = loadImg(token, name);





        }

        private void CreateTovar(string urlTovar, string token, CookieContainer cookie, MatchCollection groups, string categoryForTIU, int countGlobalCategory)
        {
            if (countTovar == 10)
            {
                Thread.Sleep(40000);
                countTovar = 0;
            }
            string idGroupTIU = null;
            otv = webRequest.getRequest(urlTovar);
            int v = 0;
            foreach (Match str in groups)
            {
                string ss = groups[v].ToString();
                if (ss.Contains("13012473"/*categoryForTIU*/))
                {
                    string group = groups[v].ToString();
                    idGroupTIU = new Regex(".*?(?=\" data-path)").Match(group).ToString();
                    break;
                }
                v++;
            }

            idGroupTIU = ReturnCategoryTovar(categoryForTIU);

            string urlImg = new Regex("(?<=class=\"avatar-view \"><link rel=\"image_src\" href=\").*?(?=\"><a href=\")").Match(otv).ToString();
            idGroupTIU = idGroupTIU.Replace("\"", "");

            List<string> tovar = nethouse.GetProductList(cookieBike, urlTovar);
            string name = tovar[4];
            urlImg = tovar[32];
            urlImg = urlImg.Replace("\\/", "/");

            MatchCollection ampers = new Regex("&.*?;").Matches(name);
            foreach (Match str in ampers)
            {
                name = name.Replace(str.ToString(), "");
            }
            name = Replace(name);

            string price = tovar[9];
            string articl = tovar[6];
            string miniText = tovar[7];
            string fullText = tovar[8];
            SaveAllImages(tovar[44].ToString(), articl);

            string urlVK = new Regex("<span style=\"color: #ff0000;\">.*?</span>").Match(miniText).ToString();
            if (urlVK != "")
            {

            }
            //miniText = miniText.Replace(urlVK, "");

            string emailBike18 = new Regex("Звоните!.*?moto@bike18.ru").Match(otv).ToString();
            if (emailBike18 != "")
                fullText = fullText.Replace(emailBike18, "");

            miniText = miniText.Replace("BIKE18.RU", "");
            fullText = fullText.Replace("BIKE18.RU", "");

            if (tovar[7].Contains("<"))
            {
                MatchCollection urls = new Regex("<.*?>").Matches(tovar[7]);
                foreach (Match str in urls)
                {
                    if (str.ToString().Contains("font-weight: bold;"))
                        miniText = miniText.Replace(str.ToString(), "<strong>").Replace("</span>", "</strong>");
                }
            }
            if (tovar[8].Contains("<"))
            {
                MatchCollection urls = new Regex("<.*?>").Matches(tovar[8]);
                foreach (Match str in urls)
                {
                    if (str.ToString().Contains("font-weight: bold;"))
                        fullText = fullText.Replace(str.ToString(), "<strong>").Replace("</span>", "</strong>");
                }
                /*MatchCollection urls = new Regex("<.*?>").Matches(tovar[8]);
                foreach (Match str in urls)
                {
                    fullText = fullText.Replace(str.ToString(), "").Replace("&nbsp;", "");
                }*/

            }
            //miniText = miniText.Replace("rel=\"noopener noreferrer\"", "");
            MatchCollection links = new Regex("(?<=<a).*?(?=>)").Matches(miniText);
            foreach (Match ss in links)
            {
                string str = ss.ToString();
                if (str.Contains("vk.com/bike18"))
                    miniText = miniText.Replace(str, " href=\"https://vk.com/bike18\" target=\"_blank\"");
            }
            string punkts = new Regex("(?<=<p>1. Нажмите).*<p>6.").Match(fullText).ToString();
            if (punkts != "")
                fullText = fullText.Replace("&nbsp;", " ").Replace(punkts, "");


            fullText = fullText.Replace("&nbsp;", " ");
            miniText = miniText.Replace("&nbsp;", " ");
            punkts = new Regex("<p>1..*?</p>").Match(fullText).ToString();
            if (punkts != "")
                fullText = fullText.Replace("&nbsp;", " ").Replace(punkts, "");
            MatchCollection urlsMinitext = new Regex("<a.*?\">").Matches(miniText);
            //miniText = miniText.Replace("</a>", "");
            foreach (Match str in urlsMinitext)
            {
                if (!str.ToString().Contains("vk.com/bike18"))
                    miniText = miniText.Replace(str.ToString(), "");
            }
            ampers = new Regex("&.*?;").Matches(tovar[7]);
            miniText = miniText.Replace("<span style=\"font-weight: bold; font-weight: bold; \">", "<strong>").Replace("</span>", "</strong>").Replace("<span style=\"color: #000000; font-weight: bold; font-weight: bold;\">", "<strong>");
            fullText = fullText.Replace("<span style=\"font-weight: bold; font-weight: bold; \">", "<strong>").Replace("</span>", "</strong>").Replace("<span style=\"color: #000000; font-weight: bold; font-weight: bold;\">", "<strong>");
            miniText = miniText.Replace("<p><br /></p>", "").Replace("<span style=\"font-weight: bold; font-weight: bold;\">", "");
            miniText = miniText.Replace("<p><br /></p>", "").Replace("<span style=\"font-weight: bold; font-weight: bold;\">", "").Replace("</strong></strong>", "</strong>").Replace("</p></p>", "</p>").Replace("<span>", "");


            foreach (Match str in ampers)
            {
                miniText = miniText.Replace(str.ToString(), "").Replace("|", "");
            }

            ampers = new Regex("&.*?;").Matches(tovar[8]);

            foreach (Match str in ampers)
            {
                fullText = fullText.Replace(str.ToString(), "").Replace("|", "");
            }
            fullText = Replace(fullText);
            miniText = Replace(miniText);

            /*if (urlImg != "")
            {
                urlImg = urlImg.Replace("//", "");
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.81 Safari/537.36");
                try
                {
                    webClient.DownloadFile("http://" + urlImg, "pic\\" + articl + ".jpg");
                }
                catch { }
            }*/

            string imgId = "";
            string s = "";
            for (int i = 0; 10 > i; i++)
            {
                string nameImage = "pic\\" + articl + "_" + i + ".jpg";
                string[] template = File.ReadAllLines("TemplateLoadImageStart.txt");
                template[3] = token;
                template[84] = "Content-Disposition: form-data; name=\"images[]\"; filename=\"" + nameImage + "\"";
                File.WriteAllLines("TemplateLoadImageStart.txt", template);
                if (File.Exists("pic\\" + articl + "_" + i + ".jpg"))
                    otv = nethouse.PostRequestaddTovarTIUImage("https://my.tiu.ru/media/upload_image?profile=0", cookie, token, articl + "_" + i + ".jpg");
                imgId = new Regex("(?<=\"id\": ).*(?=, \"size)").Match(otv).ToString();
                if (i != 0)
                    s = s + "&images=" + imgId;
                else
                    s = imgId;

            }

            if (miniText == "")
            {
                miniText = "Подробную информацию вы можите уточнить по";
            }
            if (fullText == "")
            {
                fullText = " телефонам +7 (922) 517-31-43, отдел продаж: +7(922) 517 - 39 - 95 сервис центр: + 7(341) 277 - 31 - 43; + 7(341) 277 - 39 - 95";
            }

            string idCategory = ReturntIdCategory(countGlobalCategory);

            string strQuery = strQueryReturn(token, name, price, articl, miniText, fullText, idGroupTIU, s, idCategory);
            //otv = nethouse.PostRequestaddTovarTIU("https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0", cookie, token, strQuery);
            otv = nethouse.PostRequestaddTovarTIU("https://my.tiu.ru/cabinet/product2/create?next=%2Fcabinet%2Fproduct2%2Froot_group", cookie, token, strQuery);
            countTovar++;
        }

        private string ReturntIdCategory(int countGlobalCategory)
        {
            string idCategory = "";
            switch (countGlobalCategory)
            {
                case 13:
                    idCategory = "1204";
                    break;
                case 15:
                    idCategory = "120308";
                    break;
                case 16:
                    idCategory = "120306";
                    break;
                case 17:
                    idCategory = "1250316";
                    break;
                case 18:
                    idCategory = "203502";
                    break;

                default:
                    idCategory = "120308";
                    break;
            }
            return idCategory;
        }

        private string ReturnCategoryTovar(string categoryForTIU)
        {
            string idGroupTIU = "";
            if (categoryForTIU == "Складные велосипеды")
                idGroupTIU = "17245666";
            else if (categoryForTIU == "Горные велосипеды")
                idGroupTIU = "17245667";
            else if (categoryForTIU == "Городские велосипеды")
                idGroupTIU = "17245668";
            else if (categoryForTIU == "Туристические велосипеды")
                idGroupTIU = "17245669";
            else if (categoryForTIU == "Женские велосипеды")
                idGroupTIU = "17245670";
            else if (categoryForTIU == "Детские велосипеды")
                idGroupTIU = "17245671";
            else if (categoryForTIU == "Двухподвесные велосипеды")
                idGroupTIU = "17245672";
            else if (categoryForTIU == "Велосипеды FatBike (Фэтбайки)")
                idGroupTIU = "17245673";
            else if (categoryForTIU == "BMX")
                idGroupTIU = "17956989";


            else if (categoryForTIU == "Минитракторы")
                idGroupTIU = "17957527";
            else if (categoryForTIU == "Тракторы")
                idGroupTIU = "17957528";
            else if (categoryForTIU == "Мотоблоки и мотокультиваторы")
                idGroupTIU = "17957531";
            else if (categoryForTIU == "Плуги")
                idGroupTIU = "17957533";
            else if (categoryForTIU == "Почвофрезы")
                idGroupTIU = "17957534";
            else if (categoryForTIU == "Обработка почвы")
                idGroupTIU = "17957535";
            else if (categoryForTIU == "Картофелесажалки")
                idGroupTIU = "17957537";
            else if (categoryForTIU == "Картофелекопалки")
                idGroupTIU = "17957538";
            else if (categoryForTIU == "Сеялки")
                idGroupTIU = "17957539";
            else if (categoryForTIU == "Косилки")
                idGroupTIU = "17957540";
            else if (categoryForTIU == "Грабли")
                idGroupTIU = "17957541";
            else if (categoryForTIU == "Пресс-подборщики")
                idGroupTIU = "17957542";
            else if (categoryForTIU == "Рассадопосадочные машины")
                idGroupTIU = "17957543";
            else if (categoryForTIU == "Опрыскиватели")
                idGroupTIU = "17957544";
            else if (categoryForTIU == "Разбрасыватели минеральных удобрений")
                idGroupTIU = "17957545";
            else if (categoryForTIU == "Комбайны")
                idGroupTIU = "17957546";
            else if (categoryForTIU == "Ковши универсальные и фронтальные погрузчики для тракторов")
                idGroupTIU = "17957548";
            else if (categoryForTIU == "Экскаваторы навесные")
                idGroupTIU = "17957550";
            else if (categoryForTIU == "Отвалы, снегоуборщики (для трактора, мотоблока)")
                idGroupTIU = "17957551";
            else if (categoryForTIU == "Буры")
                idGroupTIU = "17957552";
            else if (categoryForTIU == "Доильный аппарат")
                idGroupTIU = "17957553";
            else if (categoryForTIU == "Разное")
                idGroupTIU = "17957554";
            else if (categoryForTIU == "Самоходные шасси")
                idGroupTIU = "17957555";
            else if (categoryForTIU == "Мотолебедки")
                idGroupTIU = "17957557";
            else if (categoryForTIU == "Генераторы и мотопомпы")
                idGroupTIU = "17957558";
            else if (categoryForTIU == "Прицепы для тракторов и мотоблоков")
                idGroupTIU = "17957560";
            else if (categoryForTIU == "Тяжёлая сельхозтехника")
                idGroupTIU = "17957561";

            else if (categoryForTIU == "Дорожные новые")
                idGroupTIU = "17245629";
            else if (categoryForTIU == "Кроссовые новые")
                idGroupTIU = "17245630";
            else if (categoryForTIU == "Эндуро новые")
                idGroupTIU = "17245631";
            else if (categoryForTIU == "Мотард новые")
                idGroupTIU = "17245632";
            else if (categoryForTIU == "Спортивные новые")
                idGroupTIU = "17245633";
            else if (categoryForTIU == "Чопперы, круизеры новые")
                idGroupTIU = "17245634";
            else if (categoryForTIU == "Трициклы и грузовые мотоциклы")
                idGroupTIU = "17245663";


            else if (categoryForTIU == "Питбайки")
                idGroupTIU = "17245584";

            else if (categoryForTIU == "Мопеды")
                idGroupTIU = "17245585";

            else if (categoryForTIU == "Скутеры")
                idGroupTIU = "17245586";

            else if (categoryForTIU == "Детские квадроциклы")
                idGroupTIU = "17245589";

            else if (categoryForTIU == "Багги")
                idGroupTIU = "17970928";

            else if (categoryForTIU == "Вездеходы")
                idGroupTIU = "17245591";

            else if (categoryForTIU == "Алюминиевые лодки")
                idGroupTIU = "17971672";
            else if (categoryForTIU == "Стеклопластиковые лодки")
                idGroupTIU = "17971674";
            else if (categoryForTIU == "Моторно-гребные лодки ALFA")
                idGroupTIU = "17973757";
            else if (categoryForTIU == "Моторно-гребные лодки SIRIUS")
                idGroupTIU = "17973758";
            else if (categoryForTIU == "Надувные лодки Zongshen")
                idGroupTIU = "17973759";
            else if (categoryForTIU == "Лодки LAKER")
                idGroupTIU = "17973760";
            else if (categoryForTIU == "Моторно-гребные лодки COMPASS")
                idGroupTIU = "17973761";
            else if (categoryForTIU == "Моторные лодки повышенной мореходности PRO ultra")
                idGroupTIU = "17973762";
            else if (categoryForTIU == "Моторные лодки PRO")
                idGroupTIU = "17973763";
            else if (categoryForTIU == "Лодки Joker")
                idGroupTIU = "17973764";
            else if (categoryForTIU == "Лодка ПВХ SibRiver")
                idGroupTIU = "17973765";
            else if (categoryForTIU == "Лодки Инзер")
                idGroupTIU = "17973766";
            else if (categoryForTIU == "Лодки SEA-PRO")
                idGroupTIU = "17973767";
            else if (categoryForTIU == "Лодки Шкипер, Бриз, Пиранья")
                idGroupTIU = "17973768";
            else if (categoryForTIU == "Лодочные моторы Nissan Marine")
                idGroupTIU = "17973769";
            else if (categoryForTIU == "Моторы MARLIN")
                idGroupTIU = "17973770";
            else if (categoryForTIU == "Лодочные моторы SKIPPER")
                idGroupTIU = "17973771";
            else if (categoryForTIU == "Лодочные моторы HANGKAI")
                idGroupTIU = "17973772";
            else if (categoryForTIU == "Лодочные моторы Zongshen")
                idGroupTIU = "17973773";
            else if (categoryForTIU == "Лодочные моторы BREEZE")
                idGroupTIU = "17973775";
            else if (categoryForTIU == "Электрические лодочные моторы Haswing")
                idGroupTIU = "17973776";
            else if (categoryForTIU == "Лодочные моторы PATRIOT")
                idGroupTIU = "17973777";
            else if (categoryForTIU == "Лодочные моторы ООО «МТР Северо-Запад»")
                idGroupTIU = "17973778";
            else if (categoryForTIU == "Лодочные моторы MIKATSU")
                idGroupTIU = "17973779";
            else if (categoryForTIU == "Прицепы для катеров и лодок")
                idGroupTIU = "17971678";
            else if (categoryForTIU == "Катера")
                idGroupTIU = "17971679";

            else if (categoryForTIU == "Мотобуксировщики Мухтар (IRBIS)")
                idGroupTIU = "17245683";
            else if (categoryForTIU == "Мотобуксировщики Буксир и Север (Ижевск)")
                idGroupTIU = "17245684";
            else if (categoryForTIU == "Мотобуксировщики Север СК")
                idGroupTIU = "17245685";
            else if (categoryForTIU == "Мотобуксировщики Лидер")
                idGroupTIU = "17245686";
            else if (categoryForTIU == "Мотобуксировщики УРАГАН")
                idGroupTIU = "17245687";
            else if (categoryForTIU == "Мотобуксировщики Тофалар")
                idGroupTIU = "17245688";
            else if (categoryForTIU == "Мотобуксировщики БТС")
                idGroupTIU = "17245689";
            else if (categoryForTIU == "Мотобуксировщики Мужик")
                idGroupTIU = "17245690";
            else if (categoryForTIU == "Мотобуксировщики БАРС")
                idGroupTIU = "17245691";
            else if (categoryForTIU == "Мотобуксировщики Бурлак")
                idGroupTIU = "17245692";
            else if (categoryForTIU == "Мотобуксировщики Ahtar (Ахтар)")
                idGroupTIU = "17245693";
            else if (categoryForTIU == "Мотобуксировщики и мотособаки KOiRA")
                idGroupTIU = "17245694";
            else if (categoryForTIU == "Мотобуксировщик ЁРШ")
                idGroupTIU = "17245695";
            else if (categoryForTIU == "Мотобуксировщики Рекс")
                idGroupTIU = "17245696";
            else if (categoryForTIU == "Мотобуксировщики Помор")
                idGroupTIU = "17245697";
            else if (categoryForTIU == "Мотобуксировщики Норка")
                idGroupTIU = "17245698";
            else if (categoryForTIU == "Мотобуксировщики Альбатрос")
                idGroupTIU = "17245699";
            else if (categoryForTIU == "Мотобуксировщики Пахус (Paxus)")
                idGroupTIU = "17245700";
            else if (categoryForTIU == "Снегоходы и мотобуксировщики АВМ")
                idGroupTIU = "17245701";
            else if (categoryForTIU == "Мотобуксировщики и снегоболотоход завод им. В.А. Дегтерева")
                idGroupTIU = "17245702";
            else if (categoryForTIU == "Мотобуксировщики Rayda")
                idGroupTIU = "17245703";
            else if (categoryForTIU == "Мотобуксировщики ОНЕГО")
                idGroupTIU = "17245704";
            else if (categoryForTIU == "Мотобуксировщики BALTMOTORS")
                idGroupTIU = "17245705";
            else if (categoryForTIU == "Мотобуксировщики Волкодав")
                idGroupTIU = "17245706";
            else if (categoryForTIU == "Мотобуксировщики Xtreme-Motors")
                idGroupTIU = "17245707";
            else if (categoryForTIU == "Мотобуксировщики S2 Motors")
                idGroupTIU = "17245708";
            else if (categoryForTIU == "Мотобуксировщики FANTEK")
                idGroupTIU = "17245709";
            else if (categoryForTIU == "Всесезонные плавающие мотобуксировщики \"БК\" от РОСТИН")
                idGroupTIU = "17245710";
            else if (categoryForTIU == "Мотобуксировщики FORZA")
                idGroupTIU = "17279387";
            else if (categoryForTIU == "Мотобуксировщики Разгуляй")
                idGroupTIU = "17975381";
            else if (categoryForTIU == "Мотобуксировщики ЮКОН")
                idGroupTIU = "17975382";

            else if (categoryForTIU == "Снегоходы новые (Китай, Россия)")
                idGroupTIU = "17980517";
            else if (categoryForTIU == "Снегоходы и аксессуары ALPINA SHERPA")
                idGroupTIU = "17980518";
            else if (categoryForTIU == "Снегоходы Stels")
                idGroupTIU = "17980519";

            else if (categoryForTIU == "Снегоуборщики")
                idGroupTIU = "17245598";

            else if (categoryForTIU == "Электротранспорт")
                idGroupTIU = "17279389";

            else
            {

            }


            return idGroupTIU;
        }

        private string UploadImagesTIU(string articl, string token)
        {
            string imgId = "";
            for (int i = 0; 9 > i; i++)
            {
                Thread.Sleep(5000);
                if (File.Exists("pic\\" + articl + "_" + i + ".jpg"))
                {
                    string nameImage = "pic\\" + articl + "_" + i + ".jpg";
                    string[] template = File.ReadAllLines("TemplateLoadImageStart.txt");
                    template[3] = token;
                    template[84] = "Content-Disposition: form-data; name=\"images[]\"; filename=\"" + nameImage + "\"";
                    File.WriteAllLines("TemplateLoadImageStart.txt", template);
                    otv = nethouse.PostRequestaddTovarTIUImage("https://my.tiu.ru/media/upload_image?profile=0", cookie, token, articl + ".jpg");
                    string id = new Regex("(?<=\"id\": ).*(?=, \"size)").Match(otv).ToString();
                    imgId = imgId + "&images=" + id;
                }
            }
            /*
            string nameImage = "pic\\" + articl + ".jpg";
            string[] template = File.ReadAllLines("TemplateLoadImageStart.txt");
            template[3] = token;
            template[84] = "Content-Disposition: form-data; name=\"images[]\"; filename=\"" + nameImage + "\"";
            File.WriteAllLines("TemplateLoadImageStart.txt", template);
            if (File.Exists("pic\\" + articl + ".jpg"))
                otv = nethouse.PostRequestaddTovarTIUImage("https://my.tiu.ru/media/upload_image?profile=0", cookie, token, articl + ".jpg");*/
            return imgId;
        }

        private void SaveAllImages(string str, string articl)
        {
            string[] images = str.Split(';');
            int i = 0;
            foreach (string ss in images)
            {
                Thread.Sleep(4000);
                string url = ss;
                if (url == "")
                    continue;
                url = url.Replace("\\/", "/");

                url = url.Replace("//", "");
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.81 Safari/537.36");
                try
                {
                    if (url.Contains("i.siteapi.org"))
                        webClient.DownloadFile("http:/" + url, "pic\\" + articl + "_" + i + ".jpg");
                    else
                        webClient.DownloadFile("https://bike18.ru" + url, "pic\\" + articl + "_" + i + ".jpg");
                }
                catch
                {
                    i--;
                }
                i++;
            }
        }

        private string replaceName(string name)
        {

            name = name.Replace("(", "").Replace(")", "").Replace("&", "");
            return name;
        }

        private void addGroupInTIU(string strNameGlobalCategory, CookieContainer cookie, string token)
        {
            int sleep = 10000;
            Thread.Sleep(sleep);
            bool b = false;
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/group_create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0");
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
                string inquiry = "csrf_token=" + token + "&user_id=2269119&group-next=https://my.tiu.ru/cabinet/product2/index/5187992?status=0&group-parent_group=13815355&group-name=1 " + strNameGlobalCategory + "&group-selling_type=1&group-description=&group-group_seo_text=&group-managers=66604&group-use_default_seo_settings=1&group-submit_button=Сохранить группу&save_=1";
                otv = nethouse.PostRequest("https://my.tiu.ru/cabinet/product2/group_create", cookie, token, inquiry);

            }
            sleep++;
        }

        private void addPodGroupInTIU(string strNameGlobalCategory, CookieContainer cookie, string token, string strUpCategory)
        {
            Thread.Sleep(5000);
            bool b = false;
            string codeGroupTIU = null;
            string subsection = null;
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/group_create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0");
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
                otv = nethouse.PostRequest("https://my.tiu.ru/cabinet/product2/group_create", cookie, token, inquiry);

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

        public CookieContainer Authorizacion(string login, string pass)
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
            string toke = res.Cookies[2].Value;

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
            byte[] ms = Encoding.ASCII.GetBytes("phone_email=" + login + "&password=" + pass + "&csrf_token=" + toke + "&_save=YES&");
            req.ContentLength = ms.Length;
            Stream stre = req.GetRequestStream();
            stre.Write(ms, 0, ms.Length);
            stre.Close();
            res = (HttpWebResponse)req.GetResponse();

            return cookie;
        }

        public string tokenReturn(CookieContainer cookie)
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

        public string strQueryReturn(string token, string nameTovar, string price, string articl, string miniText, string fullText, string idGroupTIU, string imgId, string idCategory)
        {
            string str = "csrf_token=" + token + "&user_id=2269119&next=%2Fcabinet%2Fproduct2%2Froot_group&model_id=&model_binding_source=&category=" + idCategory + "&name=" + nameTovar + "&group=" + idGroupTIU + "&selling_type=1&price=" + price + "&price_currency=4003&measure_unit=1000&presence=avail&sku=" + articl + "&images=" + imgId + "&description=" + miniText + fullText + "&keywords=" + nameTovar + "&keywords=" + nameTovar + "&attributes_present=1&attributes_custom_present=1&attribute_18=&attribute_228=&attribute_11217=&attributes_custom-0-attribute_name=&attributes_custom-0-attribute_value=&attributes_custom-0-attribute_id=&attributes_custom-0-attribute_exists=y&attributes_custom-1-attribute_name=&attributes_custom-1-attribute_value=&attributes_custom-1-attribute_id=&attributes_custom-1-attribute_exists=y&attributes_custom-2-attribute_name=&attributes_custom-2-attribute_value=&attributes_custom-2-attribute_id=&attributes_custom-2-attribute_exists=y&attributes_custom-3-attribute_name=&attributes_custom-3-attribute_value=&attributes_custom-3-attribute_id=&attributes_custom-3-attribute_exists=y&attributes_custom-4-attribute_name=&attributes_custom-4-attribute_value=&attributes_custom-4-attribute_id=&attributes_custom-4-attribute_exists=y&attributes_custom-5-attribute_name=&attributes_custom-5-attribute_value=&attributes_custom-5-attribute_id=&attributes_custom-5-attribute_exists=y&attributes_custom-6-attribute_name=&attributes_custom-6-attribute_value=&attributes_custom-6-attribute_id=&attributes_custom-6-attribute_exists=y&attributes_custom-7-attribute_name=&attributes_custom-7-attribute_value=&attributes_custom-7-attribute_id=&attributes_custom-7-attribute_exists=y&attributes_custom-8-attribute_name=&attributes_custom-8-attribute_value=&attributes_custom-8-attribute_id=&attributes_custom-8-attribute_exists=y&attributes_custom-9-attribute_name=&attributes_custom-9-attribute_value=&attributes_custom-9-attribute_id=&attributes_custom-9-attribute_exists=y&attributes_custom-10-attribute_name=&attributes_custom-10-attribute_value=&attributes_custom-10-attribute_id=&attributes_custom-10-attribute_exists=y&attributes_custom-11-attribute_name=&attributes_custom-11-attribute_value=&attributes_custom-11-attribute_id=&attributes_custom-11-attribute_exists=y&attributes_custom-12-attribute_name=&attributes_custom-12-attribute_value=&attributes_custom-12-attribute_id=&attributes_custom-12-attribute_exists=y&attributes_custom-13-attribute_name=&attributes_custom-13-attribute_value=&attributes_custom-13-attribute_id=&attributes_custom-13-attribute_exists=y&attributes_custom-14-attribute_name=&attributes_custom-14-attribute_value=&attributes_custom-14-attribute_id=&attributes_custom-14-attribute_exists=y&attributes_custom-15-attribute_name=&attributes_custom-15-attribute_value=&attributes_custom-15-attribute_id=&attributes_custom-15-attribute_exists=y&attributes_custom-16-attribute_name=&attributes_custom-16-attribute_value=&attributes_custom-16-attribute_id=&attributes_custom-16-attribute_exists=y&attributes_custom-17-attribute_name=&attributes_custom-17-attribute_value=&attributes_custom-17-attribute_id=&attributes_custom-17-attribute_exists=y&attributes_custom-18-attribute_name=&attributes_custom-18-attribute_value=&attributes_custom-18-attribute_id=&attributes_custom-18-attribute_exists=y&attributes_custom-19-attribute_name=&attributes_custom-19-attribute_value=&attributes_custom-19-attribute_id=&attributes_custom-19-attribute_exists=y&attributes_custom-20-attribute_name=&attributes_custom-20-attribute_value=&attributes_custom-20-attribute_id=&attributes_custom-20-attribute_exists=y&attributes_custom-21-attribute_name=&attributes_custom-21-attribute_value=&attributes_custom-21-attribute_id=&attributes_custom-21-attribute_exists=y&attributes_custom-22-attribute_name=&attributes_custom-22-attribute_value=&attributes_custom-22-attribute_id=&attributes_custom-22-attribute_exists=y&attributes_custom-23-attribute_name=&attributes_custom-23-attribute_value=&attributes_custom-23-attribute_id=&attributes_custom-23-attribute_exists=y&attributes_custom-24-attribute_name=&attributes_custom-24-attribute_value=&attributes_custom-24-attribute_id=&attributes_custom-24-attribute_exists=y&attributes_custom-25-attribute_name=&attributes_custom-25-attribute_value=&attributes_custom-25-attribute_id=&attributes_custom-25-attribute_exists=y&attributes_custom-26-attribute_name=&attributes_custom-26-attribute_value=&attributes_custom-26-attribute_id=&attributes_custom-26-attribute_exists=y&attributes_custom-27-attribute_name=&attributes_custom-27-attribute_value=&attributes_custom-27-attribute_id=&attributes_custom-27-attribute_exists=y&attributes_custom-28-attribute_name=&attributes_custom-28-attribute_value=&attributes_custom-28-attribute_id=&attributes_custom-28-attribute_exists=y&attributes_custom-29-attribute_name=&attributes_custom-29-attribute_value=&attributes_custom-29-attribute_id=&attributes_custom-29-attribute_exists=y&attributes_custom-30-attribute_name=&attributes_custom-30-attribute_value=&attributes_custom-30-attribute_id=&attributes_custom-30-attribute_exists=y&attributes_custom-31-attribute_name=&attributes_custom-31-attribute_value=&attributes_custom-31-attribute_id=&attributes_custom-31-attribute_exists=y&attributes_custom-32-attribute_name=&attributes_custom-32-attribute_value=&attributes_custom-32-attribute_id=&attributes_custom-32-attribute_exists=y&attributes_custom-33-attribute_name=&attributes_custom-33-attribute_value=&attributes_custom-33-attribute_id=&attributes_custom-33-attribute_exists=y&attributes_custom-34-attribute_name=&attributes_custom-34-attribute_value=&attributes_custom-34-attribute_id=&attributes_custom-34-attribute_exists=y&attributes_custom-35-attribute_name=&attributes_custom-35-attribute_value=&attributes_custom-35-attribute_id=&attributes_custom-35-attribute_exists=y&attributes_custom-36-attribute_name=&attributes_custom-36-attribute_value=&attributes_custom-36-attribute_id=&attributes_custom-36-attribute_exists=y&attributes_custom-37-attribute_name=&attributes_custom-37-attribute_value=&attributes_custom-37-attribute_id=&attributes_custom-37-attribute_exists=y&attributes_custom-38-attribute_name=&attributes_custom-38-attribute_value=&attributes_custom-38-attribute_id=&attributes_custom-38-attribute_exists=y&attributes_custom-39-attribute_name=&attributes_custom-39-attribute_value=&attributes_custom-39-attribute_id=&attributes_custom-39-attribute_exists=y&attributes_custom-40-attribute_name=&attributes_custom-40-attribute_value=&attributes_custom-40-attribute_id=&attributes_custom-40-attribute_exists=y&attributes_custom-41-attribute_name=&attributes_custom-41-attribute_value=&attributes_custom-41-attribute_id=&attributes_custom-41-attribute_exists=y&attributes_custom-42-attribute_name=&attributes_custom-42-attribute_value=&attributes_custom-42-attribute_id=&attributes_custom-42-attribute_exists=y&attributes_custom-43-attribute_name=&attributes_custom-43-attribute_value=&attributes_custom-43-attribute_id=&attributes_custom-43-attribute_exists=y&attributes_custom-44-attribute_name=&attributes_custom-44-attribute_value=&attributes_custom-44-attribute_id=&attributes_custom-44-attribute_exists=y&attributes_custom-45-attribute_name=&attributes_custom-45-attribute_value=&attributes_custom-45-attribute_id=&attributes_custom-45-attribute_exists=y&attributes_custom-46-attribute_name=&attributes_custom-46-attribute_value=&attributes_custom-46-attribute_id=&attributes_custom-46-attribute_exists=y&attributes_custom-47-attribute_name=&attributes_custom-47-attribute_value=&attributes_custom-47-attribute_id=&attributes_custom-47-attribute_exists=y&attributes_custom-48-attribute_name=&attributes_custom-48-attribute_value=&attributes_custom-48-attribute_id=&attributes_custom-48-attribute_exists=y&attributes_custom-49-attribute_name=&attributes_custom-49-attribute_value=&attributes_custom-49-attribute_id=&attributes_custom-49-attribute_exists=y&attributes_custom-50-attribute_name=&attributes_custom-50-attribute_value=&attributes_custom-50-attribute_id=&attributes_custom-50-attribute_exists=y&attributes_custom-51-attribute_name=&attributes_custom-51-attribute_value=&attributes_custom-51-attribute_id=&attributes_custom-51-attribute_exists=y&attributes_custom-52-attribute_name=&attributes_custom-52-attribute_value=&attributes_custom-52-attribute_id=&attributes_custom-52-attribute_exists=y&attributes_custom-53-attribute_name=&attributes_custom-53-attribute_value=&attributes_custom-53-attribute_id=&attributes_custom-53-attribute_exists=y&attributes_custom-54-attribute_name=&attributes_custom-54-attribute_value=&attributes_custom-54-attribute_id=&attributes_custom-54-attribute_exists=y&attributes_custom-55-attribute_name=&attributes_custom-55-attribute_value=&attributes_custom-55-attribute_id=&attributes_custom-55-attribute_exists=y&attributes_custom-56-attribute_name=&attributes_custom-56-attribute_value=&attributes_custom-56-attribute_id=&attributes_custom-56-attribute_exists=y&attributes_custom-57-attribute_name=&attributes_custom-57-attribute_value=&attributes_custom-57-attribute_id=&attributes_custom-57-attribute_exists=y&attributes_custom-58-attribute_name=&attributes_custom-58-attribute_value=&attributes_custom-58-attribute_id=&attributes_custom-58-attribute_exists=y&attributes_custom-59-attribute_name=&attributes_custom-59-attribute_value=&attributes_custom-59-attribute_id=&attributes_custom-59-attribute_exists=y&attributes_custom-60-attribute_name=&attributes_custom-60-attribute_value=&attributes_custom-60-attribute_id=&attributes_custom-60-attribute_exists=y&attributes_custom-61-attribute_name=&attributes_custom-61-attribute_value=&attributes_custom-61-attribute_id=&attributes_custom-61-attribute_exists=y&attributes_custom-62-attribute_name=&attributes_custom-62-attribute_value=&attributes_custom-62-attribute_id=&attributes_custom-62-attribute_exists=y&attributes_custom-63-attribute_name=&attributes_custom-63-attribute_value=&attributes_custom-63-attribute_id=&attributes_custom-63-attribute_exists=y&attributes_custom-64-attribute_name=&attributes_custom-64-attribute_value=&attributes_custom-64-attribute_id=&attributes_custom-64-attribute_exists=y&attributes_custom-65-attribute_name=&attributes_custom-65-attribute_value=&attributes_custom-65-attribute_id=&attributes_custom-65-attribute_exists=y&attributes_custom-66-attribute_name=&attributes_custom-66-attribute_value=&attributes_custom-66-attribute_id=&attributes_custom-66-attribute_exists=y&attributes_custom-67-attribute_name=&attributes_custom-67-attribute_value=&attributes_custom-67-attribute_id=&attributes_custom-67-attribute_exists=y&attributes_custom-68-attribute_name=&attributes_custom-68-attribute_value=&attributes_custom-68-attribute_id=&attributes_custom-68-attribute_exists=y&attributes_custom-69-attribute_name=&attributes_custom-69-attribute_value=&attributes_custom-69-attribute_id=&attributes_custom-69-attribute_exists=y&use_default_seo_settings=1&packaging=&ajax_create=true&status=0&redirect_to_list=true";
            return str;
        }

        public string Replace(string str)
        {
            str = str.Replace(";", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("MOTOLAND", "MotoLand").Replace("MotoLand", "MotoLand").Replace("S1", "s1").Replace("&", "");
            return str;
        }

        private void btnBRPToTIU_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.loginTIU = tbLoginTIU.Text;
            Properties.Settings.Default.passTIU = tbPassTIU.Text;
            Properties.Settings.Default.loginNethause = tbLoginNethause.Text;
            Properties.Settings.Default.passNethause = tbPassNethause.Text;
            Properties.Settings.Default.Save();

            if (tbLoginNethause.Text == "" || tbLoginTIU.Text == "" || tbPassNethause.Text == "" || tbPassTIU.Text == "")
            {
                MessageBox.Show("Логин или пароль не введен");
                return;
            }
            cookie = Authorizacion(tbLoginTIU.Text, tbPassTIU.Text);
            cookieBike = nethouse.CookieNethouse(tbLoginNethause.Text, tbPassNethause.Text);
            if (cookie.Count != 9 || cookieBike.Count != 4)
            {
                MessageBox.Show("Логин или пароль не верные");
                return;
            }

            string token = tokenReturn(cookie);
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/create?next=%2Fcabinet%2Fproduct2%2Froot_group");

            MatchCollection groups = new Regex("(?<=<option value=\")[\\w\\W]*?(?=</option>)").Matches(otv);

            otv = webRequest.getRequest("http://bike18.ru/");
            otv = webRequest.getRequest("https://bike18.ru/products/category/2266212/page/all");
            MatchCollection globalCategory = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
            MatchCollection nameGlobalCategory = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div>)").Matches(otv);

            for (int i = 0; globalCategory.Count > i; i++)
            {
                Thread.Sleep(10000);
                string categoryForTIU = "1 " + nameGlobalCategory[i].ToString();
                string urlBRP = "https://bike18.ru" + globalCategory[i].ToString() + "?page=all";
                otv = webRequest.getRequest(urlBRP);
                MatchCollection podCategoryURL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                MatchCollection podCategoryName = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                MatchCollection tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                if (tovars.Count != 0)
                {
                    for (int z = 0; tovars.Count > z; z++)
                    {
                        Thread.Sleep(8000);
                        CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                    }
                }
                else
                {
                    for (int l = 0; podCategoryURL.Count > l; l++)
                    {
                        categoryForTIU = "1 " + podCategoryName[l].ToString();
                        otv = webRequest.getRequest("https://bike18.ru" + podCategoryURL[l].ToString() + "?page=all");
                        MatchCollection category2Name = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                        MatchCollection category2URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                        tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                        if (tovars.Count != 0)
                        {
                            for (int z = 0; tovars.Count > z; z++)
                            {
                                Thread.Sleep(8000);
                                CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                            }
                        }
                        else
                        {
                            for (int t = 2; category2URL.Count > t; t++)
                            {
                                categoryForTIU = "1 " + category2Name[t].ToString();
                                otv = webRequest.getRequest(category2URL[t].ToString() + "/page/all");
                                MatchCollection category3URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                MatchCollection category3Name = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                                tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                                if (tovars.Count != 0)
                                {

                                    for (int z = 0; tovars.Count > z; z++)
                                    {
                                        Thread.Sleep(8000);
                                        CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                                    }
                                }
                                else
                                {
                                    for (int r = 0; category3URL.Count > r; r++)
                                    {
                                        categoryForTIU = "1 " + category3Name[r].ToString();
                                        otv = webRequest.getRequest(category3URL[r].ToString() + "/page/all");
                                        MatchCollection category4URL = new Regex("(?<=<div class=\"category-capt-txt -text-center\"><a href=\").*?(?=\" class=\"blue\">)").Matches(otv);
                                        MatchCollection category4Name = new Regex("(?<=\" class=\"blue\">).*?(?=</a></div></div></div>)").Matches(otv);
                                        tovars = new Regex("(?<=<div class=\"product-link -text-center\"><a href=\").*?(?=\" >)").Matches(otv);
                                        if (tovars.Count != 0)
                                        {

                                            for (int z = 0; tovars.Count > z; z++)
                                            {
                                                Thread.Sleep(8000);
                                                CreateTovar(tovars[z].ToString(), token, cookie, groups, categoryForTIU, i);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }










            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/index/5187992?status=0");
            otv = webRequest.getRequest(cookie, "https://my.tiu.ru/cabinet/product2/create?parent_group=5187992&group=5187992&next=https%3A%2F%2Fmy.tiu.ru%2Fcabinet%2Fproduct2%2Findex%2F5187992%3Fstatus%3D0");
            string nameTovar = "Тестовый товар666  попытка";
            string groupTovar = "13729006";
            string priceTovar = "777";
            string articlTovar = "А12366321";
            string DescriptionAttributeTovar = "Тестовый товар описание";
            string keywords = "Ключевые словики";

            //string stringloadImg = loadImg(token, name);





        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbLoginNethause.Text = Properties.Settings.Default.loginNethause;
            tbPassNethause.Text = Properties.Settings.Default.passNethause;
            tbLoginTIU.Text = Properties.Settings.Default.loginTIU;
            tbPassTIU.Text = Properties.Settings.Default.passTIU;
        }
    }
}
