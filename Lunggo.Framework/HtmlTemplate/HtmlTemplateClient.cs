namespace Lunggo.Framework.HtmlTemplate
{
    internal abstract class HtmlTemplateClient
    {
        internal abstract void Init();
        internal abstract string GenerateTemplate<T>(T objectParam, string type);
    }
}
