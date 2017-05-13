
(function () {
	'use strict';

	// controller function
	function dirCtrl($scope, $http) {

		var uri = "/api/owners";

		$scope.owners = [];
		$scope.title = "Hello";
		$scope.currentPets = [];
		$scope.result = {};

		$scope.click = click;
		$scope.removeOwner = removeOwner;
		$scope.addOwner = addOwner;

		function click(ref) {
			$scope.currentPets = ref.owner.pets;
		}	

		function removeOwner(owner) {
			var index = $scope.owners.indexOf(owner);
			if (index > -1) {
				$scope.owners.splice(index, 1);
			}
			removeFromDb(owner, $scope, $http);
		}

		function addOwner() {
			addToDb(this.newOwner, $scope, $http)
		}

		function addToDb(name, $scope, $http) {
			$http({
				method: 'POST',
				url: uri,
				dataType: 'json',
				params: { ownerName: name }
			}
			)
			.then(function (response) {
				if (response.status = 201) {
					$scope.owners.Add(respons.data)
					$scope.result = "Created new owner with name: " + response.data.ownerName;
					console.log(response);

				}
			});
		}

		function removeFromDb(owner, $scope, $http) {
			$http({
				method: 'DELETE',
				url: uri+"/"+ owner.ownerId,
				//dataType: 'json',
				//params: { dir: JSON.stringify(dirs) }
			}
			)
			.then(function (response) {
				console.log(response);
			});
		}

		$("#LoadingImage").show();

		$http.get(uri)
			.then(function (response) {
				var data = response.data;
				$scope.owners = data;
				console.log(data);
			});
	}

	angular
		.module('OwnersPets', [])
		.controller('OwnersCtrl', dirCtrl);

	dirCtrl.$inject = ['$scope', '$http'];
})();