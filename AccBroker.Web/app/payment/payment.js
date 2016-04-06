(function () {
    'use strict';
    var controllerId = 'payment';
    angular.module('app').controller(controllerId,
        ['common', 'datacontext', '$location', '$routeParams', 'config', '$rootScope', payment]);

    function payment(common, datacontext, $location, $routeParams, config, $rootScope) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var KeyCodes = config.KeyCodes;


        var vm = this;

        vm.paymentCount = 0;
        vm.payments = [];
        vm.title = 'Payment';
        vm.gotoPayment = gotoPayment;

        vm.paymentSearch = $routeParams.search || '';
        vm.companyId = null;
        vm.search = search;
        vm.filteredPayments = [];
        vm.companeisFilter = paymentsFilter;
        vm.paymentCount = 0;
        vm.paymentFilteredCount = 0;
        vm.pageChanged = pageChanged;
        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 5,
            pageSize: 10
        };

        Object.defineProperty(vm.paging, 'pageCount', {
            get: function () {
                return Math.floor(vm.paymentFilteredCount / vm.paging.pageSize) + 1;
            }
        });

        activate();

        function activate() {
            vm.companyId = $rootScope.companyId;
            var promises = [getPayments()];
            common.activateController(promises, controllerId)
                .then(function () {
                    // applyFilter = common.createSearchThrottle(vm, 'payments');
                    if (vm.paymentSearch) { applyFilter(true); }
                    log('Activated Payment View');
                });
        }

        function getPaymentsCount() {
            return datacontext.payment.getCount()
                .then(function (data) {
                    return vm.paymentCount = data.data.length;
                });
        }

        function getPaymentFilteredCount() {
            vm.paymentFilteredCount = datacontext.payment.getFilteredCount(vm.paymentSearch);
        }

        function getPayments() {
            return datacontext.payment.getPayments(null,
                vm.paging.currentPage, vm.paging.pageSize, null, vm.paymentSearch, vm.companyId, null)
                .then(function (data) {
                    vm.filteredPayments = vm.payments = data.data;
                    vm.paymentFilteredCount = vm.paymentCount = data.data.length;
                    console.log(data.headers());

                    var responsePagination = JSON.parse(data.headers("x-pagination"));
                    vm.paging.currentPage = responsePagination.currentPage;
                    vm.paging.pageSize = responsePagination.pageSize;
                    vm.paging.totalCount = vm.invoiceCount = responsePagination.totalCount;

                    return vm.filteredPayments;
                });
        }

        function gotoPayment(payment) {
            if (payment && payment.id) {
                $location.path('payment/' + payment.id);
            }
        }

        function pageChanged(page) {
            if (!page) { return; }
            vm.paging.currentPage = page;
            getPayments();
        }

        function refresh() {
            //  getPayments(true);
        }

        function search($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.paymentSearch = '';
            }

            // applyFilter(true);
            //} else {
            applyFilter();

            //getPayments();
        }

        function applyFilter() {
            vm.filteredPayments = vm.payments.filter(paymentsFilter);
            vm.paymentFilteredCount = vm.filteredPayments.length;
        }

        function paymentsFilter(payment) {
            var isMatch = vm.paymentSearch
            ? common.textContains(payment.name, vm.paymentSearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }
    }
})();