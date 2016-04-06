(function () {
    'use strict';

    var serviceId = 'repository.address';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryAddress]);

    function RepositoryAddress(AbstractRepository, $http) {
        var entityName = 'Address';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getAddresses = getAddresses;
            this.getCount = getCount;
            this.getAddress = getAddress;
            this.saveAddress = saveAddress;
            this.deleteAddress = deleteAddress;
            this.updateAddress = updateAddress;
            this.getAddressCount = getAddressCount;
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getAddresses(sort, page, pageSize, feilds) {
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

            return $http.get(this.hostEndPoint + "/api/Address" + query);
        }

        function getCount() {
            return $http.get(this.hostEndPoint + "/api/Address");
        }

        function getAddress(ComNo) {
            //http://localhost:57148
            return $http.get(this.hostEndPoint + "/api/Address/" + ComNo);
        }

        function getAddressCount() {

            return $http.get(this.hostEndPoint + "/api/Address/" + ComNo);
        }

        function saveAddress(Address) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };

            var request = $http({
                method: "post",
                headers: headers,
                url: this.hostEndPoint + "/api/Address",
                data: Address
            });
            return request
        }

        function deleteAddress(ComNo) {
            var headers = {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            };

            var request = $http({
                method: "delete",
                //headers:  headers,
                url: this.hostEndPoint + "/api/Address/" + ComNo
            });

            return request;
        }

        function updateAddress(ComNo, Address) {

            var request = $http({
                method: "put",
                url: this.hostEndPoint + "/api/Address/" + ComNo,
                data: Address
            });
            return request;
        }


    }
})();




