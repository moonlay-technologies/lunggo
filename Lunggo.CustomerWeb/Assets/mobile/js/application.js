// check if angular exist
if (typeof (angular) == 'object') {
    var app = angular.module('Travorama', ['ngRoute']);

    // root scope
    app.run(function($rootScope) {

        // general page config and function
        $rootScope.PageConfig = {
            
            // **********
            // General variables
            Loaded: true,
            Busy: false,

            // **********
            // functions

            // body no scroll
            BodyNoScroll: false,
            SetBodyNoScroll: function (state) {
                $rootScope.PageConfig.BodyNoScroll = state;
                
            }, // body no scroll end

            // toggle burger menu
            BurgerShown: false,
            ToggleBurger : function() {

                if ($rootScope.PageConfig.BurgerShown == false) {
                    $rootScope.PageConfig.BurgerShown = true;
                    $rootScope.PageConfig.SetBodyNoScroll(true);
                } else {
                    $rootScope.PageConfig.BurgerShown = false;
                    $rootScope.PageConfig.SetBodyNoScroll(false);
                }

            }, // toggle burger menu end


            // page overlay
            Overlay: '',
            SetOverlay: function (overlay) {
                if (overlay) {
                    $rootScope.PageConfig.Overlay = overlay;
                } else {
                    $rootScope.PageConfig.Overlay = '';
                }
            }, // page overlay end

            // page popup
            Popup: '',
            SetPopup: function (popup) {
                if (popup) {
                    $rootScope.PageConfig.Popup = popup;
                } else {
                    $rootScope.PageConfig.Popup = '';
                }
            }, // page popup end

        }; // $rootScope.PageConfig

    });

}

// --------------------
// general variables


// --------------------
// general functions

// get parameter
function getParam(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}