(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId,
        ['$rootScope', 'common', 'config',
            'repositories', datacontext]);

    function datacontext($rootScope, common, config,
        repositories) {

        var getLogFn = common.logger.getLogFn;
        //var hostEndPoint = "http://localhost:58242"
        var hostEndPoint = "http://accbrokerapi-alpha.azurewebsites.net"; 
        var events = config.events;
        var log = getLogFn(serviceId);
        var logError = getLogFn(serviceId, 'error');
        var logSuccess = getLogFn(serviceId, 'success');

        var $q = common.$q;
        var primePromise;
        var repoNames = ['company','client','address','contact','invoice','payment','product','invoicetotal'];




        var service = {
           
        };

        init();

        return service;

        function init() {
            repositories.init(hostEndPoint);
            defineLazyLoadedRepos();
        }

        function defineLazyLoadedRepos() {
            repoNames.forEach(function (name) {
                Object.defineProperty(service, name, {
                    configurable: true,
                    get: function () {
                        var repo = repositories.getRepo(name);
                        Object.defineProperty(service, name, {
                            value: repo,
                            configurable: false,
                            enumeable: true
                        });
                        return repo;
                    }
                });
            });
        }

    }
})();

