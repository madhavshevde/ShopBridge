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
		private EFDbContext context = new EFDbContext();
		public IEnumerable<Product> Products
		{
			get
			{
				return context.Products;
			}
		}

		public Product DeleteProduct(int productID)
		{
			Product dbEntry = context.Products.Find(productID);
			if (dbEntry != null)
			{
				context.Products.Remove(dbEntry);
				context.SaveChanges();
			}
			else
			{

				throw new ArgumentException($"No product with ID = {productID}");
			}

			return dbEntry;
		}

		public Product GetProduct(int productID)
		{
			Product product = context.Products.Find(productID);
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
				context.Products.Add(product);
			}
			else
			{
				Product dbEntry = context.Products.Find(product.ProductID);
				if (dbEntry != null)
				{
					dbEntry.ProductName = product.ProductName;
					dbEntry.Price = product.Price;
				}
				else
				{
					throw new ArgumentException($"No product with ID = {product.ProductID}");
				}
			}
			context.SaveChanges();

			return product;
		}
	}
}
