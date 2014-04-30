using Lunggo.Framework.SharedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public class MailModel
    {
        public List<string> ListRecepient { get; set; }
        public List<string> ListCC { get; set; }
        public List<string> ListBCC { get; set; }
        public List<FileInfo> ListFileInfo { get; set; }
        
    }
}
