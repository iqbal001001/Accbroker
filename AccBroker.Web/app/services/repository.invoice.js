(function () {
    'use strict';

    var serviceId = 'repository.invoice';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryInvoice]);

    function RepositoryInvoice(AbstractRepository, $http) {
        var entityName = 'Invoice';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getInvoices = getInvoices;
            this.getCount = getCount;
            this.getInvoice = getInvoice;
            this.saveInvoice = saveInvoice;
            this.deleteInvoice = deleteInvoice;
            this.updateInvoice = updateInvoice;
            this.getInvoiceCount = getInvoiceCount;
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getInvoices(sort, page, pageSize, feilds,
            searchInvoiceNo, searchCompanyId, searchClientId) {
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

            if (searchInvoiceNo != null) {
                if (query != "?") { query = query + "&" }
                query = query + "searchInvoiceNo=" + searchInvoiceNo;
            }

            if (searchCompanyId != null) {
                if (query != "?") { query = query + "&" }
                query = query + "searchCompanyId=" + searchCompanyId;
            }

            if (searchClientId != null) {
                if (query != "?") { query = query + "&" }
                query = query + "searchClientId=" + searchClientId;
            }

            return $http.get(this.hostEndPoint + "/api/Invoice" + query);
        }

        function getCount() {
            return $http.get(this.hostEndPoint + "/api/Invoice");
        }

        function getInvoice(ComNo) {
            //http://localhost:57148
            return $http.get(this.hostEndPoint + "/api/Invoice/" + ComNo);
        }

        function getInvoiceCount() {

            return $http.get(this.hostEndPoint + "/api/Invoice/" + ComNo);
        }

        function saveInvoice(Invoice) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };

            var request = $http({
                method: "post",
                headers: headers,
                url: this.hostEndPoint + "/api/Invoice",
                data: Invoice
            });
            return request
        }

        function deleteInvoice(ComNo) {
            var headers = {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            };

            var request = $http({
                method: "delete",
                //headers:  headers,
                url: this.hostEndPoint + "/api/Invoice/" + ComNo
            });

            return request;
        }

        function updateInvoice(ComNo, Invoice) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };

            var request = $http({
                method: "put",
                headers: headers,
                url: this.hostEndPoint + "/api/Invoice/" + ComNo,
                data: Invoice
            });
            return request;
        }


    }
})();




