(function () {
    'use strict';

    var controllerId = 'topnav';
    angular.module('app').controller(controllerId,
        ['$route', '$location', 'config', 'routes', '$scope', 'security', topnav]);

    function topnav($route, $location, config, routes, $scope, security) {
        var vm = this;

        activate();

        function activate() { }

        vm.isActive = function (viewLocation) {
            return viewLocation === $location.path();
        };
    };
})();