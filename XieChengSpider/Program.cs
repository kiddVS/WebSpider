﻿using System;

namespace XieChengSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            var cityUrl = "http://hotels.ctrip.com/citylist";
            var kiddSpider = new KiddSpider();
            kiddSpider.OnStart += ((s, e) =>
            {
                Console.WriteLine("爬虫开始抓取地址：" + e.Uri.ToString());
            });
            kiddSpider.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出错！");
            };
            kiddSpider.OnCompleted += (s, e) =>
            {
                Console.WriteLine(e.PageSource);
                Console.WriteLine("==================================");
                Console.WriteLine("爬虫抓取完毕！");
                Console.WriteLine("耗时：" + e.Milliseconds + "秒");
                Console.WriteLine("线程：" + e.ThreadId);
                Console.WriteLine("地址：" + e.Uri.ToString());
            };
            kiddSpider.Start(new Uri(cityUrl), null);
            Console.Read();
        }
    }
}
