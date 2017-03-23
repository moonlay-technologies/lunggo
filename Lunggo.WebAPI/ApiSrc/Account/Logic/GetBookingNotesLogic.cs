using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.Results;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Roles;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using ServiceStack.Text;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetBookingNotesApiResponse GetBookingNotesLogic()
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            var response = User.GetBookingNotes(userId);
            var notes = new List<BookingNotes>();
            if (response != null && response.Count > 0)
            {
                notes = ProcessResponse(response);
            }
            return new GetBookingNotesApiResponse
            { 
                StatusCode = HttpStatusCode.OK,
                BookingNotes = notes
            };
        }

        public static List<BookingNotes> ProcessResponse(List<UserBookingNotes> notes)
        {
            return notes.Select(note => new BookingNotes
            {
                UserId = note.UserId, Title = note.Title, Description = note.Description
            }).ToList();
        }
    }
}