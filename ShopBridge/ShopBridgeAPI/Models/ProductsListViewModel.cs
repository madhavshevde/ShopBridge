using ShopBridge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBridgeAPI.Models
{
	public class ProductsListViewModel
	{
		public IQueryable<ProductInfo> Products { get; set; }
		public PagingInfo PagingInfo { get; set; }
		public string CurrentCategory { get; set; }
		public string CurrentProductName { get; set; }
	}
}