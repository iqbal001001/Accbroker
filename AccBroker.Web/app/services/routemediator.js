(function () {
    'use strict';

    var serviceId = 'routemediator';
    angular.module('app').
        factory(serviceId, ['$location', '$rootScope', 'config', 'logger', 'security', routemediator]);

    function routemediator($location, $rootScope, config, logger, security) {
        var handleRouteChangeError = false;
        var service = {
            setRoutingHandlers: setRoutingHandlers
        };

        return service;

        function setRoutingHandlers() {
            handleRoutingHandlers();
            updateDocTitle();
            redirectToLogin();
        }

        function handleRoutingHandlers() {
            $rootScope.$on('$routeChangeError',
               function (event, current, previous, rejection) {
                   if (handleRouteChangeError) { return; }
                   handleRouteChangeError = true;
                   var msg = 'Error routing: ' + (current && current.name)
                   + ' ' + (rejection.msg || '');
                   logger.logWarning(msg, current, serviceId, true);
                   $location.path('/');
               })
        }

        function updateDocTitle() {
            $rootScope.$on('$routeChangeSuccess',
                function (event, current, previous) {
                    handleRouteChangeError = false;
                    var title = config.docTitle + ' ' + (current.title || '');
                    $rootScope.title = title;
                })
        }

        function redirectToLogin() {
            $rootScope.$on('$routeChangeStart',
                function (event, next, current, previous) {
                    handleRouteChangeError = false;
                    if (next.templateUrl && !security.user) {
                        // no logged user, we should be going to #login
                        if (next.templateUrl.indexOf("app/security") !=-1) {
                            // already going to #login, no redirect needed
                        //}else if (next.templateUrl == "app/security/join/join.html") {
                        //    // already going to #login, no redirect needed
                        }else {
                            // not going to #login, we should redirect now
                            $location.path("/login");
                        }
                    }
                })
        }
    }
})();