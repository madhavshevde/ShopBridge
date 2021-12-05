using ShopBridge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Domain.Abstract
{
	public interface IProductRepository
	{
		IQueryable<ProductInfo> Products { get; }
		IQueryable<ProductCategoryInfo> ProductCategories { get; }
		Product SaveProduct(Product product);
		Product DeleteProduct(int productID);
		Product GetProduct(int productID);
	}
}
