(function () {
	'use strict';

	var serviceId = 'repository.client';
	angular.module('app').factory(serviceId,
        ['repository.abstract','$http', RepositoryClient]);

	function RepositoryClient(AbstractRepository, $http) {
	    var resource = "/api/Client";

		function Ctor(endpoint) {
			this.serviceId = serviceId;
			this.resource = resource;
			this.hostEndPoint = endpoint;
			//Exposed data access functions
			this.getClients = getClients;
			this.getClientByCode = getClientByCode;
			this.codeAvailable = codeAvailable;
			this.getCount = getCount;
			this.getClient = getClient;
			this.saveClient = saveClient;
			this.deleteClient = deleteClient;
			this.updateClient = updateClient;
			this.getClientCount = getClientCount;
		}

		AbstractRepository.extend(Ctor);

		return Ctor;

		function getClients(sort,page,pageSize,feilds,search) {
			var query = "?";

			if (sort != null) {	query = "sort=" + sort; }

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

			return $http.get(this.hostEndPoint + resource + query);
		}

		function getClientByCode(code) {
		    return $http.get(this.hostEndPoint + resource + "/" + code + "/Code");
		}

		function codeAvailable(id, code) {
		    return $http.get(this.hostEndPoint + resource + "/" + id + "/" + code + "/codeAvailable");
		}

		function getCount() {
		    return $http.get(this.hostEndPoint + resource);
		}

		function getClient(ComNo) {
			//http://localhost:57148
		    return $http.get(this.hostEndPoint + resource + "/" + ComNo);
		}

		function getClientCount() {

		    return $http.get(this.hostEndPoint + resource + "/" + ComNo);
		}

		function saveClient(Client) {
			var headers = {
				//'Access-Control-Allow-Origin': 'localhost:58243'
				//'Access-Control-Allow-Methods': '*',
				//'Content-Type': 'application/json',
				//'Accept': 'application/json'
			};

			var request = $http({
				method: "post",
				headers:headers,
				url: this.hostEndPoint + resource,
				data: Client
			});
			return request
		}

		function deleteClient(ComNo) {
			var headers = {
				'Access-Control-Allow-Origin': '*',
				'Access-Control-Allow-Methods': '*',
				'Content-Type': 'application/json',
				'Accept': 'application/json'
			};

			var request = $http({
				method: "delete",
				//headers:  headers,
				url: this.hostEndPoint + resource + "/" + ComNo
			});
			
			return request;
		}

		function updateClient(ComNo, Client) {

			var request = $http({
				method: "put",
				url: this.hostEndPoint + resource  + "/" + ComNo,
				data: Client
			});
			return request;
		}
		

	}
})();




