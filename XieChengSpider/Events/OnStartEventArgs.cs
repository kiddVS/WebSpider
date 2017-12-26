using System;
using System.Collections.Generic;
using System.Text;

namespace XieChengSpider.Events
{
   public class OnStartEventArgs
    {
        public Uri Uri { get; set; }
        public OnStartEventArgs(Uri uri)
        {
            this.Uri = uri;
        }
    }
}
