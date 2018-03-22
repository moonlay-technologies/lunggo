using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.Framework.Encoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Service
{
    public partial class AccountService
    {
        public ResettingPasswordOutput ResettingPassword(ResettingPasswordInput resettingPasswordInput)
        {
            UpdatePasswordToUserDb(resettingPasswordInput);
            return new ResettingPasswordOutput
            {
                isSuccess = true
            };
        }

        public List<string> GetIdsByContact(ResettingPasswordInput resettingPasswordInput)
        {
            var requestOtpInput = new RequestOtpInput
            {
                PhoneNumber = resettingPasswordInput.PhoneNumber,
                Email = resettingPasswordInput.Email
            };
            return GetUserIdFromDb(requestOtpInput).ToList();
        }
    }
}
