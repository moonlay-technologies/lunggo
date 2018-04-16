namespace Lunggo.Framework.Mail
{
    internal abstract class MailClient
    {
        internal abstract void Init(string apiKey);
        internal abstract void SendEmailWithTableTemplate<T>(T objectParam, MailModel mailModel, string type);
        //internal abstract void SendPlainEmail(MailModel mailModel, string content);
    }
}
