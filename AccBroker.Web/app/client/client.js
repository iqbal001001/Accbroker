(function () {
    'use strict';
    var controllerId = 'client';
    angular.module('app').controller(controllerId,
        ['common', 'datacontext', '$location', '$routeParams', 'config', client]);

    function client(common, datacontext, $location, $routeParams, config) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var KeyCodes = config.KeyCodes;


        var vm = this;

        vm.clientCount = 0;
        vm.clients = [];
        vm.title = 'Client';
        vm.gotoClient = gotoClient;

        vm.clientSearch = $routeParams.search || '';
        vm.search = search;
        vm.filteredClients = [];
        vm.companeisFilter = clientsFilter;
        vm.clientCount = 0;
        vm.clientFilteredCount = 0;
        vm.pageChanged = pageChanged;
        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 5,
            pageSize: 10
        };

        Object.defineProperty(vm.paging, 'pageCount', {
            get: function () {
                return Math.floor(vm.clientFilteredCount / vm.paging.pageSize) + 1;
            }
        });

        activate();

        function activate() {
            var promises = [getClients()];
            common.activateController(promises, controllerId)
                .then(function () {
                    // applyFilter = common.createSearchThrottle(vm, 'clients');
                    if (vm.clientSearch) { applyFilter(true); }
                    log('Activated Client View');
                });
        }

        function getClientsCount() {
            return datacontext.client.getCount()
                .then(function (data) {
                    return vm.clientCount = data.data.length;
                });
        }

        function getClientFilteredCount() {
            vm.clientFilteredCount = datacontext.client.getFilteredCount(vm.clientSearch);
        }

        function getClients() {
            return datacontext.client.getClients(null,
                vm.paging.currentPage, vm.paging.pageSize, null, vm.orderSearch)
                .then(function (data) {
                    vm.filteredClients = vm.clients = data.data;
                    vm.clientFilteredCount = vm.clientCount = data.data.length;
                    console.log(data.headers());

                    //var responsePagination = JSON.parse(data.headers("x-pagination"));
                    //vm.paging.currentPage = responsePagination.currentPage;
                    //vm.paging.pageSize = responsePagination.pageSize;

                    return vm.filteredClients;
                });
        }

        function gotoClient(client) {
            if (client && client.id) {
                $location.path('client/' + client.id);
            }
        }

        function pageChanged(page) {
            if (!page) { return; }
            vm.paging.currentPage = page;
            getClients();
        }

        function refresh() {
            //  getClients(true);
        }

        function search($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.clientSearch = '';
            }

            // applyFilter(true);
            //} else {
            applyFilter();

            //getClients();
        }

        function applyFilter() {
            vm.filteredClients = vm.clients.filter(clientsFilter);
            vm.clientFilteredCount = vm.filteredClients.length;
        }

        function clientsFilter(client) {
            var isMatch = vm.clientSearch
            ? common.textContains(client.name, vm.clientSearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }
    }
})();