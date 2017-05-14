// controller function
angular
	.module('OwnersPets')
	.controller('OwnersCtrl',['$scope', '$http', '$location', '$route', '$routeParams', OwnerCtrl])

function OwnerCtrl($scope, $http, $route, $routeParams, $location) {
	var uri = "/api/owners/";

	$scope.owners = [];
	$scope.title = "Hello";
	$scope.currentPets = [];
	$scope.result = "";

	$scope.click = click;
	$scope.removeOwner = removeOwner;
	$scope.addOwner = addOwner;
	$scope.viewPets = viewPets;

	function viewPets(owner) {

	}

	function click(ref) {
		//$scope.currentPets = ref.owner.pets;
		//$('#OwnersPage').hide();
	}

	function removeOwner(owner) {
		var index = $scope.owners.indexOf(owner);
		if (index > -1) {
			$scope.owners.splice(index, 1);
		}
		removeFromDb(owner, $scope, $http);
		updateTotal();
	}

	function addOwner() {
		if (this.newOwner) {
			addToDb(this.newOwner, $scope, $http);
			updateTotal();
		}
		$scope.result = "Too short name";

	}

	//function addToDb(name, $scope, $http) {
	//	$http({
	//		method: 'POST',
	//		url: uri,
	//		dataType: 'json',
	//		params: { ownerName: name }
	//	}
	//	)
	//	.then(function (response) {
	//		if (response.status = 201) {
	//			$scope.owners.unshift(response.data)
	//			$scope.result = "Created new owner with name: " + response.data.ownerName;
	//			console.log(response);

	//		}
	//	});
	//}

	function addToDb(ownerName, $scope, $http) {
		var owner = {
			ownerName: ownerName,
		}
		$http.post(uri, owner)
		.then(function (response) {
			if (response.status = 201) {
				$scope.owners.unshift(response.data)
				$scope.result = "Created new owner with name: " + response.data.ownerName;
				console.log(response);

			}
		});
	}

	function removeFromDb(owner, $scope, $http) {
		$http({
			method: 'DELETE',
			url: uri  + owner.ownerId
		}
		)
		.then(function (response) {
			var name = response.data.ownerName;
			$scope.result = "Owner with name: " + name + " removed"
			console.log(response);
		});
	}
	function updateTotal() {
		$scope.totalCount = $scope.owners.length;
	}

	$http.get(uri)
		.then(function (response) {
			var data = response.data;
			$scope.totalCount = data.length;
			$scope.owners = data;
			updateTotal();
			console.log(data);
		});
}
