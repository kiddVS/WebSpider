using HtmlAgilityPack;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace XieChengSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BufferHeight = 1000;
            var cityUrl = "http://hotels.ctrip.com/citylist";
            var kiddSpider = new KiddCrawler();
            kiddSpider.OnStart += ((s, e) =>
            {
                Console.WriteLine("爬虫开始抓取地址：" + e.Uri.ToString());
            });
            kiddSpider.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出错：" + e.Message);
            };
            kiddSpider.OnCompleted += (s, e) =>
            {
                //Console.WriteLine(e.PageSource);
                //File.Create("E:/XieChen.txt");
                //using (StreamWriter sw = new StreamWriter(@"XieChen.txt"))
                //{
                //    sw.Write(e.PageSource);
                //    sw.Flush();
                //}
                //int count = HandleHtmlData4XieChengCity(e.PageSource);
                int count = HandleXieChengCityDatByReg(e.PageSource);
                Console.WriteLine("==================================");
                Console.WriteLine("爬虫抓取完毕！");
                Console.WriteLine("总记录条数：" + count);
                Console.WriteLine("耗时：" + e.Milliseconds + "秒");
                Console.WriteLine("线程：" + e.ThreadId);
                Console.WriteLine("地址：" + e.Uri.ToString());
            };
            kiddSpider.Start(new Uri(cityUrl), null);
            Console.Read();
        }
        public static int HandleHtmlData4XieChengCity(string html)
        {
            int count = 0;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection cityNodes = doc.DocumentNode.SelectNodes("//div[@class='custom_list']");
            foreach (var cityNode in cityNodes)
            {
                HtmlNodeCollection nodes = cityNode.SelectNodes("dd/a");
                foreach (var node in nodes)
                {
                    string cityName = node.InnerText;
                    string cityUrl = node.GetAttributeValue("href", "");
                    Console.WriteLine("\t城市名称：" + cityName + "          \t超链接地址：" + "http://hotels.ctrip.com" + cityUrl);
                    count++;
                }
            }
            return count;
            //HtmlNodeCollection itemNodes = doc.GetElementbyId("c_list_1").SelectNodes("div/dd/a");
            //foreach (var node in itemNodes)
            //{
            //    string cityName = node.InnerText;
            //    string cityUrl = node.GetAttributeValue("href", "");
            //    Console.WriteLine("城市名称："+cityName+"    超链接地址："+cityUrl);
            //}
        }
        public static int HandleXieChengCityDatByReg(string html)
        {
            int count = 0;
           var matches= Regex.Matches(html, @"<dd><a[^>]href=""(.*?)"" title=""(.*?)"">(.*?)</a></dd>");
            foreach (Match match in matches)
            {
                string nodeHtml = match.Value;
                string cityUrl = match.Groups[1].Value;
                string cityName = match.Groups[2].Value;
                Console.WriteLine("\t城市名称：" + cityName + "          \t超链接地址：" + "http://hotels.ctrip.com" + cityUrl);
                count++;
            }
            return count;
        }
    }
}
