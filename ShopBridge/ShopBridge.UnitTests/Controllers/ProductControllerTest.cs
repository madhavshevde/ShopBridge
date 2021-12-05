using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridgeAPI;
using ShopBridgeAPI.Controllers.Api;
using Moq;
using ShopBridge.Domain.Abstract;
using ShopBridge.Domain.Entities;
using System.Linq;
using ShopBridgeAPI.Models;

namespace ShopBridge.UnitTests.Controllers
{
	/// <summary>
	/// Summary description for ProductControllerTest
	/// </summary>
	[TestClass]
	public class ProductControllerTest
	{
		public ProductControllerTest()
		{
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion
		[TestMethod]
		public void Can_Paginate()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);
			productApiController.PageSize = 3;

			// Act
			ProductsListViewModel result = productApiController.List(null,2);

			// Assert
			ProductInfo[] prodArray = result.Products.ToArray();
			Assert.IsTrue(prodArray.Length == 2);
			Assert.AreEqual(prodArray[0].ProductName, "P4");
			Assert.AreEqual(prodArray[1].ProductName, "P5");
		}

		[TestMethod]
		public void Can_Send_Pagination_View_Model()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);
			productApiController.PageSize = 3;

			// Act
			ProductsListViewModel result = productApiController.List(null, 2);

			// Assert
			PagingInfo pageInfo = result.PagingInfo;
			Assert.AreEqual(pageInfo.CurrentPage, 2);
			Assert.AreEqual(pageInfo.ItemsPerPage, 3);
			Assert.AreEqual(pageInfo.TotalItems, 5);
			Assert.AreEqual(pageInfo.TotalPages, 2);
		}

		[TestMethod]
		public void Can_Filter_Products()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);
			productApiController.PageSize = 3;

			// Act
			ProductInfo[] result = productApiController.List("Accessories", 1).Products.ToArray();

			// Assert
			Assert.AreEqual(result.Length, 3);
			Assert.IsTrue(result[0].ProductName == "P3" && result[0].ProductCategoryName == "Accessories");
			Assert.IsTrue(result[1].ProductName == "P4" && result[0].ProductCategoryName == "Accessories");
			Assert.IsTrue(result[2].ProductName == "P5" && result[0].ProductCategoryName == "Accessories");
		}

		[TestMethod]
		public void Can_Return_Categories()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);

			// Act
			ProductCategoryInfo[] result = productApiController.GetCategories().ToArray();

			// Assert
			Assert.AreEqual(result.Length, 2);
			Assert.IsTrue(result[0].ProductCategoryID == 2 && result[0].ProductCategoryName == "Components");
			Assert.IsTrue(result[1].ProductCategoryID == 4 && result[1].ProductCategoryName == "Accessories");
		}

		[TestMethod]
		public void Indicates_Selected_Category()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);
			string categoryToSelect = "Components";

			// Act
			ProductsListViewModel result = productApiController.List(categoryToSelect, 2);

			// Assert
			Assert.AreEqual(result.CurrentCategory, categoryToSelect);
		}


		[TestMethod]
		public void Generate_Category_Specific_Product_Count()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);
			productApiController.PageSize = 3;

			// Act
			// Action - test the product counts for different categories
			int result1 = productApiController.List("Components", 1).PagingInfo.TotalItems;
			int result2 = productApiController.List("Accessories", 1).PagingInfo.TotalItems;

			// Assert
			Assert.AreEqual(result1, 2);
			Assert.AreEqual(result2, 3);
		}

		[TestMethod]
		public void Can_Search_Product()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);
			productApiController.PageSize = 3;

			// Act
			productApiController.Search("P", 1);
			ProductInfo[] result = productApiController.Search("P", 1).Products.ToArray();

			// Assert
			Assert.AreEqual(result.Length, 3);
			Assert.IsTrue(result[0].ProductName == "P1" && result[0].ProductID == 1);
			Assert.IsTrue(result[1].ProductName == "P2" && result[1].ProductID == 2);
			Assert.IsTrue(result[2].ProductName == "P3" && result[2].ProductID == 3);
		}



		[TestMethod]
		public void Can_Get_Product()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);

			// Act
			Product product1 = productApiController.Get(1);
			Product product2 = productApiController.Get(2);
			Product product3 = productApiController.Get(3);

			// Assert
			Assert.AreEqual(product1.ProductName, "P1");
			Assert.AreEqual(product2.ProductName, "P2");
			Assert.AreEqual(product3.ProductName, "P3");
		}

		[TestMethod]
		public void Can_Insert_Product()
		{
			// Arrange mock repository
			Mock<IProductRepository> mock = GetMockRepository();
			ProductApiController productApiController = new ProductApiController(mock.Object);
			Product product = new Product { ProductName = "Test" };

			// Act
			productApiController.Insert(product);

			// Assert - check that the repository was called
			mock.Verify(m => m.SaveProduct(product));
		}

		[TestMethod]
		public void Can_Delete_Product()
		{
			// Arrange
			Mock<IProductRepository> mock = GetMockRepository();
			ProductApiController productApiController = new ProductApiController(mock.Object);
			Product product = new Product { ProductID = 2, ProductName = "P2", Price = 99.50M };

			// Act
			productApiController.Delete(product.ProductID);

			// Assert - ensure that the repository delete method was
			// called with the correct Product
			mock.Verify(m => m.DeleteProduct(product.ProductID));
		}


		private Mock<IProductRepository> GetMockRepository()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			List<ProductInfo> productInfoList = new List<ProductInfo> {
				new ProductInfo {ProductID = 1, ProductName = "P1",Price=55.50M,ProductCategoryID=2,ProductCategoryName="Components"},
				new ProductInfo {ProductID = 2, ProductName = "P2",Price=99.50M,ProductCategoryID=2,ProductCategoryName="Components"},
				new ProductInfo {ProductID = 3, ProductName = "P3",Price=45.50M,ProductCategoryID=4,ProductCategoryName="Accessories"},
				new ProductInfo {ProductID = 4, ProductName = "P4",Price=495.50M,ProductCategoryID=4,ProductCategoryName="Accessories"},
				new ProductInfo {ProductID = 5, ProductName = "P5",Price=355.50M,ProductCategoryID=4,ProductCategoryName="Accessories"},
			}.ToList();
			mock.Setup(m => m.Products).Returns(productInfoList.AsQueryable());
			mock.Setup(m => m.GetProduct(It.IsAny<int>())).Returns<int>(productID => {
				ProductInfo productInfo = productInfoList.Where(p => p.ProductID == productID).FirstOrDefault();
				Product product = null;
				if(productInfo != null)
				{
					product = new Product
					{
						ProductID = productInfo.ProductID,
						ProductCategoryID = productInfo.ProductCategoryID,
						ProductName = productInfo.ProductName,
						ProductImageBase64 = productInfo.ProductImageBase64,
						ProductImageName = productInfo.ProductImageName
					};
				}
				return product;
			});
			mock.Setup(m => m.ProductCategories).Returns(productInfoList
				.GroupBy(productInfo => new { productInfo.ProductCategoryID, productInfo.ProductCategoryName })
				.OrderBy(grp => grp.Key.ProductCategoryID)
				.Select(grp => new ProductCategoryInfo { ProductCategoryID = grp.Key.ProductCategoryID, ProductCategoryName = grp.Key.ProductCategoryName })
				.ToList()
				.AsQueryable()
			);

			return mock;
		}
	}
}
