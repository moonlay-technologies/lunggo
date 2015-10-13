using Lunggo.Framework.Database;

namespace Lunggo.WebAPI.ApiSrc.v1.Newsletter.Query
{
    public class CheckEmailQuery : QueryBase<CheckEmailQuery, int>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT COUNT(1) FROM NewsletterSubscriber WHERE Email = @Email";
        }
    }
}