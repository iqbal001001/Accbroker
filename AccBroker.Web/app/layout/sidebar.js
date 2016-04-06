(function () { 
    'use strict';
    
    var controllerId = 'sidebar';
    angular.module('app').controller(controllerId,
        ['$route', 'config', 'routes', 'datacontext', '$rootScope', sidebar]);

    function sidebar($route, config, routes, datacontext, $rootScope) {
        var vm = this;

        vm.isCurrent = isCurrent;
        vm.selectedCompany = null;

        vm.getCompanies = getCompanies;
        vm.selectedCompanyChanged = selectedCompanyChanged;

        activate();

        function activate() { getNavRoutes(); getCompanies() }
        
        function getNavRoutes() {
            vm.navRoutes = routes.filter(function(r) {
                return r.config.settings && r.config.settings.nav;
            }).sort(function(r1, r2) {
                return r1.config.settings.nav - r2.config.settings.nav;
            });
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

        function selectedCompanyChanged() {
            $rootScope.companyId = vm.selectedCompany.id;
        }
    };
})();
