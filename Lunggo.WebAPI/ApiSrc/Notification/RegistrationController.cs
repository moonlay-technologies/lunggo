using System;
using System.Threading.Tasks;
using System.Web.Http;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Notification.Logic;
using Lunggo.WebAPI.ApiSrc.Notification.Model;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Notification
{
    public class RegistrationController : ApiController
    {
        private readonly NotificationHubClient _hub;

        public RegistrationController()
        {
            _hub = Model.Notification.Instance.Hub;
        }

        // POST api/register
        // This creates a registration id
        [HttpPost]
        [Route("v1/notification/registration")]
        [Authorize]
        public async Task<ApiResponseBase> RegisterDevice()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<RegisterDeviceApiRequest>();
                var apiResponse = await RegistrationLogic.RegisterDevice(request, _hub);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        // PUT api/register/5
        // This creates or updates a registration (with provided channelURI) at the specified id
        [HttpPut]
        [Route("v1/notification/registration")]
        [Authorize]
        public async Task<ApiResponseBase> UpdateRegistration()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<UpdateRegistrationApiRequest>();
                var apiResponse = await RegistrationLogic.UpdateRegistration(request, _hub);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        // DELETE api/register/5
        [HttpDelete]
        [Route("v1/notification/registration")]
        [Authorize]
        public async Task<ApiResponseBase> DeleteRegistration()
        {
            try
            {
                var request = Request.Content.ReadAsStringAsync().Result.Deserialize<DeleteRegistrationApiRequest>();
                var apiResponse = await RegistrationLogic.DeleteRegistration(request, _hub);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e);
            }
        }

        
    }
}
