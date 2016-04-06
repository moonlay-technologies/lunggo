using Lunggo.ApCommon.Flight.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.TransferIdentifier
{
    public class TransferIdentifierService
    {
        private static readonly TransferIdentifierService Instance = new TransferIdentifierService();
        private bool _isInitialized;
        private TransferIdentifierService()
        {
            
        }

        public static TransferIdentifierService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }

        public int GetTransferIdentifier(decimal price, string token) 
        {
            bool isExist = true;
            Random rnd = new Random();
            int uniqueId;
            decimal candidatePrice;
            //Generate Unique Id
            if (price <= 999)
            {
                uniqueId = Decimal.ToInt32(price);
            }
            else 
            {
                do
                {
                    uniqueId = rnd.Next(1, 999);
                    candidatePrice = price - uniqueId;
                    isExist = FlightService.GetInstance().isRedisExist(candidatePrice.ToString());
                } while (isExist);
                Dictionary<string, int> dict = new Dictionary<string, int>();
                dict.Add(token, uniqueId);
                FlightService.GetInstance().SaveUniquePriceinCache(candidatePrice.ToString(), dict);
                FlightService.GetInstance().SaveTokenTransferCodeinCache(token, uniqueId.ToString());
            }
            
            return uniqueId;
        }
    }
}
