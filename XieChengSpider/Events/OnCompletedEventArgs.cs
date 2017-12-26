using System;
using System.Collections.Generic;
using System.Text;

namespace XieChengSpider.Events
{
    public class OnCompletedEventArgs
    {
        public Uri Uri { get; set; }
        public int  ThreadId { get; set; }
        public string  PageSource { get; set; }
        public long Milliseconds { get; set; }
        public OnCompletedEventArgs(Uri uri,int threadId,string pageSource,long milliseconds)
        {
            this.Uri = uri;
            this.ThreadId = threadId;
            this.PageSource = pageSource;
            this.Milliseconds = milliseconds;
        }
    }
}
