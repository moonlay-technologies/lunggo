using System;
using System.Threading.Tasks;
using System.Web.Http;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Notification.Logic;
using Lunggo.WebAPI.ApiSrc.Notification.Model;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;


namespace Lunggo.WebAPI.ApiSrc.Notification
{
    public class NotificationController : ApiController
    {
        // POST api/register
        // This creates a registration id
        [HttpPut]
        [Route("v1/notification/registration")]
        [Level1Authorize]
        public ApiResponseBase RegisterDevice()
        {
            RegisterDeviceApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<RegisterDeviceApiRequest>();
                var apiResponse = RegistrationLogic.RegisterDevice(request);
                return apiResponse; 
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        // // PATCH api/register/5
        // // This creates or updates a registration (with provided channelURI) at the specified id
        // [HttpPatch]
        // [Route("v1/notification/registration")]
        // [Level1Authorize]
        // public async Task<ApiResponseBase> UpdateRegistration()
        // {
        //     UpdateRegistrationApiRequest request = null;
        //     try
        //     {
        //         request = ApiRequestBase.DeserializeRequest<UpdateRegistrationApiRequest>();
        //         var apiResponse = await RegistrationLogic.UpdateRegistration(request);
        //         return apiResponse;
        //     }
        //     catch (Exception e)
        //     {
        //         return ApiResponseBase.ExceptionHandling(e, request);
        //     }
        // }

        // DELETE api/register/5
        [HttpDelete]
        [Route("v1/notification/registration")]
        [Level1Authorize]
        public ApiResponseBase DeleteRegistration()
        {
            DeleteRegistrationApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<DeleteRegistrationApiRequest>();
                var apiResponse = RegistrationLogic.DeleteRegistration(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        [HttpPut]
        [Route("v1/operator/notification/registration")]
        [Level1Authorize]
        public async Task<ApiResponseBase> OperatorRegisterDevice()
        {
            RegisterDeviceApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<RegisterDeviceApiRequest>();
                var apiResponse = await RegistrationLogic.OperatorRegisterDevice(request);
                return apiResponse; 
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }

        // PUT api/register/5
        // This creates or updates a registration (with provided channelURI) at the specified id
        [HttpPatch]
        [Route("v1/operator/notification/registration")]
        [Level1Authorize]
        public async Task<ApiResponseBase> OperatorUpdateRegistration()
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
        [Route("v1/operator/notification/registration")]
        [Level1Authorize]
        public async Task<ApiResponseBase> OperatorDeleteRegistration()
        {
            DeleteRegistrationApiRequest request = null;
            try
            {
                request = ApiRequestBase.DeserializeRequest<DeleteRegistrationApiRequest>();
                var apiResponse = await RegistrationLogic.OperatorDeleteRegistration(request);
                return apiResponse;
            }
            catch (Exception e)
            {
                return ApiResponseBase.ExceptionHandling(e, request);
            }
        }
        
    }
}
