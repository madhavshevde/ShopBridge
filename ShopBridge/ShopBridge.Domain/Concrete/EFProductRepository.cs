using ShopBridge.Domain.Abstract;
using ShopBridge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Domain.Concrete
{
	public class EFProductRepository : IProductRepository
	{
		private EFDbContext _context = new EFDbContext();


		/// <summary>
		///  Return only product categories mapped with products.
		/// </summary>
		public IQueryable<ProductCategoryInfo> ProductCategories
		{
			get
			{
				return _context.ProductCategories.Join(_context.Products,
					productCategory => productCategory.ProductCategoryID,
					product => product.ProductCategoryID,
					(productCategory, product) => productCategory
				)
				.GroupBy(productCategory => new { productCategory.ProductCategoryID, productCategory.ProductCategoryName })
				.OrderBy(grp => grp.Key.ProductCategoryName)
				.Select(grp => new ProductCategoryInfo{ProductCategoryID=grp.Key.ProductCategoryID,ProductCategoryName=grp.Key.ProductCategoryName});
			}
		}

		public IQueryable<ProductInfo> Products
		{
			get
			{
				return _context.Products.Join(_context.ProductCategories, product => product.ProductCategoryID,
					productCategory => productCategory.ProductCategoryID,
					(product, productCategory) => new ProductInfo
					{
						ProductID = product.ProductID,
						ProductName = product.ProductName,
						Price = product.Price,
						ProductImageName = product.ProductImageName,
						ProductImageBase64 = product.ProductImageBase64,
						ProductCategoryID = product.ProductCategoryID,
						ProductCategoryName = productCategory.ProductCategoryName
					});
			}
		}

		public Product DeleteProduct(int productID)
		{
			Product dbEntry = _context.Products.Find(productID);
			if (dbEntry != null)
			{
				_context.Products.Remove(dbEntry);
				_context.SaveChanges();
			}
			else
			{

				throw new ArgumentException($"No product with ID = {productID}");
			}

			return dbEntry;
		}

		public Product GetProduct(int productID)
		{
			Product product = _context.Products.Find(productID);
			if(product == null)
			{
				throw new ArgumentException($"No product with ID = {productID}");
			}

			return product;
		}

		public Product SaveProduct(Product product)
		{
			if (product.ProductID == 0)
			{
				_context.Products.Add(product);
			}
			else
			{
				Product dbEntry = _context.Products.Find(product.ProductID);
				if (dbEntry != null)
				{
					dbEntry.ProductName = product.ProductName;
					dbEntry.ProductCategoryID = product.ProductCategoryID;
					dbEntry.Price = product.Price;
					dbEntry.ProductImageName = product.ProductImageName;
					dbEntry.ProductImageBase64 = product.ProductImageBase64;
				}
				else
				{
					throw new ArgumentException($"No product with ID = {product.ProductID}");
				}
			}
			_context.SaveChanges();

			return product;
		}
	}
}
