(function () {
    'use strict';

    var app = angular.module('app');

    // Configure Toastr
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    // For use with the HotTowel-Angular-Breeze add-on that uses Breeze
    var remoteServiceName = 'breeze/Breeze';

    var events = {
        controllerActivateSuccess: 'controller.activateSuccess',
        spinnerToggle: 'spinner.toggle',
        companyChanged: 'company.changed',
        storage: {
            error: 'store.error',
            storeChanged: 'store.changed',
            wipChanged: 'wip.changed'
            
        }
    };

    var KeyCodes = {
        backspace: 8,
        tab: 9,
        enter: 13,
        esc: 27,
        space: 32,
        pageup: 33,
        pagedown: 34,
        end: 35,
        home: 36,
        left: 37,
        up: 38,
        right: 39,
        down: 40,
        insert: 45,
        del: 46
    }

    var config = {
        appErrorPrefix: '[HT Error] ', //Configure the exceptionHandler decorator
        docTitle: 'AccBroker: ',
        events: events,
        remoteServiceName: remoteServiceName,
        version: '2.1.0',
        KeyCodes: KeyCodes
    };

    app.value('config', config);
    
    app.config(['$logProvider', function ($logProvider) {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }
    }]);
    
    //#region Configure the common services via commonConfig
    app.config(['commonConfigProvider', function (cfg) {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.spinnerToggleEvent = config.events.spinnerToggle;
    }]);
    //#endregion

    // reference : http://www.webdeveasy.com/interceptors-in-angularjs-and-useful-examples/

    //app.factory('sessionInjector', ['SessionService', function (SessionService) {
    //    var sessionInjector = {
    //        request: function (config) {
    //            if (!SessionService.isAnonymus) {
    //                config.headers['x-session-token'] = SessionService.token;
    //            }
    //            return config;
    //        }
    //    };
    //    return sessionInjector;
    //}]);
    //app.config(['$httpProvider', function ($httpProvider) {
    //    $httpProvider.interceptors.push('sessionInjector');
    //}]);

    app.factory('addBaseUrl', ['$rootScope', function ($rootScope) {
        var urlMarker = {
            request: function (config) {
                // extend config.url to add a base url to the request
                if (config.url.indexOf('api') !== -1 ||
                    config.url.indexOf('token') !== -1 ||
                    config.url.indexOf('manage') !== -1) {
                    config.url = $rootScope.baseUrl + config.url;
                }

                return config; //|| $q.when(config);
            }
        };
        return urlMarker;
    }]);
    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('addBaseUrl');
    }]);



})();