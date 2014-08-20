using Dapper;
using Lunggo.CustomerWeb.Areas.UW100.Models;
using Lunggo.Hotel.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW100.Logic
{
    public class UW100SearchLogic
    {
        const string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Bayu\Lunggo\Lunggo.CustomerWeb\App_Data\DummyDB.mdf;Integrated Security=True";
        public UW100SearchParamViewModel GetAllParameter(UW100SearchParamViewModel ViewModel)
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var rows = con.Query("select CountryCode from CountryRef where CountryArea = '" + ViewModel.CountryArea + "'");
                if(rows.Count()>0)
                    ViewModel.CountryCode = rows.FirstOrDefault().CountryCode;
            }
            return ViewModel;
        }
        public UW100SearchResultViewModel Search(UW100SearchParamViewModel ViewModel)
        {
            UW100SearchResultViewModel ResultSearch = new UW100SearchResultViewModel();
            ResultSearch.SearchViewModel = ViewModel;
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "select a.*, b.Discount, b.price as MinimumPrice from Hotel a left join Room b on b.HotelId = a.HotelId where (";
                if (ViewModel.CountryCode != null)
                    query += "CountryCode = " + ViewModel.CountryCode;
                else if (ViewModel.ProvinceCode!=null)
                    query += "ProvinceCode = " + ViewModel.ProvinceCode;
                else if (ViewModel.LargeCode != null)
                    query += "LargeCode = " + ViewModel.LargeCode;
                else
                    query += "HotelName like '%" + ViewModel.Keyword + "%' or CountryArea like '%" + ViewModel.Keyword + "%' or ProvinceArea like '%" + ViewModel.Keyword + "%' or LargeArea like '%" + ViewModel.Keyword + "%'";
                query += " )and b.price = (select min(price) from room where HotelId=a.HotelId)";
                
                var rows = con.Query(query);
                foreach (var row in rows)
                {
                    HotelDetailBase TemporaryResult = new HotelDetailBase();
                    TemporaryResult.HotelId = row.HotelId;
                    TemporaryResult.HotelName = row.HotelName;
                    TemporaryResult.Address = row.Address;
                    TemporaryResult.LargeArea = row.LargeArea;
                    TemporaryResult.ProvinceArea = row.ProvinceArea;
                    TemporaryResult.CountryArea = row.CountryArea;
                    TemporaryResult.Description = row.Description;
                    TemporaryResult.MinimumPrice = row.MinimumPrice;
                    TemporaryResult.Discount = row.Discount;
                    ResultSearch.ListHotel.Add(TemporaryResult);
                }
            }
            return ResultSearch;
        }
        public ActionResult SearchLogic(UW100SearchParamViewModel ViewModel)
        {

            UW100SearchResultViewModel ResultSearch = Search(ViewModel);
            var result = new ViewResult()
            {
                ViewName = "../UW100Search/UW100Index"
            };
            result.ViewData.Model = ResultSearch; // ViewData may need to be instantiated first.

            return result;
        }
    }
}