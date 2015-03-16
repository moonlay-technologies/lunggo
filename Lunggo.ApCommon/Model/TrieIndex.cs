using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Dictionary;

namespace Lunggo.ApCommon.Model
{
    public static class TrieIndex
    {
        public static readonly TrieNode Airline = new TrieNode();
        public static readonly TrieNode Airport = new TrieNode();
        public static readonly TrieNode Hotel = new TrieNode();

        static TrieIndex()
        {
            foreach (var airline in Code.Airline)
            {
                Airline.InsertWord(airline.Value.Code, airline.Key);
                Airline.InsertWordsBySentence(airline.Value.Name, airline.Key);
            }
            foreach (var airport in Code.Airport)
            {
                Airport.InsertWord(airport.Value.Code, airport.Key);
                Airport.InsertWordsBySentence(airport.Value.Name, airport.Key);
                Airport.InsertWordsBySentence(airport.Value.City, airport.Key);
            }
            foreach (var hotel in Code.Hotel)
            {
                Airport.InsertWordsBySentence(hotel.Value.CountryName, hotel.Key);
                Airport.InsertWordsBySentence(hotel.Value.LocationName, hotel.Key);
                Airport.InsertWordsBySentence(hotel.Value.RegionName, hotel.Key);
            }
        }
    }
}
