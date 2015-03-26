// ************************
// variables

var SearchHotelConfig = {
	Url : 'http://travorama-apidev.azurewebsites.net/api/v1/hotels/',
	ResultCount : '20'
};

// ************************x
// functions


// ************************x
// angular app
(function(){

	var app = angular.module('travorama', ['ngRoute']);
	
	app.controller('HotelController', ['$http', '$scope', function ($http, $scope) {
	    
		// hotel list
		var hotel_list = this;
		hotel_list.hotels = [];

		// hotel search params
		$scope.HotelSearchParams = {};

		// *******************************
		// sample content
		$scope.HotelSearchParams.StayDate = '2015-05-05';
		$scope.HotelSearchParams.StayLength = '1';
		$scope.HotelSearchParams.LocationId = '640254';
		// *******************************

		$scope.Loadtime;

		$scope.stopwatch = function(state){
		    if (state == 'start') {
				$scope.Loadtime = 0;
				$scope.timer = setInterval(function(){ $scope.Loadtime = $scope.Loadtime + 0.1 }, 100);
			}else if( state == 'stop' ){
			    clearInterval($scope.timer);
				$scope.Loadtime = Math.round( $scope.Loadtime * 10 )/10;
			}

		}

		// load hotel list function
		$scope.load_hotel_list = function(page){

			console.log('--------------------------------');
			console.log('Searching for hotel with params:');
			console.log( $scope.HotelSearchParams );

			$scope.stopwatch('start');

			// set default page
			$scope.CurrentPage = page || 1;

			// set startIndex
			$scope.CurrentPage = $scope.CurrentPage - 1;
			$scope.HotelSearchParams.StartIndex = (SearchHotelConfig.ResultCount * $scope.CurrentPage);

			// show 'loading' text
			$('.hotel-list-content').prepend('<h1 class="preload text-center notif">LOADING</h1>');

			// generate StarRating
			$scope.HotelSearchParams.StarRating = [ $scope.HotelSearchParams.star0, $scope.HotelSearchParams.star1, $scope.HotelSearchParams.star2, $scope.HotelSearchParams.star3, $scope.HotelSearchParams.star4, $scope.HotelSearchParams.star5 ].join('');
			if( $scope.HotelSearchParams.StarRating.length == 0 ){
				$scope.HotelSearchParams.StarRating = '-1,1,2,3,4,5';
			}
			// http request
			$http.get(SearchHotelConfig.Url, {
				params : {
					LocationId : $scope.HotelSearchParams.LocationId,
					StayDate : $scope.HotelSearchParams.StayDate,
					StayLength : $scope.HotelSearchParams.StayLength,
					SearchId : $scope.HotelSearchParams.SearchId,
					SortBy : $scope.HotelSearchParams.SortBy,
					StartIndex : $scope.HotelSearchParams.StartIndex,
					ResultCount : SearchHotelConfig.ResultCount,
					MinPrice : $scope.HotelSearchParams.MinPrice,
					MaxPrice : $scope.HotelSearchParams.MaxPrice,
					StarRating : $scope.HotelSearchParams.StarRating
				}
			// if success
			}).success(function(data){
				// console data
				console.log('Hotel search result:');
				//console.log(data);

				//remove notif
				$('.notif').remove();

			    
				// add data to $scope
			    hotel_list.hotels = data.HotelList;
			    
			    $scope.hotels = hotel_list.hotels;
			    console.log(hotel_list.hotels);

			    $scope.HotelSearchParams.SearchId = data.SearchId;

				// pagination
				$scope.MaxPage = Math.ceil(data.TotalFilteredCount / SearchHotelConfig.ResultCount );
				$scope.show_page( $scope.MaxPage );

				console.log('loaded');
				console.log('--------------------------------');

				$scope.stopwatch('stop');

		   	// if error
		    }).error(function(){
		    	console.log('REQUEST ERROR');
		    	$('.notif').remove();
		    	$('.hotel-list-content').prepend('<h1 class="report text-center notif">ERROR</h1>');
		    });

		};

		// paging function
		$scope.show_page = function (max_page) {
			$('footer .pagination').html('');
			for( n=1 ; n<=max_page ; n++ ){
				$('footer .pagination').append('<li><a href="#" data-page="'+n+'">'+n+'</a></li>');
			}
			$('footer .pagination a').click(function(){
				var page = $(this).attr('data-page');
				$scope.load_hotel_list(page);
			});
		}


	}]);

})();
