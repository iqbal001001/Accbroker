(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId,
        ['$rootScope', 'common', 'config', 'repositories', 'localStorage', datacontext]);

    function datacontext($rootScope, common, config, repositories, localStorage) {

        var getLogFn = common.logger.getLogFn;
        var hostEndPoint = "";
        //var hostEndPoint = "http://accbrokerapi-alpha.azurewebsites.net"; 
        var events = config.events;
        var log = getLogFn(serviceId);
        var logError = getLogFn(serviceId, 'error');
        var logSuccess = getLogFn(serviceId, 'success');

        var $q = common.$q;
        var primePromise;
        var repoNames = ['company','client','address','contact','invoice','payment','product','invoicetotal'];




        var service = {
            localStorage: localStorage
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

        function prime() {
            if (primePromise) return primePromise;

            //var storageEnabledAndHasData = zStorage.load(manager);

            primePromise = $q.all([service.company.getCompanies('code')])

            //primePromise = storageEnabledAndHasData ?
            //    $q.when('Loading entities and metadata from local storage') :
            //    $q.all([service.employee.getAll(), service.customer.getAll(), service.product.getAll()])
            //.then(extendMetadata);


            return primePromise.then(success);

            function success() {
               // service.lookup.setLookups();
                //zStorage.save();
                log('Prime the data');
            }
        }

    }
})();

