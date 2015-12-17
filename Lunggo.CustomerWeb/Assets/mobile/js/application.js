// check if angular exist
if (typeof (angular) == 'object') {
    var app = angular.module('Travorama', ['ngRoute']);

    // root scope
    app.run(function($rootScope) {

        // general page config and function
        $rootScope.PageConfig = {
            
            // **********
            // variables
            Loaded: true,
            Busy: false,

            // **********
            // functions

            // body no scroll
            BodyNoScroll: false,
            SetNoScroll: function (state) {

            },

            // toggle burger menu
            BurgerShown: false,
            ToggleBurger : function() {

                if ($rootScope.PageConfig.BurgerShown == false) {
                    $rootScope.PageConfig.BurgerShown = true;
                    $rootScope.PageConfig.BodyNoScroll = true;
                } else {
                    $rootScope.PageConfig.BurgerShown = false;
                    $rootScope.PageConfig.BodyNoScroll = false;
                }

            }

        };


    });

}

// --------------------
// general variables


// --------------------
// general functions
