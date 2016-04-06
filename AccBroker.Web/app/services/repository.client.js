(function () {
	'use strict';

	var serviceId = 'repository.client';
	angular.module('app').factory(serviceId,
        ['repository.abstract','$http', RepositoryClient]);

	function RepositoryClient(AbstractRepository, $http) {
		var entityName = 'Client';

		function Ctor(endpoint) {
			this.serviceId = serviceId;
			this.entityName = entityName;
			this.hostEndPoint = endpoint;
			//Exposed data access functions
			this.getClients = getClients;
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

			return $http.get(this.hostEndPoint + "/api/Client" + query);
		}

		function getCount() {
			return $http.get(this.hostEndPoint + "/api/Client");
		}

		function getClient(ComNo) {
			//http://localhost:57148
			return $http.get(this.hostEndPoint + "/api/Client/" + ComNo);
		}

		function getClientCount() {

			return $http.get(this.hostEndPoint + "/api/Client/" + ComNo);
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
				url: this.hostEndPoint + "/api/Client",
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
				url: this.hostEndPoint + "/api/Client/" + ComNo
			});
			
			return request;
		}

		function updateClient(ComNo, Client) {

			var request = $http({
				method: "put",
				url: this.hostEndPoint + "/api/Client/" + ComNo,
				data: Client
			});
			return request;
		}
		

	}
})();




