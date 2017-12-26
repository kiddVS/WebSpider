using HttpCode.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XieChengSpider.Events;

namespace XieChengSpider
{
    public class KiddCrawler
    {
        public event EventHandler<OnStartEventArgs> OnStart;
        public event EventHandler<OnCompletedEventArgs> OnCompleted;
        public event EventHandler<Exception> OnError;
        public CookieContainer CookiesContainer { get; set; }
        public KiddCrawler()
        {
        }
        public async Task<string> Start(Uri uri, WebProxy proxy = null)
        {
            return await Task.Run(() =>
            {
                var pageSource = string.Empty;
                try
                {
                    if (this.OnStart != null) this.OnStart(this, new OnStartEventArgs(uri));
                    var watch = new Stopwatch();
                    watch.Start();
                    //=======发送http请求开始===========
                    HttpHelpers helpers = new HttpHelpers();
                    HttpItems items = new HttpItems();
                    items.Url = uri.ToString();
                    items.Accept = "*/*";
                    items.Allowautoredirect = false;
                    items.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                    items.Timeout = 5000;
                    items.Method = "GET";
                    //if (proxy != null) items.pr = proxy;TODO:代理服务器的逻辑
                    items.Container = this.CookiesContainer;
                    HttpResults result = helpers.GetHtml(items);
                    foreach (Cookie cookie in result.CookieCollection)
                    {
                        this.CookiesContainer.Add(cookie);
                    }
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    var milliseconds = watch.ElapsedMilliseconds;
                    pageSource = result.Html;
                    if (this.OnCompleted != null) OnCompleted(this, new OnCompletedEventArgs(uri, threadId, pageSource, milliseconds));                   
                }
                catch (Exception e)
                {
                    if (this.OnError != null)
                    {
                        OnError(this, e);
                    }
                }
                return pageSource;
            });
        }
    }
}
