(function () {
    'use strict';

    var controllerId = 'productdetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext', '$q',
            productdetail]);

    function productdetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext, $q) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');


        vm.activate = activate;
        vm.productIdParameter = $routeParams.id;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.checkCode = checkCode;
        vm.newproduct;
        vm.deleteProduct = deleteProduct;
        vm.hasChanges = false;
        vm.isSaving = false;

        vm.guid;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        activate();

        function activate() {
            common.activateController([getRequestedProduct()], controllerId)
                                .then(function () { log('Activated Productdetail View'); });

        }

        function deleteProduct() {
            return bsDialog.deleteDialog('Product').
                then(confirmDelete);

            function confirmDelete() {
                datacontext.product.deleteProduct(vm.product.id)
                .then(success, failed);

                function success() {
                    gotoProducts();
                }

                function failed(error) { cancel(); }
            }

        }

        function getRequestedProduct() {
            var val = $routeParams.id;
            if (val === 'new') {
                vm.guid = common.createGuid();
                return vm.newproduct = true;
            }

            var wip = datacontext.localStorage.get(
                { entity: 'product', id: val });

            if (wip) {
                if (wip.state == 'Add') {
                    vm.guid = wip.id;
                    vm.newproduct = true;
                }
                else {
                    vm.newproduct = false;
                }
                vm.product = wip.current;
                vm.originalProduct = wip.original;
                //bindData(vm.product);
                return vm.hasChanges = true;
            }

            return datacontext.product.getProduct(val)
            .then(function (data) {
                vm.product = angular.copy(data.data);
                vm.originalProduct = angular.copy(data.data);
                vm.newproduct = false;
            }, function (error) {
                logError('Unable to get product ' + val);
                gotoProducts();
            });
        }



        function cancel() {
            vm.product = angular.copy(vm.originalProduct);
            removeFromStore();
            //gotoProducts();
        }

        function gotoProducts() {
            $location.path('/products');
        }

        function goBack() { $window.history.back(); }


        function save() {
            SaveProduct();
           // if (vm.product.id == null) {
           //     vm.newproduct = true;
           //     return SaveProduct();
           // }
           // return datacontext.product.getProduct(vm.product.id)
           //.then(function (data) {
           //    if (data != null)
           //    { vm.newproduct = false; }
           //    else
           //    { vm.newproduct = true; }

           //    SaveProduct();
           //},
           //    function (error) {
           //        if (error.status == 404) {
           //            vm.newproduct = true;
           //            SaveProduct();
           //        }
           //    })

        }

        function SaveProduct() {
            vm.isSaving = true;
            if (vm.newproduct === true) {
                return datacontext.product.saveProduct(vm.product)
                    .then(function (saveResult) {
                        vm.client.id = saveResult.data.id;
                        removeFromStore();
                        vm.originalProduct = angular.copy(vm.product);
                        vm.newclient = false;
                        vm.hasChanges = false;
                    }, function (error) {

                    }).finally(function () {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.product.updateProduct(vm.product.id, vm.product)
                           .then(function (saveResult) {
                               removeFromStore();
                               vm.originalProduct = angular.copy(vm.product);
                               vm.hasChanges = false;
                           }, function (error) {

                           }).finally(function () {
                               vm.isSaving = false;
                           })
            }
        }

        $scope.$watch('vm.product', function (newValue, oldValue) {
            if (newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.product, vm.originalProduct);
                if (vm.hasChanges) {
                    var obj = {
                        entity: 'product',
                        id: vm.product.id,
                        orginal: vm.originalProduct,
                        current: vm.product,
                        route: '/product',
                        state: vm.newclient == true ? 'Add' : 'update',
                        date: new Date()
                    };
                    datacontext.localStorage.add(obj);
                }
            }
        }, true);

        function addToStore() {
            var obj = {
                entity: 'product',
                id: vm.newproduct == true ? vm.guid : vm.product.id,
                orginal: vm.originalProduct,
                current: vm.product,
                description: vm.product.name,
                route: '/product',
                state: vm.newproduct == true ? 'Add' : 'update',
                date: new Date()
            };
            datacontext.localStorage.add(obj);
        }

        function removeFromStore() {
            datacontext.localStorage.remove({ entity: 'product', id: vm.newproduct == true ? vm.guid : vm.product.id });
        }

        function checkCode(code) {
            if (code) {
                return $q(function (resolve, reject) {
                    datacontext.product.codeAvailable(vm.product !=  null ? vm.product.id : null  , code)
                                            //.$promise
                                            .then(function (result) {
                                                if (result.data) {
                                                    if (result.data.codeAvailable == true) {
                                                        resolve(result);
                                                    } else {
                                                        reject(result);
                                                    }
                                                } else {
                                                    reject(result);
                                                }
                                            },
                                            function (error) {
                                                resolve("unexpected error");
                                            });
                });
            } else {
                return $q(function (resolve) {
                    resolve();
                });
            }
        }
    }
})();