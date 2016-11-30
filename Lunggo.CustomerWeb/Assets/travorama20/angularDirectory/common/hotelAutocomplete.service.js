app.factory('hotelAutocompleteSvc', ['$log', '$resource', function ($log, $resource) {
    var factory = {};

    factory.resource = $resource(HotelAutocompleteConfig.Url + '/:prefix',
        { prefix: '@prefix' },
        {
            get: {
                method: 'GET',
                params: {},
                isArray: false
            }
        }
    );

    return factory;
}]);

