(function () {
    'use strict';

    var controllerId = 'invoicedetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window', '$filter',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            invoicedetail]);

    function invoicedetail($routeParams, $location, $scope, $rootScope, $window, $filter,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');
        var KeyCodes = config.KeyCodes;

        vm.detailTitle = 'Invoice Detail'
        vm.activate = activate;
        vm.invoiceIdParameter = $routeParams.id;
        vm.companyId = $rootScope.companyId;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.gotoInvoiceItem = gotoInvoiceItem;
        vm.gotoPayment = gotoPayment;
        vm.addInvoiceItem = addInvoiceItem;
        vm.removeInvoiceItem = removeInvoiceItem;
        vm.newInvoiceItem = false;
        vm.invoiceItemSearch = '';
        vm.search = search;
        vm.newinvoice;
        vm.deleteInvoice = deleteInvoice;
        vm.hasChanges = false;
        vm.isSaving = false;

        vm.openedInvoiceDate = false;
        vm.btnInvoiceDate = btnInvoiceDate;
        vm.openedDueDate = false;
        vm.btnDueDate = btnDueDate;

        vm.showPayment = false;
        vm.paymentTotal = 0.0;

        vm.guid;

        vm.invoiceItemTypes = [
            { "id": 1, "name": 'Product' },
            { "id": 2, "name": 'Json' }
           
        ];

        vm.clients = [];
        vm.products = [];

        vm.selectedClientChanged = selectedClientChanged;
        vm.selectedTypeChanged = selectedTypeChanged;
        vm.selectedProductChanged = selectedProductChanged;

        vm.seqUp = seqUp;
        vm.seqDown = seqDown;

        //Object.defineProperty(vm, 'hasChanged', {
        //    get: vm.hasChanged
        //});

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
            if (vm.newInvoiceItem == true) return false;
            if (vm.invoiceItems && vm.invoiceItems.length > 0 && vm.invoiceItem) {
                var seq = vm.invoiceItem[prop];
                    if (seq > 1)
                        return true;
                    else
                        return false;
            }
        }

        function canSeqDown() {
            var prop = "sequenceNo";
            if (vm.newInvoiceItem == true) return false;
            if (vm.invoiceItems && vm.invoiceItems.length > 0 && vm.invoiceItem) {
                var seq = vm.invoiceItem[prop];

                if (seq >= vm.invoiceItems.length)
                    return false;
                else
                    return true;
            }
        }

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        activate();

        function activate() {
            //onDestroy();
            onHasChanges();
            //initLookups();
            common.activateController([getProducts(),getClients(),getRequestedInvoice()], controllerId)
                                .then(function () {
                                    log('Activated Invoicedetail View');
                                });

        }

        function btnInvoiceDate($event) {
            $event.preventDefault();
            $event.stopPropagation();
            vm.openedInvoiceDate = true;
        };

        function btnDueDate($event) {
            $event.preventDefault();
            $event.stopPropagation();
            vm.openedDueDate = true;
        };

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

        function getClients() {
            return datacontext.client.getClients(null,
                null, null, null, null)
                .then(function (data) {
                    vm.clients = data.data;
                    return vm.clients;
                });
        }

        function getProducts() {
            return datacontext.product.getProducts(null,
                null, null, null, null)
                .then(function (data) {
                    vm.products = data.data;
                    return vm.products;
                });
        }

        function getRequestedInvoice() {
            var val = $routeParams.id;
            if (val === 'new') {
                vm.guid = common.createGuid();
                vm.invoice = {};
                vm.invoice.companyId = vm.companyId;
                vm.filteredInvoiceItems = vm.invoiceItems = [];
                vm.invoice.invoiceItems = vm.invoiceItems;
                return vm.newinvoice = true; 
            }

            var wip = datacontext.localStorage.get(
                { entity: 'invoice', id: val });

            if (wip) {
                if (wip.state == 'Add') {
                    vm.guid = wip.id;
                    vm.newinvoice = true;
                }
                else {
                    vm.newinvocie = false;
                }
                vm.invoice = wip.current;
                vm.originalInvoice = wip.original;
                bindData(vm.invoice);
                return vm.hasChanges = true;
            }

            return datacontext.invoice.getInvoice(val)
            .then(function (data) {
                vm.invoice = angular.copy(data.data);
                vm.originalInvoice = angular.copy(data.data);
                bindData(data.data);
                vm.newinvoice = false;
            }, function (error) {
                logError('Unable to get invoice ' + val);
                gotoInvoices();
            });
        }

        function bindData(data) {
                if (!data.invoiceItems) data.invoiceItems = [];
                vm.filteredInvoiceItems = vm.invoiceItems = data.invoiceItems;
                vm.paymentItems = data.paymentItems;
                vm.paymentTotal = getsum(vm.paymentItems,"amount")
                if (vm.filteredInvoiceItems && vm.filteredInvoiceItems.length > 0)
                    gotoInvoiceItem(findByProperty(vm.filteredInvoiceItems, "sequenceNo", 1));  //gotoInvoiceItem(vm.filteredInvoiceItems[0]);
               // vm.newinvoice = false;
        }

        function gotoInvoiceItem(invoiceItem, idx) {
            if (invoiceItem ) {   //&& invoiceItem.id
                vm.newInvoiceItem = false;
                var found = $filter('filter')(vm.invoiceItems, {id: invoiceItem.id}, true);
                if (found.length) {
                    vm.invoiceItem = found[0];
                } else {
                    vm.invoiceItem = 'Not found';
                }
            }
        }

        function gotoPayment(paymentItem) {
            if (paymentItem && paymentItem.PaymentId) {
                $location.path('payment/' + paymentItem.PaymentId);
            }
        }

        function seqUp() {
            var index1 = vm.invoiceItem.sequenceNo;
            var index2 = index1 - 1;
            swap(index1, index2, vm.invoiceItems,"sequenceNo");
            
        }

        function seqDown() {
            var index1 = vm.invoiceItem.sequenceNo;
            var index2 = index1 + 1;
            swap(index1, index2, vm.invoiceItems, "sequenceNo");
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
               // throw "Couldn't find object with id: " + id;
            }
        }

        function addInvoiceItem() {
            
            if (vm.newInvoiceItem == false) {
                var productInvoiceItem = 
                    {
                        "invoiceItemID" : 0,
                        "productID" : 0,
                        "productCode" : '',
                        "productName" : '',
                        "quantity" : 0.0,
                        "unitPrice" : 0.0
                    }
                var newinvoiceItem =
                    {
                        "id": 0,
                        "invoiceType": 1,
                        "description": '',
                        "discount" : 0,
                        "gst": 0,
                        "amount": 0,
                        "productInvoiceItem": productInvoiceItem
                    }
                vm.newInvoiceItem = true;
                vm.invoiceItem = newinvoiceItem;
            } else {
                vm.invoiceItem.sequenceNo = vm.invoiceItems.length + 1;
                vm.invoiceItems.push(vm.invoiceItem);
                vm.newInvoiceItem = false;
            }
        }

        function removeInvoiceItem(idx) {
            if (vm.invoiceItems && vm.invoiceItems.length > 0) {
                while (vm.canSeqDown) {
                    vm.seqDown();
                };
                var pos = findPositionByProperty(vm.invoiceItems, "sequenceNo", vm.invoiceItems.length)
                vm.invoiceItems.splice(pos, 1);
                //   vm.filteredInvoiceItems.splice(idx, 1);
            }
        };
      
        $scope.$watch("vm.filteredInvoiceItems", function () {
            if (vm.invoice && vm.invoiceItem && vm.filteredInvoiceItems) {

                //vm.invoiceItem.gst = parseFloat(vm.invoiceItem.amount * 0.10);
                //vm.invoiceItem.total = parseFloat(vm.invoiceItem.amount) + parseFloat(vm.invoiceItem.gst);

                vm.invoice.gst = getsum(vm.filteredInvoiceItems,"gst");
                vm.invoice.amount = getsum(vm.filteredInvoiceItems,"amount");
                vm.invoice.total = getsum(vm.filteredInvoiceItems,"total");
            }
            //$log.debug("    ** $watchCollection()");
        }, true);

        $scope.$watch("vm.invoiceItem", function () {
            if (vm.invoice && vm.invoiceItem && vm.invoiceItem.productInvoiceItem) {

                if (vm.invoiceItem.productInvoiceItem.quantity > 0 && vm.invoiceItem.productInvoiceItem.unitPrice) {
                    vm.invoiceItem.amount =
                        parseFloat(vm.invoiceItem.productInvoiceItem.quantity) * parseFloat(vm.invoiceItem.productInvoiceItem.unitPrice);

                    vm.invoiceItem.gst = (parseFloat(vm.invoiceItem.amount) - parseFloat(vm.invoiceItem.discount)) * 0.10;
                    vm.invoiceItem.total = (parseFloat(vm.invoiceItem.amount) - parseFloat(vm.invoiceItem.discount)) + parseFloat(vm.invoiceItem.gst);

                }
            }
            //$log.debug("    ** $watchCollection()");
        }, true);

        function getsum(array,prop) {
            var total = 0
            if (array) {
                for (var i = 0, _len = array.length; i < _len; i++) {
                    total += parseFloat(array[i][prop]);
                }
            }
            return total
        }

        function search($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.invoiceItemSearch = '';
            }
            vm.filteredInvoiceItems = vm.invoiceItems.filter(invoiceItemsFilter);
            if (vm.filteredInvoiceItems && vm.filteredInvoiceItems.length == 0) {
                addInvoiceItem()
            }
            else {
                vm.invoiceItem = vm.filteredInvoiceItems[0];
                vm.newInvoiceItem = false;
            }
        }

        function invoiceItemsFilter(invoiceItem) {
            var isMatch = vm.invoiceItemSearch
            ? common.textContains(invoiceItem.description, vm.invoiceItemSearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }

        function gotoInvoices() {
            $location.path('/invoices');
        }

        function goBack() { $window.history.back(); }

        function cancel() {
            vm.invoice = angular.copy(vm.originalInvoice);
            removeFromStore();
            //datacontext.cancel();
            //removeWipEntity();
            //helper.replaceLocationUrlGuidWithId(vm.employee.employeeID);
            //if (vm.employee.entityAspect.entityState.isDetached()) {
           // gotoInvoices();
            //}
        }

        function save() {
            SaveInvoice();
           // if (vm.invoice.id == null) {
           //     vm.newinvoice = true;
           //     return SaveInvoice();
           // }
           // return datacontext.invoice.getInvoice(vm.invoice.id)
           //.then(function (data) {
           //    if (data != null)
           //    { vm.newinvoice = false; }
           //    else
           //    { vm.newinvoice = true; }

           //    SaveInvoice();
           //},
           //    function (error) {
           //        if (error.status == 404) {
           //            vm.newinvoice = true;
           //            SaveInvoice();
           //        }
           //    })

        }

        function SaveInvoice() {
            vm.isSaving = true;
            if (vm.newinvoice === true) {
                return datacontext.invoice.saveInvoice(vm.invoice)
                    .then(function (saveResult) {
                        vm.invoice.id = saveResult.data.id;
                        removeFromStore();
                        vm.originalInvocie = angular.copy(vm.invoice);
                        vm.newinvoice = false;
                        vm.hasChanges = false;
                    }, function (error) {

                    }).finally(function () {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.invoice.updateInvoice(vm.invoice.id, vm.invoice)
                           .then(function (saveResult) {
                               removeFromStore();
                               vm.originalInvoice = angular.copy(vm.invoice);
                               vm.hasChanges = false;
                           }, function (error) {

                           }).finally(function () {
                               vm.isSaving = false;
                           })
            }
        }

        function selectedClientChanged() {
            var client = vm.invoice.clientID;
            var found = $filter('filter')(vm.clients, { id: vm.invoice.clientID }, true);
            if (found.length) {
                vm.invoice.clientName = found[0].name.trim();
                //vm.invoiceItem.productInvoiceItem.productName = found[0].productName.trim();
                //vm.invoiceItem.productInvoiceItem.unitPrice = found[0].sellPrice;
                // $scope.selected = JSON.stringify(found[0]);
            } else {
                //$scope.selected = 'Not found';
            }
        }

        function selectedTypeChanged() {
            var Type = vm.invoiceItem.invoiceType;
        }

        function selectedProductChanged() {
            var product = vm.invoiceItem.productInvoiceItem.productID;
            var found = $filter('filter')(vm.products, { id: vm.invoiceItem.productInvoiceItem.productID }, true);
            if (found.length) {
                vm.invoiceItem.productInvoiceItem.productCode = found[0].productCode.trim();
                vm.invoiceItem.productInvoiceItem.productName = found[0].productName.trim();
                vm.invoiceItem.productInvoiceItem.unitPrice = found[0].sellPrice;
               // $scope.selected = JSON.stringify(found[0]);
            } else {
                //$scope.selected = 'Not found';
            }
        }

        $scope.$watch('vm.invoice', function(newValue, oldValue) {
            if(newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.invoice, vm.originalInvoice);
                if (vm.hasChanges) {
                    addToStore();
                }
            }
        },true);

        function addToStore() {
            var obj = {
                entity: 'invoice',
                id: vm.newinvoice == true ? vm.guid : vm.invoice.id,
                orginal: vm.originalInvoice,
                current: vm.invoice,
                description: vm.invoice.name,
                route: '/invoice',
                state: vm.newinvoice == true ? 'Add' : 'update',
                date: new Date()
            };
            datacontext.localStorage.add(obj);
        }

        function removeFromStore() {
            datacontext.localStorage.remove(
                { entity: 'invoice', id: vm.newinvoice == true ? vm.guid : vm.invoice.id });
        }

        function onHasChanges() {
            //$scope.$on(config.events.hasChangesChanged,
            //    function (event, data) {
            //        vm.hasChanges = data.hasChanges;
            //    });
            
        }
    }
})();