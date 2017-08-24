app.config(function ($logProvider) {
    var debugEnabled = true;
    $logProvider.debugEnabled(debugEnabled);
});

app.run(function ($rootScope) {
    $rootScope.traGetRange = function (max, min) {

        var startFrom = 1;
        if (min != null || min !== undefined) {
            startFrom = min;
        }

        var returnValue = [];
        if (max <= min) {
            return returnValue;
        }
        for (var i = startFrom; i <= max; i++) {
            returnValue.push(i);
        }
        return returnValue;
    };

    $rootScope.traGetRangeDesc = function (count) {
        var returnValue = [];
        if (count < 1) {
            return returnValue;
        }
        for (var i = count; i >= 1; i++) {
            returnValue.push(i);
        }
        return returnValue;
    };
});

//This is for images in Search Hotel Desktop
app.directive('hotelListImage', function ($http, $log, $q) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var isImage = function (src) {
                var deferred = $q.defer();
                var image = new Image();
                image.onerror = function () { deferred.reject(false); };
                image.onload = function () { deferred.resolve(true); };
                image.src = src;
                return deferred.promise;
            };
            var resizeImages = function () {
                var img = $(element);
                var div = $("<div />").css({
                    background: "url(" + img.attr("src") + ") no-repeat",
                    width: "100%",
                    height: "140px",
                    "background-size": "cover",
                    "background-position": "center"
                });
                img.replaceWith(div);
            };

            attrs.$observe('ngSrc', function (ngSrc) {
                isImage(ngSrc).then(function () {
                    $log.debug('image exist');
                }, function () {
                    var altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel.png';
                    $log.debug('image not exist');

                    element.removeAttr('src');
                    element.attr('src', altImagePath); // set default image
                }).finally(function () {
                    // Always execute this on both error and success
                    resizeImages();
                });
            });
        }
    };
});
//Alternative image
app.directive('altImage', function($http, $log, $q) {
    return {
        restrict: 'A',
        link: function(scope, element, attrs) {
            var altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel.png';

            var isImage = function(src) {
                var deferred = $q.defer();
                var image = new Image();
                image.onerror = function() { deferred.reject(false); };
                image.onload = function() { deferred.resolve(true); };
                image.src = src;
                return deferred.promise;
            };

            attrs.$observe('ngSrc', function(ngSrc) {
                isImage(ngSrc).then(function() {
                }, function() {
                    element.removeAttr('src');
                    element.attr('src', altImagePath); // set default image
                    return true;
                });
            });

            attrs.$observe('src', function(src) {
                isImage(src).then(function() {
                }, function() {
                    element.attr('src', altImagePath); // set default image
                    return true;
                });
            });

            attrs.$observe('dataThumb', function(dataThumb) {
                isImage(dataThumb).then(function() {
                }, function() {
                    element.removeAttr('src');
                    element.attr('src', altImagePath); // set default image
                    return true;
                });
            });


            //observe background image
            attrs.$observe('style', function(style) {
                style = "width:100px;" + style;
                style = style.replace(/\s+/g, "");


                var backgroundImage = style.substring(style.indexOf("background-image"));
                //"background-image:url(https://photos.hotelbeds.com/giata/14/145936/145936a_hb_a_010.jpg);"

                backgroundImage = backgroundImage.substring(backgroundImage.indexOf("(") + 1, backgroundImage.indexOf(")"));


                isImage(backgroundImage).then(function() {
                }, function() {
                    var altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel-lg.png';
                    $log.debug('image not exist');

                    element.removeAttr('style');
                    element.attr('style', "background-image: url(" + altImagePath + ");"); // set default image
                    return true;
                });
            });
        }
    }
});

//This is for load page in mobile
app.directive('searchScroll', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            
            $(window).scroll(function () {
                var a = Math.round($(window).scrollTop());
                var b = $(window).height();
                var c = $(document).height();
                if (Math.round($(window).scrollTop()) + $(window).height() == $(document).height() && scope.page < scope.pageCount) {
                    scope.page++;
                    scope.bottomPage = true;
                    window.scrollTo(0, document.body.scrollHeight);
                    scope.searchHotel('', '', true);
                }
            });

            }
        };
    });
//app.directive('altBackgroundImage', function (style) {
//    return function (scope, element, attrs) {
//        element.bind("keydown keypress", function (event) {
//            if (event.which === 13) {
//                scope.$apply(function () {
//                    scope.$eval(attrs.traOnEnter);
//                });
//                event.preventDefault();
//            }
//        });
//    };
//});

//This is for entering input and filter
app.directive('traOnEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.traOnEnter);
                });
                event.preventDefault();
            }
        });
    };
});

app.factory('imageSvc', ['$log', '$q', function ($log, $q) {
    var factory = {};
    var _altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel.png';
    var _isImage = function (src) {
        var deferred = $q.defer();
        var image = new Image();
        image.onerror = function () { deferred.reject(false); };
        image.onload = function () { deferred.resolve(true); };
        image.src = src;
        return deferred.promise;
    };

    factory.isImage = _isImage;

    return factory;
}]);