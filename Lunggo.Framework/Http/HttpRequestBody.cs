using System;
using System.Text;

namespace Lunggo.Framework.Http
{
    public class HttpRequestBody
    {
        public static readonly Encoding _defaultEncoding = Encoding.UTF8;
        private byte[] _body;
        
        public Encoding BodyEncoding
        {
            get;
            private set;
        }
        public int ContentLength
        {
            get
            {
                return _body.Length;   
            }
        }
        public String ContentType 
        { 
            get; 
            private set; 
        }
        public HttpRequestBody()
        {
            BodyEncoding = _defaultEncoding;
        }

        public HttpRequestBody(Encoding newEncoding)
        {
            BodyEncoding = newEncoding;
        }
        public void SetBody(String body, String contentType)
        {
            SetBody(ConvertBodyToBytes(body), contentType);
        }
        public void SetBody(byte[] body, String contentType)
        {
            _body = body;
            ContentType = contentType;
        }
        public byte[] GetBody()
        {
            return _body;
        }
        private byte[] ConvertBodyToBytes(String body)
        {
            return BodyEncoding.GetBytes(body);
        }
    }
}
