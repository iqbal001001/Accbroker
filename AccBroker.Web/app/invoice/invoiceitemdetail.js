(function () {
    'use strict';

    var controllerId = 'invoicedetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            invoicedetail]);

    function invoicedetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');


        vm.activate = activate;
        vm.invoiceIdParameter = $routeParams.id;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.newinvoice;
        vm.deleteInvoice = deleteInvoice;
        vm.isSaving = false;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        function canSave() { return !vm.isSaving; }

        activate();

        function activate() {
            common.activateController([getRequestedInvoice()], controllerId)
                                .then(function () { log('Activated Invoicedetail View'); });

        }

        function deleteInvoice() {
            return bsDialog.deleteDialog('Invoice').
                then(confirmDelete);

            function confirmDelete() {
                datacontext.invoice.deleteInvoice(vm.invoice.id)
                .then(success, failed);

                function success() {
                    gotoInvoices();
                }

                function failed(error) { cancel(); }
            }

        }




        function getRequestedInvoice() {
            var val = $routeParams.id;
            if (val === 'new') {
                return vm.newinvoice = true;
            }

            return datacontext.invoice.getInvoice(val)
            .then(function (data) {
                vm.invoice = data.data;
                vm.newinvoice = false;
            }, function (error) {
                logError('Unable to get invoice ' + val);
                gotoInvoices();
            });
        }



        function cancel() {
            gotoInvoices();
        }

        function gotoInvoices() {
            $location.path('/invoices');
        }

        function goBack() { $window.history.back(); }


        function save() {
            if (vm.invoice.id == null) {
                vm.newinvoice = true;
                return SaveInvoice();
            }
            return datacontext.invoice.getInvoice(vm.invoice.id)
           .then(function (data) {
               if (data != null)
               { vm.newinvoice = false; }
               else
               { vm.newinvoice = true; }

               SaveInvoice();
           },
               function (error) {
                   if (error.status == 404) {
                       vm.newinvoice = true;
                       SaveInvoice();
                   }
               })

        }

        function SaveInvoice() {
            vm.isSaving = true;
            if (vm.newinvoice === true) {
                return datacontext.invoice.saveInvoice(vm.invoice)
                    .then(function (saveResult) {
                        vm.isSaving = false;
                    }, function (error) {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.invoice.updateInvoice(vm.invoice.id, vm.invoice)
                           .then(function (saveResult) {
                               vm.isSaving = false;
                           }, function (error) {
                               vm.isSaving = false;
                           })
            }
        }

    }
})();