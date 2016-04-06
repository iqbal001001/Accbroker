(function () {
    'use strict';

    var controllerId = 'paymentdetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window', '$filter',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            paymentdetail]);

    function paymentdetail($routeParams, $location, $scope, $rootScope, $window, $filter,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');
        var KeyCodes = config.KeyCodes;

        vm.activate = activate;
        vm.paymentIdParameter = $routeParams.id;
        vm.companyId = $rootScope.companyId;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.gotoPaymentItem = gotoPaymentItem;
        vm.addPaymentItem = addPaymentItem;
        vm.removePaymentItem = removePaymentItem;
        vm.newPaymentItem = false;
        vm.paymentItemSearch = '';
        vm.search = search;
        vm.newpayment;
        vm.deletePayment = deletePayment;
        vm.hasChanges = false;
        vm.isSaving = false;

        vm.btnSelectInvoice = btnSelectInvoice;

        vm.openedPaymentDate = false;
        vm.btnPaymentDate = btnPaymentDate;

        vm.invoices = [];

        vm.paymentTypes = [
          { "id": 1, "name": 'Credit Card' },
          { "id": 2, "name": 'Cheque' },
          { "id": 3, "name": 'Cash' }

        ];

        vm.clients = [];

        vm.selectedClientChanged = selectedClientChanged;
        vm.selectedPaymentTypeChanged = selectedPaymentTypeChanged;
        vm.selectedInvoiceChanged = selectedInvoiceChanged;

        vm.seqUp = seqUp;
        vm.seqDown = seqDown;


        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        Object.defineProperty(vm, 'canSeqUp', {
            get: canSeqUp
        });

        Object.defineProperty(vm, 'canSeqDown', {
            get: canSeqDown
        });

        function canSeqUp() {
            var prop = "sequenceNo";
            if (vm.newPaymentItem == true) return false;
            if (vm.paymentItems && vm.paymentItems.length > 0 && vm.paymentItem) {
                var seq = vm.paymentItem[prop];
                if (seq > 1)
                    return true;
                else
                    return false;
            }
        }

        function canSeqDown() {
            var prop = "sequenceNo";
            if (vm.newPaymentItem == true) return false;
            if (vm.paymentItems && vm.paymentItems.length > 0 && vm.paymentItem) {
                var seq = vm.paymentItem[prop];

                if (seq >= vm.paymentItems.length)
                    return false;
                else
                    return true;
            }
        }

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        activate();

        function activate() {
            common.activateController([getClients(), getRequestedPayment()], controllerId)
                                .then(function () { log('Activated Paymentdetail View'); });

        }

        function deletePayment() {
            return bsDialog.deleteDialog('Payment').
                then(confirmDelete);

            function confirmDelete() {
                datacontext.payment.deletePayment(vm.payment.id)
                .then(success, failed);

                function success() {
                    gotoPayments();
                }

                function failed(error) { cancel(); }
            }

        }

        function getClients() {
            return datacontext.client.getClients(null,
                null, null, null, null)
                .then(function (data) {
                    vm.clients = data.data;
                    //getInvoices()
                    return vm.clients;
                });
        }

        function getInvoices(clientId) {
            return datacontext.invoice.getInvoices(null,
                null, null, null, null, null, clientId)
                .then(function (data) {
                    vm.invoices = data.data;
                    return vm.invoices;
                });
        }


        function getRequestedPayment() {
            var val = $routeParams.id;
            if (val === 'new') {
                vm.payment = {};
                vm.payment.companyId = vm.companyId;
                vm.filteredPaymentItems = vm.paymentItems = [];
                vm.payment.paymentItems = vm.paymentItems;
                return vm.newpayment = true;
            }

            return datacontext.payment.getPayment(val)
            .then(function (data) {
                vm.payment = angular.copy(data.data);
                vm.originalPayment = angular.copy(data.data);
                if (!data.data.paymentItems) data.data.paymentItems = []
                vm.filteredPaymentItems = vm.paymentItems = data.data.paymentItems;
                if (vm.filteredPaymentItems && vm.filteredPaymentItems.length > 0) 
                    gotoPaymentItem(findByProperty(vm.filteredPaymentItems, "sequenceNo", 1)); //gotoPaymentItem(vm.filteredPaymentItems[0]);
                vm.newpayment = false;
            }, function (error) {
                logError('Unable to get payment ' + val);
                gotoPayments();
            });
        }

        function gotoPaymentItem(paymentItem) {
            if (paymentItem && paymentItem.id) {
                vm.newPaymentItem = false;
                var found = $filter('filter')(vm.paymentItems, { id: paymentItem.id }, true);
                if (found.length) {
                    getInvoices(vm.payment.clientID);
                    vm.paymentItem = found[0];
                    vm.selectedInvoiceChanged(); // TODO: Remove after adding invoice Details on the payment item DTO
                } else {
                    vm.paymentItem = 'Not found';
                }
            }
        }

        function seqUp() {
            var index1 = vm.paymentItem.sequenceNo;
            var index2 = index1 - 1;
            swap(index1, index2, vm.paymentItems, "sequenceNo");

        }

        function seqDown() {
            var index1 = vm.paymentItem.sequenceNo;
            var index2 = index1 + 1;
            swap(index1, index2, vm.paymentItems, "sequenceNo");
        }

        function swap(seq1, seq2, array, prop) {
            var index1 = findPositionByProperty(array, prop, seq1);
            var index2 = findPositionByProperty(array, prop, seq2);
            if (index1 <= array.length && index2 <= array.length) {
                var temp = array[index2][prop];
                array[index2][prop] = array[index1][prop];
                array[index1][prop] = temp;
            }
        }

        function findPositionByProperty(source, prop, value) {
            for (var i = 0; i < source.length; i++) {
                var x = source[i][prop];
                if (source[i][prop] == value) {
                    return i;
                }
            }
            //throw "Couldn't find object with id: " + id;
            return -1;
        }

        function findByProperty(source, prop, value) {
            if (source) {
                for (var i = 0; i < source.length; i++) {
                    var x = source[i][prop];
                    if (source[i][prop] == value) {
                        return source[i];
                    }
                }
                //throw "Couldn't find object with id: " + value;
            }
        }

        function addPaymentItem() {

            if (vm.newPaymentItem == false) {
                var newpaymentItem =
                    {
                        "id": 0,
                        "description": '',
                        "amount": 0,
                        "invoiceNo": '',
                        "invoiceId": '',
                        "invoiceDescription": '',
                        "invoiceGst": '',
                        "invoiceAmount": '',
                        "invoiceTotal": '',
                    }
                vm.newPaymentItem = true;
                vm.paymentItem = newpaymentItem;
            } else {
                vm.paymentItem.sequenceNo = vm.paymentItems.length + 1;
                vm.paymentItems.push(vm.paymentItem);
                vm.newPaymentItem = false;
            }
        }

        $scope.$watch("vm.filteredPaymentItems", function () {
            if (vm.payment && vm.paymentItem && vm.filteredPaymentItems) {

                //vm.paymentItem.total = parseFloat(vm.paymentItem.amount);

                vm.payment.amount = getsum("amount");
             
            }
            //$log.debug("    ** $watchCollection()");
        }, true);

        function getsum(prop) {
            var total = 0
            if (vm.filteredPaymentItems) {
                for (var i = 0, _len = vm.filteredPaymentItems.length; i < _len; i++) {
                    total += parseFloat(vm.filteredPaymentItems[i][prop]);
                }
            }
            return total
        }

        function search($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.paymentItemSearch = '';
            }
            vm.filteredPaymentItems = vm.paymentItems.filter(paymentItemsFilter);
            if (vm.filteredPaymentItems && vm.filteredPaymentItems.length == 0) {
                addPaymentItem()
            }
            else {
                vm.paymentItem = vm.filteredPaymentItems[0];
                vm.newPaymentItem = false;
            }
        }

        function paymentItemsFilter(paymentItem) {
            var isMatch = vm.paymentItemSearch
            ? common.textContains(paymentItem.description, vm.paymentItemSearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }

        function removePaymentItem(idx) {
            if (vm.paymentItems && vm.paymentItems.length > 0) {
                while (vm.canSeqDown) {
                    vm.seqDown();
                };
                var pos = findPositionByProperty(vm.paymentItems, "sequenceNo", vm.paymentItems.length)
                vm.paymentItems.splice(pos, 1);
                vm.paymentItem = [];
                // vm.filteredPaymentItems.splice(idx, 1);
            }
        }

        function cancel() {
            gotoPayments();
        }

        function gotoPayments() {
            $location.path('/payments');
        }

        function goBack() { $window.history.back(); }


        function save() {
            if (vm.payment.id == null) {
                vm.newpayment = true;
                return SavePayment();
            }
            return datacontext.payment.getPayment(vm.payment.id)
           .then(function (data) {
               if (data != null)
               { vm.newpayment = false; }
               else
               { vm.newpayment = true; }

               SavePayment();
           },
               function (error) {
                   if (error.status == 404) {
                       vm.newpayment = true;
                       SavePayment();
                   }
               })

        }

        function SavePayment() {
            vm.isSaving = true;
            if (vm.newpayment === true) {
                return datacontext.payment.savePayment(vm.payment)
                    .then(function (saveResult) {
                        vm.isSaving = false;
                    }, function (error) {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.payment.updatePayment(vm.payment.id, vm.payment)
                           .then(function (saveResult) {
                               vm.isSaving = false;
                           }, function (error) {
                               vm.isSaving = false;
                           })
            }
        }

        function btnSelectInvoice(item) {
            //return bsDialog.editDialog('Product', '', vm);
            //$rootScope.selectedInvocie = item.invoice;
            bsDialog.confirmationDialogfromPage('Invoice', 'app/invoice/invoice.html')
                .then(function () { item.invoice = null; })
            ;

        }

        function btnPaymentDate($event) {
            $event.preventDefault();
            $event.stopPropagation();
            vm.openedPaymentDate = true;
        };

        function selectedClientChanged() {
            var client = vm.payment.clientID;
            var found = $filter('filter')(vm.clients, { id: vm.payment.clientID }, true);
            if (found.length) {
                vm.payment.clientName = found[0].name.trim();
                getInvoices(found[0].id);
            } else {
            }
        }

        function selectedPaymentTypeChanged() {
            var Type = vm.payment.paymentType;
        }

        function selectedInvoiceChanged() {
            var product = vm.paymentItem.invoiceId;
            var found = $filter('filter')(vm.invoices, { id: vm.paymentItem.invoiceID }, true);
            if (found.length) {
                vm.paymentItem.invoiceNo = found[0].invoiceNo.trim();
                vm.paymentItem.invoiceDescription = found[0].invoiceDescription.trim();
                vm.paymentItem.invoiceGst = found[0].gst;
                vm.paymentItem.invoiceAmount = found[0].amount;
                vm.paymentItem.invoiceTotal = parseFloat(vm.paymentItem.invoiceGst) + parseFloat(vm.paymentItem.invoiceAmount);
                //vm.paymentItem.amount = vm.paymentItem.invoiceTotal;
                // $scope.selected = JSON.stringify(found[0]);
            } else {
                //$scope.selected = 'Not found';
            }
        }

        $scope.$watch('vm.payment', function (newValue, oldValue) {
            if (newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.payment, vm.originalPayment);
            }
        }, true);


    }
})();