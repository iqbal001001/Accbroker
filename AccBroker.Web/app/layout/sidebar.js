(function () { 
    'use strict';
    
    var controllerId = 'sidebar';
    angular.module('app').controller(controllerId,
        ['$route', 'config', 'routes', 'datacontext', '$rootScope', 'bootstrap.dialog', '$location', 'security', '$scope', sidebar]);

    function sidebar($route, config, routes, datacontext, $rootScope, bsDialog, $location, security, $scope) {
        var vm = this;
        var KeyCodes = config.KeyCodes;

        vm.security = security;
        vm.clearStorage = clearStorage;
        vm.isCurrent = isCurrent;
        vm.routes = routes;
        vm.searchInvoiceNo = '';
        vm.searchInvoice = searchInvoice;
        vm.wip = [];
        vm.wipChangedEvent = config.events.storage.wipChanged;
        vm.selectedCompany = null;

        vm.getCompanies = getCompanies;
        vm.selectedCompanyChanged = selectedCompanyChanged;

        activate();

        function activate() {
            getNavRoutes();
            getCompanies();
            vm.wip = datacontext.localStorage.getAll();
        }

        $scope.$watch("vm.security.user", function () {
            if (vm.security.user) {
                getCompanies();
            }
        }, true);
        
        function getNavRoutes() {
            vm.navRoutes = routes.filter(function(r) {
                return r.config.settings && r.config.settings.nav;
            }).sort(function(r1, r2) {
                return r1.config.settings.nav - r2.config.settings.nav;
            });
        }

        function clearStorage() {
            return bsDialog.deleteDialog('local storage')
            .then(confirmDelete, cancelDelete);

            function confirmDelete() { datacontext.localStorage.clear(); }
            function cancelDelete() { }
        }
        
        function getCompanies() {
            return datacontext.company.getCompanies(null,
                null, null, null, null)
                .then(function (data) {
                    vm.companies = data.data;
                    vm.selectedCompany = vm.companies[0];
                    if (vm.selectedCompany)
                        $rootScope.companyId = vm.selectedCompany.id;
                    return vm.companies;
                });
        }

        function isCurrent(route) {
            if (!route.config.title || !$route.current || !$route.current.title) {
                return '';
            }
            var menuName = route.config.title;
            return $route.current.title.substr(0, menuName.length) === menuName ? 'current' : '';
        }

        function searchInvoice($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.searchInvoiceNo = '';
                return;
            }

            if ($event.type === 'click' || $event.KeyCodes === KeyCodes.enter) {
                var route = '/invoices/search/'
                $location.path(route + vm.searchInvoiceNo);
            }
        }

        function selectedCompanyChanged() {
            $rootScope.companyId = vm.selectedCompany.id;
            $rootScope.$broadcast(config.events.companyChanged);

            $location.path('/');
            

        }
    };
})();
