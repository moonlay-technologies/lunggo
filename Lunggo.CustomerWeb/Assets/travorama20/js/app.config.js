
app.config(function ($logProvider) {
    var debugEnabled = true;
    $logProvider.debugEnabled(debugEnabled);
});

app.directive('altImage', function ($http, $log, $q) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel.png';

            var isImage = function (src) {
                var deferred = $q.defer();
                var image = new Image();
                image.onerror = function () { deferred.reject(false); };
                image.onload = function () { deferred.resolve(true); };
                image.src = src;
                return deferred.promise;
            };

            attrs.$observe('ngSrc', function (ngSrc) {
                isImage(ngSrc).then(function () {
                }, function () {
                    element.removeAttr('src');
                    element.attr('src', altImagePath); // set default image
                    return true;
                });
            });

            attrs.$observe('src', function (src) {
                isImage(src).then(function () {
                }, function () {
                    element.attr('src', altImagePath); // set default image
                    return true;
                });
            });

            attrs.$observe('dataThumb', function (dataThumb) {
                isImage(dataThumb).then(function () {
                }, function () {
                    element.removeAttr('src');
                    element.attr('src', altImagePath); // set default image
                    return true;
                });
            });


            //observe background image
            attrs.$observe('style', function (style) {
                style = "width:100px;" + style;
                style = style.replace(/\s+/g, "");


                var backgroundImage = style.substring(style.indexOf("background-image"));
                //"background-image:url(http://photos.hotelbeds.com/giata/14/145936/145936a_hb_a_010.jpg);"

                backgroundImage = backgroundImage.substring(backgroundImage.indexOf("(") + 1, backgroundImage.indexOf(")"));


                isImage(backgroundImage).then(function () {
                }, function () {
                    var altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel-lg.png';
                    $log.debug('image not exist');

                    element.removeAttr('style');
                    element.attr('style', "background-image: url(" + altImagePath + ");"); // set default image
                    return true;
                });
            });



            //attrs.$observe('altImage', function (altImage) {
            //    isImage(altImage).then(function () {
            //        $log.debug('image exist');
            //    }, function () {
            //        var altImagePath = document.location.origin + '/Assets/travorama20/images/Hotel/no-hotel.png';
            //        $log.debug('image not exist');

            //        element.removeAttr('src');
            //        //element.attr('src', "background-image: url(" + altImagePath + "); width: 100%; height: 450px; background-size: cover; background-position: center center;"); // set default image
            //    });
            //});

        }
    };
})
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
