﻿using Lunggo.Hotel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.CustomerWeb.Areas.UW100.Models
{
    public class UW100SearchResultViewModel
    {
        public UW100SearchParamViewModel SearchViewModel { get; set; }
        public List<HotelDetailBase> ListHotel { get; set; }
        public UW100SearchResultViewModel()
        {
            ListHotel = new List<HotelDetailBase>();
            SearchViewModel = new UW100SearchParamViewModel();
        }
    }
}