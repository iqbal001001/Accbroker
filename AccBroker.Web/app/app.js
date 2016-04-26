(function () {
    'use strict';
    
    var app = angular.module('app', [
        // Angular modules 
        'ngAnimate',        // animations
        'ngRoute',          // routing
        'ngSanitize',       // sanitizes html bindings (ex: sidebar.js)
        'ngMessages',       // Client side validation message
        'ngStorage',        // Local Storage

        // Custom modules 
        'common',           // common functions, logger, spinner
        'common.bootstrap', // bootstrap dialog wrapper functions
        'security',         // securtiy
          // autofield
        'autofields.bootstrap',

        // 3rd Party Modules
        'ui.bootstrap'      // ui-bootstrap (ex: carousel, pagination, dialog)
    ]);
    
    //// Handle routing errors and success events
    //app.run(['$route', function ($route) {
    //    // Include $route to kick start the router.

    //}]);

    app.run(['$rootScope', 'datacontext', 'routemediator', 'security',
        function ($rootScope, datacontext, routemediator, security) {
            $rootScope.loggedUser = null;
            $rootScope.security = security;
            $rootScope.baseUrl = "http://localhost:58242";
            //$rootScope.baseUrl = "http://accbrokerapi-alpha.azurewebsites.net"; 

           // datacontext.prime();
            routemediator.setRoutingHandlers();

            $rootScope.$on('$stateChangeStart', function (e, toState, toParams
                                                   , fromState, fromParams) {

                var isLogin = toState.name === "login";
                if (isLogin) {
                    return; // no need to redirect 
                }

                // now, redirect only not authenticated

                if (!security.user) {
                    e.preventDefault(); // stop current execution
                    $state.go('login'); // go to login
                }else
                {
                    $state.go('dashboard');
                }
            });
        }]);


})();