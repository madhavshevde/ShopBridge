(function () {
	'use strict';
	angular
		.module('ShopBridge')
		.controller('productController', productController);
	function productController($scope, $http) {
		$scope.model = {
			products: [],
			productCategories : [],
			pagingInfo: {},
			currentCategory: null,
			currentProductName : null,
			editor: {
				productID: "",
				productName: "",
				price: "",
				productImageName: "Upload Image",
				productImageBase64: "",
				productCategoryID : "-1"
			},
			displaySummary: true,
			formTitle: ""
		};

		$scope.callback = function (response) {
			$scope.model.products = response.data.Products;
			$scope.model.pagingInfo = response.data.PagingInfo;
			$scope.model.currentCategory = response.data.CurrentCategory;
			$scope.model.currentProductName = response.data.CurrentProductName;
		};

		$scope.pageLinks = function () {
			var input = [];
			for (var i = 1; i <= $scope.model.pagingInfo.TotalPages; i++) {
				input.push(i);
			}

			return input;
		};

		$scope.getPage = function (page) {
			let url = "Page" + page;
			if ($scope.model.currentCategory) {
				url = "category/" + $scope.model.currentCategory + "/" + url;
			}
			if ($scope.model.currentProductName) {
				url = "product" + "/" + $scope.model.currentProductName + "/" + url;
			}
			$scope.sendAjaxRequest("GET", $scope.callback, url);
		};

		$scope.getCurrentPage = function (page) {
			let url = "Page" + $scope.model.pagingInfo.CurrentPage;
			if ($scope.model.currentCategory) {
				url = "category/" + $scope.model.currentCategory + "/" + url;
			}
			if ($scope.model.currentProductName) {
				url = "product" + "/" + $scope.model.currentProductName + "/" + url;
			}
			$scope.sendAjaxRequest("GET", $scope.callback, url);
		};

		$scope.getCategory = function (productCategory) {
			let url = "Page1";
			if (productCategory)
				url = "category" + "/" + productCategory.ProductCategoryName + "/" + url;
			$scope.sendAjaxRequest("GET", $scope.callback, url);
		};

		$scope.searchProduct = function () {
			let url = "Page1";
			url = "product" + "/" + $scope.model.currentProductName + "/" + url;
			$scope.sendAjaxRequest("GET", $scope.callback, url);
		};

		$scope.removeProduct = function (product) {
			$scope.sendAjaxRequest("DELETE", function () {
				$scope.getCurrentPage();
			}, product.ProductID);
		};

		$scope.handleCreateClick = function () {
			$scope.model.editor.productID = 0;
			$scope.model.editor.productName = "";
			$scope.model.editor.price = "";
			$scope.model.editor.productImageName = "Upload image";
			$scope.model.editor.productImageBase64 = " ";
			$scope.model.editor.productCategoryID = "-1";
			$scope.model.displaySummary = false;
			$scope.model.formTitle = "Create";
		};

		$scope.handleEditClick = function (product) {
			$scope.model.editor.productID = product.ProductID;
			$scope.model.editor.productName = product.ProductName;
			$scope.model.editor.price = product.Price;
			$scope.model.editor.productImageName = product.ProductImageName || "Upload image";
			$scope.model.editor.productImageBase64 = product.ProductImageBase64 || " ";
			$scope.model.editor.productCategoryID = product.ProductCategoryID.toString();
			$scope.model.displaySummary = false;
			$scope.model.formTitle = "Edit";
		};

		$scope.sendAjaxRequest = function (httpMethod, callback, url, requestData) {
			var request = {
				method: httpMethod, url: "/api/ProductApi" + (url ? "/" + url : ""), data: requestData
			};
			var promise = $http(
				request
			).then(callback);

			return promise;
		};

		$scope.getAllProducts = function () {
			$scope.sendAjaxRequest("GET", $scope.callback, null);
		};

		$scope.getProductCategories = function () {
			$scope.sendAjaxRequest("GET", function (response) {
				$scope.model.productCategories = response.data;
			}, "Categories");
		};

		$scope.handleEditorClick = function () {
			let verb = $scope.model.editor.productID === 0 ? "POST" : "PUT";
			var promise = $scope.sendAjaxRequest(verb, function (response) {
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
				Price: $scope.model.editor.price,
				ProductImageName: $scope.model.editor.productImageName,
				ProductImageBase64: $scope.model.editor.productImageBase64,
				ProductCategoryID : $scope.model.editor.productCategoryID
			});
			promise.then(function () {
				$scope.getCurrentPage();
			}
			);
		};

		$scope.back = function () {
			$scope.model.displaySummary = true;
		};

		$scope.getPage(1);
		$scope.getProductCategories();
	}
})();
