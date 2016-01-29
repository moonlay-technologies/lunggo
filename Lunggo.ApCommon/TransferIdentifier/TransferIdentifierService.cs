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

        public int GetTransferIdentifier(decimal price) 
        {
            bool isExist = true;
            Random rnd = new Random();
            Debug.Print("Try to generate the Unique Id ");
            int uniqueId;
            decimal candidatePrice;
            do
            {
                uniqueId = rnd.Next(1, 999);
                Debug.Print("Generated Code : " + uniqueId);
                candidatePrice = price + uniqueId;
                isExist = FlightService.GetInstance().isRedisExist(candidatePrice.ToString());
                Debug.Print("Candidate Price : " + candidatePrice +" IsExist : "+isExist);
            } while (isExist);

            FlightService.GetInstance().SaveUniquePriceinCache(candidatePrice.ToString());
            return uniqueId;
        }
    }
}
