(function () {
    'use strict';
    var controllerId = 'company';
    angular.module('app').controller(controllerId,
        ['common', 'datacontext', '$location', '$routeParams', 'config', company]);

    function company(common, datacontext, $location, $routeParams, config) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var KeyCodes = config.KeyCodes;
        

        var vm = this;

        vm.companyCount = 0;
        vm.companies = [];
        vm.title = 'Company';
        vm.gotoCompany = gotoCompany;

        vm.companySearch = $routeParams.search || '';
        vm.search = search;
        vm.filteredCompanies = [];
        vm.companeisFilter = companiesFilter;
        vm.companyCount = 0;
        vm.companyFilteredCount = 0;
        vm.pageChanged = pageChanged;
        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 5,
            pageSize: 2,
            totalCount: 0,
            totalPages: 0
        };

        Object.defineProperty(vm.paging, 'pageCount', {
            get: function () {
                return Math.floor(vm.paging.totalCount / vm.paging.pageSize) + 1;
            }
        });

        activate();

        function activate() {
            var promises = [getCompanies()];
            common.activateController(promises, controllerId)
                .then(function () {
                   // applyFilter = common.createSearchThrottle(vm, 'companies');
                    if (vm.companySearch) { applyFilter(true); }
                        log('Activated Company View');
                });
        }

        function getCompaniesCount() {
            return datacontext.company.getCount()
                .then(function (data) {
                return vm.companyCount = data.data.length;
            });
        }

        function getCompanyFilteredCount() {
            vm.companyFilteredCount = datacontext.company.getFilteredCount(vm.companySearch);
        }

        function getCompanies() {
            return datacontext.company.getCompanies(null,
                vm.paging.currentPage, vm.paging.pageSize,null, vm.orderSearch)
                .then(function (data) {
                    vm.filteredCompanies  = vm.companies = data.data;
                    vm.companyFilteredCount = vm.companyCount = data.data.length;
                    console.log(data.headers());

                    var responsePagination = JSON.parse(data.headers("x-pagination"));
                    vm.paging.currentPage = responsePagination.currentPage;
                    vm.paging.pageSize = responsePagination.pageSize;
                    vm.paging.totalCount = responsePagination.totalCount

                    return vm.filteredCompanies;
            });
        }

        function gotoCompany(company) {
            if (company && company.id) {
                $location.path('company/' + company.id);
            }
        }

        function pageChanged(page) {
            if (!page) { return; }
            vm.paging.currentPage = page;
            getCompanies();
        }

        function refresh() {
          //  getCompanies(true);
        }

        function search($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.companySearch = '';}
            
               // applyFilter(true);
            //} else {
                applyFilter();
            
            //getCompanies();
        }

        function applyFilter() {
            vm.filteredCompanies = vm.companies.filter(companiesFilter);
            vm.companyFilteredCount = vm.filteredCompanies.length;
         }

        function companiesFilter(company) {
            var isMatch = vm.companySearch
            ? common.textContains(company.name, vm.companySearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }

        
    }
})();