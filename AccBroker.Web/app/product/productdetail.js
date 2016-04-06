(function () {
    'use strict';

    var controllerId = 'productdetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            productdetail]);

    function productdetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');


        vm.activate = activate;
        vm.productIdParameter = $routeParams.id;
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.newproduct;
        vm.deleteProduct = deleteProduct;
        vm.hasChanges = false;
        vm.isSaving = false;

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
                return vm.newproduct = true;
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
            gotoProducts();
        }

        function gotoProducts() {
            $location.path('/products');
        }

        function goBack() { $window.history.back(); }


        function save() {
            if (vm.product.id == null) {
                vm.newproduct = true;
                return SaveProduct();
            }
            return datacontext.product.getProduct(vm.product.id)
           .then(function (data) {
               if (data != null)
               { vm.newproduct = false; }
               else
               { vm.newproduct = true; }

               SaveProduct();
           },
               function (error) {
                   if (error.status == 404) {
                       vm.newproduct = true;
                       SaveProduct();
                   }
               })

        }

        function SaveProduct() {
            vm.isSaving = true;
            if (vm.newproduct === true) {
                return datacontext.product.saveProduct(vm.product)
                    .then(function (saveResult) {
                        vm.isSaving = false;
                        vm.originalProduct = angular.copy(vm.product);
                    }, function (error) {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.product.updateProduct(vm.product.id, vm.product)
                           .then(function (saveResult) {
                               vm.isSaving = false;
                               vm.originalProduct = angular.copy(vm.product);
                           }, function (error) {
                               vm.isSaving = false;
                           })
            }
        }

        $scope.$watch('vm.product', function (newValue, oldValue) {
            if (newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.product, vm.originalProduct);
            }
        }, true);

    }
})();