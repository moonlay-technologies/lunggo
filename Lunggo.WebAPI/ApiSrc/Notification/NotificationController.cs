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
    public class NotificationController : ApiController
    {
        // POST api/register
        // This creates a registration id
        [HttpPost]
        [Route("v1/notification/registration")]
        [Authorize]
        public async Task<ApiResponseBase> RegisterDevice()
        {
            RegisterDeviceApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<RegisterDeviceApiRequest>();
                var apiResponse = await RegistrationLogic.RegisterDevice(request);
                return apiResponse; 
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        // PUT api/register/5
        // This creates or updates a registration (with provided channelURI) at the specified id
        [HttpPut]
        [Route("v1/notification/registration")]
        [Authorize]
        public async Task<ApiResponseBase> UpdateRegistration()
        {
            UpdateRegistrationApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<UpdateRegistrationApiRequest>();
                var apiResponse = await RegistrationLogic.UpdateRegistration(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        // DELETE api/register/5
        [HttpDelete]
        [Route("v1/notification/registration")]
        [Authorize]
        public async Task<ApiResponseBase> DeleteRegistration()
        {
            DeleteRegistrationApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<DeleteRegistrationApiRequest>();
                var apiResponse = await RegistrationLogic.DeleteRegistration(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        
    }
}
