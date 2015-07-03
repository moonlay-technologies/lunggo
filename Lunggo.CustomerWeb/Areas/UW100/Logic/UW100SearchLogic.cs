using Dapper;
using Lunggo.ApCommon.Query;
using Lunggo.CustomerWeb.Areas.UW100.Models;
using Lunggo.Framework.Database;
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
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var query = GetCountryCodeByAreaQuery.GetInstance();
                var record = query.Execute(connection, new { ViewModel.CountryArea });
                if (record.Any())
                    ViewModel.CountryCode = record.FirstOrDefault().CountryCode;
            }
            return ViewModel;
        }
        public UW100SearchResultViewModel Search(UW100SearchParamViewModel ViewModel)
        {
            UW100SearchResultViewModel ResultSearch = new UW100SearchResultViewModel();
            ResultSearch.SearchViewModel = ViewModel;
            using (var connection = DbService.GetInstance().GetOpenConnection())
            {
                var query = GetHotelDetailBySearchParamQuery.GetInstance();
                ViewModel.Keyword = "%" + ViewModel.Keyword + "%";
                var listHotel = query.Execute(connection, ViewModel, ViewModel);
                int i = 10;

                ResultSearch.ListHotel = listHotel.ToList();
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