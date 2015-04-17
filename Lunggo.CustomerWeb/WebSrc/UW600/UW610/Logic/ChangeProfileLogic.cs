using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Lunggo.CustomerWeb.WebSrc.UW600.UW610.Model;
using Lunggo.CustomerWeb.WebSrc.UW600.UW610.Object;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Lunggo.Framework.Database;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW610.Logic
{
    public class ChangeProfileLogic 
    {
        public static Uw610ChangePofile SendPofile(Uw610ChangeProfileRespone request)
        {
            var response = GetMemberDetail(request.IdMember);

            return response;
        }

        public static Uw610ChangePofile GetMemberDetail(String idMember)
        {
            var memberTable = MemberTableRepo.GetInstance();
            var connOpen = DbService.GetInstance().GetOpenConnection();

            var memberDetail = memberTable.FindAll(connOpen).Single(x => x.IdMember == idMember);

            var respone = new Uw610ChangePofile()
            {
                IdMember = memberDetail.IdMember,
                Address = memberDetail.Address,
                Country = memberDetail.Country,
                Email = memberDetail.Email,
                Name = memberDetail.Name,
                Password = memberDetail.Password,
                BirthPlace = memberDetail.BirthPlace,
                BornDate = (DateTime) memberDetail.BornDate,
                PhoneNumber = memberDetail.PhoneNumber
            };

            return respone;
        }

    }
}
