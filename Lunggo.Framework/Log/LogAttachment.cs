using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Log
{
    public class LogAttachment
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public LogAttachment()
        {
            
        }

        public LogAttachment(string title, string text)
        {
            Title = title;
            Text = text;
        }
    }
}
