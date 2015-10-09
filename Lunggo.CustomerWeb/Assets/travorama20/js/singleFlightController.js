// travorama angular app - Flight Controller

app.controller('singleFlightController', [
    '$http', '$scope', function ($http, $scope) {

        // **********
        // on document ready
        angular.element(document).ready(function () {
            $scope.getFlight();
        });

        // **********
        // general variables
        $scope.busy = false;
        $scope.loading = false;
        $scope.flightList = [];
        $scope.expiryTime = '';
        $scope.flightRequest = {
            cabin: FlightSearchConfig.flightForm.cabin,
            AdultCount: FlightSearchConfig.flightForm.passenger.adult,
            ChildCount: FlightSearchConfig.flightForm.passenger.child,
            InfantCount: FlightSearchConfig.flightForm.passenger.infant,
            TripType: FlightSearchConfig.flightForm.type,
            Trips: FlightSearchConfig.flightForm.trips,
        };

        // **********
        // general functions
        // translate month
        $scope.translateMonth = function translateMonth(month) {
            switch (month) {
                case 0:
                    month = 'Jan';
                    break;
                case 1:
                    month = 'Feb';
                    break;
                case 2:
                    month = 'Mar';
                    break;
                case 3:
                    month = 'Apr';
                    break;
                case 4:
                    month = 'May';
                    break;
                case 5:
                    month = 'Jun';
                    break;
                case 6:
                    month = 'Jul';
                    break;
                case 7:
                    month = 'Aug';
                    break;
                case 8:
                    month = 'Sep';
                    break;
                case 9:
                    month = 'Oct';
                    break;
                case 10:
                    month = 'Nov';
                    break;
                case 11:
                    month = 'Dec';
                    break;
            }
            return month;
        }
        $scope.getDate = function(theDate) {
            theDate = new Date(theDate);
            return theDate.getDate();
        }

        // **********
        // flight detail function
        $scope.flightActive = -1;
        $scope.setFlightActive = function (flightNo) {
            if ($scope.flightActive == flightNo) {
                $scope.flightActive = -1;
            } else {
                $scope.flightActive = flightNo;
            }
        }

        // **********
        // get flight function
        $scope.getFlight = function() {

            $scope.busy = true;
            $scope.loading = true;

            console.log('----------');
            console.log('Getting flight list with parameter');
            console.log(FlightSearchConfig.flightForm);

            // ajax
            $http.get(FlightSearchConfig.Url, {
                params: {
                    request: $scope.flightRequest
                }
            }).success(function(returnData) {
                $scope.busy = false;
                $scope.loading = false;

                console.log(returnData);

                // set expiry time
                $scope.expiryTime = new Date(returnData.ExpiryTime);

                // generate flight list
                $scope.generateFlightList(returnData.FlightList);

                console.log('Finished getting flight');
                console.log('----------');


            }).error(function() {
                
            });

        }

        // generate flight list
        $scope.generateFlightList = function(data) {

            $scope.flightList = data;

        }

    }
]);// flight controller

