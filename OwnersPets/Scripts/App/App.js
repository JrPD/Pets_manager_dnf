(function () {
	'use strict';

	function routing($routeProvider, $locationProvider) {
		$routeProvider
		  .when('/Pets/:ownerId', {
		  	templateUrl: 'Pets.html',
		  	controller: 'PetCtrl',
		  	controllerAs: 'PetCtrl'
		  })
		 .when('/', {
		 	templateUrl: 'Owners.html',
		 	controller: 'OwnersCtrl',
		 	controllerAs: 'OwnersCtrl'
		 })
		.otherwise({
			templateUrl: 'Owners.html',
			controller: 'OwnersCtrl',
			controllerAs: 'OwnersCtrl'
		})		;
		$locationProvider.html5Mode(true);
	}

	var app = angular
		.module('OwnersPets',['ngRoute'])
		.config(['$routeProvider', '$locationProvider',routing])

})();