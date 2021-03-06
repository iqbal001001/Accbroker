﻿(function () {
    'use strict';
    var controllerId = 'invoice';
    angular.module('app').controller(controllerId,
        ['common', 'datacontext', '$location', '$routeParams', 'config', '$rootScope', invoice]);

    function invoice(common, datacontext, $location, $routeParams, config, $rootScope) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var KeyCodes = config.KeyCodes;


        var vm = this;

        vm.invoiceCount = 0;
        vm.invoices = [];
        vm.title = 'Invoice';
        vm.gotoInvoice = gotoInvoice;

        vm.invoiceSearch = $routeParams.search || '';
        vm.companyId = null;
        vm.search = search;
        vm.searchInvoiceNo = searchInvoiceNo;
        vm.filteredInvoices = [];
        vm.companeisFilter = invoicesFilter;
        vm.invoiceCount = 0;
        vm.invoiceFilteredCount = 0;
        vm.pageChanged = pageChanged;
        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 10,
            pageSize: 10
        };

        Object.defineProperty(vm.paging, 'pageCount', {
            get: function () {
                return Math.floor(vm.invoiceFilteredCount / vm.paging.pageSize) + 1;
            }
        });

        activate();

        function activate() {
            vm.companyId = $rootScope.companyId;
            var promises = [getInvoices()];
            common.activateController(promises, controllerId)
                .then(function () {
                    // applyFilter = common.createSearchThrottle(vm, 'invoices');
                    if (vm.invoiceSearch) { applyFilter(true); }
                    log('Activated Invoice View');
                });
        }

        function getInvoicesCount() {
            return datacontext.invoice.getCount()
                .then(function (data) {
                    return vm.invoiceCount = data.data.length;
                });
        }

        function getInvoiceFilteredCount() {
            vm.invoiceFilteredCount = datacontext.invoice.getFilteredCount(vm.invoiceSearch);
        }

        function getInvoices() {
            return datacontext.invoice.getInvoices(null,
                vm.paging.currentPage, vm.paging.pageSize, null, vm.invoiceSearch, vm.companyId, null)
                .then(function (data) {
                    vm.filteredInvoices = vm.invoices = data.data;
                    vm.invoiceFilteredCount = data.data.length;
                    console.log(data.headers());

                    var responsePagination = JSON.parse(data.headers("x-pagination"));
                    vm.paging.currentPage = responsePagination.currentPage;
                    vm.paging.pageSize = responsePagination.pageSize;
                    vm.paging.totalCount = vm.invoiceCount = responsePagination.totalCount;
                    
                    return vm.filteredInvoices;
                });
        }

        function gotoInvoice(invoice) {
            if (invoice && invoice.id) {
                $location.path('invoice/' + invoice.id);
            }
        }

        function pageChanged(page) {
            if (!page) { return; }
            vm.paging.currentPage = page;
            getInvoices();
        }

        function refresh() {
            //  getInvoices(true);
        }

        function search($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.invoiceSearch = '';
            }

            // applyFilter(true);
            //} else {
            applyFilter();

            //getInvoices();
        }

        function searchInvoiceNo() {
            getInvoices();
        }

        function applyFilter() {
            vm.filteredInvoices = vm.invoices.filter(invoicesFilter);
            vm.invoiceFilteredCount = vm.filteredInvoices.length;
        }

        function invoicesFilter(invoice) {
            var isMatch = vm.invoiceSearch
            ? common.textContains(invoice.name, vm.invoiceSearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }
    }
})();