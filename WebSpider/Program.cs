using HtmlAgilityPack;
using HttpCode.Core;
using System;

namespace WebSpider
{
    class Program
    {
        static void Main(string[] args)
        {
           // int pageIndex = 1;
            int maxPageIndex = 10;
            HttpHelpers httpHelpers = new HttpHelpers();
            HttpItems httpItems = new HttpItems();
            httpItems.Url = "https://www.cnblogs.com/mvc/AggSite/PostList.aspx";
            for (int i = 0; i < maxPageIndex; i++)
            {
                httpItems.Method = "Post";
                httpItems.Postdata = "{ \"CategoryType\": \"SiteHome\", \"ParentCategoryId\": 0, \"CategoryId\": 808, \"PageIndex\":" +(i+1)+" \"TotalPostCount\": 4000,  \"ItemListActionName\": \"PostList\"}";
                HttpResults results = httpHelpers.GetHtml(httpItems);
                //Console.WriteLine(results.Html);
                //Console.Read();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(results.Html);
                HtmlNodeCollection articleNodes = doc.DocumentNode.SelectNodes("div[@class='post_item']/div[@class='post_item_body']");
                Console.WriteLine("************************第" + i+"页的信息************************");
                foreach (var articleNode in articleNodes)
                {
                    var nodeA = articleNode.SelectSingleNode("h3/a");
                    string title = nodeA.InnerText;
                    string articleUrl = nodeA.GetAttributeValue("href", "");
                    var nodeB = articleNode.SelectSingleNode("p[@class='post_item_summary']");
                    string briefIntroduce = nodeB.InnerText;
                    var authorNode = articleNode.SelectSingleNode("div[@class='post_item_foot']/a[@class='lightblue']");
                    string authorName = authorNode.InnerText;
                    Console.WriteLine("--------------------------------------------------------------------------");
                    Console.WriteLine("标题：" + title);
                    Console.WriteLine("作者：" + authorName);
                    Console.WriteLine("地址：" + articleUrl);
                    Console.WriteLine("摘要" + briefIntroduce);
                    Console.WriteLine("--------------------------------------------------------------------------");
                }            
            }
            Console.Read();
        }
    }
}
