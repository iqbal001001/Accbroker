(function () {
    'use strict';

    var serviceId = 'repository.product';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryProduct]);

    function RepositoryProduct(AbstractRepository, $http) {
        var entityName = 'Product';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getProducts = getProducts;
            this.getCount = getCount;
            this.getProduct = getProduct;
            this.saveProduct = saveProduct;
            this.deleteProduct = deleteProduct;
            this.updateProduct = updateProduct;
            this.getProductCount = getProductCount;
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getProducts(sort, page, pageSize, feilds, search) {
            var query = "?";

            if (sort != null) { query = "sort=" + sort; }

            if (page != null) {
                if (query != "?") { query = query + "&" }
                query = query + "page=" + page;
            }

            if (pageSize != null) {
                if (query != "?") { query = query + "&" }
                query = query + "pageSize=" + pageSize;
            }

            if (feilds != null) {
                if (query != "?") { query = query + "&" }
                query = query + "feilds=" + feilds;
            }

            if (search != null) {
                if (query != "?") { query = query + "&" }
                query = query + "searchCode=" + search;
            }

            return $http.get(this.hostEndPoint + "/api/Product" + query);
        }

        function getCount() {
            return $http.get(this.hostEndPoint + "/api/Product");
        }

        function getProduct(ComNo) {
            //http://localhost:57148
            return $http.get(this.hostEndPoint + "/api/Product/" + ComNo);
        }

        function getProductCount() {

            return $http.get(this.hostEndPoint + "/api/Product/" + ComNo);
        }

        function saveProduct(Product) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };

            var request = $http({
                method: "post",
                headers: headers,
                url: this.hostEndPoint + "/api/Product",
                data: Product
            });
            return request
        }

        function deleteProduct(ComNo) {
            var headers = {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            };

            var request = $http({
                method: "delete",
                //headers:  headers,
                url: this.hostEndPoint + "/api/Product/" + ComNo
            });

            return request;
        }

        function updateProduct(ComNo, Product) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };

            var request = $http({
                method: "put",
                headers: headers,
                url: this.hostEndPoint + "/api/Product/" + ComNo,
                data: Product
            });
            return request;
        }


    }
})();




