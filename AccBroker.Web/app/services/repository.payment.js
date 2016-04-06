(function () {
    'use strict';

    var serviceId = 'repository.payment';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryPayment]);

    function RepositoryPayment(AbstractRepository, $http) {
        var entityName = 'Payment';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getPayments = getPayments;
            this.getCount = getCount;
            this.getPayment = getPayment;
            this.savePayment = savePayment;
            this.deletePayment = deletePayment;
            this.updatePayment = updatePayment;
            this.getPaymentCount = getPaymentCount;
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getPayments(sort, page, pageSize, feilds,
            searchPaymentNo, searchCompanyId, searchClientId) {
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

            if (searchPaymentNo != null) {
                if (query != "?") { query = query + "&" }
                query = query + "searchPaymentNo=" + searchPaymentNo;
            }

            if (searchCompanyId != null) {
                if (query != "?") { query = query + "&" }
                query = query + "searchCompanyId=" + searchCompanyId;
            }

            if (searchClientId != null) {
                if (query != "?") { query = query + "&" }
                query = query + "searchClientId=" + searchClientId;
            }

            return $http.get(this.hostEndPoint + "/api/Payment" + query);
        }

        function getCount() {
            return $http.get(this.hostEndPoint + "/api/Payment");
        }

        function getPayment(No) {
            //http://localhost:57148
            return $http.get(this.hostEndPoint + "/api/Payment/" + No);
        }

        function getPaymentCount() {

            return $http.get(this.hostEndPoint + "/api/Payment/" + No);
        }

        function savePayment(Payment) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };

            var request = $http({
                method: "post",
                headers: headers,
                url: this.hostEndPoint + "/api/Payment",
                data: Payment
            });
            return request
        }

        function deletePayment(No) {
            var headers = {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            };

            var request = $http({
                method: "delete",
                //headers:  headers,
                url: this.hostEndPoint + "/api/Payment/" + No
            });

            return request;
        }

        function updatePayment(No, Payment) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };
            var request = $http({
                method: "put",
                headers: headers,
                url: this.hostEndPoint + "/api/Payment/" + No,
                data: Payment
            });
            return request;
        }


    }
})();




