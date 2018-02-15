﻿using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Log;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lunggo.WebAPI.App_Start
{
    public class FilterConfig
    {
        public class FunctionFilter : ActionFilterAttribute
        {
            public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
            {
                var actionFilterTable = new ActionFilterTableStorage();
                var parameterStringBuilder = new StringBuilder();
                actionFilterTable.PartitionKey = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
                actionFilterTable.Action = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                actionFilterTable.Method = actionExecutedContext.ActionContext.Request.Method.Method;
                actionFilterTable.Url = actionExecutedContext.Request.RequestUri.AbsoluteUri;
                actionFilterTable.Parameter = actionExecutedContext.ActionContext.ActionArguments.Serialize();
                actionFilterTable.User = HttpContext.Current.User.Identity.GetId();
                
                var clientId = HttpContext.Current.Request.Headers["X-Client-ID"];
                actionFilterTable.ClientID = clientId;
                HttpContext.Current.Request.RequestContext.RouteData.Values.TryGetValue("body", out object body);
                actionFilterTable.Body = (string)body;
                
                
                string platformType = null;
                string platformVersion = null;

                if (!string.IsNullOrEmpty(clientId))
                {
                    var platform = Client.GetPlatform(clientId);
                    platformType = platform.type.ToString();
                    var version = platform.version;
                }
                else
                {
                    var mobileUrl = ConfigManager.GetInstance().GetConfigValue("general", "mobileUrl");
                    if (HttpContext.Current.Request.Url.Host == mobileUrl)
                    {
                        platformType = PlatformType.MobileWebsite.ToString();
                    }
                    else
                    {
                        platformType = PlatformType.DesktopWebsite.ToString();
                    }
                }
                actionFilterTable.Platform = platformType;
                actionFilterTable.Version = platformVersion;
                var response = actionExecutedContext.Response;
                if (response != null)
                {
                    actionFilterTable.Response = response.Content.ReadAsStringAsync().Result;
                }
                actionFilterTable.InsertActionFilter();
            }
        }

        public class ExceptionFilter : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext actionExecutedContext)
            {
                var log = "Exception : " + actionExecutedContext.Exception.Message
                    + "\nStackTrace : \n" + actionExecutedContext.Exception.StackTrace;
                var TableLog = new GlobalLog();
                TableLog.PartitionKey = "GLOBAL UNHANDLED EXCEPTION LOG";
                TableLog.Log = log;
                TableLog.Logging();
                
            }
        }
    }

    public class ActionFilterTableStorage : TableEntity
    {
        public string Action { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string Parameter { get; set; }
        public string Body { get; set; }
        public string Platform { get; set; }
        public string ClientID { get; set; }
        public string Version { get; set; }
        public string User { get; set; }
        public string Response { get; set; }

        public void InsertActionFilter()
        {
            RowKey = Guid.NewGuid().ToString();
            TableStorageService.GetInstance().InsertEntityToTableStorage(this, "ApiAnalyticsHit");
            
        }
    }
}