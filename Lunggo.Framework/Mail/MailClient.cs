namespace Lunggo.Framework.Mail
{
    internal abstract class MailClient
    {
        internal abstract void Init(string apiKey);
        internal abstract void SendEmail<T>(T objectParam, MailModel mailModel, string type);
    }
}
