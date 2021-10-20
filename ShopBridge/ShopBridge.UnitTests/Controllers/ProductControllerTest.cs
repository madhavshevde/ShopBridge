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
		public void Can_List_Products()
		{
			// Arrange
			ProductApiController productApiController = new ProductApiController(GetMockRepository().Object);

			// Act
			IEnumerable<Product> products = productApiController.List();

			// Assert
			Product[] prodArray = products.ToArray();
			Assert.IsTrue(prodArray.Length == 3);
			Assert.AreEqual(prodArray[0].ProductName, "P1");
			Assert.AreEqual(prodArray[1].ProductName, "P2");
			Assert.AreEqual(prodArray[2].ProductName, "P3");
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
			List<Product> products = new List<Product> {
				new Product {ProductID = 1, ProductName = "P1",Price=55.50M},
				new Product {ProductID = 2, ProductName = "P2",Price=99.50M},
				new Product {ProductID = 3, ProductName = "P3",Price=45.50M},
			}.ToList();
			mock.Setup(m => m.Products).Returns(products);
			mock.Setup(m => m.GetProduct(It.IsAny<int>())).Returns<int>(productID => { return products.Where(product => product.ProductID == productID).FirstOrDefault();});

			return mock;
		}
	}
}
