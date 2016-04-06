(function () {
	'use strict';

	var serviceId = 'repository.company';
	angular.module('app').factory(serviceId,
        ['repository.abstract','$http', RepositoryCompany]);

	function RepositoryCompany(AbstractRepository, $http) {
		var entityName = 'Company';

		function Ctor(endpoint) {
			this.serviceId = serviceId;
			this.entityName = entityName;
			this.hostEndPoint = endpoint;
			//Exposed data access functions
			this.getCompanies = getCompanies;
			this.getCount = getCount;
			this.getCompany = getCompany;
			this.saveCompany = saveCompany;
			this.deleteCompany = deleteCompany;
			this.updateCompany = updateCompany;
			this.getCompanyCount = getCompanyCount;
		}

		AbstractRepository.extend(Ctor);

		return Ctor;

		function getCompanies(sort,page,pageSize,feilds) {
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

			return $http.get(this.hostEndPoint + "/api/Company" + query);
		}

		function getCount() {
			return $http.get(this.hostEndPoint + "/api/Company");
		}

		function getCompany(ComNo) {
			//http://localhost:57148
			return $http.get(this.hostEndPoint + "/api/Company/" + ComNo);
		}

		function getCompanyCount() {

			return $http.get(this.hostEndPoint + "/api/Company/" + ComNo);
		}

		function saveCompany(Company) {
			var headers = {
				//'Access-Control-Allow-Origin': 'localhost:58243'
				//'Access-Control-Allow-Methods': '*',
				//'Content-Type': 'application/json',
				//'Accept': 'application/json'
			};

			var request = $http({
				method: "post",
				headers:headers,
				url: this.hostEndPoint + "/api/Company",
				data: Company
			});
			return request
		}

		function deleteCompany(ComNo) {
			var headers = {
				'Access-Control-Allow-Origin': '*',
				'Access-Control-Allow-Methods': '*',
				'Content-Type': 'application/json',
				'Accept': 'application/json'
			};

			var request = $http({
				method: "delete",
				//headers:  headers,
				url: this.hostEndPoint + "/api/Company/" + ComNo
			});
			
			return request;
		}

		function updateCompany(ComNo, Company) {

			var request = $http({
				method: "put",
				url: this.hostEndPoint + "/api/Company/" + ComNo,
				data: Company
			});
			return request;
		}
		

	}
})();




