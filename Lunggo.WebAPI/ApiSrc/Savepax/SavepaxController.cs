
using System;
using System.Collections.Generic;
using System.Web.Http;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Product.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Savepax.Model;

namespace Lunggo.WebAPI.ApiSrc.Savepax
{
    public class SavepaxController : ApiController
    {
        [HttpPost]
        [Route("v1/savepax/create")]

        public CreateApiResponse Create()
        {
            try
            {
                var request = ApiRequestBase.DeserializeRequest<SavepaxApiRequest>();
                var newPax = request.PaxForDisplay.ConvertToPax();
                var createApiResponse = new CreateApiResponse
                {
                    Id = SavePax.Create(request.Email, newPax),
                    PaxForDisplay = newPax.ConvertToPaxForDisplay()
                };
                return createApiResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("v1/savepax/read")]
        public List<PaxForDisplay> Read()
        {
            try
            {
                var request = ApiRequestBase.DeserializeRequest<SavepaxApiRequest>();
                return SavePax.Read(request.Email).ConvertToPaxForDisplay();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("v1/savepax/update")]
        public void Update()
        {
            try
            {
                var request = ApiRequestBase.DeserializeRequest<SavepaxApiRequest>();
                var newPax = request.PaxForDisplay.ConvertToPax();
                SavePax.Update(request.Email, request.Id, newPax);
            }
            catch (Exception)
            { }
        }

        [HttpPost]
        [Route("v1/savepax/delete")]
        public void Delete()
        {
            try
            {
                var request = ApiRequestBase.DeserializeRequest<SavepaxApiRequest>();
                SavePax.Delete(request.Email, request.Id);
            }
            catch (Exception)
            { }
        }
    }

}
