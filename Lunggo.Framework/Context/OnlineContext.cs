using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.Framework.Constant;

namespace Lunggo.Framework.Context
{
    public static class OnlineContext
    {
        public static String GetActiveLanguageCode()
        {
            return (String) HttpContext.Current.Items[SystemConstant.HttpContextLangCode];
        }
    }
}
