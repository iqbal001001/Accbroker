(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', 'datacontext', dashboard]);

    function dashboard(common, datacontext) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        vm.news = {
            title: 'Hot Towel Angular',
            description: 'Hot Towel Angular is a SPA template for Angular developers.'
        };
        vm.messageCount = 0;
        vm.invoices = [];
        vm.invoiceowing = 0
        vm.invoicedueover0to29daystotal = 0
        vm.invoicedueover30to59daystotal = 0
        vm.invoicedueover60to89daystotal = 0
        vm.invoicedueover90to119daystotal = 0
        vm.invoicedueover120andabovedaystotal = 0
        vm.title = 'Dashboard';

        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 5,
            pageSize: 20
        };

        activate();

        

        function activate() {
            var promises = [getInvoices(), getInvoiceDueDate()];
            common.activateController(promises, controllerId)
                .then(function () {
                    // applyFilter = common.createSearchThrottle(vm, 'invoices');
                  //  if (vm.invoiceSearch) { applyFilter(true); }
                    log('Activated DashBoard View');
                });
        }

        function getInvoiceDueDate() {
            datacontext.invoicetotal.getInvoicesByDueDate('2016-03-25', '2016-03-29', false)
                .then(function (res) {
                    vm.invoicedueover0to29daystotal = res.data;
                });
            datacontext.invoicetotal.getInvoicesByDueDate('2016-03-20', '2016-03-24', false)
                .then(function (res) {
                    vm.invoicedueover30to59daystotal = res.data;
                });
            datacontext.invoicetotal.getInvoicesByDueDate('2016-03-15','2016-03-19', false)
                .then(function (res) {
                    vm.invoicedueover60to89daystotal = res.data;
                });
            datacontext.invoicetotal.getInvoicesByDueDate('2016-03-15', '2016-03-19', false)
               .then(function (res) {
                   vm.invoicedueover90to119daystotal = res.data;
               });
            datacontext.invoicetotal.getInvoicesByDueDate('2016-03-10', '2016-03-14', false)
                .then(function (res) {
                    vm.invoicedueover120andabovedaystotal = res.data;
                });

            return true;
        }

     
        function getInvoices() {
            return datacontext.invoicetotal.getInvoices(null,
                vm.paging.currentPage, vm.paging.pageSize, null,
                null, null, null, null, null, null,
                vm.invoiceSearch, null)
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
    }
})();