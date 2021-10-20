(function () {
	'use strict';
	angular
		.module('ShopBridge')
		.controller('productController', productController);
	function productController($scope, $http) {
		$scope.model = {
			products: [],
			editor: {
				productID: "",
				productName: "",
				price: ""
			},
			displaySummary: true,
			formTitle: ""
		};

		$scope.removeProduct = function (product) {
			$scope.sendAjaxRequest("DELETE", function () {
				$scope.getAllProducts();
			}, product.ProductID);
		};

		$scope.handleCreateClick = function () {
			$scope.model.editor.productID = 0;
			$scope.model.editor.productName = "";
			$scope.model.editor.price = "";
			$scope.model.displaySummary = false;
			$scope.model.formTitle = "Create";
		};

		$scope.handleEditClick = function (product) {
			$scope.model.editor.productID = product.ProductID;
			$scope.model.editor.productName = product.ProductName;
			$scope.model.editor.price = product.Price;
			$scope.model.displaySummary = false;
			$scope.model.formTitle = "Edit";
		};

		$scope.sendAjaxRequest = function (httpMethod, callback, url, requestData) {
			var request = {
				method: httpMethod, url: "/api/ProductApi" + (url ? "/" + url : ""), data: requestData
			};
			$http(
				request
			).then(callback);
		};

		$scope.getAllProducts = function () {
			$scope.sendAjaxRequest("GET", function (response) {
				let data = response.data;
				$scope.model.products.length = 0;
				for (var i = 0; i < data.length; i++) {
					$scope.model.products.push(data[i]);
				}
			});
		};

		$scope.handleEditorClick = function () {
			let verb = $scope.model.editor.productID === 0 ? "POST" : "PUT";
			$scope.sendAjaxRequest(verb, function (response) {
				let newProduct = response.data;
				let index = $scope.model.products.findIndex(product => product.ProductID === newProduct.ProductID);
				if (index !== -1) {
					$scope.model.products.splice(index, 1);
				}
				$scope.model.products.push(newProduct);
				$scope.model.displaySummary = true;
			}, null, {
				ProductID: $scope.model.editor.productID,
				ProductName: $scope.model.editor.productName,
				Price: $scope.model.editor.price
			});
		};

		$scope.back = function () {
			$scope.model.displaySummary = true;
		};

		$scope.getAllProducts();
	}
})();
