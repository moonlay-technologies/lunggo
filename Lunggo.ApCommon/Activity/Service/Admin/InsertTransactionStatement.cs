using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public bool InsertTransactionStatement(InsertTransactionStatementInput insertTransactionStatementInput)
        {
            var trxNo = TrxNoSequence.GetInstance().GetNext();
            insertTransactionStatementInput.TrxNo = trxNo.ToString("D10");
            InsertTransactionStatementToDb(insertTransactionStatementInput);
            return true;   
        }

        public string GenerateNoTrx()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] randomByte = new byte[8];
            rng.GetBytes(randomByte);
            var randomInt = Math.Abs(BitConverter.ToInt32(randomByte, 0));
            var intTrx = randomInt % 100000000;
            var trx = intTrx.ToString("D8");
            return trx;
        }
    }
}
