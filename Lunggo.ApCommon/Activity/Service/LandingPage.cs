using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.Framework.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public LandingPageOutput InsertContactLandingPageToBlob(LandingPageInput input)
        {
            var a = new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    Container = "LandingPageContainer",
                    FileInfo = new FileInfo
                    {
                        ContentType = "",
                        FileData = Encoding.UTF8.GetBytes(input.Contact),
                        FileName = input.Contact
                    }
                },
                SaveMethod = SaveMethod.Force
            };
            BlobStorageService.GetInstance().WriteFileToBlob(a);
            return new LandingPageOutput
            {
                isSuccess = true
            };
        }
    }
}
