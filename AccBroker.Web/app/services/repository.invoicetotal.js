(function () {
    'use strict';

    var serviceId = 'repository.invoicetotal';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryInvoiceTotal]);

    function RepositoryInvoiceTotal(AbstractRepository, $http) {
        var api = '/api/InvoiceTotal';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.api = api;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getInvoices = getInvoices;
            this.getInvoicesByInvoiceDate = getInvoicesByInvoiceDate;
            this.getInvoicesByDueDate = getInvoicesByDueDate;
            this.getInvoicesByCompany = getInvoicesByCompany;
            this.getInvoicesByClient = getInvoicesByClient;
            this.getCount = getCount;
            this.getInvoice = getInvoice;
           
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getInvoices(sort, page, pageSize, feilds,
            clientid, companyid,
            startinvoicedate, endinvoicedate,
            startduedate, endduedate,
            paid,search) {
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
                query = query + "searchInvoiceNo=" + search;
            }

            if (paid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "paid=" + paid;
            }

            if (clientid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "clientid=" + clientid;
            }

            if (companyid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "companyid=" + companyid;
            }

            if (startinvoicedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "startinvoicedate=" + JSON.stringify(startinvoicedate);
            }

            if (endinvoicedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "endinvoicedate=" + JSON.stringify(endinvoicedate);
            }

            if (startduedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "startduedate=" + JSON.stringify(startduedate);
            }

            if (endduedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "endduedate=" + JSON.stringify(endduedate);
            }

            return $http.get(this.hostEndPoint + api + query);
        }

        function getInvoicesByInvoiceDate(startinvoicedate, endinvoicedate,paid) {
            var query = "?";

            if (paid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "paid=" + paid;
            }

            if (startinvoicedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "startinvoicedate=" + startinvoicedate;
            }

            if (endinvoicedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "endinvoicedate=" + endinvoicedate;
            }

            return $http.get(this.hostEndPoint + api + "/InvoiceDate" + query);
        }

        function getInvoicesByDueDate(startduedate, endduedate, paid) {
            var query = "?";

            if (paid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "paid=" + paid;
            }

            if (startduedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "startDueDate=" + encodeURIComponent(startduedate);
            }

            if (endduedate != null) {
                if (query != "?") { query = query + "&" }
                query = query + "endDueDate=" + encodeURIComponent(endduedate);
            }

            return $http.get(this.hostEndPoint + api + "/DueDate" + query);
        }

        function getInvoicesByClient(clientid, paid) {
            var query = "?";

            if (paid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "paid=" + paid;
            }

            if (clientid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "clientid=" + clientid;
            }

            return $http.get(this.hostEndPoint + api + "/Client" + query);
        }

        function getInvoicesByCompany(companyid, paid) {
            var query = "?";

            if (paid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "paid=" + paid;
            }

            if (companyid != null) {
                if (query != "?") { query = query + "&" }
                query = query + "companyid=" + companyid;
            }

            return $http.get(this.hostEndPoint + api + "/Company" + query);
        }

        function getCount() {
            return $http.get(this.hostEndPoint + "/api/Invoice");
        }

        function getInvoice(ComNo) {
            //http://localhost:57148
            return $http.get(this.hostEndPoint + "/api/Invoice/" + ComNo);
        }
    }
})();




