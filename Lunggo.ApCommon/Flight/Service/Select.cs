using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string SelectFlight(string searchId, List<int> registerNumbers)
        {
            if (ParseTripType(searchId) == TripType.Return)
            {
                var depItin = GetItineraryFromSearchCache(searchId, registerNumbers[0], 1);
                var retItin = GetItineraryFromSearchCache(searchId, registerNumbers[1], 2);
                var depCombo = depItin.ComboSet;
                var retCombo = retItin.ComboSet;
                if (depCombo != null && retCombo != null)
                {
                    var comboRegByDep = depCombo.BundledRegisterNumber[depCombo.PairRegisterNumber.IndexOf(registerNumbers[1])];
                    var comboRegByRet = retCombo.BundledRegisterNumber[retCombo.PairRegisterNumber.IndexOf(registerNumbers[0])];
                    if (comboRegByDep != -1 && comboRegByRet != -1 && comboRegByDep == comboRegByRet)
                    {
                        var token = SaveItineraryFromSearchToCache(searchId, comboRegByDep, 0);
                        return token;
                    }
                }
                var depToken = SaveItineraryFromSearchToCache(searchId, registerNumbers[0]);
                var retToken = SaveItineraryFromSearchToCache(searchId, registerNumbers[1]);
                var bundledToken = BundleFlight(new List<string> {depToken, retToken});
                return bundledToken;
            }
            else
            {
                var token = SaveItineraryFromSearchToCache(searchId, registerNumbers[0]);
                return token;
            }
        }
    }
}
