(function () {
    'use strict';
    var controllerId = 'product';
    angular.module('app').controller(controllerId,
        ['common', 'datacontext', '$location', '$routeParams', 'config', product]);

    function product(common, datacontext, $location, $routeParams, config) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var KeyCodes = config.KeyCodes;


        var vm = this;

        vm.productCount = 0;
        vm.products = [];
        vm.title = 'Product';
        vm.gotoProduct = gotoProduct;

        vm.productSearch = $routeParams.search || '';
        vm.search = search;
        vm.searchCode = searchCode;
        vm.filteredProducts = [];
        vm.companeisFilter = productsFilter;
        vm.productCount = 0;
        vm.productFilteredCount = 0;
        vm.pageChanged = pageChanged;
        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 5,
            pageSize: 10,
            totalCount: 0,
            totalPages: 0
        };

        Object.defineProperty(vm.paging, 'pageCount', {
            get: function () {
                return Math.floor(vm.productFilteredCount / vm.paging.pageSize) + 1;
            }
        });

        activate();

        function activate() {
            var promises = [getProducts()];
            common.activateController(promises, controllerId)
                .then(function () {
                    // applyFilter = common.createSearchThrottle(vm, 'products');
                    if (vm.productSearch) { applyFilter(true); }
                    log('Activated Product View');
                });
        }

        function getProductsCount() {
            return datacontext.product.getCount()
                .then(function (data) {
                    return vm.productCount = data.data.length;
                });
        }

        function getProductFilteredCount() {
            vm.productFilteredCount = datacontext.product.getFilteredCount(vm.productSearch);
        }

        function getProducts() {
            return datacontext.product.getProducts(null,
                vm.paging.currentPage, vm.paging.pageSize, null, vm.productSearch)
                .then(function (data) {
                    vm.filteredProducts = vm.products = data.data;
                    vm.productFilteredCount = vm.productCount = data.data.length;
                    console.log(data.headers());

                     var responsePagination = JSON.parse(data.headers("x-pagination"));
                    vm.paging.currentPage = responsePagination.currentPage;
                    vm.paging.pageSize = responsePagination.pageSize;
                    vm.paging.totalCount = responsePagination.totalCount

                    return vm.filteredProducts;
                });
        }

        function gotoProduct(product) {
            if (product && product.id) {
                $location.path('product/' + product.id);
            }
        }

        function pageChanged(page) {
            if (!page) { return; }
            vm.paging.currentPage = page;
            getProducts();
        }

        function refresh() {
            //  getProducts(true);
        }

        function search($event) {
            if ($event.keyCode === KeyCodes.esc) {
                vm.productSearch = '';
            }
            //applyFilter();
        }

        function searchCode() {
            getProducts();
        }

        function applyFilter() {
            vm.filteredProducts = vm.products.filter(productsFilter);
            vm.productFilteredCount = vm.filteredProducts.length;
        }

        function productsFilter(product) {
            var isMatch = vm.productSearch
            ? common.textContains(product.name, vm.productSearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }
    }
})();