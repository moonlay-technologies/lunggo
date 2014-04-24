using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lunggo.Framework.Http
{
    public class HttpUtil
    {
        public static String CreateUrlWithParameter(String url, List<HttpParameter> parameterList)
        {
            var builder = new UriBuilder(url);
            
            if(parameterList == null || !parameterList.Any())
            {
                return builder.ToString();
            }
            
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            
            foreach(var parameter in parameterList)
            {
                query[parameter.Name] = parameter.Value;
            }
            
            builder.Query = query.ToString();
            return builder.ToString();               
        }
    }
}
