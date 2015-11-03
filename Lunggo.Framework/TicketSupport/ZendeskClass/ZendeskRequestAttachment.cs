using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{
    public interface IAttachments : ICore
    {
        ZendeskUpload UploadAttachment(FileInfo file);
        ZendeskUpload UploadAttachments(IEnumerable<FileInfo> files);

        Task<ZendeskUpload> UploadAttachmentAsync(FileInfo file);
        Task<ZendeskUpload> UploadAttachmentsAsync(IEnumerable<FileInfo> files);

		/// <summary>
		/// Uploads a file to zendesk and returns the corresponding token id.
		/// To upload another file to an existing token just pass in the existing token.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="token"></param>
		/// <returns></returns>  
        Task<ZendeskUpload> UploadAttachmentAsync(FileInfo file, string token = "");
    }

    public class Attachments : ZendeskCore, IAttachments
    {
        public Attachments(string yourZendeskUrl, string user, string password, string apiToken)
            : base(yourZendeskUrl, user, password, apiToken)
        { }
        public ZendeskUpload UploadAttachment(FileInfo file)
        {
            return UploadAttachment(file, "");
        }

        public ZendeskUpload UploadAttachments(IEnumerable<FileInfo> files)
        {
            if (!files.Any())
                return null;

            var res = UploadAttachment(files.First());

            if (files.Count() > 1)
            {
                var otherFiles = files.Skip(1);
                foreach (var curFile in otherFiles)
                    res = UploadAttachment(curFile, res.Token);
            }

            return res;
        }

        /// <summary>
        /// Uploads a file to zendesk and returns the corresponding token id.
        /// To upload another file to an existing token just pass in the existing token.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="token"></param>
        /// <returns></returns>       
        ZendeskUpload UploadAttachment(FileInfo file, string token = "")
        {
            var requestUrl = ZendeskUrl;
            if (!requestUrl.EndsWith("/"))
                requestUrl += "/";

            requestUrl += string.Format("uploads.json?filename={0}", file.FileName);
            if (!string.IsNullOrEmpty(token))
                requestUrl += string.Format("&token={0}", token);

            WebRequest req = WebRequest.Create(requestUrl);
            req.ContentType = file.ContentType;
            req.Method = "POST";
            req.ContentLength = file.FileData.Length;
            req.Headers["Authorization"] = GetPasswordOrTokenAuthHeader();
            //var credentials = new System.Net.CredentialCache
            //                      {
            //                          {
            //                              new System.Uri(ZendeskUrl), "Basic",
            //                              new System.Net.NetworkCredential(User, Password)
            //                              }
            //                      };

            //req.Credentials = credentials;
            req.PreAuthenticate = true;
            //req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequired;
            var dataStream = req.GetRequestStream();
            dataStream.Write(file.FileData, 0, file.FileData.Length);
            dataStream.Close();

            WebResponse response = req.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            return responseFromServer.ConvertToObject<UploadResult>().Upload;
        }
        public async Task<ZendeskUpload> UploadAttachmentAsync(FileInfo file)
        {
            return await UploadAttachmentAsync(file, "");
        }

        public async Task<ZendeskUpload> UploadAttachmentsAsync(IEnumerable<FileInfo> files)
        {
            if (!files.Any())
                return null;

            var res = UploadAttachmentAsync(files.First());

            if (files.Count() > 1)
            {
                var otherFiles = files.Skip(1);
                foreach (var curFile in otherFiles)
                {
                    res = await res.ContinueWith(x =>  UploadAttachmentAsync(curFile, x.Result.Token));                    
                }
                    
            }

            return await res;
        }

        /// <summary>
        /// Uploads a file to zendesk and returns the corresponding token id.
        /// To upload another file to an existing token just pass in the existing token.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="token"></param>
        /// <returns></returns>  
        public async Task<ZendeskUpload> UploadAttachmentAsync(FileInfo file, string token = "")
        {
            var requestUrl = ZendeskUrl;
            if (!requestUrl.EndsWith("/"))
                requestUrl += "/";

            requestUrl += string.Format("uploads.json?filename={0}", file.FileName);
            if (!string.IsNullOrEmpty(token))
                requestUrl += string.Format("&token={0}", token);


            HttpWebRequest req = WebRequest.Create(requestUrl) as HttpWebRequest;
            req.ContentType = file.ContentType;
            //req.Credentials = new System.Net.NetworkCredential(User, Password);
            req.Headers["Authorization"] = GetPasswordOrTokenAuthHeader();
            req.Method = "POST"; //GET POST PUT DELETE

            req.Accept = "application/json, application/xml, text/json, text/x-json, text/javascript, text/xml";                                        
            
            var requestStream = Task.Factory.FromAsync(
                req.BeginGetRequestStream,
                asyncResult => req.EndGetRequestStream(asyncResult),
                (object)null);
            
            var dataStream = await requestStream.ContinueWith(t => t.Result.WriteAsync(file.FileData, 0, file.FileData.Length));
            Task.WaitAll(dataStream);
            

            Task<WebResponse> task = Task.Factory.FromAsync(
            req.BeginGetResponse,
            asyncResult => req.EndGetResponse(asyncResult),
            (object)null);

            return await task.ContinueWith(t =>
            {
                var httpWebResponse = t.Result as HttpWebResponse;
                return ReadStreamFromResponse(httpWebResponse).ConvertToObject<UploadResult>().Upload;
            });
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }


    }

    public static class RequestExtensions
    {

        public static T ConvertToObject<T>(this string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
    }


}
