using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XieChengSpider.Events;

namespace XieChengSpider
{
    public class KiddSpider
    {
        public event EventHandler<OnStartEventArgs> OnStart;
        public event EventHandler<OnCompletedEventArgs> OnCompleted;
        public event EventHandler<Exception> OnError;
        public CookieContainer CookiesContainer { get; set; }
        public KiddSpider()
        {
        }
        public async Task<string> Start(Uri uri, WebProxy proxy = null)
        {
            
            return await Task.Run(() =>
            {
                var pageSource = String.Empty;
            try
            {
                if (this.OnStart != null) this.OnStart(this, new OnStartEventArgs(uri));
                    var watch = new Stopwatch();
                    watch.Start();
                  
                    var request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Accept = "*/*";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.AllowAutoRedirect = false;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                    request.Timeout = 5000;
                    request.Method = "GET";
                    if (proxy != null) request.Proxy = proxy;
                    request.CookieContainer = this.CookiesContainer;
                    request.ServicePoint.ConnectionLimit = int.MaxValue;
                    var response = (HttpWebResponse)request.GetResponse();
                    foreach (Cookie cookie in response.Cookies)
                    {
                        this.CookiesContainer.Add(cookie);
                    }
                    var stream = response.GetResponseStream();
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    pageSource = reader.ReadToEnd();
                    watch.Stop();
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    var milliseconds = watch.ElapsedMilliseconds;
                    reader.Close();
                    stream.Close();
                    request.Abort();
                    response.Close();
                    if (this.OnCompleted != null) OnCompleted(this, new OnCompletedEventArgs(uri, threadId, pageSource, milliseconds));
                }
                catch (Exception e)
                {
                    if (this.OnError != null) OnError(this, e);
                }

                return pageSource;
            });

        }
    }
}
