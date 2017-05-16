angular
	.module('OwnersPets')
	.controller('PetCtrl', ['$scope', '$http','$routeParams', PetCtrl])
function PetCtrl($scope, $http, $routeParams) {
	$scope.name = 'PetCtrl';
	$scope.ownerId = $routeParams.ownerId;
	//$('#OwnersPage').hide();
	//$('#petsView').show();
	$scope.removePet = removePet;
	$scope.addPet = addPet;

	$scope.goBack = function () {
		//$('#OwnersPage').show();
		//$('#petsView').hide();
		
	}
	var uriOwner = "/api/owners/" + $scope.ownerId
	var uri = "/api/pets/";
	$http.get(uriOwner)
		.then(function (response) {
			var data = response.data;
			$scope.owner = data;
			updateTotal();
			console.log(data);
		});

	function updateTotal() {
		$scope.totalCount = $scope.owner.pets.length;
	}
	function addPet() {
		if (this.newPet) {
			addToDb(this.newPet, $scope, $http);
		}
		$scope.result = "Too short name";
	}

	function addToDb(name, $scope, $http) {
		var pet = {
			petName: name,
			ownerId: $scope.owner
		};
		$http.post(uri + $scope.ownerId, pet)
		.then(function (response) {
			if (response.status = 201) {
				$scope.owner.pets.unshift(response.data)
				$scope.result = "Created new pet with name: " + response.data.petName;
				console.log(response);
				updateTotal();
			}
		});
	}

	function removePet(pet) {
		var index = $scope.owner.pets.indexOf(pet);
		if (index > -1) {
			$scope.owner.pets.splice(index, 1);
		}
		removeFromDb(pet, $scope, $http);
		updateTotal();
	}
	function removeFromDb(pet, $scope, $http) {
		$http({
			method: 'DELETE',
			url: uri + pet.petId
		})
		.then(function (response) {
			var name = response.data.petName;
			$scope.result = "Pet with name: " + name + " removed"
			console.log(response);
		});
	}
}