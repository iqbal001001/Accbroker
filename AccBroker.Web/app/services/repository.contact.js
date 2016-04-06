(function () {
    'use strict';

    var serviceId = 'repository.contact';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryContact]);

    function RepositoryContact(AbstractRepository, $http) {
        var entityName = 'Contact';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getContacts = getContacts;
            this.getCount = getCount;
            this.getContact = getContact;
            this.saveContact = saveContact;
            this.deleteContact = deleteContact;
            this.updateContact = updateContact;
            this.getContactCount = getContactCount;
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getContacts(sort, page, pageSize, feilds) {
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

            return $http.get(this.hostEndPoint + "/api/Contact" + query);
        }

        function getCount() {
            return $http.get(this.hostEndPoint + "/api/Contact");
        }

        function getContact(ComNo) {
            //http://localhost:57148
            return $http.get(this.hostEndPoint + "/api/Contact/" + ComNo);
        }

        function getContactCount() {

            return $http.get(this.hostEndPoint + "/api/Contact/" + ComNo);
        }

        function saveContact(Contact) {
            var headers = {
                //'Access-Control-Allow-Origin': 'localhost:58243'
                //'Access-Control-Allow-Methods': '*',
                //'Content-Type': 'application/json',
                //'Accept': 'application/json'
            };

            var request = $http({
                method: "post",
                headers: headers,
                url: this.hostEndPoint + "/api/Contact",
                data: Contact
            });
            return request
        }

        function deleteContact(ComNo) {
            var headers = {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            };

            var request = $http({
                method: "delete",
                //headers:  headers,
                url: this.hostEndPoint + "/api/Contact/" + ComNo
            });

            return request;
        }

        function updateContact(ComNo, Contact) {

            var request = $http({
                method: "put",
                url: this.hostEndPoint + "/api/Contact/" + ComNo,
                data: Contact
            });
            return request;
        }


    }
})();




