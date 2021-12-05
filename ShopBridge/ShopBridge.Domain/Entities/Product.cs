using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Domain.Entities
{
	public class Product
	{
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public string ProductImageName { get; set; }
		public string ProductImageBase64 { get; set; }
		public int ProductCategoryID { get; set; }
	}
}
