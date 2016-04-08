using System;
using System.Reflection;

namespace Lunggo.CloudApp.CaptchaReader.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}